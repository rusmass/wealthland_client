using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;
using Core;

namespace Net
{
	public class NetSession
	{
		public void Connect()
		{
			Connect (_OnConnected);
		}

		private void Connect(Action OnConnected)
		{
			var sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
			sock.SendBufferSize    	= 8192;
			sock.ReceiveBufferSize	= 8192;
			sock.SendTimeout		= 2000;
			sock.ReceiveTimeout 	= 2000;
			sock.NoDelay			= true;

			sock.BeginConnect("127.0.0.1", 8501, _ConnectCallback, OnConnected);
			_sock = sock;
		}

		public void DisConnect()
		{
			if (null != _sock) 
			{
				_sock.Close ();
				_sock = null;
			}
		}

		private void _OnConnected ()
		{
			
		}

		private void _ConnectCallback (IAsyncResult result)
		{
			try
			{
				var sock = _sock;
				sock.EndConnect(result);

				if (!sock.Connected)
				{
					Console.Error.WriteLine("[_ConnectCallback()] Unable to connect to host.");
					return;
				}

				var OnConnected = result.AsyncState as Action;
				Loom.QueueOnMainThread(()=> OnConnected());

				var netThread = new Thread(_NetworkThread);
				netThread.Start();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine ("[NetSession._ConnectCallback] Error :" + ex.ToStringEx());
			}
		}

		private void _NetworkThread ()
		{
			try
			{
				var sock = _sock;
				sock.Blocking = false;

				var readBuffer = new byte[4096];
				var osReadBuffer = new OctetsStream();
				var osWriteBuffer = new OctetsStream();

				while(sock.Connected)
				{
					_PollOut(sock, osWriteBuffer);
					_PollIn(sock, osReadBuffer, readBuffer);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine (ex.ToStringEx());
			}
		}

		private void _PollOut (Socket sock, OctetsStream osWriteBuffer)
		{
			if (_osSendBuffer.readableBytes() > 0)
			{
				lock (_sendLocker)
				{
					osWriteBuffer.append(_osSendBuffer);
					_osSendBuffer.clear();
				}
			}

			if (osWriteBuffer.readableBytes() == 0)
			{
				return;
			}

			if (!sock.Poll(_pollWaitTime, SelectMode.SelectWrite))
			{
				return;
			}

			var sendNumber = sock.Send(osWriteBuffer.array(), 0, osWriteBuffer.size(), SocketFlags.None);
			if (sendNumber > 0)
			{
				osWriteBuffer.erase (0, sendNumber);
			}
			else
			{
				throw new SocketException((int) SocketError.Fault);
			}
		}

		private void _PollIn (Socket sock, OctetsStream osReadBuffer, byte[] readBuffer)
		{
			if (!sock.Poll(_pollWaitTime, SelectMode.SelectRead))
			{
				return;
			}

			var readLength = sock.Receive(readBuffer, readBuffer.Length, SocketFlags.None);
			if (readLength == 0)
			{
				throw new SocketException((int) SocketError.ConnectionReset);
			}

			NetUtil.Decode(osReadBuffer, readBuffer, readLength);
		}

		public void DoAction<T>(object param = null)
		{
			var action = _actionFactory.Create<T> ();
			var bytes = action.GetRequestMsg (param);

			_osSendBuffer.append (bytes);
		}

		private Socket _sock;
		private int _pollWaitTime = 1000;

		private readonly object _sendLocker = new object();
		private readonly OctetsStream _osSendBuffer = new OctetsStream();
		private static readonly ActionFactory _actionFactory = ActionFactory.Instance;
	}
}