using System;

namespace Client.GameFSM
{
	public class NetSelectRoleState:FSMState
	{
        /// <summary>
        ///  未使用
        /// </summary>
        /// <param name="content"></param>
		public NetSelectRoleState (Game content):base(content,FSMStateType.NetSelectRoleEvent)
		{
		}

		protected override void _OnEnter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State lastState)
		{
//			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIChooseRoleNetWindowController>();
//			control.setVisible(true);
		}

		protected override Core.FSM.FiniteStateMachine<Game>.State _DoEvent (Core.FSM.Event e)
		{
			switch((FSMEventType)e.ID)
			{
			case FSMEventType.GameHallEvent:
//				return new GameHallState(_Content);
			case FSMEventType.LoadingStateEvent:
//				return new LoadingState (_Content);			
			default:
				break;
			}
			return this;
		}


		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State nextState)
		{
			//base._OnExit (e, nextState);
		}

	}
}

