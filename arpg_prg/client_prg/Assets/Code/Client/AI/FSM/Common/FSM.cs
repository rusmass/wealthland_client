using System;
using Core.FSM;
using Client.Unit;

namespace Client.UnitFSM
{
    /// <summary>
    /// 状态机 ， 玩家处于不同状态后，进行数据的处理
    /// </summary>
	public class FSM : FiniteStateMachine<Room>
	{
		public FSM(Room content)
			:base(content)
		{
			_enterStateConstructor = _CreateEnterState;

		}

        /// <summary>
        ///  返回当前状态机的类型
        /// </summary>
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

        /// <summary>
        ///  返回当前的状态
        /// </summary>
		public FSMState CurrentState
		{
			get 
			{
				return _currentState as FSMState;
			}
		}

        /// <summary>
        ///  状态机初始化时默认的状态
        /// </summary>
        /// <returns></returns>
		private FSMState _CreateEnterState()
		{
			return new StayState(_Content);
		}
	}
}

