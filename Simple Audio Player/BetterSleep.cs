
using System;
using System.Diagnostics;

public static class BetterSleep
{
	static readonly Stopwatch sw = new Stopwatch();
	
	public static void Sleep(long ms) {
		sw.Start();
		while (sw.ElapsedMilliseconds < ms) { /* wait */ }
		sw.Reset();
	}
}
