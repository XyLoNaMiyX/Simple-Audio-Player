
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Simple_Audio_Player
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{	
			if (SingleInstance.IsRunning)
			{
				new InterProcessCommunication("SimpleAudioPlayer")
					.SendMessage(args.Length > 0 ? String.Join("?", args) : " ");
				return;
			}
			
			Settings.Init("LonamiWebs\\Simple Audio Player", new Dictionary<string, dynamic>
			              {
			              	{ "AnimateTitle", true },
			              	{ "FromColor", Color.FromArgb(150, 255, 150) },
			              	{ "ToColor", Color.FromArgb(150, 150, 250) },
			              	
			              	{ "Repeat", false },
			              	{ "Shuffle", false },
			              	
			              	{ "SearchMode", 0 },
			              	
			              	{ "Style", "Default" },
			              	
			              	{ "Toast", false },
			              	{ "ToastLocation", 0 },
			              });
			
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
