using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

public class Toast : Form
{
	static Toast LastToast; // This ~slightly~ fix its, at least if too, too quick. If slow, it'd be better to use List<Toast>
	
	// Toasts won't gain focus with this (theorically)
	protected override bool ShowWithoutActivation
	{ get { return true; } }
	
	const int WS_EX_TOOLWINDOW = 0x00000080;
    const int WS_EX_TOPMOST = 0x00000008;
    
	readonly Stopwatch sw = new Stopwatch();
	
	const int MaxLength = 48;
    
	protected override CreateParams CreateParams
	{
	    get
	    {
	        var Params = base.CreateParams;
	        Params.ExStyle |= WS_EX_TOOLWINDOW; // Toasts won't show in Alt+TAB with this
          	Params.ExStyle |= WS_EX_TOPMOST; // Toasts won't gain focus (again)
	        return Params;
	    }
	}
	
	public enum ToastLocation
	{
		TopLeft = 0, TopCenter = 1, TopRight = 2,
		BottomLeft = 3, BottomCenter = 4, BottomRight = 5
	};
	
	Timer t;
	
	int Step, CurStep;
	const int Steps = 20;
	const int Speed = 10;
	const int Timeout = 2000;
	const int ToastMargin = 40;
	const int ToastWidth = 300;
	const int ToastHeight = 110;
	
	static int ScreenWidth { get { return Screen.PrimaryScreen.Bounds.Width; } }
	static int ScreenHeight { get { return Screen.PrimaryScreen.Bounds.Height; } }
	
	bool Showing = true, Forced;
	
	public Toast(string title, string details)
	{ Init(ToastLocation.TopLeft, title, details); }
	
	public Toast(ToastLocation location, string title, string details)
	{ Init(location, title, details); }
	
	void Init(ToastLocation location, string title, string details)
	{
		if (LastToast != null)
			LastToast.Step = 2;
		
		LastToast = this;
		
		BackColor = Color.FromArgb(50, 50, 50);
		FormBorderStyle = FormBorderStyle.None;
		ClientSize = new Size(ToastWidth, ToastHeight);
		Opacity = 0;
		ShowInTaskbar = false;
		
		var titleL = new Label {
			Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
			ForeColor = Color.White,
			Location = new Point(12, 9),
			Size = new Size(275, 38),
			Text = title
		};
		
		var contentL = new Label {
			Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
			ForeColor = Color.White,
			Location = new Point(32, 47),
			Size = new Size(253, 200),
			Text = details.Length > MaxLength ? details.Substring(0, MaxLength - 3) + "..." : details
		};
		
		Controls.Add(titleL);
		Controls.Add(contentL);
		
		
		Click += (sender, e) => { Step = 2; t.Interval = Speed; };
		titleL.Click += (sender, e) => { Step = 2; t.Interval = Speed; };
		contentL.Click += (sender, e) => { Step = 2; t.Interval = Speed; };
		
		t = new Timer { Interval = Speed, Enabled = true };
		t.Tick +=  t_Tick;
		
		Show(); 
		switch (location)
		{
			case ToastLocation.TopLeft:
				Location = new Point(ToastMargin, ToastMargin);
				break;
			case ToastLocation.TopCenter:
				Location = new Point(ScreenWidth / 2 - ToastWidth / 2, ToastMargin);
				break;
			case ToastLocation.TopRight:
				Location = new Point(ScreenWidth - ToastWidth - ToastMargin, ToastMargin);
				break;
			case ToastLocation.BottomLeft:
				Location = new Point(ToastMargin, ScreenHeight - ToastHeight - ToastMargin);
				break;
			case ToastLocation.BottomCenter:
				Location = new Point(ScreenWidth / 2 - ToastWidth / 2, ScreenHeight - ToastHeight - ToastMargin);
				break;
			case ToastLocation.BottomRight:
				Location = new Point(ScreenWidth - ToastWidth - ToastMargin, ScreenHeight - ToastHeight - ToastMargin);
				break;
		}
	}

	void t_Tick(object sender, EventArgs e)
	{
		switch(Step)
		{
			case 0: // BeginShow()
				
				if (Showing)
				{
					if (CurStep <= Steps)
					{
						Opacity = (float)CurStep / (float)Steps;
						CurStep++;
					}
					else
					{
						t.Interval = Timeout;
						Step++;
						LastToast = null;
					}
				}
				
				break;
				
			case 1: // Sleeping
				Step++;
				t.Interval = Speed;
				break;
				
			case 2: // EndShow();
				
				Showing = false;
				
				if (!Forced)
				{
					if (CurStep > 0)
					{
						Opacity = (float)CurStep / (float)Steps;
						CurStep--;
					}
					else
					{
						t.Tick -= t_Tick;
						t.Dispose();
						
						Hide();
						Dispose();
					}
				}
				
				break;
		}
	}
	
	void ForceEndShow()
	{
		Forced = true;
		Hide();
		Dispose();
	}
}