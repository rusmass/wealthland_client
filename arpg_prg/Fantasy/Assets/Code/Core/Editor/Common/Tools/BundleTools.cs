using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace Core
{
	public static class BundleTools
	{
		public static long GetTotalSize (string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return 0;
			}

			var bundleSize  = os.path.getsize(path);
			if (bundleSize == 0)
			{
				return 0;
			}

			var manifestPath= path + ".manifest";
			if (File.Exists (manifestPath)) 
			{
				var lines = File.ReadAllLines (manifestPath);
				for (int i = 0; i < lines.Length; ++i) 
				{
					var item = lines [i];
					if (item.EndsWith (Constants.BundleExtension)) 
					{
						try
						{
							path =  "../.." + item.Substring(item.IndexOf("arpg_res") - 1);
						}
						catch(Exception e) 
						{
							Console.Error.WriteLine (e.ToString());
						}

						if (File.Exists (path)) 
						{
							bundleSize += os.path.getsize (path);	
						}
					}
				}
			}

			return bundleSize;
		}
	}
}