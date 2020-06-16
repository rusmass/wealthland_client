using System;
using System.Collections.Generic;

namespace Core
{
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

		public static Value SetDefaultEx<Key, Value>(this IDictionary<Key, Value> dict, Key key)
		{
			return dict.SetDefaultEx(key, default(Value));
		}

		public static Value SetDefaultEx<Key, Value>(this IDictionary<Key, Value> dict, Key key, Value defaultValue)
		{
			if(null != dict)
			{
				Value value;
				if (!dict.TryGetValue(key, out value))
				{
					value = defaultValue;
					dict.Add(key, value);
				}

				return value;
			}

			return default(Value);
		}

		public static Value AddEx<Key, Value>(this IDictionary<Key, Value> dict, Key key, Value v)
		{
			if(null != dict)
			{
				dict.Add(key, v);
			}

			return v;
		}

		public static void AddRangeEx<Key, Value>(this IDictionary<Key, Value> dictTo, IDictionary<Key, Value> dictFrom) where Key : IComparable
		{
			if(null != dictTo && null != dictFrom)
			{
				var e = dictFrom.GetEnumerator();
				while(e.MoveNext())
				{
					var pair = e.Current;
					dictTo [pair.Key] = pair.Value;
				}
			}
		}
	}
}