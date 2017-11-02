namespace Simple_Audio_Player
{
	partial class SettingsForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label titleL;
		private System.Windows.Forms.Button closeB;
		private System.Windows.Forms.CheckBox animateCB;
		private System.Windows.Forms.Label fromL;
		private System.Windows.Forms.Label toL;
		private System.Windows.Forms.Panel fromP;
		private System.Windows.Forms.Panel toP;
		private System.Windows.Forms.ColorDialog cd;
		private System.Windows.Forms.Label styleL;
		private System.Windows.Forms.Label spamL;
		private System.Windows.Forms.LinkLabel contactLL;
		private System.Windows.Forms.Label toastL;
		private System.Windows.Forms.CheckBox toastsCB;
		private System.Windows.Forms.GroupBox toastsLoc;
		private System.Windows.Forms.RadioButton bottomRightRB;
		private System.Windows.Forms.RadioButton bottomCenterRB;
		private System.Windows.Forms.RadioButton bottomLeftRB;
		private System.Windows.Forms.RadioButton topRightRB;
		private System.Windows.Forms.RadioButton topCenterRB;
		private System.Windows.Forms.RadioButton topLeftRB;
		private System.Windows.Forms.Button testToastB;
		private System.Windows.Forms.ComboBox styleCB;
		private System.Windows.Forms.Button styleB;
		private System.Windows.Forms.LinkLabel whatsStyleLL;
		private System.Windows.Forms.PictureBox logoPB;
		private System.Windows.Forms.Button updatesB;
		private System.Windows.Forms.RadioButton simpleSearchRB;
		private System.Windows.Forms.RadioButton advSearchRB;
		
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
			this.titleL = new System.Windows.Forms.Label();
			this.closeB = new System.Windows.Forms.Button();
			this.animateCB = new System.Windows.Forms.CheckBox();
			this.fromL = new System.Windows.Forms.Label();
			this.toL = new System.Windows.Forms.Label();
			this.fromP = new System.Windows.Forms.Panel();
			this.toP = new System.Windows.Forms.Panel();
			this.cd = new System.Windows.Forms.ColorDialog();
			this.styleL = new System.Windows.Forms.Label();
			this.spamL = new System.Windows.Forms.Label();
			this.contactLL = new System.Windows.Forms.LinkLabel();
			this.toastL = new System.Windows.Forms.Label();
			this.toastsCB = new System.Windows.Forms.CheckBox();
			this.toastsLoc = new System.Windows.Forms.GroupBox();
			this.bottomRightRB = new System.Windows.Forms.RadioButton();
			this.bottomCenterRB = new System.Windows.Forms.RadioButton();
			this.bottomLeftRB = new System.Windows.Forms.RadioButton();
			this.topRightRB = new System.Windows.Forms.RadioButton();
			this.topCenterRB = new System.Windows.Forms.RadioButton();
			this.topLeftRB = new System.Windows.Forms.RadioButton();
			this.testToastB = new System.Windows.Forms.Button();
			this.styleCB = new System.Windows.Forms.ComboBox();
			this.styleB = new System.Windows.Forms.Button();
			this.whatsStyleLL = new System.Windows.Forms.LinkLabel();
			this.logoPB = new System.Windows.Forms.PictureBox();
			this.updatesB = new System.Windows.Forms.Button();
			this.simpleSearchRB = new System.Windows.Forms.RadioButton();
			this.advSearchRB = new System.Windows.Forms.RadioButton();
			this.toastsLoc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPB)).BeginInit();
			this.SuspendLayout();
			// 
			// titleL
			// 
			this.titleL.BackColor = System.Drawing.Color.Silver;
			this.titleL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleL.Location = new System.Drawing.Point(0, 0);
			this.titleL.Name = "titleL";
			this.titleL.Size = new System.Drawing.Size(640, 36);
			this.titleL.TabIndex = 0;
			this.titleL.Text = "Ajustes";
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
			this.closeB.Size = new System.Drawing.Size(41, 25);
			this.closeB.TabIndex = 3;
			this.closeB.TabStop = false;
			this.closeB.UseVisualStyleBackColor = false;
			this.closeB.Click += new System.EventHandler(this.CloseBClick);
			// 
			// animateCB
			// 
			this.animateCB.Appearance = System.Windows.Forms.Appearance.Button;
			this.animateCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.animateCB.Checked = true;
			this.animateCB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.animateCB.FlatAppearance.BorderSize = 0;
			this.animateCB.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.animateCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.animateCB.Location = new System.Drawing.Point(12, 39);
			this.animateCB.Name = "animateCB";
			this.animateCB.Size = new System.Drawing.Size(215, 24);
			this.animateCB.TabIndex = 4;
			this.animateCB.Text = "¿Animar color de la barra de título?";
			this.animateCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.animateCB.UseVisualStyleBackColor = false;
			this.animateCB.CheckedChanged += new System.EventHandler(this.AnimateCBCheckedChanged);
			// 
			// fromL
			// 
			this.fromL.Location = new System.Drawing.Point(12, 79);
			this.fromL.Name = "fromL";
			this.fromL.Size = new System.Drawing.Size(75, 23);
			this.fromL.TabIndex = 5;
			this.fromL.Text = "Del color:";
			// 
			// toL
			// 
			this.toL.Location = new System.Drawing.Point(163, 79);
			this.toL.Name = "toL";
			this.toL.Size = new System.Drawing.Size(64, 23);
			this.toL.TabIndex = 6;
			this.toL.Text = "al color:";
			// 
			// fromP
			// 
			this.fromP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.fromP.Location = new System.Drawing.Point(12, 105);
			this.fromP.Name = "fromP";
			this.fromP.Size = new System.Drawing.Size(64, 64);
			this.fromP.TabIndex = 7;
			this.fromP.Click += new System.EventHandler(this.FromPClick);
			// 
			// toP
			// 
			this.toP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toP.Location = new System.Drawing.Point(163, 105);
			this.toP.Name = "toP";
			this.toP.Size = new System.Drawing.Size(64, 64);
			this.toP.TabIndex = 8;
			this.toP.Click += new System.EventHandler(this.ToPClick);
			// 
			// cd
			// 
			this.cd.AnyColor = true;
			this.cd.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
			this.cd.FullOpen = true;
			// 
			// styleL
			// 
			this.styleL.Location = new System.Drawing.Point(371, 62);
			this.styleL.Name = "styleL";
			this.styleL.Size = new System.Drawing.Size(154, 23);
			this.styleL.TabIndex = 9;
			this.styleL.Text = "Estilo de la barra:";
			// 
			// spamL
			// 
			this.spamL.Location = new System.Drawing.Point(12, 377);
			this.spamL.Name = "spamL";
			this.spamL.Size = new System.Drawing.Size(518, 14);
			this.spamL.TabIndex = 10;
			this.spamL.Text = "Simple Audio Player, creado por Lonami - LonamiWebs © 2015. Para soporte, por fav" +
	"or visite este link:";
			// 
			// contactLL
			// 
			this.contactLL.Location = new System.Drawing.Point(536, 377);
			this.contactLL.Name = "contactLL";
			this.contactLL.Size = new System.Drawing.Size(92, 14);
			this.contactLL.TabIndex = 11;
			this.contactLL.TabStop = true;
			this.contactLL.Text = "LonamiWebs";
			this.contactLL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ContactLLinkClicked);
			// 
			// toastL
			// 
			this.toastL.Location = new System.Drawing.Point(299, 225);
			this.toastL.Name = "toastL";
			this.toastL.Size = new System.Drawing.Size(125, 23);
			this.toastL.TabIndex = 12;
			this.toastL.Text = "¿Mostrar tostadas?";
			this.toastL.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// toastsCB
			// 
			this.toastsCB.Appearance = System.Windows.Forms.Appearance.Button;
			this.toastsCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.toastsCB.BackgroundImage = global::Simple_Audio_Player.Resources.toast_nope;
			this.toastsCB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.toastsCB.FlatAppearance.BorderSize = 0;
			this.toastsCB.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.toastsCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.toastsCB.Location = new System.Drawing.Point(354, 251);
			this.toastsCB.Name = "toastsCB";
			this.toastsCB.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.toastsCB.Size = new System.Drawing.Size(64, 64);
			this.toastsCB.TabIndex = 13;
			this.toastsCB.UseVisualStyleBackColor = false;
			this.toastsCB.CheckedChanged += new System.EventHandler(this.ToastsCBCheckedChanged);
			// 
			// toastsLoc
			// 
			this.toastsLoc.Controls.Add(this.bottomRightRB);
			this.toastsLoc.Controls.Add(this.bottomCenterRB);
			this.toastsLoc.Controls.Add(this.bottomLeftRB);
			this.toastsLoc.Controls.Add(this.topRightRB);
			this.toastsLoc.Controls.Add(this.topCenterRB);
			this.toastsLoc.Controls.Add(this.topLeftRB);
			this.toastsLoc.Enabled = false;
			this.toastsLoc.Location = new System.Drawing.Point(424, 225);
			this.toastsLoc.Name = "toastsLoc";
			this.toastsLoc.Size = new System.Drawing.Size(204, 100);
			this.toastsLoc.TabIndex = 14;
			this.toastsLoc.TabStop = false;
			this.toastsLoc.Text = "Posición de las tostadas";
			// 
			// bottomRightRB
			// 
			this.bottomRightRB.Location = new System.Drawing.Point(175, 72);
			this.bottomRightRB.Name = "bottomRightRB";
			this.bottomRightRB.Size = new System.Drawing.Size(18, 18);
			this.bottomRightRB.TabIndex = 20;
			this.bottomRightRB.UseVisualStyleBackColor = true;
			this.bottomRightRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// bottomCenterRB
			// 
			this.bottomCenterRB.Location = new System.Drawing.Point(88, 72);
			this.bottomCenterRB.Name = "bottomCenterRB";
			this.bottomCenterRB.Size = new System.Drawing.Size(18, 18);
			this.bottomCenterRB.TabIndex = 19;
			this.bottomCenterRB.UseVisualStyleBackColor = true;
			this.bottomCenterRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// bottomLeftRB
			// 
			this.bottomLeftRB.Location = new System.Drawing.Point(6, 72);
			this.bottomLeftRB.Name = "bottomLeftRB";
			this.bottomLeftRB.Size = new System.Drawing.Size(18, 18);
			this.bottomLeftRB.TabIndex = 18;
			this.bottomLeftRB.UseVisualStyleBackColor = true;
			this.bottomLeftRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// topRightRB
			// 
			this.topRightRB.Location = new System.Drawing.Point(175, 19);
			this.topRightRB.Name = "topRightRB";
			this.topRightRB.Size = new System.Drawing.Size(18, 18);
			this.topRightRB.TabIndex = 17;
			this.topRightRB.UseVisualStyleBackColor = true;
			this.topRightRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// topCenterRB
			// 
			this.topCenterRB.Location = new System.Drawing.Point(88, 19);
			this.topCenterRB.Name = "topCenterRB";
			this.topCenterRB.Size = new System.Drawing.Size(18, 18);
			this.topCenterRB.TabIndex = 16;
			this.topCenterRB.UseVisualStyleBackColor = true;
			this.topCenterRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// topLeftRB
			// 
			this.topLeftRB.Checked = true;
			this.topLeftRB.Location = new System.Drawing.Point(6, 19);
			this.topLeftRB.Name = "topLeftRB";
			this.topLeftRB.Size = new System.Drawing.Size(18, 18);
			this.topLeftRB.TabIndex = 15;
			this.topLeftRB.TabStop = true;
			this.topLeftRB.UseVisualStyleBackColor = true;
			this.topLeftRB.Click += new System.EventHandler(this.TopLeftRBClick);
			// 
			// testToastB
			// 
			this.testToastB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.testToastB.FlatAppearance.BorderSize = 0;
			this.testToastB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.testToastB.Location = new System.Drawing.Point(354, 331);
			this.testToastB.Name = "testToastB";
			this.testToastB.Size = new System.Drawing.Size(274, 23);
			this.testToastB.TabIndex = 15;
			this.testToastB.Text = "¡Probar tostada!";
			this.testToastB.UseVisualStyleBackColor = false;
			this.testToastB.Click += new System.EventHandler(this.TestToastBClick);
			// 
			// styleCB
			// 
			this.styleCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.styleCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.styleCB.FormattingEnabled = true;
			this.styleCB.Location = new System.Drawing.Point(496, 59);
			this.styleCB.Name = "styleCB";
			this.styleCB.Size = new System.Drawing.Size(121, 21);
			this.styleCB.TabIndex = 16;
			this.styleCB.SelectedIndexChanged += new System.EventHandler(this.StyleCBSelectedIndexChanged);
			// 
			// styleB
			// 
			this.styleB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.styleB.FlatAppearance.BorderSize = 0;
			this.styleB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.styleB.Location = new System.Drawing.Point(462, 88);
			this.styleB.Name = "styleB";
			this.styleB.Size = new System.Drawing.Size(155, 23);
			this.styleB.TabIndex = 17;
			this.styleB.Text = "Abrir carpeta de estilos";
			this.styleB.UseVisualStyleBackColor = false;
			this.styleB.Click += new System.EventHandler(this.StyleBClick);
			// 
			// whatsStyleLL
			// 
			this.whatsStyleLL.Location = new System.Drawing.Point(371, 93);
			this.whatsStyleLL.Name = "whatsStyleLL";
			this.whatsStyleLL.Size = new System.Drawing.Size(85, 24);
			this.whatsStyleLL.TabIndex = 18;
			this.whatsStyleLL.TabStop = true;
			this.whatsStyleLL.Text = "¿Qué es esto?";
			this.whatsStyleLL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WhatsStyleLLLinkClicked);
			// 
			// logoPB
			// 
			this.logoPB.Image = global::Simple_Audio_Player.Resources.simple_1;
			this.logoPB.Location = new System.Drawing.Point(12, 204);
			this.logoPB.Name = "logoPB";
			this.logoPB.Size = new System.Drawing.Size(150, 150);
			this.logoPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoPB.TabIndex = 19;
			this.logoPB.TabStop = false;
			// 
			// updatesB
			// 
			this.updatesB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.updatesB.FlatAppearance.BorderSize = 0;
			this.updatesB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.updatesB.Location = new System.Drawing.Point(168, 256);
			this.updatesB.Name = "updatesB";
			this.updatesB.Size = new System.Drawing.Size(100, 100);
			this.updatesB.TabIndex = 20;
			this.updatesB.Text = "Comprobar\r\nactualizaciones";
			this.updatesB.UseVisualStyleBackColor = false;
			this.updatesB.Click += new System.EventHandler(this.UpdatesBClick);
			// 
			// simpleSearchRB
			// 
			this.simpleSearchRB.Checked = true;
			this.simpleSearchRB.Location = new System.Drawing.Point(424, 145);
			this.simpleSearchRB.Name = "simpleSearchRB";
			this.simpleSearchRB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.simpleSearchRB.Size = new System.Drawing.Size(193, 24);
			this.simpleSearchRB.TabIndex = 21;
			this.simpleSearchRB.TabStop = true;
			this.simpleSearchRB.Text = "Modo de búsqueda simple";
			this.simpleSearchRB.UseVisualStyleBackColor = true;
			this.simpleSearchRB.Click += new System.EventHandler(this.SearchRBClick);
			// 
			// advSearchRB
			// 
			this.advSearchRB.Location = new System.Drawing.Point(424, 175);
			this.advSearchRB.Name = "advSearchRB";
			this.advSearchRB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.advSearchRB.Size = new System.Drawing.Size(193, 24);
			this.advSearchRB.TabIndex = 22;
			this.advSearchRB.TabStop = true;
			this.advSearchRB.Text = "Modo de búsqueda avanzado";
			this.advSearchRB.UseVisualStyleBackColor = true;
			this.advSearchRB.Click += new System.EventHandler(this.SearchRBClick);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(640, 400);
			this.Controls.Add(this.advSearchRB);
			this.Controls.Add(this.simpleSearchRB);
			this.Controls.Add(this.updatesB);
			this.Controls.Add(this.logoPB);
			this.Controls.Add(this.whatsStyleLL);
			this.Controls.Add(this.styleB);
			this.Controls.Add(this.styleCB);
			this.Controls.Add(this.testToastB);
			this.Controls.Add(this.toastsLoc);
			this.Controls.Add(this.toastsCB);
			this.Controls.Add(this.toastL);
			this.Controls.Add(this.contactLL);
			this.Controls.Add(this.spamL);
			this.Controls.Add(this.styleL);
			this.Controls.Add(this.toP);
			this.Controls.Add(this.fromP);
			this.Controls.Add(this.toL);
			this.Controls.Add(this.fromL);
			this.Controls.Add(this.animateCB);
			this.Controls.Add(this.closeB);
			this.Controls.Add(this.titleL);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = global::Simple_Audio_Player.Resources.simple;
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.toastsLoc.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.logoPB)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
