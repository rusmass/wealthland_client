using System;
using Core.FSM;

namespace Client.GameFSM
{
    /// <summary>
    ///  状态机，切换不同场景，游戏状体
    /// </summary>
    public class FSM : FiniteStateMachine<Game>
	{
		public FSM(Game content)
			:base(content)
		{
			_enterStateConstructor = _CreateEnterState;
		}

        /// <summary>
        ///  返回当前的状态机类型 ，暂时未引用
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
        /// 创建默认启动的状态时开始界面
        /// </summary>
        /// <returns></returns>
		private FSMState _CreateEnterState()
		{
			return new StartState (_Content);
		}
	}
}

