using System;
using Client.Unit;
using Client.UI;

namespace Client.UnitFSM
{
    /// <summary>
    ///  升级进入内圈的状态
    /// </summary>
    public class UpGradeState : FSMState
    {
        public UpGradeState(Room content)
            : base(content, FSMStateType.UpGradeState)
        {
        }

       
        /// <summary>
        ///  进入当前状态，显示进内圈的卡牌
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
        public override void Enter(Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
        {
            var func = (e as UpGradeEvent).lpfnUpGradeToInner;
            func();

            CardManager.Instance.OpenCard((int)SpecialCardType.UpGradType);
        }

        protected override void _OnExit(Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
        {
            
        }

        /// <summary>
        ///  接受状态机事件，切换状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override Core.FSM.FiniteStateMachine<Room>.State _DoEvent(Core.FSM.Event e)
        {
            switch ((FSMEventType)e.ID)
            {
                case FSMEventType.StayEvent:
                    return new StayState(_Content);
                case FSMEventType.WalkEvent:
                    return new WalkState(_Content);

                default:
                    break;
            }

            return this;
        }

        protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return this;
        }
    }
}

