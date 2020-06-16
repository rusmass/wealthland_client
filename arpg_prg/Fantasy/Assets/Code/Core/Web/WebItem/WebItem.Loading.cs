using System;
using System.Collections;

namespace Core.Web
{
	public partial class WebItem
	{
		private static void _Loading(WebItem item)
		{
			if (null != item) 
			{
				_Enqueue(item);
				if (_currentLoadingCount < _maxLoadingCount)
				{
					var isSequentialRoutine = false;
					if (!_hasSequentialRoutine)
					{
						isSequentialRoutine = true;
						_hasSequentialRoutine = true;
					}

					CoroutineManager.StartCoroutine(_CoLoadDiskItems(isSequentialRoutine));
				}
			}
		}

		private static IEnumerator _CoLoadDiskItems(bool isSequentialRoutine)
		{
			++_currentLoadingCount;
			while (true) 
			{
				var web = _Dequeue(isSequentialRoutine);
				
				if (null == web)
				{
					break;
				}

				if (!web.isKilled && !web.IsDisposed())
				{
					web._CreateWeb();
					if (!web.isDone) 
					{
						yield return web;
					}

					web._nodeState.isLoaded = true;

					if (!web.isKilled)
					{
						web._HandCallBack();
					}
				}
			}

			if (isSequentialRoutine) 
			{
				_hasSequentialRoutine = false;
			}
			--_currentLoadingCount;
		}

		private void _CreateWeb()
		{	
			ACacheItem cacheItem;
			var localPath = argument.localPath;
			if (!_cacheItems.TryGetValue (localPath, out cacheItem)) 
			{
				cacheItem = CacheItemFactory.Create(argument);
				_cacheItems.Add(localPath, cacheItem);
			}

			_SetCacheItem (ref cacheItem);
		}

		public static void SetMaxLoadingCount(int count)
		{
			_maxLoadingCount = Math.Max (1, count);
		}

		private static int _currentLoadingCount;
		private static int _maxLoadingCount = 6;
		private static bool _hasSequentialRoutine;
	}
}