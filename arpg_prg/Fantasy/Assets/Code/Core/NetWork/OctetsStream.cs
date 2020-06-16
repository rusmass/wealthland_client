using System;

namespace Net
{
	public class OctetsStream : Octets
	{
		public int readableBytes()
		{
			return Math.Max (0, count - _readerIndex);
		}

		public void setReaderIndex(int index)
		{
			_readerIndex = 0;
		}

		public int readerIndex()
		{
			return _readerIndex;
		}

		public void markReaderIndex()
		{
			_markReaderIndex = _readerIndex;
		}

		public void resetReaderIndex()
		{
			_readerIndex = _markReaderIndex;
		}

		public int readInt()
		{
			var afterRead = _readerIndex + 4;
			if (afterRead > count) 
			{
				Console.Error.WriteLine ("[OctetsStream.readInt] Error afterRead > count!");
			}
			int num = 0;
			for (int i = _readerIndex; i < afterRead; ++i) 
			{
				num <<= 8;
				num |= buffer [i];
			}

			_readerIndex = afterRead;
			return num;
		}

		public void readBytes(ref byte[] bytes)
		{
			if (count <= 0 || count <= _readerIndex) 
			{
				bytes = EMPTY;
			}
			else if (bytes.Length > (count - _readerIndex))
			{
				Console.Error.WriteLine ("[OctetsStream.readBytes] Error bytes.length > (count - _readerIndex)");
			}

			Buffer.BlockCopy(buffer, _readerIndex, bytes, 0, bytes.Length);
			_readerIndex += bytes.Length;
		}

		private int _readerIndex;
		private int _markReaderIndex;
	}
}

