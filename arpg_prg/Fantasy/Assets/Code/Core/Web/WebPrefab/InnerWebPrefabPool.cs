using UnityEngine;
using System;
using System.Collections;
using Core.Web;

namespace Core
{
	internal class InnerPrefabPool: ASmartPointer, ILocalPath
	{
		public InnerPrefabPool (WebArgument argument)
		{
			localPath = argument.localPath;
			_webPrefab = new WebPrefab (argument, _OnLoadWebPrefab);
		}

		public InnerPrefabPool (string resourceKey, GameObject mainAsset)
		{
			localPath = resourceKey;
			_SetMainAsset(mainAsset);
		}

		protected override void _DoDispose (bool isDisposing)
		{
			_Clear();

			_webPrefab = null;
			_mainAsset = null;
		}

		private void _OnLoadWebPrefab (WebPrefab webPrefab)
		{
			_SetMainAsset(webPrefab.mainAsset);
		}

		private void _SetMainAsset (GameObject mainAsset)
		{
			_mainAsset  = mainAsset;
			AssertTools.IsNotNull(mainAsset);
		}

		public void Recycle (GameObject go)
		{
			if (null != go && go != _mainAsset)
			{
				go.transform.parent = null;
				go.SetActive(false);

				var queue = _CheckCreateQueue();
				queue.Enqueue(go);
			}
		}

		private Queue _CheckCreateQueue ()
		{
			if (null == _goQueue)
			{
				_goQueue = new Queue(8);
			}

			return _goQueue;
		}

		private void _Clear ()
		{
			var queue = _goQueue;

			if (null != queue && queue.Count > 0)
			{
				var e = queue.GetEnumerator();
				while (e.MoveNext())
				{
					var go = e.Current as GameObject;
					go.DestroyEx();
				}

				queue.Clear();
			}
		}

		public void Preload (int count, Action<GameObject> initAction)
		{
			var queue = _CheckCreateQueue();
			var tobePreloadCount = count - queue.Count;

			for (int i= 0; i< tobePreloadCount ; ++i)
			{
				var cloned = _CloneOne();
				if (null != initAction)
				{
					initAction(cloned);
				}

				Recycle(cloned);
			}
		}

		public GameObject Spawn (bool isActivate)
		{
			GameObject go;
			var queue = _goQueue;

			if (null != queue && queue.Count > 0)
			{
				go = queue.Dequeue() as GameObject;
			}
			else 
			{
				go = _CloneOne();
			}

			if (null != go)
			{
				go.SetActive(isActivate);
			}

			return go;
		}

		private GameObject _CloneOne ()
		{
			const bool reserveName = true;
			var cloned = _mainAsset.CloneEx(reserveName);

			if (null != cloned && null != _webPrefab)
			{
				var script = cloned.GetComponent<MBPrefabAid>();
				if (null != script)
				{
					script._Init();
				}
			}

			return cloned;
		}

		public WebPrefab GetWebPrefab ()
		{
			return _webPrefab;
		}

		public GameObject GetMainAsset ()
		{
			return _mainAsset;
		}

		public override string ToString ()
		{
			return string.Format("[InnerPrefabPool: mainAsset= {0}, referCount={1}]"
				, null != _mainAsset? _mainAsset.name: string.Empty
				, GetReference().ToString());
		}

		// 必須要保留_webPrefab引用，原因是PrefabPool創建後不一定會調用Spawn()，如果不保留_webPrefab，則InnerWebPrefab
		// 會在PrefabPool不知情的情況下被回收

		private WebPrefab   _webPrefab;
		private GameObject	_mainAsset;

		public string localPath { get; set;}
		// 将缓存的结构由stack改为queue，是为了缓解粒子特效收尾的问题，希望放进去的特效延迟一点时间再拿出来用
		private Queue _goQueue;
	}
}
