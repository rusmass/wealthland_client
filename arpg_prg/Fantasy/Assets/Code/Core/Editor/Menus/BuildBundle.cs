using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using Core.Xml;

namespace Core.PackResources
{
	[Serializable]
	public class AssetBundleFolders
	{
		public string[] normal_folders = new string[2];
	}

	public class BuildScript
	{
		[MenuItem ("AssetBundles/Build AssetBundles %#d", false, 999)]
		public static void BuildAssetBundle()
		{
			Debug.Log ("Pack Resources Start!");

			try
			{
				_ClearAssetBundlesName ();
				_EndueAssetBundleName ();	
			}
			catch(Exception e)
			{
				EditorUtility.ClearProgressBar ();
				Console.WriteLine (e.ToStringEx ());
			}

			var path = System.IO.Path.GetFullPath ("../arpg_res/resources");
			path = os.path.join (path, PathTools.PlatformResFolder);
			BuildPipeline.BuildAssetBundles (path, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
			AssetDatabase.Refresh ();

			Debug.Log ("Pack Resources End!");
		}

		private static void _ClearAssetBundlesName()
		{
			EditorUtility.DisplayProgressBar("Delete AssetName", "Delete AssetName...", 0f);
			string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames ();

			for (int i = 0; i < oldAssetBundleNames.Length; i++) 
			{
				EditorUtility.DisplayProgressBar("Delete AssetName", "Delete AssetName...", 1f * i / oldAssetBundleNames.Length);
				AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[i],true);
			}
			EditorUtility.ClearProgressBar ();
		}

		private static void _EndueAssetBundleName()
		{
			var abf = XmlTools.Deserialize<AssetBundleFolders> (Constants.AssetBundleFoldersPath);
			var folders = abf.normal_folders;
			EditorUtility.DisplayProgressBar("Endue AssetName", "Enduing AssetName...", 0f);

			for (int i = 0; i < folders.Length; ++i) 
			{
				EditorUtility.DisplayProgressBar("Endue AssetName", "Enduing AssetName...", 1f * i / folders.Length);
				var source = os.path.join (Application.dataPath, folders [i]);
				_EndueAssetBundleName (source);
			}

			EditorUtility.ClearProgressBar ();
		}

		private static void _EndueAssetBundleName(string source)
		{
			DirectoryInfo folder = new DirectoryInfo (source);
			FileSystemInfo[] files = folder.GetFileSystemInfos ();
			int length = files.Length;

			for (int i = 0; i < length; i++) 
			{
				var file = files [i];
				if(file is DirectoryInfo)
				{
					_EndueAssetBundleName(file.FullName);
				}
				else if(!file.Name.EndsWith(".meta") && !file.Name.EndsWith(".DS_Store"))
				{
					_EndueName (file.FullName);
				}
			}
		}

		private static void _EndueName(string source)
		{
			string _source = os.path.normpath (source);
			string assetPath = "Assets" + _source.Substring (Application.dataPath.Length);
			string assetName = _source.Substring (Application.dataPath.Length + 1);

			AssetImporter assetImporter = AssetImporter.GetAtPath (assetPath);

			var fileExtension = Constants.BundleExtension;
			var fileReplace = Path.GetExtension (assetName);
			if (assetName.Contains ("prefabs/animator")) 
			{
				var folder = Path.GetFileName (Path.GetDirectoryName (_source));
				fileExtension = folder + Constants.BundleExtension;
				fileReplace = Path.GetFileName (assetName);
			}

			assetName = assetName.Replace (fileReplace, fileExtension);
			assetImporter.assetBundleName = assetName;

			if (assetPath.Contains ("share/atlas")) 
			{
				TextureImporter textureImporter = (TextureImporter)assetImporter;
				textureImporter.textureType = TextureImporterType.Sprite;
                //				textureImporter.mipmapEnabled = false;

                var fileName = Path.GetFileName(assetName);
                var extension = Path.GetExtension(assetName);
                var dirName = Path.GetDirectoryName (assetPath);
                var packTag = Path.GetFileName(dirName);
                textureImporter.spritePackingTag = packTag;
                textureImporter.assetBundleName = assetName.Replace(fileName,packTag + extension);
			}
			else if (assetPath.Contains ("share/texture")) 
			{
//				TextureImporter textureImporter = (TextureImporter)assetImporter;
//				textureImporter.textureType = TextureImporterType.Advanced;
//				textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
//				textureImporter.mipmapEnabled = false;
//				textureImporter.alphaIsTransparency = true;
//
//				textureImporter.SetPlatformTextureSettings ("iPhone", 2048, TextureImporterFormat.RGBA32);
//				textureImporter.SetPlatformTextureSettings ("Android", 1024, TextureImporterFormat.ETC2_RGBA8);
			}
		}
	}
}