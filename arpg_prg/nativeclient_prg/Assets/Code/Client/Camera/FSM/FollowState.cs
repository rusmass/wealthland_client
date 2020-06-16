using Core;
using Core.FSM;
using Client.Cameras;

namespace Client.CamerasFSM
{
    public class FollowState : FSMState
    {
        public FollowState(SmartCamera content)
            : base(content, FSMStateType.Follow)
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
				case FSMEventType.Stay: return new StayState(_Content);
				case FSMEventType.SideScrolling: return new SideScrollingState(_Content);

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

			if (_Content.FollowRelativeTargetPosition)
            {
				UnityEngine.Quaternion followRelativeRotation = _Content.FollowRelativeRotation * _Content.RelativeTargetRotation;
				if (_Content.FollowRelativeTargetRotation)
                {
					var targetPos = target.GetPosition() + target.GetRotation() * _Content.FollowRelativePosition;
                    var dir = target.GetRotation() * followRelativeRotation * UnityEngine.Vector3.back;
					_Content.TranslateTo(targetPos + dir * _Content.FollowDistance, _Content.FollowSmoothTime);
					_Content.RotateTo(target.GetRotation() * followRelativeRotation, _Content.FollowSmoothTime);
                }
                else
                {
					var targetPos = target.GetPosition() + followRelativeRotation * _Content.FollowRelativePosition;
                    var dir = followRelativeRotation * UnityEngine.Vector3.back;
					_Content.TranslateTo(targetPos + dir * _Content.FollowDistance, _Content.FollowSmoothTime);
					_Content.RotateTo(followRelativeRotation, _Content.FollowSmoothTime);
                }
            }
            else
            {
				var dir = target.GetPosition() - _Content.GetPosition();
				if (_Content.FollowRelativeTargetRotation)
                {
					dir += target.GetRotation() * _Content.FollowRelativePosition;
                }
                else
                {
					dir += _Content.FollowRelativePosition;
                }
                var right = UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, dir);
                var up = UnityEngine.Vector3.Cross(dir, right);
                dir.Normalize();
                up.Normalize();
				_Content.RotateTo(dir, up, _Content.FollowSmoothTime);
            }

            return this;
        }
    }
}
