using System;
using Client.Unit;
using Client.UI;

namespace Client.UnitFSM
{
    /// <summary>
    ///  玩家胜利的状态
    /// </summary>
    public class SuccessState : FSMState
    {
        public SuccessState(Room content)
            : base(content, FSMStateType.SuccessState)
        {
        }

        /// <summary>
        ///  进入玩家胜利状态，打开胜利界面卡牌
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
        public override void Enter(Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
        {
            CardManager.Instance.OpenCard((int)SpecialCardType.SuccessType);
        }

        protected override void _OnExit(Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
        {

        }

        protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return this;
        }
    }
}

