using System;
using Core.FSM;

namespace Server.UnitFSM
{
    /// <summary>
    /// 放弃卡牌，切换下一个玩家
    /// </summary>
    public class GiveUpState : FSMState
    {
        public GiveUpState(Room content)
            : base(content, FSMStateType.GiveUpState)
        {

        }

        public override void Enter(Event e, FSM.State lastState)
        {
            //玩家在规定时间内，没有摇出骰子，视为放弃
            //放弃后，下名玩家 进入StayState
            //服务器通知玩家可以要骰子了
			Console.WriteLine ("某玩家放弃了 玩家index = {0}", _Content.CurrentPlayerIndex);
			//告知所有玩家，新玩家开始，玩家可以摇骰子
        }

        protected override void _OnExit(Event e, FSM.State nextState)
        {

        }

        protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return new StayState(_Content) ;
        }
    }
}

