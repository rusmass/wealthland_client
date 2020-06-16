using System;
using System.Collections;

namespace Core
{
	internal class CoroutinePool
	{
		public CoroutinePool()
		{
			_capacity = 32;
			_items = new CoroutineItem[_capacity];
		}

		public void Spawn(IEnumerator routine, bool isRecyclable)
		{
			AssertTools.IsNotNull (routine);

			CoroutineItem item;
			var checkIndex = _size;
			if (checkIndex < _capacity && null != _items [checkIndex]) 
			{
				item = _items [checkIndex];
				item.isDone = false;
				item.isKilled = false;
			}
			else 
			{
				if (checkIndex + 1 > _capacity)
				{
					Array.Resize(ref _items, _capacity + 16);
				}

				item = new CoroutineItem();
				_items[checkIndex] = item;
			}

			++_size;
			item.routine = routine;
			item.isRecyclable = isRecyclable;
		}

		public void Recycle()
		{
			int count = _size;
			int i = 0;

			for (; i < count; ++i) 
			{
				var item = _items[i];
				if (item.isDone || item.isKilled)
				{
					item.routine = null;
					break;
				}
			}

			if (i < count) 
			{
				for (int j = i + 1; j < count; ++j)
				{
					var item = _items[j];
					if (item.isDone || item.isKilled)
					{
						if (item.isRecyclable)
						{
							item.routine = null;
						}
						else
						{
							_items[j] = null;
						}
					}
					else
					{
						os.swap(ref _items[i++], ref _items[j]);
					}
				}

				_size = i;
			}
		}

		public CoroutineItem this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				_items[index] = value;
			}
		}

		public void Clear()
		{
			_size = 0;
		}

		public int Count { get { return _size; } }

		private int _size;
		private int _capacity;
		private CoroutineItem[] _items;
	}
}
