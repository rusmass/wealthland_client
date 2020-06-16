using System;

namespace Net
{
	public class EnterRoomAction : LogicAction<EnterRoomRequest, EnterRoomRespond>
	{
		public EnterRoomAction() : base(1)
		{
			
		}

		public override void OnRequest (EnterRoomRequest request, object userdata)
		{
			request.sessionID = 1111;
			request.roomID = 2222;
			request.playerID = 3333;

			request.list.Add (4444);
		}

		public override void OnRespond (EnterRoomRespond response, object userdata)
		{
			Console.WriteLine (response.roomID);
		}
	}
}

