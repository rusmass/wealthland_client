using System;
using Client.Unit;

namespace Client.UnitFSM
{
    /// <summary>
    /// 掷筛子 状态
    /// </summary>
	public class RollState : FSMState
	{
		public RollState (Room content)
			:base(content, FSMStateType.RollState)
		{
		}

        /// <summary>
        /// 接受状态机事件，如果是stay，切换到站的状态，如果walk，切换到走的状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override Core.FSM.FiniteStateMachine<Room>.State _DoEvent (Core.FSM.Event e)
		{
			switch ((FSMEventType)e.ID) 
			{
			case FSMEventType.StayEvent:
				return new StayState (_Content);
			case FSMEventType.WalkEvent:
				return new WalkState (_Content);
            
			default :
				break;
			}
			return this;
		}

        /// <summary>
        ///  进入掷筛子的状态，调用的函数
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
		{
		}

        /// <summary>
        ///  退出当前状态时，ui界面掷筛子
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nextState"></param>
		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
		{
			// ytf0927 所有掷色子都可以看到 
			var controller = UIControllerManager.Instance.GetController<Client.UI.UIBattleController> ();
			if (null != controller) 
			{
				if(null != _Content.CurrentPointsArr)
				{
					controller.Re_RequestRollArrs (_Content.CurrentPointsArr);
				}
				else
				{
					controller.Re_RequestRoll (_Content.CurrentPoints);
				}				   
			}
			return;
//            if (_Content.CurrentPlayerIndex == 0)
//            {
//                var controller = UIControllerManager.Instance.GetController<Client.UI.UIBattleController> ();
//			    if (null != controller) 
//			    {
//					if(null != _Content.CurrentPointsArr)
//					{
//						
//						controller.Re_RequestRollArrs (_Content.CurrentPointsArr);
//					}
//					else
//					{
//						controller.Re_RequestRoll (_Content.CurrentPoints);
//					}				   
//			    }
//            }
//			else
//			{
//				var playerInfor= PlayerManager.Instance.Players[_Content.CurrentPlayerIndex];
//				if (null != playerInfor)
//				{
//					if (playerInfor.isThreeRoll == true)
//					{
//						playerInfor.isThreeRoll = false;
//					}
//				}
//			}
		}

		protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick (float deltaTime)
		{
			return this;
		}
	}
}

