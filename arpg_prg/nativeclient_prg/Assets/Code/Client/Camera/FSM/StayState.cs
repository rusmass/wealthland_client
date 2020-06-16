using Core;
using Core.FSM;
using Client.Cameras;

namespace Client.CamerasFSM
{
    public class StayState : FSMState
    {
        public StayState(SmartCamera content)
            : base(content, FSMStateType.Stay)
        {
        }

        protected override void _OnReset()
        {
        }

        protected override void _OnEnter(Event e, FSM.State lastState)
        {
        }

        protected override void _OnExit(Event e, FSM.State nextState)
        {
        }

        protected override FSM.State _DoEvent(Event e)
        {
            switch ((FSMEventType)e.ID)
            {
				case FSMEventType.Follow: return new FollowState(_Content);
				case FSMEventType.SideScrolling: return new SideScrollingState(_Content);

                default: break;
            }
            return this;
        }

        protected override FSM.State _DoTick(float deltaTime)
        {
            return this;
        }
    }
}
