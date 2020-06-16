using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.IO;

namespace Core
{
	internal class LogCollector
	{
		public void InitLogInfo ()
		{
			try 
			{
				var lastLogPath = Constants.LastLogPath;
				FileTools.DeleteSafely(lastLogPath);

				var logPath = Constants.LogPath;
				if (File.Exists(logPath))
				{
					File.Move(logPath, lastLogPath);
				}

				var stream = new FileStream(logPath, FileMode.Create, FileAccess.Write , FileShare.ReadWrite);
				_logWriter = new StreamWriter(stream);

				Application.logMessageReceived += HandleLogCallBack;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("[UniqueMain._InitLogInfo()] ex={0}", ex.ToStringEx());
			}
		}

		private void HandleLogCallBack(string logString, string stacktrace, LogType type)
		{
			if (_lastLogString != logString) 
			{
				_lastLogString = logString;

				_logs.Add(logString);

				if (type == LogType.Error || type == LogType.Exception || type == LogType.Warning)
				{
					_logs.Add(stacktrace);
				}

				_logs.Add(os.linesep);
			}
		}

		public void Tick()
		{
			var count = _logs.Count;
			if (null != _logWriter) 
			{
				for (int i = 0; i < count; ++i) 
				{
					var log = _logs[i];
					_logWriter.WriteLine(log);	
				}

				_logWriter.Flush();
				_logs.Clear();
			}
		}

		public void Dispose()
		{
			Application.logMessageReceived -= HandleLogCallBack;
			_logWriter.Close ();
		}

		public static readonly LogCollector Instance = new LogCollector();

		private StreamWriter _logWriter;
		private string _lastLogString;
		private readonly List<string> _logs = new List<string>();
	}
}