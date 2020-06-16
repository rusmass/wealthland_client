using System;

namespace Client.GameFSM
{
    /// <summary>
    ///  状态机的状态
    /// </summary>
	public class FSMState : FSM.State
	{
		public FSMState(Game content, FSMStateType type)
			:base(content)
		{
			_stateType = type;
		}

		public FSMStateType StateType { get { return _stateType; } }
		private FSMStateType _stateType;
	}
}

