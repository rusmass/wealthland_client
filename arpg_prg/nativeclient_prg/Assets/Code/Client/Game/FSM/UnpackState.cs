using System;
using Core.FSM;

namespace Client.GameFSM
{
	public class UnpackState : FSMState
	{
		public UnpackState (Game content)
			:base(content, FSMStateType.UnpackState)
		{
		}

		protected override FSM.State _DoTick (float deltaTime)
		{
			return new DownloadState (_Content);	
		}
	}
}

