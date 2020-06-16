using System;
using Core.FSM;
using Client;

namespace Server.UnitFSM
{
    /// <summary>
    /// 站立状态，切换当前玩家。单机模式自动切换，网络模式根据后台数据切换
    /// </summary>
	public class StayState : FSMState
	{
		public StayState (Room content) 
			:base(content, FSMStateType.StayState)
		{
            
        }


		public override void Enter (Event e, FSM.State lastState)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				for (var i = 0; i < Client.PlayerManager.Instance.Players.Length; i++)
				{
					if (GameModel.GetInstance.NetCurrentPlayerId == Client.PlayerManager.Instance.Players [i].playerID)
					{
//						Console.Error.WriteLine ("玩家的Id--------"+GameModel.GetInstance.NetCurrentPlayerId);
						_Content.CurrentPlayerIndex = i;
						break;
					}
				}
			}

//			Console.Error.WriteLine ("站立状态下房间的索引值是：-------"+_Content.CurrentPlayerIndex.ToString());

			_Content.CurrentPlayerIndex++;
			Console.WriteLine ("Enter StayState index = {0}",_Content.CurrentPlayerIndex);

			var tmpPlayers = Client.PlayerManager.Instance.Players;
			var tmpLen = _Content.players.Length;
			if (null !=tmpPlayers)
			{				
				if (tmpPlayers[_Content.CurrentPlayerIndex].isEmployment == false)
				{
					tmpPlayers [_Content.CurrentPlayerIndex].isEmployment = true;
					_Content.CurrentPlayerIndex++;
				}
			}

			if (GameModel.GetInstance.isPlayNet == true)
			{
				if (tmpPlayers [_Content.CurrentPlayerIndex].isNetOnline == false ||tmpPlayers [_Content.CurrentPlayerIndex].isReconectGame==true)
				{
					_Content.CurrentPlayerIndex++;
					if (tmpPlayers [_Content.CurrentPlayerIndex].isNetOnline == false||tmpPlayers [_Content.CurrentPlayerIndex].isReconectGame==true)
					{
						_Content.CurrentPlayerIndex++;
						if (tmpPlayers [_Content.CurrentPlayerIndex].isNetOnline == false||tmpPlayers [_Content.CurrentPlayerIndex].isReconectGame==true)
						{
							_Content.CurrentPlayerIndex++;
						}
					}
				}
			}

            _timer = new Counter(VirtualServer.Instance.StayStateTime);
			VirtualServer.Instance.Send_NewStayState (_Content.CurrentPlayerIndex);
        }

		protected override void _OnExit (Event e, FSM.State nextState)
		{
			Console.WriteLine ("Exit StayState");
		}

		protected override FiniteStateMachine<Room>.State _DoEvent (Event e)
		{
			switch ((FSMEventType)e.ID) 
			{
			case FSMEventType.RollEvent:
				return new RollState (_Content);
			case FSMEventType.SelectEvent:


				return this;
			}
			return this;
		}

        protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
			if (null != _timer)
			{
				if (_timer.Increase(deltaTime))
				{
					return new GiveUpState(_Content);
				}
			}
           
            return this;
        }

        private Counter _timer;
    }
}

