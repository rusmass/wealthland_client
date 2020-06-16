using System;

namespace Client.GameFSM
{
	public class SelectRoleState:FSMState
	{
		public SelectRoleState (Game content)
			:base(content,FSMStateType.SelecRoleState)
		{
		}

        /// <summary>
        ///  显示选择角色界面
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		protected override void _OnEnter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State lastState)
		{
			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIChooseRoleWindowController>();
			control.setVisible(true);
		
		}

		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State nextState)
		{
			
		}

        /// <summary>
        /// 接受事件，切换状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override Core.FSM.FiniteStateMachine<Game>.State _DoEvent (Core.FSM.Event e)
		{
			switch((FSMEventType)e.ID)
			{
			case FSMEventType.GameHallEvent:
				return new GameHallState(_Content);
			default:
				break;
			}
			return this;
		}



		protected override Core.FSM.FiniteStateMachine<Game>.State _DoTick (float deltaTime)
		{
			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIChooseRoleWindowController>();
			if(control.IsSelectedIngame==true)
			{
				control.IsSelectedIngame = false;
				return new LoadingState(_Content);
			}
			return this;

		}
	}
}

