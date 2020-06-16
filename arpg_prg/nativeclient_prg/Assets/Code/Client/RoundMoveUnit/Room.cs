using System;
using Client.UnitFSM;
using Client.Unit;
using UnityEngine;
using Client.UI;

namespace Client
{
	/// <summary>
	/// Room.模拟本地的房间
	/// </summary>
	public class Room
	{
		public Room()
		{
//			_instance = this;
		}

		public void Tick(float deltaTime)
		{
			if (null != _fsm) {
				_fsm.Tick(deltaTime);
			}
		}

        /// <summary>
        /// 进入站立状态
        /// </summary>
        /// <param name="playerIndex"></param>
        public void Re_StayState(int playerIndex)
		{
			CurrentPlayerIndex = playerIndex;

            //默认从房主开始,所以到达房主时为一圈
            if (CurrentPlayerIndex == 0)
            {
                _currentTurnCount++;
            }

			if (null == _fsm) 
			{
				_fsm = new FSM (this);
				_fsm.Start ();
			}
			else 
			{
//				Debug.LogError ("进入站立状态了啊啊啊啊啊啊"+_fsm.CurrentStateType.ToString());

				if (_fsm.CurrentStateType == Client.UnitFSM.FSMStateType.StayState)
				{
					(_fsm.CurrentState as StayState).Enter (null, null);
				}
				else
				{
					_fsm.Input (new StayEvent ());	
				}
//				Debug.LogError ("进入站立状态了啊啊啊啊啊啊ccccccc"+_fsm.CurrentStateType.ToString());
			}
		}

        /// <summary>
        /// 掷筛子状态
        /// </summary>
        /// <param name="points"></param>
        /// <param name="value"></param>
        public void Re_RollState(int points,int[] value=null)
		{
			//mmmm  chu huo
			if(null !=_currentPlayer)
			{
				//之前存在的人 隐身
		
				_currentPlayer.setPlayerParticleHide();
				_currentPlayer.setPlayerShaderAlpah();

				Console.WriteLine("yin");
			}
				
			Console.WriteLine("chu xian");
			_currentPlayer = getCurrentPlay();

			Console.WriteLine("当前人物信息--——————--————___"+points.ToString());		
//			_currentPlayer.ShowLightImage();
			//当前的人显示，出火
			_currentPlayer.setPlayerParticleShow();
			_currentPlayer.setPlayerShaderInitial();

			_currentPoints = points;
			_currentPointsArr = value;
			_fsm.Input (new RollEvent (points));
		}

        /// <summary>
        /// 开始走
        /// </summary>
        public void Re_WalkState()
		{
			_players [_currentPlayerIndex].CalRoundWalkInfos (_currentPoints);
			_fsm.Input (new WalkEvent (_players[_currentPlayerIndex].RoundWalk));
		}

        /// <summary>
        /// 选择卡牌
        /// </summary>
        /// <param name="id"></param>
		public void Re_SelectState(int id)
		{
			//mie huo yin shen
			if(null !=_currentPlayer)
			{
				if(_currentPlayer._blockType == BlockType.Outer)
				{
					_currentPlayer.SetUpGridReduction(BlockType.Outer);
				}
				else if(_currentPlayer._blockType == BlockType.Inner)
				{
					_currentPlayer.SetUpGridReduction(BlockType.Inner);
				}
			}
			_fsm.Input (new SelectEvent (_players[_currentPlayerIndex].CurrentPoint, id));
		}

        /// <summary>
        /// 升级进内圈
        /// </summary>
        public void Re_UpGradeState()
        {
            //			if(getCurrentPlay().PlayerID == PlayerManager.Instance.HostPlayerInfo.playerID)
            //			{				
            ////				getCurrentPlay().enterInnerShow();
            ////				Re_UpPlayerState();
            //				var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
            //				controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;
            //				controller.setVisible (true);
            //			}
            //			else
            //			{
            ////				if (GameModel.GetInstance.isPlayNet == false)
            ////				{
            //_fsm.Input(new UpGradeEvent(_players[_currentPlayerIndex].UpGradeToInner));
            ////				}
            //			}
            
            getCurrentPlay().enterInnerShow();
        }

        /// <summary>
        /// 不切换状态，直接进入内圈
        /// </summary>
        public void UpdateInner_Direct()
        {
            this.getCurrentPlay().UpGradeToInner();
        }


		public void Re_UpPlayerState()
		{
			_fsm.Input(new UpGradeEvent(_players[_currentPlayerIndex].UpGradeToInner));
		}

        /// <summary>
        /// 胜利
        /// </summary>
        public void Re_SuccessState()
        {
            _fsm.Input(new SuccessEvent());
        }

        /// <summary>
        /// 当前玩家索引值
        /// </summary>
        public int CurrentPlayerIndex
		{
			get
			{
				return _currentPlayerIndex;
			}

			private set
			{
				_currentPlayerIndex = value % _players.Length;
			}
		}
        /// <summary>
        /// 当前的回合数
        /// </summary>
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
        /// 掷筛子的点数
        /// </summary>
		public int CurrentPoints
		{
			get
			{
				return _currentPoints;
			}
		}

        /// <summary>
        /// 掷筛子的点数数组
        /// </summary>
		public int[] CurrentPointsArr
		{
			get
			{
				return _currentPointsArr;
			}
		}

		private int _currentTurnCount = 0;
		private int _currentPoints = 0;
		private int _currentPlayerIndex = 0;

		private int[] _currentPointsArr;

        private FSM _fsm;
        public int RoomID = 133;

		private static Room _instance;
		public static Room Instance
		{
			get
			{
				if (null == _instance)
				{
					_instance = new Room ();
				}
				return _instance;
			}
		}
        /// <summary>
        /// 设置玩家模型信息
        /// </summary>
        /// <param name="value"></param>
        public void SetPlayerModel(PlayerInfo[] value)
		{
//			_players [0] = new Player (value[0].modelPath){ PlayerID = "11" };
//			_players [1] = new Player (value[1].modelPath){ PlayerID = "22" };
//			_players [2] = new Player (value[2].modelPath){ PlayerID = "33" };
//			_players [3] = new Player (value[3].modelPath){ PlayerID = "44" };

//			_players [0] = new Player (value[0].modelPath){ PlayerID = value[0].playerID };
//			_players [1] = new Player (value[1].modelPath){ PlayerID = value[1].playerID  };
//			_players [2] = new Player (value[2].modelPath){ PlayerID =  value[2].playerID  };
//			_players [3] = new Player (value[3].modelPath){ PlayerID =  value[3].playerID  };

//			if(null==_players)

			for (var i = 0; i < 4; i++)
			{
				var tmpPlayer = _players [i];
//				Console.WriteLine ("角色嘻嘻嘻嘻嘻爱-----"+value[i].playerName+"水电费水电费是的;;"+value[i].modelPath);

				if (null == tmpPlayer)
				{
//					Console.Error.WriteLine ("的顶顶顶顶顶顶顶顶顶顶");
					tmpPlayer = new Player (value [i].modelPath){PlayerID = value[i].playerID };
					_players [i] = tmpPlayer;
				}
			}

//			_players [0].NetSetPlayerInitData (2, false);
//			_players [1].NetSetPlayerInitData (18, false);
//			_players [2].NetSetPlayerInitData (23, false);
//			_players [3].NetSetPlayerInitData (16,true);
			SetPlayerPositionInifor(value);
		}

		/// <summary>
		/// Sets the player position inifor.设置玩家的位置信息
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetPlayerPositionInifor(PlayerInfo[] value)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				for (var i = 0; i < _players.Length; i++)
				{
//					Console.Error.WriteLine ("当前的点数+++++"+value[i].roundPostion.ToString()+"--------当前玩家的id:"+value[i].playerID+"--------玩家名称:"+value[i].playerName);
					_players [i].NetSetPlayerInitData (value [i].roundPostion, value [i].isEnterInner);
				}
			}
		}

		private Player _currentPlayer;

		/// <summary>
		/// 当前人物
		/// </summary>
		/// <returns>The current play.</returns>
		public Player getCurrentPlay()
		{
			return _players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
		}

        /// <summary>
        /// 未引用
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public Player GetCurrentPlayerByIndex(int index)
		{
			return _players[index];
		}

		public void Dispose()
		{
//			if (null == _instance)
//			{
//				return;
//			}

			for (var i = 0; i < _players.Length; i++)
			{
				var tmpPlayer = _players[i];
				tmpPlayer.Dispose ();
				tmpPlayer = null;
				_players [i] = null;
			}

//			_players = null;


			if (null != _fsm)
			{
				_fsm.Stop ();
				_fsm = null;
			}

			_currentPlayer = null;
//			_instance = null;
		}

        /// <summary>
        /// 重新开始游戏
        /// </summary>
        public void ReStartGame()
		{
			_currentTurnCount = 0;
			_currentPoints = 0;
			_currentPlayerIndex = 0;

			for (var i = 0; i < _players.Length; i++)
			{
				var tmpPlayer = _players[i];
				if (null != tmpPlayer)
				{
					tmpPlayer.ReStartGame ();
				}
			}

			if (null != _fsm)
			{
				_fsm.Stop ();
				_fsm = null;
			}
		}

		private Player[] _players = new Player[4];
	}
}