using System;
using UnityEditor;

namespace Core
{
	public static class ScanTools
	{
		public static bool ScanAll (string title, string[] paths, Action<string> handler)
		{
			if (null == paths || paths.Length == 0 || null == handler)
			{
				return false;
			}

			title = title ?? string.Empty;
			var length = paths.Length;

			var invLength = 1.0f / length;
			var startTime = System.DateTime.Now;
			var lastProgress = 0.0f;

			try
			{
				for (int i= 0; i< length; ++i)
				{
					var path = paths[i];
					var progress = i * invLength;
					if (progress - lastProgress < 0.05f)
					{
						progress = lastProgress;
					}
					else 
					{
						lastProgress = progress;
					}

					var isCanceled = EditorUtility.DisplayCancelableProgressBar(title, string.Empty, progress);
					if (isCanceled)
					{
						return false;
					}

					handler(path);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

			var timeSpan = System.DateTime.Now - startTime;
			Console.WriteLine("[ScanTools.ScanAll()] {0}, costTime={1}s", title, timeSpan.TotalSeconds.ToString("F2"));
			return true;
		}

	}
}