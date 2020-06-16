using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using Core;
using Core.Xml;
using Core.PackResources;

public class CheckWindow
{
	[MenuItem("*Resource/CheckRes")]
	public static void CheckArpgRes()
	{
		var abf = XmlTools.Deserialize<AssetBundleFolders> (Constants.AssetBundleFoldersPath);
		var folders = abf.normal_folders;

		for (int i = 0; i < folders.Length; ++i) 
		{
			var source = os.path.join (Application.dataPath, folders [i]);

			var paths = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
			ScanTools.ScanAll("CheckRes", paths, path => {
				for (int j = 0; j < path.Length; ++j)
				{
					if ((int)path[j] > 127)
					{
						Console.WriteLine(path);
						File.Delete(path);
						break;
					}
				}
			});
		}
	}
}