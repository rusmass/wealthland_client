using System;
using ProtoBuf;

namespace Net
{
	[ProtoContract]
	public class SocketMessage
	{
		[ProtoMember(1)]
		public int sessionID;
	}
}

