using System;
using UnityEngine;

namespace Core.Web
{
	internal class LocalBundleCacheItem : ABundleCacheItem
	{
		//I want to use LoadFromFile, but there are some problems in iOS.
		//I use LoadFromFile to load a plane with a texture, but the texture can not display well.
		//So, i use loadfromcacheordownload replace.
		public LocalBundleCacheItem(string localPath) : base (localPath)
		{
			_assetbundle = AssetBundle.LoadFromFile (url);
		}

		protected override void _DoDispose (bool isDisposing)
		{
			base._DoDispose (isDisposing);
		}

		public override string url 
		{
			get 
			{
				var info = WebManager.Instance.GetMappingInfo(localPath);
				return info.GetFullPath();
			}
		}

		public override float  progress { get { return 1.0f; } }
		public override bool   isDone   { get { return true; } }
		public override string error    { get { return null; } }
	}
}