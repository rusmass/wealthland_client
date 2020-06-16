using System;
using UnityEngine;
using System.IO;

namespace Core.Web
{
	public partial class WebManager
	{
		internal void Init()
		{
			_basePaths [(int)LoadType.Disk] = PathTools.DiskPath;
			_basePaths [(int)LoadType.Builtin] = PathTools.BuiltinPath;
			_baseUrls [(int)LoadType.Disk] = PathTools.DiskUrl;
			_baseUrls [(int)LoadType.Builtin] = PathTools.BuiltinUrl;

			Console.WriteLine ("[WebManager.Init()]\n diskPath = {0}\n diskUrl = {1}\n builtinPath = {2}\n builtinUrl = {3}"
			                   ,_basePaths[(int)LoadType.Disk], _baseUrls[(int)LoadType.Disk]
			                   ,_basePaths[(int)LoadType.Builtin], _baseUrls[(int)LoadType.Builtin]);

			_LoadDefaultMappingInfo ();
		}

		internal void Dispose()
		{
			_isInited = false;
			_mappingDB.Dispose ();
		}

		public static bool IsInited()
		{
			return _isInited;
		}

		public WebItem LoadWebItem(string localPath, Action<WebItem> handler)
		{
			var argument = GetWebArgument(localPath);
			return LoadWebItem (argument, handler);
		}

		public WebItem LoadWebItem(WebArgument argument, Action<WebItem> handler)
		{
			var web = new WebItem (argument, handler);
			return web;
		}

		public WebPrefab LoadWebPrefab (string localPath, Action<WebPrefab> handler)
		{
			var argument  = new WebArgument{ localPath = localPath };
			var webPrefab = LoadWebPrefab(argument, handler);
			return webPrefab;
		}

		public WebPrefab LoadWebPrefab (WebArgument argument, Action<WebPrefab> handler)
		{
			var webPrefab = new WebPrefab(argument, handler);
			return webPrefab;
		}

		public void SetWebManagerParam()
		{
			Application.runInBackground = true;

			if (!Application.isEditor)
			{
				Application.targetFrameRate = 40;
			}
			else
			{
				Application.targetFrameRate = -1;
			}

			Caching.maximumAvailableDiskSpace = 317608096;
			WebItem.SetMaxLoadingCount (6);
		}
		public WebArgument GetWebArgument(string localPath)
		{
			if (localPath.Contains ("share/atlas")) 
			{
				var fileName = Path.GetFileName(localPath);
				var extension = Path.GetExtension(localPath);
				var dirPath = Path.GetDirectoryName(localPath);
				var dirName = Path.GetFileName(dirPath);
				string assetName = fileName.Replace(extension,"");
				string packPath = localPath.Replace(assetName,dirName);
                assetName = localPath.Replace(extension, Constants.TextureExtension);
                assetName = string.Format("assets/{0}", assetName);
				return new WebArgument() { localPath = packPath, assetName = assetName };
			}
			return new WebArgument() { localPath = localPath };
		}

		public static readonly WebManager Instance = new WebManager();

		private static string[] _basePaths = new string[(int)LoadType.Count];
		private static string[] _baseUrls = new string[(int)LoadType.Count];

		public AssetBundleManifest Manifest;
		private static bool _isInited = false;
	}
}