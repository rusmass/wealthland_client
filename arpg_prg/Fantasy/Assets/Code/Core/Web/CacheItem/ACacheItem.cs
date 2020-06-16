using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;

namespace Core.Web
{
	internal abstract class ACacheItem : ASmartPointer, ILocalPath
	{
		public ACacheItem(string localPath)
		{
			this.localPath = localPath;
		}

		protected WWW _CreateWWW(WebType webType)
		{
			if (webType == WebType.LoadFromCacheOrDownload) 
			{
				int version = 0;
				if (os.isEditor) 
				{
					_GetFileVersion (ref version);
				}
					
				var www1 = WWW.LoadFromCacheOrDownload(url, version);
				return www1;
			}

			var www2 = new WWW(url);
			return www2;
		}
			
		private void _GetFileVersion(ref int version)
		{
			try
			{
				var fullPath = PathTools.GetExportPath(localPath);

				var fileInfo = new System.IO.FileInfo(fullPath);
				var key = fileInfo.Length * fileInfo.CreationTime.ToFileTime();
				var bytes = System.BitConverter.GetBytes(key);
				var md5 = Md5sum.Instance.GetAssetDigest(bytes);
				version = Convert.ToInt32(md5, 16);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("[ACacheItem._GetFileDigest] ex = {0}", ex.ToStringEx());
			}
		}

		protected void _CommanDispose()
		{
			if (null != _babies) 
			{
				var count = _babies.Length;
				for (int i = 0; i < count; ++i) 
				{
					var item = _babies[i] as IDisposable;
					item.Dispose();
				}

				_babies = null;
			}
		}

		public void HoldBabies(ref IWebNode[] babies)
		{
			_babies = babies;
		}

		public override string ToString ()
		{
			return string.Format("[{0}: localPath={1}, url={2}, progress={3}, isDone={4}, error={5}, referCount={6}]"
			                     , GetType().Name, localPath, url, progress, isDone, error, GetReference());
		}

		public abstract float progress { get; }
		public abstract bool isDone{ get; }
		public abstract string error { get; }
		public abstract string url { get; }
		public abstract byte[] bytes { get; }
		public abstract string text { get; }
		public abstract AssetBundle assetbundle { get; }
		public abstract Texture2D texture { get; }

		public abstract Sprite sprite { get; }
		public abstract AudioClip audioClip { get; }

		public bool unloadAllLoadedObjects { get; internal set;}
		public string localPath { get; set;}

        public abstract Sprite GetSprite(string assetName);

        private IWebNode[] _babies;
	}
}