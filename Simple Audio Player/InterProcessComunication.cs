/*
 * Made by Lonami, the 06/12/2014 during the evening
 * (C) LonamiWebs
 */
	
using System;
using System.IO;

public class InterProcessCommunication {
	
	#region Private
	
	FileSystemWatcher fsw;
	
	string _WorkingDir;
	
	void HandleMessage(string file) {
		if (!File.Exists(file))
			return;
		
		if (NewMessage != null)
			NewMessage(File.ReadAllText(file));
		
		if (DeleteMsgAfterRead)
			File.Delete(file);
	}
	
	bool Listening;
	
	#endregion
	
	#region Public
	
	public delegate void DelegateMessage(string message);
	
	/// <summary>
	/// Called when a new message is received
	/// </summary>
	public event DelegateMessage NewMessage;
	
	/// <summary>
	/// Should the message be deleted after read?
	/// </summary>
	public bool DeleteMsgAfterRead = true;
	
	/// <summary>
	/// The working directory. It should be a temporary directory
	/// </summary>
	public string WorkingDir {
		get { return _WorkingDir + "\\"; }
		set {
			Directory.CreateDirectory(value);
			_WorkingDir = value.Trim('\\');
		}
	}
	
	/// <summary>
	/// The name of the channel to be used
	/// </summary>
	public string Name {
		get { return Path.GetFileName(WorkingDir); }
		set { WorkingDir = Path.GetTempPath().Trim('\\') + "\\" + value; }
	}
	
	/// <summary>
	/// Initialize a new InterProcessCommunication instance
	/// </summary>
	/// <param name="name">The <see cref="Name"/> of the channel to be used</param>
	public InterProcessCommunication(string name)
	{ Name = name; }
	
	/// <summary>
	/// Start listening to messages
	/// </summary>
	public void Listen() {
		if (Listening)
			return;
		
		fsw = new FileSystemWatcher(WorkingDir, "*.msg");
		fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
			| NotifyFilters.FileName | NotifyFilters.DirectoryName;
		fsw.Changed += (s, e) => HandleMessage(e.FullPath);
		Listening = fsw.EnableRaisingEvents = true;
	}
	
	/// <summary>
	/// Send a new message
	/// </summary>
	/// <param name="msg"></param>
	public void SendMessage(string msg) {
		File.WriteAllText(WorkingDir + Guid.NewGuid() + ".msg", msg);
	}
	
	#endregion
}