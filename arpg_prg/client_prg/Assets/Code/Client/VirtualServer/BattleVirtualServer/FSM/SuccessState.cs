using System;
using Core.FSM;

namespace Server.UnitFSM
{
    /// <summary>
    ///  游戏成功
    /// </summary>
    public class SuccessState : FSMState
    {
        public SuccessState(Room content)
            : base(content, FSMStateType.SuccessState)
        {

        }

        public override void Enter(Event e, FSM.State lastState)
        {
            Console.WriteLine("Enter SuccessState!");
            VirtualServer.Instance.Send_NewSuccessState();
        }

        protected override void _OnExit(Event e, FSM.State nextState)
        {

        }

        protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return this;
        }
    }
}

