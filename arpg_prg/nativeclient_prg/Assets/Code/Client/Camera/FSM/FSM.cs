using Core.FSM;
using Client.Cameras;

namespace Client.CamerasFSM
{
	public enum FSMEventType
	{
		Tick = -1,
		Stay,
		Follow,
		SideScrolling,
	}

	public enum FSMStateType
	{
		Stay,
		Follow,
		SideScrolling,
	}

	public class FSMEvent : Core.FSM.Event
	{
		public FSMEvent(FSMEventType type)
			: base((int)type)
		{
		}
	}

	public class StayEvent : FSMEvent
	{
		public StayEvent()
			: base(FSMEventType.Stay)
		{
		}
	}

	public class FollowEvent : FSMEvent
	{
		public FollowEvent()
			: base(FSMEventType.Follow)
		{
		}
	}

	public class SideScrollingEvent : FSMEvent
	{
		public SideScrollingEvent()
			: base(FSMEventType.SideScrolling)
		{
		}
	}

	public class FSM : FiniteStateMachine<SmartCamera>
	{
		public FSM(SmartCamera content)
			: base(content)
		{
			_enterStateConstructor = _CreateEnterState;
		}

		public FSMStateType CurrentStateType
		{
			get
			{
				FSMState state = _currentState as FSMState;
				if (state != null)
				{
					return state.StateType;
				}
				return FSMStateType.Stay;
			}
		}

		private FSMState _CreateEnterState()
		{
			return new StayState(_Content);
		}
	}
}
