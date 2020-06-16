using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtendedList
{
	public static int MoveToEx(this IList srcList, IList dstList)
	{
		if (null != srcList && null != dstList) 
		{
			var count = srcList.Count;
			for (int i = 0; i < count; ++i)
			{
				var item = srcList[i];
				dstList.Add(item);
			}

			srcList.Clear();
			return count;
		}

		return 0;
	}

	public static int MoveToEx(this IList srcList, IList dstList, object locker)
	{
		if (null != srcList && null != dstList && null != locker) 
		{
			lock(locker)
			{
				var count = srcList.Count;
				for (int i = 0; i < count; ++i)
				{
					var item = srcList[i];
					dstList.Add(item);
				}

				srcList.Clear();
				return count;
			}
		}

		return 0;
	}
}