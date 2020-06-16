
using System;

namespace Client.GameFSM
{
    /// <summary>
    ///  登录状态
    /// </summary>
	public class LoginState:FSMState
	{
		public LoginState (Game content)
			:base(content,FSMStateType.LoginState)
		{
			
		}

        /// <summary>
        ///  进入登录状态，显示引导界面
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State lastState)
		{
//			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UILoginController>();
//			control.setVisible(true);

			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIStartGuildWindowController> ();
			control.setVisible (true);
		}

		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State nextState)
		{
			
		}

        /// <summary>
        /// 接受事件，切换到游戏大厅
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override Core.FSM.FiniteStateMachine<Game>.State _DoEvent (Core.FSM.Event e)
		{
			switch((FSMEventType)e.ID)
			{
			//case FSMEventType.SelectRoleEvent:
				//return new SelectRoleState(_Content);

			case FSMEventType.GameHallEvent:
				return new GameHallState(_Content);

			default:
				break;
			}
			return this;
		}

		protected override Core.FSM.FiniteStateMachine<Game>.State _DoTick (float deltaTime)
		{
			return this;
		}
	}
}

