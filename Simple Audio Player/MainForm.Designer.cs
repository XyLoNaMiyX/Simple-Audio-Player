namespace Simple_Audio_Player
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label titleL;
		private System.Windows.Forms.Button closeB;
		ListBoxScrollLeft musicLB;
		private System.Windows.Forms.Button addB;
		private System.Windows.Forms.OpenFileDialog ofd;
		private System.Windows.Forms.Button settingsB;
		private System.Windows.Forms.Button clearB;
		private System.Windows.Forms.Label listeningToL;
		private System.Windows.Forms.Label songNameL;
		private System.Windows.Forms.PictureBox progressPB;
		private System.Windows.Forms.Button playPauseB;
		private System.Windows.Forms.Button nextB;
		private System.Windows.Forms.Button stopB;
		private System.Windows.Forms.Button prevB;
		private System.Windows.Forms.CheckBox repeatCB;
		private System.Windows.Forms.CheckBox shuffleCB;
		private System.Windows.Forms.Button minimizeB;
		private System.Windows.Forms.Button minimizeTrayB;
		private System.Windows.Forms.NotifyIcon trayNI;
		private System.Windows.Forms.Button searchB;
		private System.Windows.Forms.TextBox searchTB;
		private System.Windows.Forms.Button addFolderB;
		private System.Windows.Forms.FolderBrowserDialog fbd;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.titleL = new System.Windows.Forms.Label();
			this.closeB = new System.Windows.Forms.Button();
			this.musicLB = new ListBoxScrollLeft();
			this.addB = new System.Windows.Forms.Button();
			this.ofd = new System.Windows.Forms.OpenFileDialog();
			this.settingsB = new System.Windows.Forms.Button();
			this.clearB = new System.Windows.Forms.Button();
			this.listeningToL = new System.Windows.Forms.Label();
			this.songNameL = new System.Windows.Forms.Label();
			this.progressPB = new System.Windows.Forms.PictureBox();
			this.playPauseB = new System.Windows.Forms.Button();
			this.nextB = new System.Windows.Forms.Button();
			this.stopB = new System.Windows.Forms.Button();
			this.prevB = new System.Windows.Forms.Button();
			this.repeatCB = new System.Windows.Forms.CheckBox();
			this.shuffleCB = new System.Windows.Forms.CheckBox();
			this.minimizeB = new System.Windows.Forms.Button();
			this.minimizeTrayB = new System.Windows.Forms.Button();
			this.trayNI = new System.Windows.Forms.NotifyIcon(this.components);
			this.searchB = new System.Windows.Forms.Button();
			this.searchTB = new System.Windows.Forms.TextBox();
			this.addFolderB = new System.Windows.Forms.Button();
			this.fbd = new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.progressPB)).BeginInit();
			this.SuspendLayout();
			// 
			// titleL
			// 
			this.titleL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.titleL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleL.Location = new System.Drawing.Point(0, 0);
			this.titleL.Name = "titleL";
			this.titleL.Size = new System.Drawing.Size(640, 36);
			this.titleL.TabIndex = 0;
			this.titleL.Text = "Simple Audio Player";
			this.titleL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.titleL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleLMouseDown);
			// 
			// closeB
			// 
			this.closeB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.closeB.BackgroundImage = global::Simple_Audio_Player.Resources.cross;
			this.closeB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.closeB.FlatAppearance.BorderSize = 0;
			this.closeB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.closeB.Location = new System.Drawing.Point(599, 0);
			this.closeB.Name = "closeB";
			this.closeB.Size = new System.Drawing.Size(40, 25);
			this.closeB.TabIndex = 3;
			this.closeB.TabStop = false;
			this.closeB.UseVisualStyleBackColor = false;
			this.closeB.Click += new System.EventHandler(this.CloseBClick);
			// 
			// musicLB
			// 
			this.musicLB.AllowDrop = true;
			this.musicLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.musicLB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.musicLB.FormattingEnabled = true;
			this.musicLB.Location = new System.Drawing.Point(0, 36);
			this.musicLB.Name = "musicLB";
			this.musicLB.Size = new System.Drawing.Size(192, 325);
			this.musicLB.TabIndex = 4;
			this.musicLB.DragDrop += new System.Windows.Forms.DragEventHandler(this.MusicLBDragDrop);
			this.musicLB.DragEnter += new System.Windows.Forms.DragEventHandler(this.MusicLBDragEnter);
			this.musicLB.DoubleClick += new System.EventHandler(this.MusicLBDoubleClick);
			this.musicLB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MusicLBKeyDown);
			// 
			// addB
			// 
			this.addB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.addB.FlatAppearance.BorderSize = 0;
			this.addB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addB.Location = new System.Drawing.Point(90, 365);
			this.addB.Name = "addB";
			this.addB.Size = new System.Drawing.Size(69, 23);
			this.addB.TabIndex = 5;
			this.addB.TabStop = false;
			this.addB.Text = "Añadir música";
			this.addB.UseVisualStyleBackColor = false;
			this.addB.Click += new System.EventHandler(this.AddBClick);
			// 
			// ofd
			// 
			this.ofd.Filter = "All supported files|*.wav;*.wave;*.mp3;*.flac;*.m4a;*.m4b;*.m4p;*.m4v;*.m4r;*.3gp" +
	";*.mp4;*.aac;*.ac3;*.wav;*.raw;*.pcm";
			this.ofd.Multiselect = true;
			// 
			// settingsB
			// 
			this.settingsB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
			this.settingsB.BackgroundImage = global::Simple_Audio_Player.Resources.settings_small;
			this.settingsB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.settingsB.FlatAppearance.BorderSize = 0;
			this.settingsB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.settingsB.Location = new System.Drawing.Point(0, 0);
			this.settingsB.Name = "settingsB";
			this.settingsB.Size = new System.Drawing.Size(36, 36);
			this.settingsB.TabIndex = 6;
			this.settingsB.TabStop = false;
			this.settingsB.UseVisualStyleBackColor = false;
			this.settingsB.Click += new System.EventHandler(this.SettingsBClick);
			// 
			// clearB
			// 
			this.clearB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clearB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.clearB.FlatAppearance.BorderSize = 0;
			this.clearB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.clearB.Location = new System.Drawing.Point(12, 365);
			this.clearB.Name = "clearB";
			this.clearB.Size = new System.Drawing.Size(72, 23);
			this.clearB.TabIndex = 7;
			this.clearB.TabStop = false;
			this.clearB.Text = "Limpiar lista";
			this.clearB.UseVisualStyleBackColor = false;
			this.clearB.Click += new System.EventHandler(this.ClearBClick);
			// 
			// listeningToL
			// 
			this.listeningToL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.listeningToL.Location = new System.Drawing.Point(230, 64);
			this.listeningToL.Name = "listeningToL";
			this.listeningToL.Size = new System.Drawing.Size(143, 23);
			this.listeningToL.TabIndex = 8;
			this.listeningToL.Text = "Estás escuchando a:";
			// 
			// songNameL
			// 
			this.songNameL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.songNameL.Location = new System.Drawing.Point(254, 87);
			this.songNameL.Name = "songNameL";
			this.songNameL.Size = new System.Drawing.Size(320, 83);
			this.songNameL.TabIndex = 9;
			this.songNameL.Text = "¡nada todavía!";
			// 
			// progressPB
			// 
			this.progressPB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
			this.progressPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.progressPB.Location = new System.Drawing.Point(198, 338);
			this.progressPB.Name = "progressPB";
			this.progressPB.Size = new System.Drawing.Size(430, 50);
			this.progressPB.TabIndex = 10;
			this.progressPB.TabStop = false;
			this.progressPB.Paint += new System.Windows.Forms.PaintEventHandler(this.ProgressPBPaint);
			this.progressPB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProgressPBMouseClick);
			this.progressPB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProgressPBMouseMove);
			// 
			// playPauseB
			// 
			this.playPauseB.BackgroundImage = global::Simple_Audio_Player.Resources.play;
			this.playPauseB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.playPauseB.FlatAppearance.BorderSize = 0;
			this.playPauseB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.playPauseB.Location = new System.Drawing.Point(306, 284);
			this.playPauseB.Name = "playPauseB";
			this.playPauseB.Size = new System.Drawing.Size(48, 48);
			this.playPauseB.TabIndex = 11;
			this.playPauseB.TabStop = false;
			this.playPauseB.UseVisualStyleBackColor = true;
			this.playPauseB.Click += new System.EventHandler(this.PlayPauseBClick);
			// 
			// nextB
			// 
			this.nextB.BackgroundImage = global::Simple_Audio_Player.Resources.next;
			this.nextB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.nextB.FlatAppearance.BorderSize = 0;
			this.nextB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nextB.Location = new System.Drawing.Point(360, 284);
			this.nextB.Name = "nextB";
			this.nextB.Size = new System.Drawing.Size(48, 48);
			this.nextB.TabIndex = 12;
			this.nextB.TabStop = false;
			this.nextB.UseVisualStyleBackColor = true;
			this.nextB.Click += new System.EventHandler(this.NextBClick);
			// 
			// stopB
			// 
			this.stopB.BackgroundImage = global::Simple_Audio_Player.Resources.stop;
			this.stopB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.stopB.FlatAppearance.BorderSize = 0;
			this.stopB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.stopB.Location = new System.Drawing.Point(252, 284);
			this.stopB.Name = "stopB";
			this.stopB.Size = new System.Drawing.Size(48, 48);
			this.stopB.TabIndex = 13;
			this.stopB.TabStop = false;
			this.stopB.UseVisualStyleBackColor = true;
			this.stopB.Click += new System.EventHandler(this.StopBClick);
			// 
			// prevB
			// 
			this.prevB.BackgroundImage = global::Simple_Audio_Player.Resources.prev;
			this.prevB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.prevB.FlatAppearance.BorderSize = 0;
			this.prevB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.prevB.Location = new System.Drawing.Point(198, 284);
			this.prevB.Name = "prevB";
			this.prevB.Size = new System.Drawing.Size(48, 48);
			this.prevB.TabIndex = 14;
			this.prevB.TabStop = false;
			this.prevB.UseVisualStyleBackColor = true;
			this.prevB.Click += new System.EventHandler(this.PrevBClick);
			// 
			// repeatCB
			// 
			this.repeatCB.Appearance = System.Windows.Forms.Appearance.Button;
			this.repeatCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.repeatCB.BackgroundImage = global::Simple_Audio_Player.Resources.repeat;
			this.repeatCB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.repeatCB.FlatAppearance.BorderSize = 0;
			this.repeatCB.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.repeatCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.repeatCB.Location = new System.Drawing.Point(580, 284);
			this.repeatCB.Name = "repeatCB";
			this.repeatCB.Size = new System.Drawing.Size(48, 48);
			this.repeatCB.TabIndex = 15;
			this.repeatCB.TabStop = false;
			this.repeatCB.UseVisualStyleBackColor = false;
			this.repeatCB.CheckedChanged += new System.EventHandler(this.RepeatCBCheckedChanged);
			// 
			// shuffleCB
			// 
			this.shuffleCB.Appearance = System.Windows.Forms.Appearance.Button;
			this.shuffleCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.shuffleCB.BackgroundImage = global::Simple_Audio_Player.Resources.shuffle;
			this.shuffleCB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.shuffleCB.FlatAppearance.BorderSize = 0;
			this.shuffleCB.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.shuffleCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.shuffleCB.Location = new System.Drawing.Point(526, 284);
			this.shuffleCB.Name = "shuffleCB";
			this.shuffleCB.Size = new System.Drawing.Size(48, 48);
			this.shuffleCB.TabIndex = 16;
			this.shuffleCB.TabStop = false;
			this.shuffleCB.UseVisualStyleBackColor = false;
			this.shuffleCB.CheckedChanged += new System.EventHandler(this.ShuffleCBCheckedChanged);
			// 
			// minimizeB
			// 
			this.minimizeB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.minimizeB.BackgroundImage = global::Simple_Audio_Player.Resources.minimize_tray;
			this.minimizeB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.minimizeB.FlatAppearance.BorderSize = 0;
			this.minimizeB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.minimizeB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minimizeB.Location = new System.Drawing.Point(519, 0);
			this.minimizeB.Name = "minimizeB";
			this.minimizeB.Size = new System.Drawing.Size(40, 25);
			this.minimizeB.TabIndex = 17;
			this.minimizeB.TabStop = false;
			this.minimizeB.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.minimizeB.UseVisualStyleBackColor = false;
			this.minimizeB.Click += new System.EventHandler(this.MinimizeBClick);
			// 
			// minimizeTrayB
			// 
			this.minimizeTrayB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.minimizeTrayB.BackgroundImage = global::Simple_Audio_Player.Resources.minimize;
			this.minimizeTrayB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.minimizeTrayB.FlatAppearance.BorderSize = 0;
			this.minimizeTrayB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.minimizeTrayB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minimizeTrayB.Location = new System.Drawing.Point(559, 0);
			this.minimizeTrayB.Name = "minimizeTrayB";
			this.minimizeTrayB.Size = new System.Drawing.Size(40, 25);
			this.minimizeTrayB.TabIndex = 18;
			this.minimizeTrayB.TabStop = false;
			this.minimizeTrayB.UseVisualStyleBackColor = false;
			this.minimizeTrayB.Click += new System.EventHandler(this.MinimizeTrayBClick);
			// 
			// trayNI
			// 
			this.trayNI.Icon = global::Simple_Audio_Player.Resources.simple;
			this.trayNI.Text = "Simple Audio Player";
			this.trayNI.Click += new System.EventHandler(this.TrayNIClick);
			// 
			// searchB
			// 
			this.searchB.BackColor = System.Drawing.Color.White;
			this.searchB.BackgroundImage = global::Simple_Audio_Player.Resources.magnifying_glass;
			this.searchB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.searchB.FlatAppearance.BorderSize = 0;
			this.searchB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.searchB.Location = new System.Drawing.Point(192, 36);
			this.searchB.Name = "searchB";
			this.searchB.Size = new System.Drawing.Size(24, 24);
			this.searchB.TabIndex = 20;
			this.searchB.TabStop = false;
			this.searchB.UseVisualStyleBackColor = false;
			this.searchB.Click += new System.EventHandler(this.SearchBClick);
			// 
			// searchTB
			// 
			this.searchTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.searchTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.searchTB.Location = new System.Drawing.Point(216, 36);
			this.searchTB.Name = "searchTB";
			this.searchTB.Size = new System.Drawing.Size(200, 24);
			this.searchTB.TabIndex = 21;
			this.searchTB.TextChanged += new System.EventHandler(this.SearchTBTextChanged);
			// 
			// addFolderB
			// 
			this.addFolderB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addFolderB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.addFolderB.FlatAppearance.BorderSize = 0;
			this.addFolderB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addFolderB.Location = new System.Drawing.Point(165, 365);
			this.addFolderB.Name = "addFolderB";
			this.addFolderB.Size = new System.Drawing.Size(23, 23);
			this.addFolderB.TabIndex = 22;
			this.addFolderB.TabStop = false;
			this.addFolderB.Text = "+";
			this.addFolderB.UseVisualStyleBackColor = false;
			this.addFolderB.Click += new System.EventHandler(this.AddFolderBClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(640, 400);
			this.Controls.Add(this.addFolderB);
			this.Controls.Add(this.searchTB);
			this.Controls.Add(this.searchB);
			this.Controls.Add(this.minimizeTrayB);
			this.Controls.Add(this.minimizeB);
			this.Controls.Add(this.shuffleCB);
			this.Controls.Add(this.repeatCB);
			this.Controls.Add(this.prevB);
			this.Controls.Add(this.stopB);
			this.Controls.Add(this.nextB);
			this.Controls.Add(this.playPauseB);
			this.Controls.Add(this.progressPB);
			this.Controls.Add(this.songNameL);
			this.Controls.Add(this.listeningToL);
			this.Controls.Add(this.clearB);
			this.Controls.Add(this.settingsB);
			this.Controls.Add(this.addB);
			this.Controls.Add(this.musicLB);
			this.Controls.Add(this.closeB);
			this.Controls.Add(this.titleL);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = global::Simple_Audio_Player.Resources.simple;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Simple Audio Player";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			((System.ComponentModel.ISupportInitialize)(this.progressPB)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
