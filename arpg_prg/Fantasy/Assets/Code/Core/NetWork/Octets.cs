using System;

namespace Net
{
	public class Octets
	{
		public Octets append(byte b)
		{
			reverse(count + 1);
			buffer[count++] = b;
			return this;
		}

		public Octets append(byte[] data, int pos, int size)
		{
			int len = data.Length;
			pos = Math.Max (0, pos);
			if (size <= 0 || pos >= len) return this;
				
			len -= pos;
			if (size > len) size = len;

			reverse(count + size);
			Buffer.BlockCopy(data, pos, buffer, count, size);
			count += size;
			return this;
		}

		public Octets append(byte[] data)
		{
			return append(data, 0, data.Length);
		}

		public Octets append(Octets o)
		{
			return append(o.buffer, 0, o.count);
		}

		public void clear()
		{
			count = 0;
		}

		public byte[] array()
		{
			return buffer;
		}

		public int size()
		{
			return count;
		}

		public Octets erase(int from, int to)
		{
			if(from < 0) from = 0;
			if(from >= count || from >= to) return this;
			if(to >= count) count = from;
			else
			{
				count -= to;
				Buffer.BlockCopy(buffer, to, buffer, from, count);
				count += from;
			}
			return this;
		}

		private void reverse(int size)
		{
			if(size > buffer.Length)
			{
				int cap = DEFAULT_SIZE;
				while (size > cap) cap <<= 1;

				byte[] buf = new byte[cap];
				if (count > 0) 
				{
					Buffer.BlockCopy(buffer, 0, buf, 0, count);
				}
				buffer = buf;
			}
		}

		protected int count;
		protected byte[] buffer = EMPTY;
		private const int DEFAULT_SIZE = 16;
		public static readonly byte[] EMPTY = new byte[0];
	}
}

