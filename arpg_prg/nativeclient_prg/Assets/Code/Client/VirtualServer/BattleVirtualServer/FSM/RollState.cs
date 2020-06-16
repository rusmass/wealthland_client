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
            Console.WriteLine("Enter RollState");
            int points = MathUtility.Random(1, 6);

            var arr = new int[] { 0, 0, 0 };
            var isThreeRoll = false;
            var heroInfor = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];

            if (heroInfor.isThreeRoll == true)
            {
                var tmpArr = new int[] { 0, 0, 0, 0, 0, 0, 0 };

                points = 0;
                isThreeRoll = true;
                //单机模式掷两个筛子arr.Length
                for (var i = 0; i <2 ; i++)
                {
                    var tmpPoint = MathUtility.Random(1, 6);

                    while (tmpArr[tmpPoint] == 1)
                    {
                        tmpPoint = MathUtility.Random(1, 6);
                    }
                    tmpArr[tmpPoint] = 1;
                    arr[i] = tmpPoint;
                    points += tmpPoint;
                }
            }
            //如果是联机玩家
            if (GameModel.GetInstance.isPlayNet == true)
            {
                var tmparr = GameModel.GetInstance.curRollPoints;               
                if (tmparr.Count == 3)
                {
                    points = 0;

                    for (var i = 0; i < arr.Length; i++)
                    {
                        points += tmparr[i];
                        arr[i] = tmparr[i];
                    }
                    isThreeRoll = true;
                    heroInfor.isThreeRoll = true;
                    //Console.Error.WriteLine("当前三个筛子的点数和是:"+ points.ToString());
                }
                else
                {
                    isThreeRoll = false;
                    heroInfor.isThreeRoll = false;
                    points = tmparr[0];
                    //Console.Error.WriteLine("当前的点数是："+points.ToString());
                }
            }
            //points = 18;
            _Content.players[_Content.CurrentPlayerIndex].RollPoints = points;
            heroInfor.AddRollPoint(points);
            if (isThreeRoll == true)
            {
                VirtualServer.Instance.Send_NewRollState(points, arr);
            }
            else
            {
                VirtualServer.Instance.Send_NewRollState(points);
            }
            
            _timer = new Counter(0.58f);
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

