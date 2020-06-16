using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Web
{
	public partial class WebItem
	{
		private static void _Enqueue(WebItem item)
		{
			var flags = item.argument.flags;
			if ((flags & WebFlags.SequentialLoading) != 0) 
			{
				_sequentialQueue.Enqueue(item);
			}
			else if ((flags & WebFlags.HighPriority) != 0)
			{
				_highQueue.Enqueue(item);
			}
			else if ((flags & WebFlags.LowPriority) != 0)
			{
				_lowQueue.Enqueue(item);
			}
			else 
			{
				_normalQueue.Enqueue(item);
			}
		}

		private static WebItem _Dequeue(bool isSequentialRoutine)
		{
			if (isSequentialRoutine && _sequentialQueue.Count > 0)
			{
				return _sequentialQueue.Dequeue() as WebItem;
			}
			else if (_highQueue.Count > 0)
			{
				return _highQueue.Dequeue() as WebItem;
			}
			else if (_normalQueue.Count > 0)
			{
				return _normalQueue.Dequeue() as WebItem;
			}
			else if (_lowQueue.Count > 0)
			{
				return _lowQueue.Dequeue() as WebItem;
			}

			return null;
		}

		private static readonly Queue _normalQueue = new Queue();
		private static readonly Queue _lowQueue = new Queue();
		private static readonly Queue _highQueue = new Queue();
		private static readonly Queue _sequentialQueue = new Queue();
	}
}