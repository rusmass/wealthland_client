using System;
using Core.FSM;

namespace Server.UnitFSM
{
    /// <summary>
    /// 逻辑层状态机，判断玩家的不同状态，，控制前端展示
    /// </summary>
	public class FSM : FiniteStateMachine<Room>
	{
		public FSM(Room content)
			:base(content)
		{
			_enterStateConstructor = _CreateEnterState;
		}

		public FSMState CurrentState
		{
			get 
			{
				return _currentState as FSMState;
			}
		}

		public FSMStateType CurrentStateType
		{
			get 
			{
				FSMState state = _currentState as FSMState;
				if (null != state) 
				{
					return state.StateType;
				}

				return FSMStateType.None;
			}
		}

		private FSMState _CreateEnterState()
		{
			return new StayState(_Content);
		}

    }
}

