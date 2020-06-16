using System;
using Core.Collections;

namespace Core.Web
{
	internal static class WebTools
	{
		public static void RemoveFromCache<T> (LruCache<string, T> cache, ref T cacheitem) where T : ASmartPointer, ILocalPath
		{
			if (null != cacheitem) 
			{
				cacheitem.RemoveReference();

				if (0 == cacheitem.GetReference())
				{
					cache.Remove(cacheitem.localPath);
					cacheitem = null;
				}
			}
		}
	}
}