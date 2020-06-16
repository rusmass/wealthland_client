using System;
using Client.Unit;
using Core.FSM;
using Client.UI;
//using UnityEngine;

namespace Client.UnitFSM
{
    /// <summary>
    /// 站立
    /// </summary>
	public class StayState : FSMState
	{
		public StayState (Room content) 
			:base(content, FSMStateType.StayState)
		{
			
		}

        /// <summary>
        ///  进入站立状态，当前的玩家显示掷筛子
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
        public override void Enter (Event e, FSM.State lastState)
		{
            _controller = UIControllerManager.Instance.GetController<UIBattleController>();

            if (_Content.CurrentPlayerIndex != 0) 
			{
				if (null != _controller) 
				{
					_controller.ShowCraps(false);
                }

				if (GameModel.GetInstance.isPlayNet == false)
				{
					BattleController.Instance.Send_RequestRoll();
				}               
            }
			else
			{
                if (null != _controller)
                {
                    _controller.ShowCraps(true);
                }
            }
			//每次进入这个接口就开始转换人物角色
			Console.WriteLine ("lalalalla----换人啦");		
            var args = new GameEventRoleTurnChanged(_Content.CurrentPlayerIndex);
            GameEventManager.Instance.FireEvent(args);
		}

		protected override void _OnExit (Event e, FSM.State nextState)
		{
			
		}

        /// <summary>
        /// 接收状态机事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override Core.FSM.FiniteStateMachine<Room>.State _DoEvent (Core.FSM.Event e)
		{
			switch ((FSMEventType)e.ID)
			{
			case FSMEventType.RollEvent:
				//				Console.Error.WriteLine ("进入掷骰子");
				return new RollState(_Content);
			case FSMEventType.StayEvent:
				return new StayState(_Content);
			default:
				break;
			}

			return this;
		}

		protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return this;
        }

        private UIBattleController _controller;
    }
}

