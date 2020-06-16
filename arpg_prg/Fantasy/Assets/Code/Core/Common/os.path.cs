using System;
using System.IO;

namespace Core
{
	public static partial class os
	{
		public static class path
		{
			public static string join (string path1, string path2)
			{
				if (string.IsNullOrEmpty(path1))
				{
					return path2;
				}
				
				if (string.IsNullOrEmpty(path2))
				{
					return path1;
				}
				
				if (_IsRootChar(path2[0]))
				{
					return path2;
				}
				
				var c = path1 [path1.Length - 1];
				if (!_IsRootChar(c))
				{
					return path1 + '/' + path2;
				}
				
				return path1 + path2;
			}

			public static string join (string path1, string path2, string path3)
			{
				return join(path1, join(path2, path3));
			}

			private static bool _IsRootChar(char c)
			{
				return c == Path.DirectorySeparatorChar 
					|| c == Path.AltDirectorySeparatorChar
						|| c == Path.VolumeSeparatorChar;
			}

			public static long getsize(string filename)
			{
				try
				{
					if (File.Exists(filename))
					{
						var info = new FileInfo(filename);
						return info.Length;
					}
				}
				catch(Exception ex)
				{
					Console.Error.WriteLine("[path.getsize() ex={0}]", ex.ToStringEx());
				}

				return 0;
			}

			public static string normpath(string path)
			{
				if (!string.IsNullOrEmpty(path)) 
				{
					return path.Replace("\\", "/");
				}

				return string.Empty;
			}
		}
	}
}