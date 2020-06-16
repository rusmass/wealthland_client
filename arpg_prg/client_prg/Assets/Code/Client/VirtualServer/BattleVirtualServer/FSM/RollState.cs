using System;
using Client;

namespace Server.UnitFSM
{
    /// <summary>
    /// 掷筛子
    /// </summary>
	public class RollState : FSMState
	{
		public RollState (Room content)
			:base(content, FSMStateType.RollState)
		{
		}

        /// <summary>
        /// 处理掷筛子数据，单机版随机点数，网络版读取后台数据，掷筛子的个数
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
		public override void Enter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
		{
			Console.WriteLine ("Enter RollState");
			int points = MathUtility.Random (1, 6);

			var arr = new int[]{ 0, 0, 0 };
			var isThreeRoll = false;
			var heroInfor=PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];

			if (heroInfor.isThreeRoll == true)
			{
				var tmpArr=new int[]{0,0,0,0,0,0,0};
				
				points = 0;
				isThreeRoll = true;
				for (var i = 0; i < arr.Length; i++)
				{
					var tmpPoint=MathUtility.Random (1, 6);

					while (tmpArr [tmpPoint] == 1)
					{
						tmpPoint=MathUtility.Random (1, 6);
					}
					tmpArr [tmpPoint] = 1;
					arr[i]=tmpPoint;
					points += tmpPoint;
				}
			}
			//如果是联机玩家
			if (GameModel.GetInstance.isPlayNet == true)
			{
				var  tmparr = GameModel.GetInstance.curRollPoints;
				if (tmparr.Count == 3)
				{
					points = 0;
							
					for (var i = 0; i < arr.Length; i++)
					{
						points += tmparr[i];
						arr[i]=tmparr[i];
					}
					isThreeRoll = true;
					heroInfor.isThreeRoll = true;
				}
				else
				{
					isThreeRoll = false;
					heroInfor.isThreeRoll = false;
					points = tmparr[0];
				}
			}

			if(GameDebugHelper.Instance.IsDebugRoll==true)
			{
				points = GameDebugHelper.Instance.DebugRollNumber;
				GameDebugHelper.Instance.IsDebugRoll = false;
			}


			_Content.players [_Content.CurrentPlayerIndex].RollPoints = points;

			if(isThreeRoll==true)
			{
				VirtualServer.Instance.Send_NewRollState (points,arr);
			}
			else
			{
				VirtualServer.Instance.Send_NewRollState (points);
			}
			//VirtualServer.Instance.RollStateTime
			_timer = new Counter (0.58f);
		}

		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
		{
			Console.WriteLine ("Exit RollState");
		}

		protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick (float deltaTime)
		{
			if (null != _timer)
			{
				if (_timer.Increase (deltaTime)) 
				{
					_timer = null;
					return new WalkState (_Content);
				}
			}

			return this;
		}

		private Counter _timer;
	}
}

