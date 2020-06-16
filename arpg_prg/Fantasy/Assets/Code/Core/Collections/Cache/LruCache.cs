using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Collections
{
	internal partial class LruCache<TKey, TValue> where TValue: IDisposable, IIsDisposable 
	{
		public LruCache(int capacity)
		{
			_capacity = capacity > 0 ? capacity : DefaultCapacity;
			_dict = new Dictionary<TKey, TValue> ();
			_linkedList = new LinkedList<TKey> ();
		}

		public void Add(TKey key, TValue value)
		{
			AssertTools.IsNotNull (key);
			_dict[key] = value;
			_linkedList.Remove(key);
			_linkedList.AddFirst(key);

			if (_linkedList.Count > _capacity)
			{
				//Try to remove last
				Remove(_linkedList.Last.Value);
			}
		}

		public bool Remove(TKey key)
		{
			if (null == key)
			{
				return false;
			}

			var removed = false;
			var value = _dict[key];
			if (_IsDiaposableItem(value))
			{
				_dict.Remove(key);
				_linkedList.Remove(key);
				value.DisposeEx();
				removed = true;
			}

			return removed;
		}

		public bool ContainsKey(TKey key)
		{
			return _dict.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			var success = _dict.TryGetValue(key, out value);
			if (success)
			{
				_linkedList.Remove(key);
				_linkedList.AddFirst(key);
			}

			return success;
		}

		public void TrimExcess(Action<TValue> OnItemRemoved = null)
		{
			var cur = _linkedList.First;

			while(null != cur)
			{
				var value = _dict[cur.Value];
				var next = cur.Next;
				if (_IsDiaposableItem(value))
				{
					_linkedList.Remove(cur.Value);
					_dict.Remove(cur.Value);

					var checkable = value as IDisposable;
					checkable.DisposeEx();

					if (null != OnItemRemoved)
					{
						OnItemRemoved(value);
					}
				}

				cur = next;
			}
		}

		internal void Clear()
		{
			if (Count > 0)
			{
				var iter = _dict.GetEnumerator();
				while (iter.MoveNext())
				{
					var v = iter.Current.Value;

					var disposable = v as IDisposable;
					if (null == disposable)
					{
						break;
					}

					disposable.Dispose();
				}

				_dict.Clear();
				_linkedList.Clear();
			}
		}

		private bool _IsDiaposableItem(TValue value)
		{
			if (null == value) 
			{
				return true;
			}

			var checkable = value as IIsDisposable;
			return checkable.IsDisposable ();
		}

		public int Count { get { return _dict.Count; } }

		private int _capacity;
		private const int DefaultCapacity = 255;
		private readonly Dictionary<TKey, TValue> _dict;
		private readonly LinkedList<TKey> _linkedList;
	}
}