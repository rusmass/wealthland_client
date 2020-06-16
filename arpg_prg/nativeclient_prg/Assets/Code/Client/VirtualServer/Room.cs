using System;
using Server.UnitFSM;
using UnityEngine;

namespace Server
{
	/// <summary>
	/// Room.模拟服务器房间
	/// </summary>
	public class Room
	{
		public Room()
		{
			
		}
        /// <summary>
        /// 刷新执行状态机
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(float deltaTime)
		{
			if (null != _fsm) 
			{
				_fsm.Tick(deltaTime);	
			}
		}
        /// <summary>
        /// 开始执行，启动状态机
        /// </summary>
		public void StartGame()
		{
			_fsm = new FSM(this);
			_fsm.Start ();
		}

        /// <summary>
        /// 状态机置空
        /// </summary>
        public void Dispose()
		{
			if (null != _fsm)
			{
				_fsm.Stop ();
				_fsm = null;
			}
		}
        /// <summary>
        /// 从新开始游戏
        /// </summary>
        public void ReStartGame()
		{
			_currentTurnCount = 0;
			_currentPlayerIndex = -1;

			if (null != _fsm)
			{
				_fsm.Stop ();
				_fsm = null;
			}

			for (var i = 0; i < players.Length; i++)
			{
				var tmpPlayer=players[i];
				tmpPlayer.Level = PlayerLevel.Outer;
				tmpPlayer.CurrentPos = 0;
			}
		  
		}
        /// <summary>
        /// 进入掷筛子状态
        /// </summary>
        /// <param name="roleIndex"></param>
		public void Enter_RollState(int roleIndex)
		{
			if (roleIndex == CurrentPlayerIndex) 
			{
				_fsm.Input (new RollEvent());	
			}
		}

        /// <summary>
        /// 1表示付出钱购买东西 0表示放弃
        /// </summary>
        /// <param name="sel"></param>
        public void Re_SelectedState(int sel)
		{
			if (_fsm.CurrentStateType == FSMStateType.SelectState)
			{
				var state = _fsm.CurrentState as SelectState;
				state.selected = sel;
			}
			else 
			{
				if (Client.GameModel.GetInstance.isPlayNet==true)
				{
					NetGameLostGameToStayNext ();
				}
			}
		}

		/// <summary>
		/// Nets the game lost game to stay next.当前轮到当前玩家，但是当前玩家掉线了，那么自动切换到下一位
		/// </summary>
		public void NetGameLostGameToStayNext()
		{
			if (null != _fsm)
			{
				if (_fsm.CurrentStateType == FSMStateType.StayState) 
				{
					var state = _fsm.CurrentState as StayState;
					state.Enter (null, null);
				}
			}
		}

		/// <summary>
		/// Nets the game lost to upgrad netxt. 轮到当前玩家进内圈，但是当前晚间掉线了，自动处理到下一位
		/// </summary>
		public void NetGameLostToUpgradNetxt()
		{
			if (null != _fsm)
			{
				if (_fsm.CurrentStateType == FSMStateType.UpGradState) 
				{
					UpGradeFinished = true;
					IsUpGrade = true;
//					_fsm.Input (new RollEvent());
				}
			}
		}

		/// <summary>
		/// Nets the game lost to select next.玩家在如果是选择状态下，那么自动选择，切换到下一位
		/// </summary>
		public void NetGameLostToSelectNext()
		{
			if (null != _fsm)
			{
				Re_SelectedState (1);
			}
		}

		private FSM _fsm;

        /// <summary>
        /// 只能由状态机调用,当前玩家的索引值
        /// </summary>
        public int CurrentPlayerIndex
		{
			get
			{
				return _currentPlayerIndex;
			}

			set
			{
				_currentPlayerIndex = value % players.Length;
			}
		}

		public int CurrentTurnCount
		{
			get 
			{
				return _currentTurnCount;
			}

			set 
			{
				_currentTurnCount = value;
			}
		}

        /// <summary>
        /// 未引用，获取状态机当前的状体类型
        /// </summary>
        /// <returns></returns>
        public FSMStateType getCuurrentStateType()
		{
			if (null != _fsm)
			{
				return  (FSMStateType)_fsm.CurrentStateType;
			}

			return FSMStateType.None;
		}

        public bool WalkFinished { get; set; }
        public bool UpGradeFinished { get; set; }
        public bool IsUpGrade { get; set; }

        /// <summary>
        /// 初始化玩家记录的数据
        /// </summary>
        public Player[] players = new Player[]{
			new Player(){PlayerID = Client.PlayerManager.Instance.Players[0].playerID},
			new Player(){PlayerID =  Client.PlayerManager.Instance.Players[1].playerID},
			new Player(){PlayerID =  Client.PlayerManager.Instance.Players[2].playerID},
			new Player(){PlayerID =  Client.PlayerManager.Instance.Players[3].playerID}
		};


		private int _currentTurnCount = 0;
		private int _currentPlayerIndex = -1;


	}
}