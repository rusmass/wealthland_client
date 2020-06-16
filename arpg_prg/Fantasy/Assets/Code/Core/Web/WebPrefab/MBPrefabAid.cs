using Core;
using Core.Web;
using UnityEngine;
using System.Collections;

namespace Core.Web
{
	internal partial class MBPrefabAid : MonoBehaviour
	{
		private void Awake ()
		{
			_Init();
		}

		internal void _Init ()
		{
			if (!_isInited && null != localPath)
			{
				_isInited = true;

				InnerWebPrefab inner;
				var cache = WebPrefab._GetLruCache();
				cache.TryGetValue(localPath, out inner);
				inner.AddReference();
			}
		}

		private void OnDestroy ()
		{
			if (_isInited && null != localPath) 
			{
				InnerWebPrefab inner;
				var cache = WebPrefab._GetLruCache();

				if (cache.TryGetValue(localPath, out inner))
				{
					WebTools.RemoveFromCache(cache, ref inner);
				}
			}
		}

		public string 	localPath;

		private bool _isInited;
	}
}
