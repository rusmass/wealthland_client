using UnityEngine;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Core;
using Core.IO;

public static partial class Console
{
	internal static void Init ()
	{
		_idMainThread = Thread.CurrentThread.ManagedThreadId;
	}

	internal static void Tick()
	{
		_time = Time.realtimeSinceStartup;
	}

//	[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
	public static void WriteLine(string message)
	{
		_WriteLine (_lpfnLog, message);
	}

//	[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
	public static void WriteLine(object message)
	{
		_WriteLine (_lpfnLog, message);
	}

//	[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
	public static void WriteLine(string format, params object[] args)
	{
		_WriteLine (_lpfnLog, _FormatMessage (format, args));
	}

	public static class Warning
	{
//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(string message)
		{
			_WriteLine (_lpfnLogWarning, message);
		}

//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(object message)
		{
			_WriteLine (_lpfnLogWarning, message);
		}

//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(string format, params object[] args)
		{
			_WriteLine (_lpfnLogWarning, _FormatMessage (format, args));
		}
	}

	public static class Error
	{
//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(string message)
		{
			_WriteLine (_lpfnLogError, message);
		}

//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(object message)
		{
			_WriteLine (_lpfnLogError, message);
		}

//		[Conditional("UNITY_EDITOR"), Conditional("OPEN_LOG_OUTPUT")]
		public static void WriteLine(string format, params object[] args)
		{
			_WriteLine (_lpfnLogError, _FormatMessage (format, args));
		}
	}

	private static void _WriteLine(System.Action<object> output, object message)
	{
		var isMainThread = Thread.CurrentThread.ManagedThreadId == _idMainThread;

		if (IsDetailedMessage) 
		{
			_messageFormat[1] = os.frameCount.ToString();
			_messageFormat[3] = (isMainThread ? Time.realtimeSinceStartup : _time).ToString("F3");
			_messageFormat[5] = null != message ? message.ToString() : "null text";

			message = string.Concat(_messageFormat);
		}

		try
		{
			if (isMainThread || _idMainThread == 0)
			{
				output(message);
			}
			else
			{
				if (os.isEditor)
				{
					var sbText = new StringBuilder(message.ToString());
					sbText.AppendLine();

					var text = _AppendStackTrace(sbText);
					Loom.QueueOnMainThread(() => output(text));
				}
				else
				{
					Loom.QueueOnMainThread(() => output(message));
				}
			}
		}
		catch(System.Exception e)
		{
			throw new System.Exception("[Console._WriteLine(...)] catch :" + e.ToString());
		}
	}

	private static string _AppendStackTrace(StringBuilder sb)
	{
		var trace = new System.Diagnostics.StackTrace (2, true);
		var frames = trace.GetFrames ();

		for (int i = 0; i < frames.Length; ++i) 
		{
			var frame = frames[i];
			sb.AppendFormat("{0} (at {1}:{2})\n"
			                , frame.GetMethod().ToString()
			                , frame.GetFileName()
			                , frame.GetFileLineNumber());
		}

		var result = sb.ToString ();
		return result;
	}

	private static string _FormatMessage(string format, params object[] args)
	{
		var message = null != format ? string.Format (null, format, args) : "null format.";
		return message;
	}

	private static void _Log (object message)
	{
		UnityEngine.Debug.Log(message);
	}
	
	private static void _LogWarning (object message)
	{
		UnityEngine.Debug.LogWarning(message);
	}
	
	private static void _LogError (object message)
	{
		UnityEngine.Debug.LogError(message);
	}

	public static bool  IsDetailedMessage = true;

	private static int _idMainThread;
	private static float _time;

	private static System.Action<object> _lpfnLog = _Log;
	private static System.Action<object> _lpfnLogWarning = _LogWarning;
	private static System.Action<object> _lpfnLogError = _LogError;

	private static readonly string[] _messageFormat = 
	{
		"[frame=",
		null,
		", time=",
		null,
		"] ",
		null
	};
}

