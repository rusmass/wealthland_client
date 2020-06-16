using System;
using UnityEngine;
using Core.Reflection;

namespace Core.Web
{
	internal static class CacheItemFactory
	{
		static CacheItemFactory()
		{
			if (os.isAndroid) 
			{
				_lpfnBundleCreate = _Android_BundleCreate;
			}
			else if (os.isIPhonePlayer) 
			{
				_lpfnBundleCreate = _iPhone_BundleCreate;
			}
			else if (os.isEditor) 
			{
				var targetPlatform = EditorBuildings.activeBuildTarget;
				if (targetPlatform == TargetPlatform.Android)
				{
					_lpfnBundleCreate = _Editor_Android_BundleCreate;
				}
				else if (targetPlatform == TargetPlatform.iPhone)
				{
					_lpfnBundleCreate = _Editor_iPhone_BundleCreate;
				}
				else
				{
					_lpfnBundleCreate = _Editor_iPhone_BundleCreate;
				}
			}
		}

		public static ACacheItem Create(WebArgument argument)
		{
			var localPath = argument.localPath;
			var isBundle = localPath.EndsWith (Constants.BundleExtension);

			var cacheItem = isBundle ? _lpfnBundleCreate (argument) : new WWWCacheItem (argument.localPath, WebType.NewWWW);
			cacheItem.unloadAllLoadedObjects = (argument.flags & WebFlags.UnloadAllLoadedObjects) != 0;
			return cacheItem;
		}

		private static ACacheItem _Android_BundleCreate(WebArgument argument)
		{
			var localPath = argument.localPath;
			var info = WebManager.Instance.GetMappingInfo (localPath);
			var isSmallEnough = _IsSmallEnough (info);

			ACacheItem cacheItem;

			if (info.loadType == LoadType.Builtin) 
			{
				var webType = isSmallEnough ? WebType.LoadFromCacheOrDownload : WebType.NewWWW;
				cacheItem = new RemoteBundleCacheItem(argument.localPath, webType);
				return cacheItem;
			}

			// 大文件加載，以主城為例：
			// 1. 使用NewWWW方式，加載平緩，但會產生一個4MB的WebStream
			// 2. 使用CreateFromFile，不會產生WebStream，但加載會卡頓15s

			var isNewWWW = (argument.flags & WebFlags.NewWWW) != 0;
			var isLoadFromCacheOrDownload = isSmallEnough || !isNewWWW;
			if (isLoadFromCacheOrDownload) 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.LoadFromCacheOrDownload);
			}
			else 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.NewWWW);
			}

			return cacheItem;

		}

		private static ACacheItem _iPhone_BundleCreate(WebArgument argument)
		{
			var localPath = argument.localPath;
			var isLoadFromCacheOrDownload = (argument.flags & WebFlags.NewWWW) == 0;

			ACacheItem cacheItem;
			if (isLoadFromCacheOrDownload) 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.LoadFromCacheOrDownload);
			}
			else 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.NewWWW);
			}

			return cacheItem;
		}

		private static ACacheItem _Editor_Android_BundleCreate(WebArgument argument)
		{
			var localPath = argument.localPath;
			var info = WebManager.Instance.GetMappingInfo (localPath);
			var isSmallEnough = _IsSmallEnough(info);
			var isNewWWW = (argument.flags & WebFlags.NewWWW) != 0;
			var isLoadFromCacheOrDownload = isSmallEnough || !isNewWWW;

			ACacheItem cacheItem;
			if (isLoadFromCacheOrDownload) 
			{
				cacheItem = new LocalBundleCacheItem(argument.localPath);
			}
			else 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.NewWWW);
			}

			return cacheItem;
		}

		private static ACacheItem _Editor_iPhone_BundleCreate(WebArgument argument)
		{
			var localPath = argument.localPath;
			var isLoadFromCacheOrDownload = (argument.flags & WebFlags.None) == 0;

			ACacheItem cacheItem;
			if (isLoadFromCacheOrDownload) 
			{

				cacheItem = new LocalBundleCacheItem(argument.localPath);
			}
			else 
			{
				cacheItem = new RemoteBundleCacheItem(argument.localPath, WebType.NewWWW);
			}
			
			return cacheItem;
		}

		private static bool _IsSmallEnough(MappingInfo info)
		{
			var isSmallEnough = info.selfSize < _lpfnBundleThresHold;
			return isSmallEnough;
		}

		private static Func<WebArgument, ACacheItem> _lpfnBundleCreate;
		private static readonly int _lpfnBundleThresHold = 2 * 1024 * 1024;
	}
}