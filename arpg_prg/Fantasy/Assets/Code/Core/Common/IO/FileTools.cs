using System;
using System.IO;
using Core.IO;

namespace Core.IO
{
	public static class FileTools
	{
		public static void WriteAllTextSafely (string path, string contents)
		{
			_WriteAllSafely(path, contents, File.WriteAllText);
		}
		
		public static void WriteAllLinesSafely (string path, string[] contents)
		{
			_WriteAllSafely(path, contents, File.WriteAllLines);
		}
		
		public static void WriteAllBytesSafely (string path, byte[] bytes)
		{
			_WriteAllSafely(path, bytes, File.WriteAllBytes);
		}

		private static void _WriteAllSafely<T> (string path, T contents, Action<string, T> lpfnWriteFunc)
		{
			if (string.IsNullOrEmpty (path)) 
			{
				return;
			}

			var existence = File.Exists (path);
			if (!existence) 
			{
				os.mkdir(path);
			}

			var tempFileName = path + "tmp.0623";
			lpfnWriteFunc (tempFileName, contents);

			if (existence) 
			{
				FileTools.DeleteSafely(path);
			}

			File.Move (tempFileName, path);
		}
		
		public static void DeleteSafely (string path)
		{
			if (File.Exists (path)) 
			{
				File.Delete(path);
			}
		}
	}
}

