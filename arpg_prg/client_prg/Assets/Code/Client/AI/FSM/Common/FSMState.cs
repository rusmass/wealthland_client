using System;
using Client.Unit;

namespace Client.UnitFSM
{
    /// <summary>
    ///  状态机状态
    /// </summary>
	public class FSMState : FSM.State
	{
		public FSMState (Room content, FSMStateType stateType) 
			:base (content)
		{
			_stateType = stateType;
		}

        /// <summary>
        /// 返回状态的类型
        /// </summary>
		public FSMStateType StateType { get { return _stateType; } }
		private FSMStateType _stateType;
	}
}

