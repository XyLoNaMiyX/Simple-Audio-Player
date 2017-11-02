using System;
using System.Reflection;
using System.Threading;

public static class SingleInstance {
	
	/// <summary>
	/// Check wheter is there or not another instance running already
	/// </summary>
	public static bool IsRunning { get { return !mutex.WaitOne(TimeSpan.Zero, true); } }
    
	static readonly Mutex mutex = new Mutex(true, AssemblyName);
	static string AssemblyName { get { return Assembly.GetExecutingAssembly().FullName; } }
	
}