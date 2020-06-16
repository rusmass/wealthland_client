using Core;
using Core.FSM;
using Client.Cameras;

namespace Client.CamerasFSM
{
    public class SideScrollingState : FSMState
    {
		public SideScrollingState(SmartCamera content)
			: base(content, FSMStateType.SideScrolling)
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
				case FSMEventType.Stay: return new StayState(_Content);
                default: break;
            }
            return this;
        }

		protected override FSM.State _DoTick(float deltaTime)
		{
			TransformObject target = _Content.FollowTarget;
			if (target == null)
			{
				return new StayState(_Content);
			}

			_TryInit();

			var targetPos = target.GetPosition();
			targetPos.z = 0;
			targetPos += _Content.FollowRelativePosition + _Content.FollowRelativeRotation * UnityEngine.Vector3.back * _Content.FollowDistance;
			_Content.TranslateTo(targetPos, _Content.FollowSmoothTime);

			return this;
        }

		private void _TryInit()
		{
			if (_inited)
				return;
			_inited = true;
			_Content.SetPosition(_Content.FollowRelativePosition + _Content.FollowRelativeRotation * UnityEngine.Vector3.back * _Content.FollowDistance);
			_Content.SetRotation(_Content.FollowRelativeRotation);
		}

		private bool _inited = false;
    }
}
