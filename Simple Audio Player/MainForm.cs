using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.MediaFoundation;
using CSCore.SoundOut;
using GlobalHook;

namespace Simple_Audio_Player
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		#region Consts
		
		const int WS_MINIMIZEBOX = 0x20000;
		const int CS_DBLCLKS = 0x8;
		
		
		const int StepCount = 100;
		const int Interval = 60;
		
		const int SongPosInterval = 120;
		
		const int SearchSize = 200;
		const int SearchSizeSpeed = 8;
		const int SearchSizeInterval = 12;
		
		#endregion
		
		#region Static readonly and const
		
		public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
			.Trim('\\') + "\\LonamiWebs\\Simple Audio Player\\";
		
		static readonly string Music = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic).Trim('\\')
			+ "\\";
		
		static readonly string FontPath = AppData + "Confortaa.ttf";
		static readonly string IndexLoc = AppData + "song.idx";
		
		public static bool NeedToRegen;

		static readonly string[] ValidFileTypes = {
			".wav", ".wave", ".mp3", ".flac", ".m4a", ".m4b", ".m4p", ".m4v",
			".m4r", ".3gp", ".mp4", ".aac", ".ac3", ".wav", ".raw", ".pcm"
		};
		
		static PrivateFontCollection pfc;
		public static FontFamily BeautyFont { get { return pfc.Families[0]; } }
		
		public static int ProgressWidth;
		public static int ProgressHeight;
		
		#endregion
		
		#region Overrides
		
		protected override CreateParams CreateParams
		{
		    get
		    {
		        CreateParams cp = base.CreateParams;
		        cp.Style |= WS_MINIMIZEBOX; // Allows minimizing
		        cp.ClassStyle |= CS_DBLCLKS; // by clicking SAP in the TaskBar
		        return cp;
		    }
		}
		
		#endregion
		
		#region Private variables and properties
		
		enum Status { Playing, Paused, Stopped };
		
		Status CurrentStatus = Status.Stopped;
		
		SearchMode CurrentSearchMode { get { return (SearchMode)Settings.GetValue<int>("SearchMode"); } }
		
		bool IsClosing, Shuffle, Repeat, ShowingSearch;
		
		IWaveSource soundSource;
		ISoundOut soundOut;
		
		InterProcessCommunication ipc = new InterProcessCommunication("SimpleAudioPlayer");
		
		IEnumerable<string> IndexedSongs {
			get {
				return File.ReadAllLines(IndexLoc).Where(l => !String.IsNullOrWhiteSpace(l));
			}
		}
		
		System.Windows.Forms.Timer TickTimer = new System.Windows.Forms.Timer { Interval = SongPosInterval };
		System.Windows.Forms.Timer HMTimer = new System.Windows.Forms.Timer
		{ Interval = 10 * 60 * 1000, Enabled = true };
		
		#endregion
		
		#region Public variables and properties
		
		public enum SearchMode { Simple = 0, Advanced = 1 };

		public TimeSpan SongPosition
		{ get { return soundSource != null ? ((IWaveStream)soundSource).GetPosition() : TimeSpan.FromMilliseconds(0); } }

		public TimeSpan SongDuration
		{ get { return soundSource != null ? ((IWaveStream)soundSource).GetLength() : TimeSpan.FromMilliseconds(0); } }
		
		public static Bitmap ProgressImage;
		
		UndoableInt PlayedIndicies;
		Random Timmy = new Random();
		
		#endregion
		
		#region Init
		
		public MainForm()
		{
			InitializeComponent();
			
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // Reduces (slightly) the flickering!
			
			searchTB.Width = 0;
			
			TickTimer.Tick += (s, e) => RefreshPosBar();
			HMTimer.Tick += (s, e) => {
				HookManager.KeyUp -= HookManager_KeyUp;
				HookManager.KeyUp += HookManager_KeyUp;
			};
			
			HookManager.KeyUp += HookManager_KeyUp;
			
			ProgressWidth = progressPB.Width;
			ProgressHeight = progressPB.Height;
			
			ProgressImage = File.Exists(Settings.GetValue<string>("Style")) ?
				ResizeImage(Image.FromFile(Settings.GetValue<string>("Style")), ProgressWidth, ProgressHeight)
				: ResizeImage(Resources.progress, ProgressWidth, ProgressHeight);
			
			ipc.Listen();
			ipc.NewMessage += ipc_NewMessage;
			
			titleL.BackColor = Settings.GetValue<Color>("FromColor");
			
			if (!File.Exists(FontPath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(FontPath));
				File.WriteAllBytes(FontPath, Resources.Comfortaa);
			}
			
			if (!File.Exists(IndexLoc) && Directory.Exists(Music))
				File.WriteAllLines(IndexLoc, Directory.EnumerateFiles(Music)
				                   .Where(f => ValidFileTypes.Any(f.EndsWith)));
			
			pfc = new PrivateFontCollection();
			pfc.AddFontFile(FontPath);
			
			var title = new Font(BeautyFont, titleL.Font.Size);
			var normal = new Font(BeautyFont, addB.Font.Size);
			
			titleL.Font = title;
			clearB.Font = addB.Font = musicLB.Font = listeningToL.Font = songNameL.Font = normal;
			
			RefreshLB();
			
			repeatCB.Checked = Settings.GetValue<bool>("Repeat");
			shuffleCB.Checked = Settings.GetValue<bool>("Shuffle");
		}

		void HookManager_KeyUp(object sender, KeyEventArgs e)
		{
			var validKeys = new [] { Keys.MediaPlayPause, Keys.MediaStop,
				Keys.MediaPreviousTrack, Keys.MediaNextTrack };
			
			if (validKeys.Contains(e.KeyCode))
		    {
				switch (e.KeyCode) {
					case Keys.MediaPlayPause:
						PlayPause();
						break;
					case Keys.MediaStop:
						Stop();
						break;
					case Keys.MediaPreviousTrack:
						Prev();
						break;
					case Keys.MediaNextTrack:
						Next();
						break;
			    	}
				
				e.Handled = true;
		    }
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			if (Settings.GetValue<bool>("AnimateTitle"))
				new Thread(ChangeTitleBarColor) { IsBackground = true, Priority = ThreadPriority.Lowest }.Start();
		}
		
		#endregion
		
		#region Inter Process Communication

		void ipc_NewMessage(string message)
		{
			Invoke(new MethodInvoker(() => {
             	ShowForm();
				
				if (!String.IsNullOrWhiteSpace(message)) {
					string[] songs = message.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries);
					if (songs.Length > 0) {
						File.AppendAllLines(IndexLoc, ofd.FileNames.Where(f => !songs.Contains(f)));
						musicLB.SelectedItem = Path.GetFileNameWithoutExtension(songs[0]);
						PlayPause();
					}
				}
			}));
		}
		
		#endregion
		
		#region Add, delete and refresh songs
		
		void MusicLBDragEnter(object sender, DragEventArgs e)
		{ e.Effect = DragDropEffects.Copy; }
		
		void MusicLBDragDrop(object sender, DragEventArgs e)
		{
			AddMix((string[])e.Data.GetData(DataFormats.FileDrop));
		}
		
		void AddBClick(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
				Add(ofd.FileNames);
		}
		
		void AddFolderBClick(object sender, EventArgs e)
		{
			if (fbd.ShowDialog() == DialogResult.OK)
				Add(fbd.SelectedPath);
		}
		
		void AddMix(IEnumerable<string> paths) 
		{
			if (paths.Any(Directory.Exists))
			{
				List<string> dirs = paths.Where(Directory.Exists).ToList();
				List<string> files = paths.ToList();
				
				foreach (var dir in dirs)
				{
					Add(dir);
					files.Remove(dir);
				}
				
				Add(files);
			}
			else
				Add(paths);
		}
		
		void Add(string directory) 
		{
		 	Add (Directory.GetFiles(directory,"*.*",
			                        SearchOption.AllDirectories).Where(f => ValidFileTypes.Any(f.EndsWith)));
		}
		
		void Add(IEnumerable<string> songs)
		{
			IEnumerable<string> indexedSongs = IndexedSongs;
			File.AppendAllLines(IndexLoc, songs.Where(f => !indexedSongs.Contains(f)));
			
			RefreshLB();
		}
		
		void Delete(string songName)
		{
			File.WriteAllLines(IndexLoc, File.ReadAllLines(IndexLoc)
			                   .Where(s => Path.GetFileNameWithoutExtension(s) != songName));
			RefreshLB(true);
		}
		
		void ClearBClick(object sender, EventArgs e)
		{
			File.WriteAllText(IndexLoc, "");
			RefreshLB();
		}
		
		void RefreshLB(bool deleting = false)
		{
			object item = deleting ? musicLB.SelectedIndex : musicLB.SelectedItem;
			
			musicLB.BeginUpdate();
			musicLB.Items.Clear();
			foreach (var file in GetSongs())
				musicLB.Items.Add(Path.GetFileNameWithoutExtension(file));
			musicLB.EndUpdate();
			
			if (PlayedIndicies == null)
				PlayedIndicies = new UndoableInt(new Random().Next(0, musicLB.Items.Count));
			
			if (deleting) {
				int idx = (int)item;
				if (idx > -1 && idx < musicLB.Items.Count)
					musicLB.SelectedIndex = idx;
			} else
				musicLB.SelectedItem = item;
		}
		
		IEnumerable<string> GetSongs() {
			if (searchTB.Text.Length > 0)
			{
				switch (CurrentSearchMode)
				{
					case SearchMode.Simple:
					default:
						return IndexedSongs.Where(s => Path.GetFileNameWithoutExtension(s)
					                          .ToLower().Contains(searchTB.Text.ToLower()));
						
					case SearchMode.Advanced:
						return _GetAdvSearch();
				}
			}
			return IndexedSongs;
		}
		
		IEnumerable<string> _GetAdvSearch()
		{
			var must = searchTB.Text.Trim().Split(' ');
			
			bool valid = true;
			
			foreach (var _song in IndexedSongs)
			{
				var song = Path.GetFileNameWithoutExtension(_song);
				valid = true;
				
				var spl = song.Split(' ').ToList();
				
				foreach (var m in must)
				{
					var i = spl.FirstOrDefault(s => s.IndexOf(m, StringComparison.InvariantCultureIgnoreCase) >= 0);
					if (i != null)
						spl.Remove(i);
					else
					{
						valid = false;
						break;
					}
				}
				
				if (valid)
					yield return _song;
			}
		}
		
		void ClearIndex()
		{
			var songs = File.ReadAllLines(IndexLoc);
			var validSongs = new List<string>();
			
			foreach (var song in songs)
				if (File.Exists(song))
					validSongs.Add(song);
			
			File.WriteAllLines(IndexLoc, validSongs);
			
			RefreshLB();
			if (CurrentStatus == Status.Playing)
				PlayPause();
		}
		
		#endregion
		
		#region Consts and externs
		
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
		
		#endregion
		
		#region Title bar emulation
		
		void SettingsBClick(object sender, EventArgs e)
		{
			var f = Application.OpenForms["SettingsForm"];
			if (f == null)
				new SettingsForm().Show();
			else
				f.Activate();
		}
		
		void MinimizeTrayBClick(object sender, EventArgs e)
		{ WindowState = FormWindowState.Minimized; }
		
		void MinimizeBClick(object sender, EventArgs e)
		{ HideForm(); }
		
		void TrayNIClick(object sender, EventArgs e)
		{ ShowForm(); }
		
		void HideForm() {
			WindowState = FormWindowState.Minimized;
			ShowInTaskbar = false;
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			trayNI.Visible = true;
		}
		
		void ShowForm() {
			WindowState = FormWindowState.Normal;
			ShowInTaskbar = true;
			FormBorderStyle = FormBorderStyle.None;
			trayNI.Visible = false;
			Activate();
		}

		void CloseBClick(object sender, EventArgs e)
		{
			IsClosing = true;
			Close();
		}
		
		void TitleLMouseDown(object sender, MouseEventArgs e)
		{
		    if (e.Button == MouseButtons.Left)
		    {
		        ReleaseCapture();
		        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
		    }
		}
		
		#endregion
		
		#region Song progress
		
		float GetSongPos() {
			return (float)(SongPosition.TotalMilliseconds / SongDuration.TotalMilliseconds);
		}
		
		void SetSongPos(float position)
		{
			try {
			if (soundSource != null)
				((IWaveStream)soundSource).SetPosition(TimeSpan.FromMilliseconds
				                                       (SongDuration.TotalMilliseconds * position));
			} catch (NullReferenceException) { /* TODO I'd like to avoid this... */ }
			// This occurs ^ if you press: next, play, pause, next, set song pos
		}
		
		void ProgressPBMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				SetSongPos(GetPermillageFromPosition(e.Location, (Control)sender));
				progressPB.Refresh();
			}
		}
		
		
		void ProgressPBMouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				SetSongPos(GetPermillageFromPosition(e.Location, (Control)sender));
				progressPB.Refresh();
			}
		}

		float GetPermillageFromPosition(PointF location, Control control) {
			float permillage = (float)location.X / (float)control.Width;
			if (permillage < 0)
				permillage = 0;
			else if (permillage > 1)
				permillage = 1;

			return permillage;
		}
		
		#endregion
		
		#region Play songs
		
		void PlayPause(bool force = false) {
			if (force)
				Stop();
			
			searchTB.Clear();
			
			if (CurrentStatus == Status.Playing)
				Pause();
			else if (CurrentStatus == Status.Paused)
				Resume();
			else
			{
				if (musicLB.Items.Count == 0)
					return;
				
				if (musicLB.SelectedIndex < 0)
					musicLB.SelectedIndex = Shuffle ? new Random().Next(0, musicLB.Items.Count) : 0;
				
				Play(GetSongPath());
			}
		}
		
		void Play(string song)
		{
			try
			{
				if (song == null)
					return;
				
				if (!File.Exists(song)) {
					ClearIndex();
					return;
				}
				
				if (CurrentStatus == Status.Playing || CurrentStatus == Status.Paused)
					Stop();
				
				soundSource = GetSoundSource(song);
				
				soundOut = GetSoundOut();
	            soundOut.Initialize(soundSource);
	            
	            soundOut.Stopped += SongFinished;
		            
	            soundOut.Play();
	            CurrentStatus = Status.Playing;
	            RefreshPosBar();
	            
	            if (Settings.GetValue<bool>("Toast"))
	            	new Toast((Toast.ToastLocation)Settings.GetValue<int>("ToastLocation"), "Ahora escuchando a",
	            	          Path.GetFileNameWithoutExtension(song));
	            
	            TickTimer.Enabled = true;
	            RefreshButtonImage();
			}
			catch (MediaFoundationException)
			{
				/* Thanks to Albert for having corrupted songs */
				MessageBox.Show("La canción " + Path.GetFileNameWithoutExtension(song) + " no se pudo reproducir. " +
				                "Esto puede deberse a que el archivo esté corrupto", "Archivo corrupto",
				                MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (NotSupportedException)
			{
				MessageBox.Show("La canción " + Path.GetFileNameWithoutExtension(song) + " utiliza códecs no soportados por Simple Audio Player",
				                "Códecs no soportados", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void SongFinished(object sender, EventArgs e)
		{
			if (CurrentStatus == Status.Playing)
				Next();
		}
		
		void Pause()
		{
			CurrentStatus = Status.Paused;
			soundOut.Pause();
			
            TickTimer.Enabled = false;
            RefreshButtonImage();
		}
		
		void Resume()
		{
			soundOut.Resume();
			CurrentStatus = Status.Playing;
			
            TickTimer.Enabled = true;
            RefreshButtonImage();
		}
		
		void Stop()
		{
			CurrentStatus = Status.Stopped;
			try {
				if (soundOut != null)
				{
					soundOut.Stopped -= SongFinished;
					soundOut.Stop();
				}
				if (soundSource != null)
					soundSource.Dispose();
			}
			catch (ObjectDisposedException) { }
			
            TickTimer.Enabled = false;
            RefreshPosBar();
            RefreshButtonImage();
		}
		
		public void StopAndDispose()
		{
			try
			{
				Stop();
				soundOut.Dispose();
				_SoundOut.Dispose();
			} catch (ObjectDisposedException) { }
		}
		
		void Prev() {
			if (Repeat)
				PlayPause(true);
			else
				if (Shuffle)
				{
					PlayedIndicies.Undo();
					UpdateSelected(PlayedIndicies);
				}
				else
				{
					int i = musicLB.SelectedIndex;
					if (--i < 0)
						i = musicLB.Items.Count - 1;
					UpdateSelected(i);
				}
		}
		
		void Next()
		{
			if (Repeat)
				PlayPause(true);
			else
				if (Shuffle)
				{
					if (!PlayedIndicies.Redo()) {
						int i = Timmy.Next(0, musicLB.Items.Count);
						
						if (PlayedIndicies.StepCount >= musicLB.Items.Count)
							PlayedIndicies = new UndoableInt(i);
						else {
							while (PlayedIndicies.Contains(i))
								i = Timmy.Next(0, musicLB.Items.Count);
							
							PlayedIndicies.SetValue(i);
						}
					}
					UpdateSelected(PlayedIndicies);
				}
				else
				{
					int i = musicLB.SelectedIndex;
					if (++i >= musicLB.Items.Count)
						i = 0;
					UpdateSelected(i);
				}
		}
		
		void UpdateSelected(int index)
		{
			if (index < 0 || index >= musicLB.Items.Count)
				return;
			
			musicLB.SelectedIndex = index;
			if (CurrentStatus == Status.Playing)
				PlayPause(true);
			else if (CurrentStatus == Status.Paused)
				Stop();
		}
		
		static ISoundOut _SoundOut;
		static ISoundOut GetSoundOut()
		{
			if (_SoundOut == null)
			{
				if (WasapiOut.IsSupportedOnCurrentPlatform)
					_SoundOut = new WasapiOut();
				else
					_SoundOut = new DirectSoundOut();
			}
			
			return _SoundOut;
		}
		
		IWaveSource GetSoundSource(string file)
		{ return CodecFactory.Instance.GetCodec(file); }
		
		string GetSongPath()
		{
			return musicLB.SelectedIndex < 0 ? null : IndexedSongs.ToList()[musicLB.SelectedIndex];
		}
		
		#endregion
		
		#region Color utils
		
		void ChangeTitleBarColor()
		{	
			try {
				var steps = GenSteps(Settings.GetValue<Color>("FromColor"), Settings.GetValue<Color>("ToColor"),
				                     StepCount);
				
				bool increasing = true;
				
				int p = 0;
				
				while (!IsClosing) {
					if (NeedToRegen) {
						steps = GenSteps(Settings.GetValue<Color>("FromColor"), Settings.GetValue<Color>("ToColor"),
						                 StepCount);
						NeedToRegen = false;
					}
					
					Color c = steps[p];
					titleL.Invoke(new MethodInvoker(() => titleL.BackColor = c));
					
					if (p == StepCount - 1)
						increasing = false;
					else if (p == 0)
						increasing = true;
					
					if (increasing)
						p++;
					else
						p--;
					
					Thread.Sleep(Interval);
				}
			}
			catch (ObjectDisposedException) { }
			catch (InvalidOperationException) {}
		}
		
		static Color[] GenSteps(Color fromC, Color toC, int stepCount) {
			var steps = new Color[stepCount];
			
			float rincrease = fromC.R < toC.R ?
				((float)(toC.R - fromC.R)) / (float)stepCount
			 	: ((float)(fromC.R - toC.R)) / (float)stepCount;
			
			float gincrease = fromC.G < toC.G ?
				((float)(toC.G - fromC.G)) / (float)stepCount
			 	: ((float)(fromC.G - toC.G)) / (float)stepCount;
			
			float bincrease = fromC.B < toC.B ?
				((float)(toC.B - fromC.B)) / (float)stepCount
			 	: ((float)(fromC.B - toC.B)) / (float)stepCount;
			
			for (float i = 0f; i < stepCount; i++) {
				int r = (int)(fromC.R < toC.R ? fromC.R + rincrease * i : fromC.R - rincrease * i);
				int g = (int)(fromC.G < toC.G ? fromC.G + gincrease * i : fromC.G - gincrease * i);
				int b = (int)(fromC.B < toC.B ? fromC.B + bincrease * i : fromC.B - bincrease * i);
				
				Console.WriteLine(r + ", " + g + ", " + b);
				
				steps[(int)i] = Color.FromArgb(r, g, b);
			}
			
			return steps;
		}
		
		#endregion
		
		#region Search
		
		void SearchBClick(object sender, EventArgs e)
		{
			if (ShowingSearch)
				HideSearch();
			else
				ShowSearch();
		}
		
		void SearchTBTextChanged(object sender, EventArgs e)
		{
			RefreshLB();
		}
		
		void ShowSearch()
		{
			ShowingSearch = true;
			
			searchTB.Focus();
			
			new TaskFactory().StartNew(() => {
			                           	
			                           	Invoke(new MethodInvoker(() => 
			                           	                         SuspendUpdate.Suspend(musicLB)));
			                           	                                  	
               	while (GetSearchWidth() <= SearchSize && ShowingSearch) {
               		AddSearchWidth(SearchSizeSpeed);
               		BetterSleep.Sleep(SearchSizeInterval);
              	}
			                           	
			                           	Invoke(new MethodInvoker(() => 
			                           	                         SuspendUpdate.Resume(musicLB)));
				
          	});
		}
		
		void HideSearch()
		{
			ShowingSearch = false;
			
			new TaskFactory().StartNew(() => {
			                           	
			                           	Invoke(new MethodInvoker(() => 
			                           	                         SuspendUpdate.Suspend(musicLB)));
			                           	
               	while (GetSearchWidth() > 0 && !ShowingSearch) {
               		AddSearchWidth(-SearchSizeSpeed);
               		BetterSleep.Sleep(SearchSizeInterval);
              	}
			                           	
			                           	Invoke(new MethodInvoker(() => 
			                           	                         SuspendUpdate.Resume(musicLB)));
			                           	
				SetSearchText("");
          	});
		}
		
		int GetSearchWidth() {
			int width = -1;
			searchTB.Invoke(new MethodInvoker(() => width = searchTB.Width));
			return width;
		}
		
		void AddSearchWidth(int width) {
			searchTB.BeginInvoke(new MethodInvoker(() => searchTB.Width += width));
		}
		
		void SetSearchText(string text) {
			searchTB.BeginInvoke(new MethodInvoker(() => searchTB.Text = text));
		}
		
		#endregion
		
		#region Buttons
		
		void MusicLBDoubleClick(object sender, EventArgs e) {
			HideSearch();
			PlayPause(true);
		}
		
		void PlayPauseBClick(object sender, EventArgs e) { PlayPause(); }
		
		void StopBClick(object sender, EventArgs e) { Stop(); }
		
		void NextBClick(object sender, EventArgs e) { Next(); }
		
		void PrevBClick(object sender, EventArgs e) { Prev(); }
		
		void ShuffleCBCheckedChanged(object sender, EventArgs e)
		{
			Shuffle = shuffleCB.Checked;
			
			if (musicLB.SelectedIndex < 0 && musicLB.Items.Count > 0)
			{
				if (PlayedIndicies >= musicLB.Items.Count)
					Next();
				musicLB.SelectedIndex = PlayedIndicies;
			}
			
			Settings.SetValue<bool>("Shuffle", Shuffle);
		}
		void RepeatCBCheckedChanged(object sender, EventArgs e)
		{
			Repeat = repeatCB.Checked;
			Settings.SetValue<bool>("Repeat", Repeat);
		}
		
		#endregion
		
		#region Progress bar and images
		
		public void RefreshPosBar() {
			if (progressPB.InvokeRequired)
				progressPB.Invoke(new MethodInvoker(progressPB.Refresh));
          	else
          		progressPB.Refresh();
		}
		
		public static void RefreshImage() {
			((MainForm)Application.OpenForms["MainForm"]).RefreshPosBar();
		}
		
		void RefreshButtonImage() {
			Action refresh = () => {
				playPauseB.BackgroundImage = CurrentStatus == Status.Playing ?
					Resources.pause : Resources.play;
				
				if (musicLB.SelectedIndex < 0)
					return;
				songNameL.Text = musicLB.SelectedItem.ToString();
			};
			
			if (InvokeRequired)
				Invoke(new MethodInvoker(refresh));
			else
				refresh();
		}
		
		void ProgressPBPaint(object sender, PaintEventArgs e)
		{
			int hgt = ProgressImage.Height;
			int wdt = ProgressImage.Width;
			int vol = (int)(GetSongPos() * (float)wdt);
			Bitmap bmp = ProgressImage.Clone(new Rectangle(0, 0, vol > 0 ? vol : 1, hgt), PixelFormat.Format32bppArgb);

			//e.Graphics.DrawImage(Resources.emptyprogress, 0, 0);
			e.Graphics.DrawImage(bmp, 0, 0);
		}
		
		#endregion
		
		#region Image utils
		
		public static Bitmap ResizeImage(Image img, int width, int height) {
			var newImage = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(newImage))
			{
			    gr.SmoothingMode = SmoothingMode.HighQuality;
			    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
			    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
			    gr.DrawImage(img, new Rectangle(0, 0, width, height));
			    
			}
			return newImage;
		}
		
		#endregion
		
		#region Closing
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			StopAndDispose();
			HookManager.KeyUp -= HookManager_KeyUp;
		}
		
		void MusicLBKeyDown(object sender, KeyEventArgs e)
		{
			if (musicLB.SelectedIndex < 0)
				return;
			
			if (e.KeyCode == Keys.Delete)
				Delete(musicLB.SelectedItem.ToString());
		}
		
		#endregion
		
		#if DEBUG
		
		public static void log(params object[] msg) {
			System.Diagnostics.Debug.WriteLine(String.Join(", ", msg));
		}
		
		#endif
	}
}