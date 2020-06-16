using System;
using Client.Scenes;
using UnityEngine;

namespace Client.Unit
{
    /// <summary>
    /// 游戏控制器
    /// </summary>
	public class BattleController
	{
		private BattleController ()
		{
		}

        /// <summary>
        /// 开始游戏
        /// </summary>
		public void Send_StartGame()
		{			
			VirtualServer.Instance.Handle_StartGame (133);
		}

		public void InitBattle()
		{
			
		}

		public void Tick(float deltaTime)
		{
			if (null != _room) 
			{
				_room.Tick (deltaTime);
			}
		}

        /// <summary>
        /// 发送掷筛子
        /// </summary>
		public void Send_RequestRoll()
		{
			Console.WriteLine ("Send_RequestRoll index = {0}", _room.CurrentPlayerIndex);
			VirtualServer.Instance.Handle_RequestRoll (_room.CurrentPlayerIndex, _room.RoomID);
		}

        /// <summary>
        /// 玩家做出卡牌选择
        /// </summary>
        /// <param name="sel"></param>
		public void Send_RoleSelected(int sel = 1)
		{
			if (GameModel.GetInstance.isPlayNet == false)
			{
				NetGameSeletcResult (sel);
			}
			else
			{
				
			}
		}

		public void NetGameSeletcResult(int sel = 1)
		{
			VirtualServer.Instance.Handle_RequestSelect (_room.CurrentPlayerIndex, _room.RoomID, sel);
		}

        /// <summary>
        /// 切换玩家
        /// </summary>
        /// <param name="ownerIndex"></param>
		public void Handle_NewStayState(int ownerIndex)
		{
			if (null != _room) 
			{
				_room.Re_StayState (ownerIndex);
			}
		}

        /// <summary>
        /// 掷筛子
        /// </summary>
        /// <param name="points"></param>
        /// <param name="arr"></param>
		public void Handle_NewRollState(int points,int[] arr=null)
		{
			if (null != _room) 
			{
				_room.Re_RollState (points,arr);
			}
		}

        /// <summary>
        /// 行走
        /// </summary>
		public void Handle_NewWalkState()
		{
			if (null != _room) 
			{
				_room.Re_WalkState ();
			}
		}

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="id"></param>
		public void Handle_NewSelectState(int id)
		{
			if (null != _room) 
			{
				_room.Re_SelectState (id);
			}
		}

		/// <summary>
		/// Handles the new state of the up grade. 单机版游戏进入到内圈
		/// </summary>
        public void Handle_NewUpGradeState()
        {			
//			if (GameModel.GetInstance.isPlayNet == false)
//			{
				NetHandlerUpGradeState ();
//			}
        }

		/// <summary>
		/// Nets the state of the handler up grade.  网络版调用升级的接口
		/// </summary>
		private void NetHandlerUpGradeState()
		{
			if (null != _room)
			{
				_room.Re_UpGradeState();
			}
		}



        /// <summary>
        /// 游戏胜利
        /// </summary>
        public void Handle_NewSuccessState()
        {
            if (null != _room)
            {
                _room.Re_SuccessState();
            }
        }

        /// <summary>
        /// 完成走路状态
        /// </summary>
        public void Send_WalkFinish()
        {
            VirtualServer.Instance.Handle_WalkFinish(_room.RoomID);
        }

		/// <summary>
		/// Sends up grade finish. 单机玩法发送进入内圈成功
		/// </summary>
		/// <param name="select">If set to <c>true</c> select.</param>
        public void Send_UpGradeFinish(bool select)
        {
			if (GameModel.GetInstance.isPlayNet == false)
			{
				NetGameEnterInnerFinished (select);
			}
			else
			{

			}
        }

        /// <summary>
        ///  完成进入内圈的动作
        /// </summary>
        /// <param name="iselect"></param>
		public void NetGameEnterInnerFinished(bool iselect=true)
		{
			VirtualServer.Instance.Handle_UpGradeFinished(_room.RoomID, iselect);
		}

        /// <summary>
        /// 当前玩家索引值
        /// </summary>
        public int CurrentPlayerIndex
        {
            get { return _room.CurrentPlayerIndex; }
        }

        /// <summary>
        /// 返回当前的次数
        /// </summary>
        public int CurrentTurnCount
        {
            get
			{
//				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
//				{
//					if (null != PlayerManager.Instance.HostPlayerInfo)
//					{
//						if(PlayerManager.Instance.HostPlayerInfo.Age !=_room.CurrentTurnCount)
//						{
//							PlayerManager.Instance.HostPlayerInfo.Age = _room.CurrentTurnCount;
//						}
//					}
//				}
				return _room.CurrentTurnCount;
			}
        }

		private Room _room = Room.Instance;

		private static BattleController _instance;

		public static BattleController Instance
		{
			get
			{
				if (null == _instance)
				{
					_instance= new BattleController();
				}
				return _instance;
			}
		}

		public void Dispose()
		{
//			if (null == _instance)
//			{
//				return;
//			}
			_room.Dispose ();
//			_instance = null;
		}

		public void ReStartGame()
		{
			if (null != _room)
			{
				_room.ReStartGame ();
			}
		}
	}
}

