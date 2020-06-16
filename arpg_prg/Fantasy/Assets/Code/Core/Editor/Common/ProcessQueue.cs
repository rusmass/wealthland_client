using System;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
	[Serializable]
	public class ProcessQueue
	{
		public void Enqueue (Func<Process> starter)
		{
			if (null != starter)
			{
				_starterNames.Add (starter.Method.Name);
			}
		}

		// Tick不能在OnGUI()里调用，unity出错会引起调用栈死循环溢出
		public void Tick (object target)
		{
			if (_process == null && _processId != 0)
			{
				try
				{
					_process = Process.GetProcessById(_processId);
					if (_process == null)
					{
						throw new Exception();
					}
				}
				catch (Exception)
				{
					EditorUtility.ClearProgressBar();

					++_stageIndex;
					Console.WriteLine("[Process Exited] _stageIndex={0}, {1} {2}", _stageIndex, _processFileName, _processArguments);

					_process = null;
					_processId = 0;
				}
			}

			if (null == _process && null != _starterNames && _stageIndex < _starterNames.Count)
			{
				var name = _starterNames[_stageIndex];
				Func<Process> lpfnMethod;
				TypeTools.CreateDelegate (target, name, out lpfnMethod);
				if (null != lpfnMethod)
				{
					_process = lpfnMethod();

					if (null != _process)
					{
						_processId = _process.Id;
						_processFileName = _process.StartInfo.FileName;
						_processArguments = _process.StartInfo.Arguments;

						Console.WriteLine("[Process Started] {0} {1}", _processFileName, _processArguments);
					}
				}
			}

			if (null != _process)
			{
				var hasExited = _process.HasExited;

				if (!hasExited)
				{
					var isCanceled = EditorTools.DisplayCancelableProgressBar(_processFileName, _processArguments);
					if (isCanceled)
					{
						Console.WriteLine("[Process Killed] {0} {1}", _processFileName, _processArguments);
						_process.Kill();
					}
				}
				else
				{
					EditorUtility.ClearProgressBar();

					++_stageIndex;
					Console.WriteLine("[Process Exited] _stageIndex={0}, {1} {2}", _stageIndex, _processFileName, _processArguments);

					_process = null;
					_processId = 0;
				}
			}
		}

		public override string ToString ()
		{
			return string.Format ("[ProcessQueue:] _stageIndex={0}, _starterNames=[{1}]", _stageIndex, _starterNames);
		}

		private Process 		_process;
		private int				_processId;
		private string			_processFileName;
		private string			_processArguments;
		private int 			_stageIndex;
		private List<string> 	_starterNames = new List<string>();
	}
}
