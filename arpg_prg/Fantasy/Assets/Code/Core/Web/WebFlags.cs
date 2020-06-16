using System;

namespace Core.Web
{
	public enum WebFlags: ushort
	{
		None	= 0,
		NewWWW	= 1 << 0,		// force new WWW().

		DontCache = 1 << 1,		// dispose immediately when callback func executed

		LowPriority = 1 << 2,
		HighPriority = 1 << 3,
		SequentialLoading = 1 << 4, //assetBundles with the same file name should load sequentially, such as "animation clip".
		UnloadAllLoadedObjects = 1 << 5,
	}
}