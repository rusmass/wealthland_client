using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtendedIDictionary
{   
	public static Value GetEx<Key, Value>(this IDictionary<Key, Value> dict, Key key)
	{
		return dict.GetEx(key, default(Value));
	}

	public static Value GetEx<Key, Value>(this IDictionary<Key, Value> dict, Key key, Value defaultValue)
	{
		Value v;
		if (null != dict && dict.TryGetValue(key, out v))
		{
			return v;
		}

		return defaultValue;
	}

	public static Value AddEx<Key, Value>(this IDictionary<Key, Value> dict, Key key, Value v)
	{
		if(null != dict)
		{
			dict.Add(key, v);
		}

		return v;
	}
}