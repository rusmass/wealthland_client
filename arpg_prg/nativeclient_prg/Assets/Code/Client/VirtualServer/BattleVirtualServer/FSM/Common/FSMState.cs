using System;
using Client.Unit;

namespace Server.UnitFSM
{
    /// <summary>
    /// 状态机状态
    /// </summary>
	public class FSMState : FSM.State
	{
		public FSMState (Room content, FSMStateType stateType) 
			:base (content)
		{
			_stateType = stateType;
		}

		public FSMStateType StateType { get { return _stateType; } }
		private FSMStateType _stateType;
	}
}

