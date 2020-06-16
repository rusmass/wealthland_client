using System;

public static class ExtendedDispose
{
	public static void DisposeEx(this IDisposable item)
	{
		if (null != item) 
		{
			item.Dispose();
		}
	}
}