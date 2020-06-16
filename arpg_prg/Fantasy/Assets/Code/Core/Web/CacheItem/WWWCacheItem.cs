using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Web
{
	internal class WWWCacheItem : ACacheItem
	{
		public WWWCacheItem (string localPath, WebType webType) : base (localPath)
		{
			_www = _CreateWWW (webType);
		}

		protected override void _DoDispose (bool isDisposing)
		{
			_CommanDispose ();
			os.dispose (ref _www);
		}

		public override string url 
		{
			get 
			{
				var info = WebManager.Instance.GetMappingInfo(localPath);
				return info.GetUrl();
			}
		}

		public override AssetBundle assetbundle { get { throw new NotImplementedException (); } }
		public override Texture2D texture { get { throw new NotImplementedException (); } }
		public override Sprite sprite { get { throw new NotImplementedException (); } }
		public override AudioClip audioClip { get { throw new NotImplementedException (); } }
		public override string text { get { return _www != null ? _www.text : null; } }
		public override byte[] bytes { get { return _www != null ? _www.bytes : null; } }
		public override string error { get { return _www != null ? _www.error : string.Empty; } }
		public override bool isDone { get { return _www == null || _www.isDone; } }
		public override float progress { get { return _www != null ? _www.progress : 0.0f; } }
        public override Sprite GetSprite(string assetName)
        {
           throw new NotImplementedException();
        }

        private WWW _www;
	}
}