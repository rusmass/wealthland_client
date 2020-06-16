using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace Net
{
	[ProtoContract]
	public class EnterRoomRequest : SocketMessage
	{
		[ProtoMember(2)]
		public int roomID;

		[ProtoMember(3)]
		public int playerID;

		[ProtoMember(4)]
		public List<int> list = new List<int>();
	}

	[ProtoContract]
	public class EnterRoomRespond : SocketMessage
	{
		[ProtoMember(2)]
		public int roomID;

		[ProtoMember(3)]
		public int playerID;
	}
}