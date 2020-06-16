using System;
using Core.FSM;

namespace Client.UnitFSM
{
    /// <summary>
    /// 放弃卡牌
    /// </summary>
	public class GiveUpState : FSMState
	{
		public GiveUpState(Room content)
			: base(content, FSMStateType.GiveUpState)
		{

		}

        /// <summary>
        /// 进入当前状态
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter(Event e, FSM.State lastState)
		{
			
		}

        /// <summary>
        /// 退出当前状态时执行的方法
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nextState"></param>
		protected override void _OnExit(Event e, FSM.State nextState)
		{

		}

        /// <summary>
        ///  根据收到事件，进行处理。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override FiniteStateMachine<Room>.State _DoEvent (Event e)
		{
			switch ((FSMEventType)e.ID) 
			{
			case FSMEventType.StayEvent://如果收到stayEvent，转到站立
				return new StayState (_Content);
			case FSMEventType.WalkEvent://如果收到walkEvent,转到行走
				return new WalkState (_Content);

			default :
				break;
			}

			return this;
		}

        /// <summary>
        /// tick 帧频执行，返回stat状态
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
		protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
		{
			return new StayState(_Content) ;
		}
	}
}

