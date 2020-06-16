using System;
using ProtoBuf;

namespace Net
{
	[ProtoContract]
	public class LoginRequest : SocketMessage
	{
		
	}

	[ProtoContract]
	public class LoginRespond : SocketMessage
	{
		[ProtoMember(1)]
		public int errcode;
	}
}

