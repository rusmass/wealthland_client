using System;
using Client.Unit;
using Metadata;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 游戏中管理玩家数据
    /// </summary>
	public class PlayerManager
	{
		protected PlayerManager ()
		{
			
			playerInitList = new List<PlayerInitData> ();

			var template = MetadataManager.Instance.GetTemplateTable<PlayerInitData> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as PlayerInitData;
				playerInitList.Add(value);			
			}
//            _hostPlayerInfo = new PlayerInfo();
//			_hostPlayerInfo.SetPlayerInitData (playerInitList[0]);
//			_hostPlayerInfo.playerID = "11"; 
//			_hostPlayerInfo.playerName = "张三";
//
//			var battlecontroller = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController> ();
//			if (null!=battlecontroller) 
//			{
//				battlecontroller.SetCashFlow ((int)_hostPlayerInfo.totalMoney);
//			}
//            _players[0] = _hostPlayerInfo;

        }

        /// <summary>
        /// 设置玩家数据
        /// </summary>
        /// <param name="playerdata"></param>
        /// <param name="heroIndex"></param>
        /// <param name="roleName"></param>
		public void SetPlayerHero(PlayerInitData playerdata, int heroIndex , string roleName)
		{
			_hostPlayerInfo = new PlayerInfo();
			_hostPlayerInfo.SetPlayerInitData (playerdata);
			_hostPlayerInfo.playerID = "111"; 
			// 2016-10-28 zll fix name
			_hostPlayerInfo.playerName = roleName;

			var battlecontroller = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController> ();
			if (null!=battlecontroller) 
			{
				battlecontroller.SetCashFlow ((int)_hostPlayerInfo.totalMoney);
			}
			_players[0] = _hostPlayerInfo;
			_seletcedArr [heroIndex] = 0;

			_SelectRandomNpc (1);
			_SelectRandomNpc (2);
			_SelectRandomNpc (3);

			_players[1].playerID = "222"; 
			_players[2].playerID = "334";
			_players[3].playerID = "447";

			Room.Instance.SetPlayerModel (_players);
		}

        public void SetCreateHero(PlayerInfo _playerInfo ,int heroIndex,string roleName)
        {
            _hostPlayerInfo = _playerInfo;
            _hostPlayerInfo.playerID = "111";
            // 2016-10-28 zll fix name
            _hostPlayerInfo.playerName = roleName;

            var battlecontroller = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController>();
            if (null != battlecontroller)
            {
                battlecontroller.SetCashFlow((int)_hostPlayerInfo.totalMoney);
            }
            _players[0] = _hostPlayerInfo;
            _seletcedArr[heroIndex] = 0;

            _SelectRandomNpc(1);
            _SelectRandomNpc(2);
            _SelectRandomNpc(3);

            _players[1].playerID = "222";
            _players[2].playerID = "334";
            _players[3].playerID = "447";
            Room.Instance.SetPlayerModel(_players);
        }










        /// <summary>
        /// 网络版设置玩家是数据
        /// </summary>
        /// <param name="tmpList"></param>
		public void SetNetPlayerInfor( List<PlayerInfo> tmpList)
		{

			if (_players [0] == null && _players [1] == null && _players [2] == null && _players [3] == null)
			{
				for (int i = 0; i < _players.Length; i++)
				{
					_players [i] = tmpList [i];
				}

				_hostPlayerInfo = _players [0];

				var battlecontroller = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController> ();
				if (null!=battlecontroller) 
				{
					battlecontroller.SetCashFlow ((int)_hostPlayerInfo.totalMoney);
                    //battlecontroller.SetNonLaberIncome((int)_hostPlayerInfo.incom)
				}
			}
			else
			{
				
			}

			Room.Instance.SetPlayerModel (_players);
		}
        /// <summary>
        /// 设置npc的信息
        /// </summary>
        /// <param name="npcNum"></param>
		private void _SelectRandomNpc(int npcNum)
		{
			var range = 5;
			var tmpIndex = MathUtility.Random (0,range);

			if (npcNum < 3)
			{
				while (_seletcedArr[tmpIndex] == 0)
				{
					tmpIndex = MathUtility.Random (0,range);
				}
			}
			else
			{
				for (var i = 0; i < _seletcedArr.Length; i++)
				{
					if (_seletcedArr [i] != 0)
					{
						tmpIndex = i;
						break;
					}
				}
			}
			_seletcedArr [tmpIndex] = 0;
			_players[npcNum] = new PlayerInfo();
			_players [npcNum].SetPlayerInitData (playerInitList[tmpIndex]); 
		}


        private PlayerInfo _hostPlayerInfo;
        /// <summary>
        /// 返回玩家自己的数据
        /// </summary>
        public PlayerInfo HostPlayerInfo
        {
            get
            {
                return _hostPlayerInfo;
            }
        }

        /// <summary>
        /// 是否是所有玩家都进入了内圈
        /// </summary>
        /// <returns></returns>
        public bool IsAllEnterInner()
        {
            var isAll = true;

            for(var i=0;i<_players.Length;i++)
            {
                if(_players[i].isEnterInner==false)
                {
                    isAll = false;
                    break;
                }
            }
            return isAll;
        }

        private PlayerInfo[] _players = new PlayerInfo[4];

        /// <summary>
        /// 返回四个玩家的数组
        /// </summary>
        public PlayerInfo[] Players
        {
            get
            {				
                return _players;
            }
        }

        /// <summary>
        /// 当前玩家是否是自己
        /// </summary>
        /// <returns></returns>
        public bool IsHostPlayerTurn()
        {
            var index = BattleController.Instance.CurrentPlayerIndex;
            if (index >= _players.Length)
            {
                Console.Error.WriteLine("[PlayerManager.IsHostPlayerTurn] error!");
            }
            var player = _players[index];
            return player.playerID == _hostPlayerInfo.playerID;
        }

        /// <summary>
        /// 根据id返回玩家信息
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
		public PlayerInfo GetPlayerInfo(string playerID)
        {
            for (int i = 0; i < _players.Length; ++i)
            {
                if (_players[i].playerID == playerID)
                {
                    return _players[i];
                }
            }

            return null;
        }
		private int[] _seletcedArr = { -1, -1, 0, 0, -1, -1 };
		private List<PlayerInitData> playerInitList;
		private static PlayerManager _manager;
		public static PlayerManager Instance
		{
			get
			{
				if(null == _manager)
				{					
					_manager= new PlayerManager();
				}
				return _manager;
			}
		}

		public void Dispose()
		{
			_hostPlayerInfo = null;

			_players = null;
			_manager = null;
		}

        /// <summary>
        /// 进入内圈的玩家的数
        /// </summary>
        public int InnerPlayerNumber
        {
            get
            {
                return _innerPlayer;
            }

            set
            {
                _innerPlayer = value;
                if(_innerPlayer>1 &&HostPlayerInfo.isEnterInner==true)
                {
                    _innerCardTogether = true;
                }
                else
                {
                    _innerCardTogether = false;
                }
            }
        }

        /// <summary>
        /// 是否可以一起操作内圈卡牌(有钱有闲、品质生活、投资)
        /// </summary>
        public bool InnerCardTogether
        {
            get
            {
                return _innerCardTogether;
            }
        }

        private bool _innerCardTogether=false;
        private int _innerPlayer = 0;

        /// <summary>
        /// 重新玩游戏
        /// </summary>
        public void ReStartGame()
		{
			for (var i = 0; i < _players.Length; i++)
			{
				var tmpPlayerInfor = new PlayerInfo ();
				var player=_players[i];

				if (null != player)
				{
					for (var j = 0; j < playerInitList.Count; j++)
					{
						var tmpInitData = playerInitList[j];
						if (null != tmpInitData)
						{							
							if (player.career == tmpInitData.careers)
							{
								tmpPlayerInfor.SetPlayerInitData (tmpInitData);
								tmpPlayerInfor.playerName = player.playerName;
                                tmpPlayerInfor.playerID = player.playerID;
                                _players [i] = null;
								_players[i] = tmpPlayerInfor;
								break;
							}
						}

					
					}
				}
			}
			//_players [0].playerID = "11";
			//_players [1].playerID = "22";
			//_players [2].playerID = "33";
			//_players [3].playerID = "44";
			_hostPlayerInfo = _players [0];
		}

        /// <summary>
        /// 返回当前游戏进度最快的玩家信息
        /// </summary>
        /// <returns></returns>
        public PlayerInfo FastPlayerInfor()
        {
            PlayerInfo tmpPlayer = _players[0];
            for(var i=1;i<_players.Length;i++)
            {
                if(tmpPlayer.GameProgress< _players[i].GameProgress)
                {
                    tmpPlayer = _players[i];
                }
                //for (var k= i;k<_players.Length;k++)
                //{
                //    if(_players[i].GameProgress<_players[k].GameProgress)
                //    {
                //        tmpPlayer = _players[k];
                //    }
                //}
            }

            return tmpPlayer;
        }
	}
}

