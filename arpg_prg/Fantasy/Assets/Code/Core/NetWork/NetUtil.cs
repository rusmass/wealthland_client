using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
	public static class NetUtil
	{
		public static void Decode(OctetsStream osReadBuffer, byte[] receivedBuffer, int receivedLength)
		{
			osReadBuffer.append (receivedBuffer, 0, receivedLength);

			while (true) 
			{
				if (osReadBuffer.readableBytes () < (long)HeadLength) 
				{					
					break;
				}

				osReadBuffer.markReaderIndex ();
				var len = osReadBuffer.readInt ();
				if (osReadBuffer.readableBytes () < len) 
				{
					osReadBuffer.resetReaderIndex ();
					break;
				}

				var messageID = osReadBuffer.readInt ();
				var bytes = new byte[len - (int)IDLength];
				osReadBuffer.readBytes (ref bytes);

				var action = _actionFactory.Create (messageID);
				action.PutRespondMsg (bytes);

				osReadBuffer.erase (0, len + 4);
				osReadBuffer.setReaderIndex (0);
			}
		}

		public static byte[] Encode(int messageId, SocketMessage bean)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				var one = new SocketMessage () { 
					sessionID = bean.sessionID
				};

				ProtoBuf.Serializer.Serialize<SocketMessage>(ms, one);
				ProtoBuf.Serializer.Serialize<SocketMessage>(ms, bean);
				byte[] data = new byte[ms.Length];
				ms.Position= 0;
				ms.Read(data, 0, data.Length);

				byte[] msg = new byte[HeadLength + IDLength + data.Length];
				_IntToBytes(data.Length + IDLength).CopyTo(msg, 0);
				_IntToBytes(messageId).CopyTo(msg, HeadLength);
				data.CopyTo (msg, HeadLength + IDLength);

				return msg;
			}
		}

		public static T Deserialize<T>(byte[] bytes) where T : SocketMessage
		{
			using(MemoryStream ms = new MemoryStream())
			{
				ms.Write(bytes, 0, bytes.Length);
				ms.Position = 0;

				var message = ProtoBuf.Serializer.Deserialize<T> (ms);
				return message;
			}
		}

		private static int _BytesToInt(byte[] data, int offset = 0)
		{
			int num = 0;
			for (int i = offset; i < offset + 4; i++)
			{
				num <<= 8;
				num |= data[i];
			}
			return num;
		}

		private static byte[] _IntToBytes(int num)
		{
			byte[] bytes = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				bytes[i] = (byte)(num >> (24 - i * 8));
			}

			return bytes;
		}

		private static readonly int HeadLength = 4;
		private static readonly int IDLength = 4;

		private static ActionFactory _actionFactory = ActionFactory.Instance;
	}
}