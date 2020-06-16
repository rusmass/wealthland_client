using UnityEngine;
using System;
using System.Collections;
using Core.Web;
using Core.Collections;

namespace Core
{
	public class PrefabPool: Disposable
	{
		internal PrefabPool (InnerPrefabPool innerPool)
		{
			_inner = innerPool;
			_inner.AddReference();
		}

		protected override void _DoDispose (bool isDisposing)
		{
			WebTools.RemoveFromCache(_innerPools, ref _inner);
		}

		public static PrefabPool CreatePool (string resourceKey, GameObject mainAsset)
		{
			if (null == mainAsset)
			{
				throw new ArgumentNullException("mainAsset should not be null.");
			}

			resourceKey = resourceKey ?? string.Empty;

			InnerPrefabPool innerPool;
			if (!_innerPools.TryGetValue(resourceKey, out innerPool))
			{
				innerPool = new InnerPrefabPool(resourceKey, mainAsset);
				_innerPools.Add(resourceKey, innerPool);
			}

			var pool = new PrefabPool(innerPool);
			return pool;
		}

		public static IWebNode CreatePool (string localPath, Action<PrefabPool> handler)
		{
			localPath       = localPath ?? string.Empty;
			var argument	= new WebArgument { localPath= localPath };
			return CreatePool(argument, handler);
		}

		public static IWebNode CreatePool (WebArgument argument, Action<PrefabPool> handler)
		{
			InnerPrefabPool innerPool;
			var key = argument.localPath;
			if (!_innerPools.TryGetValue(key, out innerPool))
			{
				innerPool = new InnerPrefabPool(argument);
				_innerPools.Add(key, innerPool);
			}

			CoroutineManager.StartCoroutine(_CoLoadInnerPrefabPool(innerPool, handler));
			var web = innerPool.GetWebPrefab();

			return web;
		}

		private static IEnumerator _CoLoadInnerPrefabPool (InnerPrefabPool innerPool, Action<PrefabPool> handler)
		{
			var web = innerPool.GetWebPrefab();

			while (!web.isDone)
			{
				yield return null;
			}

			var pool = new PrefabPool(innerPool);
			CallbackTools.Handle(ref handler, pool, "[PrefabPool._CoLoadInnerPrefabPool()]");
		}

		public static void RecycleGameObject (string resourceKey, GameObject go)
		{
			if (null != go)
			{
				resourceKey = resourceKey ?? string.Empty;
				InnerPrefabPool innerPool;
				_innerPools.TryGetValue(resourceKey, out innerPool);

				if (null != innerPool)
				{
					innerPool.Recycle(go);
				}
				else 
				{
					go.DestroyEx();
				}
			}
		}

		public static PrefabPool GetPool (string resourceKey)
		{
			resourceKey = resourceKey ?? string.Empty;

			InnerPrefabPool innerPool;
			_innerPools.TryGetValue(resourceKey, out innerPool);

			if (null != innerPool)
			{
				var isReady = null != innerPool.GetMainAsset();
				if (isReady)
				{
					var pool = new PrefabPool(innerPool);
					return pool;
				}
			}

			return null;
		}

		internal static LruCache<string, InnerPrefabPool> _GetLruCache ()
		{
			return _innerPools;
		}

		public GameObject Spawn (bool isActivate = true)
		{
			return _inner.Spawn(isActivate);
		}

		public void Recycle (GameObject go)
		{
			_inner.Recycle(go);
		}

		public void Preload (int count, Action<GameObject> initAction)
		{
			_inner.Preload(count, initAction);
		}

		public override string ToString ()
		{
			return _inner.ToString();
		}

		public GameObject mainAsset { get { return _inner.GetMainAsset(); } }
		private InnerPrefabPool  _inner;
		private static readonly LruCache<string, InnerPrefabPool> _innerPools = new LruCache<string, InnerPrefabPool>(32);
	}
}