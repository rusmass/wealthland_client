using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Core
{
	public static class EditorTools
	{
		static EditorTools()
		{
			EditorApplication.update += _Tick;
		}

		private static void _Tick()
		{
			if (!CoreMain.Instance.IsInited) 
			{
				var count = _updates.Count;
				for (int i = 0; i < count; ++i)
				{
					var callback = _updates[i] as Action;
					callback();
				}
			}
		}

		private static void _AttachToUpdate(Action action)
		{
			if (null != action && !_updates.Contains(action)) 
			{
				_updates.Add(action);
			}
		}

		public static bool DisplayCancelableProgressBar (string title, string info)
		{
			const float cycleLength = 1.0f;

			var current  = Time.realtimeSinceStartup;
			if (current > _nextProgressTime)
			{
				_nextProgressTime = current + cycleLength;
			}

			var progress = current + cycleLength - _nextProgressTime;
			var isCaceled = EditorUtility.DisplayCancelableProgressBar(title, info, progress);

			return isCaceled;
		}

		private static float _nextProgressTime;
		private static readonly ArrayList _updates = new ArrayList();
	}
}