using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Web
{
	internal abstract class ABundleCacheItem : ACacheItem
	{
		public ABundleCacheItem(string localPath) : base(localPath)
		{

		}

		protected override void _DoDispose (bool isDisposing)
		{
			_CommanDispose ();
			_ReleaseAssetbundle ();
		}

		private void _ReleaseAssetbundle()
		{
			var url = this.localPath;
			if (null != assetbundle) 
			{
				_assetbundle.Unload(unloadAllLoadedObjects);
				_assetbundle = null;
			}
		}

		private void _UnloadMainAsset (UnityEngine.Object asset)
		{
			Resources.UnloadAsset (asset);
			_ReleaseAssetbundle ();
		}

		private UnityEngine.Object _GetMainAsset<T>()
			where T : UnityEngine.Object
		{
			if (null != assetbundle) 
			{
				return assetbundle.LoadAsset<T> (assetbundle.GetAllAssetNames () [0]);
			}

			return null;
		}
        private UnityEngine.Object _GetMainAsset<T>(string assetName)
            where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetName))
                return _GetMainAsset<T>();
            
            if (null != assetbundle)
            {
                string[] allAssetNames = assetbundle.GetAllAssetNames();

                if (IsContain(assetName, allAssetNames))
                    return assetbundle.LoadAsset<T>(assetName);
                else
                    return assetbundle.LoadAsset<T>(allAssetNames[0]);
            }

            return null;
        }

        public override AssetBundle assetbundle 
		{
			get 
			{
				return _assetbundle;
			}
		}

		public override Texture2D texture 
		{
			get 
			{
				if (null == _target)
				{
					_target = _GetMainAsset<Texture>();
				}
				return _target as Texture2D;
			}
		}

		public override Sprite sprite 
		{
			get 
			{
				if (null == _target) 
				{
					_target = _GetMainAsset<Sprite> ();
				}
				return _target as Sprite;
			}
		}

		public override AudioClip audioClip 
		{
			get 
			{
				if (null == _target) 
				{
					_target = _GetMainAsset<AudioClip> ();
				}

				return _target as AudioClip;
			}
		}

		public override byte[] bytes 
		{
			get 
			{
				if (null == _target)
				{
					var asset = _GetMainAsset<TextAsset>();
					if (null != asset)
					{
						_target = (asset as TextAsset).bytes;
						_UnloadMainAsset(asset);
					}
					else
					{
						_target = Constants.EmptyBytes;
					}
				}

				return _target as Byte[];
			}
		}

		public override string text 
		{
			get 
			{
				if (null == _target)
				{
					var asset = _GetMainAsset<TextAsset>();
					if (null != asset)
					{
						_target = asset as TextAsset;
						_UnloadMainAsset(asset);
					}
					else
					{
						_target = string.Empty;
					}
				}
				
				return _target as string;
			}
		}
        public override Sprite GetSprite(string assetName)
        {
            return _GetMainAsset<Sprite>(assetName) as Sprite;
        }
        private bool IsContain<T>(T t,T[] tArray)
        {
            if (null == t || null == tArray)
                return false;

            for (int i = 0; i < tArray.Length; i++)
            {
                if (t.Equals(tArray[i]))
                    return true;
            }
            return false;
        }
		private object _target;
		protected AssetBundle _assetbundle;
	}
}