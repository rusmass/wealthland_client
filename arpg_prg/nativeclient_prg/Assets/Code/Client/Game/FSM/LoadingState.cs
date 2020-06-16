using System;
using Core;
using Core.Web;
using Core.FSM;
using Client.Scenes;
using Client;
using Metadata;

namespace Client.GameFSM
{
	/// <summary>
	/// Loading state.游戏开始界面
	/// </summary>
	public class LoadingState : FSMState
	{
		public LoadingState (Game content)
			:base(content, FSMStateType.LoadingState)
		{
			
		}

        /// <summary>
        ///  进入状态，开始游戏
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter (Event e, FSM.State lastState)
		{
            var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController>();
            control.setVisible(true);

            VirtualServer.Instance.Handle_RequestBuildRoom(133);
            SceneManager.Instance.Send_RequestEnterScene(0, null);

//			Console.Error.WriteLine ("游戏开始是了,创建房间了");
        }

		protected override void _OnExit (Event e, FSM.State nextState)
		{
			Console.WriteLine ("开始loading 了");
		}

        /// <summary>
        ///  接受事件，切换状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override Core.FSM.FiniteStateMachine<Game>.State _DoEvent(Core.FSM.Event e)
		{
			switch((FSMEventType)e.ID)
			{
			case FSMEventType.ReStartGameEvent:
				return new StartState(_Content);

			case FSMEventType.GameHallEvent:
				return new GameHallState (_Content);
            case FSMEventType.LoginEvent:
                return new LoginState(_Content);

                default:
				break;
			}
			return this;
		}

		protected override FSM.State _DoTick (float deltaTime)
		{
			return this;
		}

	}
}

