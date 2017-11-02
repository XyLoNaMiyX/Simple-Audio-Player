using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace updater
{
	sealed class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length < 2) {
				Error();
				return;
			}
				
			string curLoc = args[0];
			string tmpLoc = args[1];
			const int triesDelay = 500;
			int triesLeft = 5;
           	bool success = false;
           	
           	while (triesLeft > 0) {
           		try {
           			if (File.Exists(curLoc))
           				File.Delete(curLoc);
           			success = true;
           			break;
           		} catch { triesLeft--; Thread.Sleep(triesDelay); }
           	}
           	if (!success) {
           		Error();
           		return;
           	}
           	
           	File.Move(tmpLoc, curLoc);
           	Process.Start(curLoc);
		}
		
		static void Error()
		{
			MessageBox.Show("Simple Audio Player no se pudo actualizar automáticamente",
		           		                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);	
		}
	}
}
