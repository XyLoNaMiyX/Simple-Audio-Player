
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Simple_Audio_Player
{
	public partial class SettingsForm : Form
	{
		
		static int[] SafeColors = new int[] {
			6711039, 6724044, 6737049, 10053375,
			10079385, 13395711, 16737894, 12345678,
			16750950, 16777062, 6750207, 13408614,
			10092390, 10053273, 16764108, 6737151
		};
		
		readonly static string StylesLocation = MainForm.AppData + "styles\\";
		
		FileSystemWatcher fsw;
		
		#region Init
		
		public SettingsForm()
		{
			InitializeComponent();
			
			animateCB.Checked = Settings.GetValue<bool>("AnimateTitle");
			toastsCB.Checked = Settings.GetValue<bool>("Toast");
			SetLocation((Toast.ToastLocation)Settings.GetValue<int>("ToastLocation"));
			RefreshStyles();
			
			switch ((MainForm.SearchMode)Settings.GetValue<int>("SearchMode"))
			{
				case MainForm.SearchMode.Simple:
				default:
					simpleSearchRB.Checked = true;
					break;
				case MainForm.SearchMode.Advanced:
					advSearchRB.Checked = true;
					break;
			}
		
			styleCB.SelectedItem = Path.GetFileName(Settings.GetValue<string>("Style"));
			
			var title = new Font(MainForm.BeautyFont, titleL.Font.Size);
			var normal = new Font(MainForm.BeautyFont, animateCB.Font.Size);
			
			titleL.Font = title;
			animateCB.Font = fromL.Font = toL.Font = styleL.Font = spamL.Font = contactLL.Font =
				toastL.Font = toastsLoc.Font = testToastB.Font = whatsStyleLL.Font =
				styleB.Font = updatesB.Font = simpleSearchRB.Font = advSearchRB.Font = normal;
			
			fromP.BackColor = Settings.GetValue<Color>("FromColor");
			toP.BackColor = Settings.GetValue<Color>("ToColor");
			
			cd.CustomColors = SafeColors;
		
			fsw = new FileSystemWatcher(StylesLocation, "*.png");
			fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
				| NotifyFilters.FileName | NotifyFilters.DirectoryName;
			fsw.Changed += StyleAdded;
			fsw.EnableRaisingEvents = true;
		}
		
		
		#endregion
		
		#region Styles
		
		void RefreshStyles()
		{
			if (styleCB.InvokeRequired)
				styleCB.Invoke(new MethodInvoker(_RefreshStyles));
			else
				_RefreshStyles();
		}
		
		void _RefreshStyles() {
			var litem = styleCB.SelectedItem;
			styleCB.BeginUpdate();
			styleCB.Items.Clear();
			styleCB.Items.Add("Default");
			
			Directory.CreateDirectory(StylesLocation);
			foreach (var file in Directory.EnumerateFiles(StylesLocation).Where(f => f.EndsWith(".png")))
				styleCB.Items.Add(Path.GetFileName(file));
			
			styleCB.EndUpdate();
			styleCB.SelectedItem = litem;
		}
		
		void StyleAdded(object sender, EventArgs e)
		{
			RefreshStyles();
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

		void CloseBClick(object sender, EventArgs e)
		{
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
		
		#region Settings
		
		#region Title color
		
		void AnimateCBCheckedChanged(object sender, EventArgs e)
		{	
			Settings.SetValue<bool>("AnimateTitle", (toL.Enabled = toP.Enabled = animateCB.Checked));
		}
		
		void FromPClick(object sender, EventArgs e)
		{
			if (cd.ShowDialog() == DialogResult.OK) {
				Settings.SetValue<Color>("FromColor", (fromP.BackColor = cd.Color));
				MainForm.NeedToRegen = true;
			}
		}
		
		void ToPClick(object sender, EventArgs e)
		{
			if (cd.ShowDialog() == DialogResult.OK) {
				Settings.SetValue<Color>("ToColor", (toP.BackColor = cd.Color));
				MainForm.NeedToRegen = true;
			}
		}
		
		#endregion
		
		#region Toast
		
		void ToastsCBCheckedChanged(object sender, EventArgs e)
		{
			toastsCB.BackgroundImage = toastsCB.Checked ?
				Resources.toast_yup : Resources.toast_nope;
			toastsLoc.Enabled = toastsCB.Checked;
			
			Settings.SetValue<bool>("Toast", toastsCB.Checked);
		}
		
		void TestToastBClick(object sender, EventArgs e)
		{
			new Toast(GetLocation(), "Ahora escuchando a", "Tu canción favorita :)");
		}
		
		Toast.ToastLocation GetLocation() {
			if (topLeftRB.Checked)
				return Toast.ToastLocation.TopLeft;
			if (topCenterRB.Checked)
				return Toast.ToastLocation.TopCenter;
			if (topRightRB.Checked)
				return Toast.ToastLocation.TopRight;
			if (bottomLeftRB.Checked)
				return Toast.ToastLocation.BottomLeft;
			if (bottomCenterRB.Checked)
				return Toast.ToastLocation.BottomCenter;
			
			return Toast.ToastLocation.BottomRight;
		}
		
		void SetLocation(Toast.ToastLocation location) {
			switch (location)
			{
				case Toast.ToastLocation.TopLeft:
					topLeftRB.Checked = true;
					break;
				case Toast.ToastLocation.TopCenter:
					topCenterRB.Checked = true;
					break;
				case Toast.ToastLocation.TopRight:
					topRightRB.Checked = true;
					break;
				case Toast.ToastLocation.BottomLeft:
					bottomLeftRB.Checked = true;
					break;
				case Toast.ToastLocation.BottomCenter:
					bottomCenterRB.Checked = true;
					break;
				case Toast.ToastLocation.BottomRight:
					bottomRightRB.Checked = true;
					break;
			}
		}
		
		void TopLeftRBClick(object sender, EventArgs e)
		{
			Settings.SetValue<int>("ToastLocation", (int)GetLocation());
		}
		
		#endregion
		
		#region Progress bar style
		
		void StyleCBSelectedIndexChanged(object sender, EventArgs e)
		{
			try {
				if (styleCB.SelectedIndex == 0) {
					Settings.SetValue<string>("Style", "Default");
					MainForm.ProgressImage = MainForm.ResizeImage(Resources.progress,
					                                              MainForm.ProgressWidth, MainForm.ProgressHeight);
				}
				else {
					string file = StylesLocation + styleCB.SelectedItem;
					Settings.SetValue<string>("Style", file);
					MainForm.ProgressImage = MainForm.ResizeImage(Image.FromFile(file),
					                                              MainForm.ProgressWidth, MainForm.ProgressHeight);
				}
				
				((MainForm)Application.OpenForms["MainForm"]).RefreshPosBar();
			} catch (NullReferenceException) { /* I have no idea about this exception, but it happens */ }
		}
		void StyleBClick(object sender, EventArgs e)
		{
			Process.Start(StylesLocation);
		}
		
		void WhatsStyleLLLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Resources.progress.Save(StylesLocation + "default.png", ImageFormat.Png);
			MessageBox.Show("Puedes cambiar el estilo de la barra de progreso poniendo imánges personalizadas en:\r\n"
                + StylesLocation + "\r\n\r\nSe ha creado un archivo por defecto. ¡Modifícalo como quieras!",
               "Sobre los estilos personalizados", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		#endregion
		
		#endregion
	
		#region Contact
		
		void ContactLLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://lonamiwebs.github.io/contacto");
		}
		
		#endregion
		
		#region Updates
		
		readonly Uri UpdatesUri = new Uri("http://lonamiwebs.github.io/_DOWNLOADS/checkupdates.php?q=sap");
		readonly Uri GetFileUrl = new Uri("http://lonamiwebs.github.io/_DOWNLOADS/getfileurl.php?q=sapgz"); // TODO + GetArchitecture?
		readonly string UpdaterLoc = Path.Combine(Path.GetTempPath(), "updater.exe");
		
		const string LWA = "permission=lwAccess";
		const string TmpSAP = "TMP_Simple Audio Player.exe";
		const string TmpGzSAP = "TMP_Simple Audio Player.gz";
		
		
		void UpdatesBClick(object sender, EventArgs e) { CheckUpdates(); }
		
		void CheckUpdates() {
			updatesB.Enabled = false;
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UploadStringCompleted += wc_UploadStringCompleted;

                try { wc.UploadStringAsync(UpdatesUri, LWA); } catch { }
            }
		}

		void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
		{
			updatesB.Enabled = true;
			if (e.Error != null) {
				if (MessageBox.Show("No se pudo comprobar actualizaciones en este momento. Por favor inténtelo más tarde", "Error",
				                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
					updatesB.PerformClick();
				return;
			}
			
            int newVersion = Int32.Parse(e.Result);
            if (newVersion > GetVersionFromFile(Application.ExecutablePath))
            {
				if (MessageBox.Show("Una nueva versión de Simple Audio Player está disponible. ¿Quieres descargar e 'instalarla' automáticamente?",
            	                    "Se encontró una nueva versión",
				              MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            		
            		new Thread(DoUpdate) { Priority = ThreadPriority.Lowest }.Start();
			}
			else
				MessageBox.Show("Ya tienes la última versión de Simple Audio Player",
				                "Estás actualizad@", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
		}
		
		void DoUpdate()
		{
			using (var wc = new WebClient())
			{
				try {
					var url = wc.DownloadString(GetFileUrl);
					var fileUri = new Uri(url);
					
					wc.DownloadFile(fileUri, TmpGzSAP);
						
					var curLoc = Application.ExecutablePath;
					var tmpLoc = Path.Combine(Path.GetDirectoryName(curLoc), TmpSAP);
					var tmpGzLoc = Path.Combine(Path.GetDirectoryName(curLoc), TmpGzSAP);
					
					GZ.Decompress(tmpGzLoc, tmpLoc);
					
					File.Delete(tmpGzLoc);
					
					File.WriteAllBytes(UpdaterLoc, Resources.updater);
					Process.Start(UpdaterLoc, "\"" + curLoc + "\" \"" + tmpLoc + "\"");
					
					var f = (MainForm)Application.OpenForms["MainForm"];
					if (f != null)
					{
						f.StopAndDispose();
						GC.Collect();
					}
					
					Application.Exit();
					
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, ex.GetType().ToString());
					MessageBox.Show("Simple Audio Player no se pudo actualizar :(\r\nPuedes probar a intentarlo más tarde",
					                "Aww", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		
		#region Updating utils
		
		int GetVersionFromFile(string file) {
            var versionInfo = FileVersionInfo.GetVersionInfo(file);
            string version = versionInfo.ProductVersion;

            return Int32.Parse(version.Replace(".", ""));
        }

        string GetArchitecture(string file) {
            var assembly = AssemblyName.GetAssemblyName(file);
            string sassembly = assembly.ProcessorArchitecture.ToString().ToLower();

            if (sassembly.Contains("64"))
                return "64";

            if (sassembly.Contains("86"))
                return "86";
            
           	return "";

        }
		
		#endregion
		
		#endregion
		
		void SearchRBClick(object sender, EventArgs e)
		{
			if (simpleSearchRB.Checked)
				Settings.SetValue<int>("SearchMode", (int)MainForm.SearchMode.Simple);
			
			else if (advSearchRB.Checked)
				Settings.SetValue<int>("SearchMode", (int)MainForm.SearchMode.Advanced);
		}
	}
}
