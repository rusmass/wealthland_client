using System;

namespace Client.GameFSM
{
    /// <summary>
    ///  游戏大厅
    /// </summary>
	public class GameHallState:FSMState
	{
		public GameHallState (Game content):base(content,FSMStateType.GameHallState)
		{
			
		}

        /// <summary>
        ///  进入状态，显示游戏大厅界面
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		protected override void _OnEnter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State lastState)
		{
            //base._OnEnter (e, lastState);

            //VirtualServer.Instance.Dispose();
            //Client.Unit.BattleController.Instance.Dispose();
            //SceneManager.Instance.Send_RequestEnterScene(0, null);

            //Audio.AudioManager.Instance.StartMusic();
            var gameHalll = UIControllerManager.Instance.GetController<Client.UI.UIGameHallWindowController> ();
			gameHalll.setVisible (true);
            //Audio.AudioManager.Instance.StartMusic();
           // Console.Error.WriteLine("woqule，进入游戏大厅案例");
        }

        /// <summary>
        ///  接受事件，切换状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override Core.FSM.FiniteStateMachine<Game>.State _DoEvent (Core.FSM.Event e)
		{
			switch((FSMEventType)e.ID)
			{
			case FSMEventType.SelectRoleEvent:
				return new SelectRoleState(_Content);
			case FSMEventType.LoadingStateEvent:
				return new LoadingState (_Content);
			case FSMEventType.LoginEvent:
				return new LoginState (_Content);
//			case FSMEventType.NetSelectRoleEvent:
//				return new NetSelectRoleState (_Content);
			default:
				break;
			}
			return this;
		}


		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Game>.State nextState)
		{
			//base._OnExit (e, nextState);
		}

		//protected override Core.FSM.FiniteStateMachine<Game>.State _DoTick (float deltaTime)
		//{
			//return base._DoTick (deltaTime);
		//}
	}
}

