using System;
using System.Collections;
using System.Collections.Generic;
using Client.Scenes;
using Client.Unit;
using Server;

//模拟服务器行为
public class VirtualServer
{
    public VirtualServer()
    {
    }

    /// <summary>
    /// 模拟 服务器创建房间
    /// </summary>
    /// <param name="roomID"></param>
    public void Handle_RequestBuildRoom(int roomID = 133)
    {
		var room = new Room ();
//		if (Client.GameModel.GetInstance.isPlayNet == true)
//		{
//			
//			room.CurrentPlayerIndex = Client.GameModel.GetInstance.curStartIndex-1;
//
//			if (null != Client.PlayerManager.Instance.Players)
//			{
//				var tmpName = Client.PlayerManager.Instance.Players[room.CurrentPlayerIndex+1].playerName;
////				Client.MessageHint.Show ("当前开始的玩家："+tmpName);
//			}
//
//		}
		_dictRoom.Add(roomID, room);
		_roomID = roomID;
    }

    /// <summary>
    ///  进入游戏场景，初始化游戏
    /// </summary>
    /// <param name="sceneDataID"></param>
    public void Handle_RequestEnterScene(int sceneDataID)
    {
        SceneManager.Instance.Re_RequestEnterScene(sceneDataID);
        BattleController.Instance.InitBattle();
    }

    /// <summary>
    /// 点对点------客户端请求摇骰子
    /// </summary>
    /// <param name="roleIndex"></param>
    /// <param name="roomID"></param>
    public void Handle_RequestRoll(int roleIndex, int roomID)
    {
        _dictRoom[roomID].Enter_RollState(roleIndex);
    }

    /// <summary>
    /// 房间选择卡牌
    /// </summary>
    /// <param name="roleIndex"></param>
    /// <param name="roomID"></param>
    /// <param name="sel"></param>
    public void Handle_RequestSelect(int roleIndex, int roomID, int sel)
    {
        _dictRoom[roomID].Re_SelectedState(sel);
    }

    /// <summary>
    /// 完成行走的状态
    /// </summary>
    /// <param name="roomID"></param>
    public void Handle_WalkFinish(int roomID)
    {
        _dictRoom[roomID].WalkFinished = true;
    }

    /// <summary>
    ///  成功升级进入到内圈
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="select"></param>
    public void Handle_UpGradeFinished(int roomID, bool select)
    {
        _dictRoom[roomID].UpGradeFinished = true;
        _dictRoom[roomID].IsUpGrade = select;
    }

    /// <summary>
    /// 开始游戏，网络模式中，会判断掉线链接的，做重连的处理
    /// </summary>
    /// <param name="roomID"></param>
	public void Handle_StartGame(int roomID)
	{
		if (Client.GameModel.GetInstance.isPlayNet == true)
		{
//			Client.GameModel.GetInstance.curStartIndex;

			var roomStartIndex = Client.GameModel.GetInstance.curStartIndex;//当前轮到的玩家的索引值
//			Console.Error.WriteLine("当前玩家的索引值-----"+roomStartIndex.ToString());
			var players = Client.PlayerManager.Instance.Players;
			if (null != players)
			{
				if (null != players [roomStartIndex])
				{
//					Console.Error.WriteLine ("判断游戏汇总掉线的人的数量");
					if (players [roomStartIndex].isOffLineBeforGame == true||players [roomStartIndex].isReconectGame==true)
					{
//						Console.Error.WriteLine ("中途掉线玩家的名称------"+players [roomStartIndex].playerName);
						roomStartIndex++;
						roomStartIndex = roomStartIndex % players.Length;

						if (players [roomStartIndex].isOffLineBeforGame == true||players [roomStartIndex].isReconectGame==true)
						{
//							Console.Error.WriteLine ("中途掉线玩家的名称------"+players [roomStartIndex].playerName);
							roomStartIndex++;
							roomStartIndex = roomStartIndex % players.Length;

							if (players [roomStartIndex].isOffLineBeforGame == true||players [roomStartIndex].isReconectGame==true)
							{
//								Console.Error.WriteLine ("中途掉线玩家的名称------"+players [roomStartIndex].playerName);
								roomStartIndex++;
								roomStartIndex = roomStartIndex % players.Length;
							}
						}
					}
				}
			}

//			Console.Error.WriteLine ("当前玩家的索引值：-----"+roomStartIndex.ToString());
			if (Client.GameModel.GetInstance.isReconnecToGame == 1)
			{
				roomStartIndex = 2;
			}
//			Console.Error.WriteLine("当前房间的索引值"+roomStartIndex.ToString());

			var endIndex = roomStartIndex -1;

			if (endIndex < 0)
			{
				endIndex += players.Length;
			}
//			Console.Error.WriteLine("当前房间的起始位置索引值"+endIndex.ToString());
			_dictRoom [roomID].CurrentPlayerIndex = endIndex;
		}
        _dictRoom[roomID].StartGame();
	}

	public void Tick(float deltaTime)
	{
		
		if (null != _dictRoom)
		{
			if (_dictRoom.Count > 0)
			{
				var it = _dictRoom.GetEnumerator();
				while (it.MoveNext())
				{
					var room = it.Current.Value;
					room.Tick(deltaTime);
				}
			}
		}
        
	}

    /// <summary>
    /// 切换站立状态
    /// </summary>
    /// <param name="playerID"></param>
	public void Send_NewStayState(int playerID)
	{
		BattleController.Instance.Handle_NewStayState (playerID);	
	}

    /// <summary>
    /// 切换掷筛子状态
    /// </summary>
    /// <param name="points"></param>
    /// <param name="value"></param>
	public void Send_NewRollState(int points,int[] value=null)
	{
		BattleController.Instance.Handle_NewRollState (points,value);
	}

    /// <summary>
    /// 切换走路状态
    /// </summary>
	public void Send_NewWalkState()
	{
		BattleController.Instance.Handle_NewWalkState ();
	}

    /// <summary>
    /// 选择或者放弃卡牌
    /// </summary>
    /// <param name="id"></param>
	public void Send_NewSelectState(int id)
	{
		BattleController.Instance.Handle_NewSelectState (id);
	}

    /// <summary>
    /// 升级进入内圈
    /// </summary>
    public void Send_NewUpGradeState()
    {
        BattleController.Instance.Handle_NewUpGradeState();
    }

    /// <summary>
    /// 游戏胜利
    /// </summary>
    public void Send_NewSuccessState()
    {
        BattleController.Instance.Handle_NewSuccessState();
    }

    public float StayStateTime = float.MaxValue;
    public float RollStateTime = 0.58f;
    public float SelectStateTime = float.MaxValue;

    private Dictionary<int, Room> _dictRoom = new Dictionary<int, Room>();

	private static VirtualServer _instance;

	private int _roomID;

	public static VirtualServer Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance= new VirtualServer();
			}
			return _instance;
		}
	}

	public void Dispose()
	{
//		if (null == _instance)
//		{
//			return;
//		}
		
		if (null != _dictRoom && null != _dictRoom [_roomID])
		{
			_dictRoom [_roomID].Dispose ();
			_dictRoom [_roomID]=null;
		}
		_dictRoom.Clear();
//		_instance = null;
	}

	/// <summary>
	/// Nets the game lost to next one.轮到当前玩家筛子，但是当前玩家掉线了，自动到下一个玩家
	/// </summary>
	public void NetGameLostToNextOne()
	{
		if (null != _dictRoom && null != _dictRoom [_roomID])
		{
			_dictRoom [_roomID].NetGameLostGameToStayNext ();
		}
	}

    /// <summary>
    ///  轮到当前玩家选择卡牌，但是当前玩家掉线了，自动到下一个玩家
    /// </summary>
	public void NetGameLostToSelectNext()
	{
		if (null != _dictRoom && null != _dictRoom [_roomID])
		{
			_dictRoom [_roomID].NetGameLostToSelectNext ();
		}
	}

	/// <summary>
	/// Nets the game lost to up grade.轮到当前玩家升级到内圈掉线,切换到下一个人
	/// </summary>
	public void NetGameLostToUpGrade()
	{
		if (null != _dictRoom && null != _dictRoom [_roomID])
		{
			_dictRoom [_roomID].NetGameLostToUpgradNetxt ();
		}
	}

	public void ReStartGame()
	{
		if (null != _dictRoom)
		{
			if (null != _dictRoom [_roomID])
			{
				_dictRoom [_roomID].ReStartGame ();
			}
		}
	}

}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Client.Scenes;
//using Client.Unit;
//using Server;

////模拟服务器行为
//public class VirtualServer
//{
//	public VirtualServer()
//	{
//	}
//
//	//模拟 服务器创建房间//在这填写是谁的掷色子比目标值小1
//	public void Handle_RequestBuildRoom(int roomID = 133)
//	{
//		var room = new Room ();
//		if (Client.GameModel.GetInstance.isPlayNet == true)
//		{
//			room.CurrentPlayerIndex = Client.GameModel.GetInstance.curStartIndex;
//		}
//		_dictRoom.Add(roomID, room);
//		_roomID = roomID;
//	}
//
//	public void Handle_RequestEnterScene(int sceneDataID)
//	{
//		SceneManager.Instance.Re_RequestEnterScene(sceneDataID);
//		BattleController.Instance.InitBattle();
//	}
//
//	//点对点------客户端请求摇骰子
//	public void Handle_RequestRoll(int roleIndex, int roomID)
//	{
//		_dictRoom[roomID].Enter_RollState(roleIndex);
//	}
//
//	public void Handle_RequestSelect(int roleIndex, int roomID, int sel)
//	{
//		_dictRoom[roomID].Re_SelectedState(sel);
//	}
//
//	public void Handle_WalkFinish(int roomID)
//	{
//		_dictRoom[roomID].WalkFinished = true;
//	}
//
//	public void Handle_UpGradeFinished(int roomID, bool select)
//	{
//		_dictRoom[roomID].UpGradeFinished = true;
//		_dictRoom[roomID].IsUpGrade = select;
//	}
//
//	public void Handle_StartGame(int roomID)
//	{
//		_dictRoom[roomID].StartGame();
//	}
//
//	public void Tick(float deltaTime)
//	{
//
//		if (null != _dictRoom)
//		{
//			if (_dictRoom.Count > 0)
//			{
//				var it = _dictRoom.GetEnumerator();
//				while (it.MoveNext())
//				{
//					var room = it.Current.Value;
//					room.Tick(deltaTime);
//				}
//			}
//		}
//
//	}
//
//	public void Send_NewStayState(int playerID)
//	{
//		BattleController.Instance.Handle_NewStayState (playerID);	
//	}
//
//	public void Send_NewRollState(int points,int[] value=null)
//	{
//		BattleController.Instance.Handle_NewRollState (points,value);
//	}
//
//	public void Send_NewWalkState()
//	{
//		BattleController.Instance.Handle_NewWalkState ();
//	}
//
//	public void Send_NewSelectState(int id)
//	{
//		BattleController.Instance.Handle_NewSelectState (id);
//	}
//
//	public void Send_NewUpGradeState()
//	{
//		BattleController.Instance.Handle_NewUpGradeState();
//	}
//
//	public void Send_NewSuccessState()
//	{
//		BattleController.Instance.Handle_NewSuccessState();
//	}
//
//	public float StayStateTime = float.MaxValue;
//	public float RollStateTime = 0.58f;
//	public float SelectStateTime = float.MaxValue;
//
//	private Dictionary<int, Room> _dictRoom = new Dictionary<int, Room>();
//
//	private static VirtualServer _instance;
//
//	private int _roomID;
//
//	public static VirtualServer Instance
//	{
//		get
//		{
//			if (null == _instance)
//			{
//				_instance= new VirtualServer();
//			}
//			return _instance;
//		}
//	}
//
//	public void Dispose()
//	{
//		//		if (null == _instance)
//		//		{
//		//			return;
//		//		}
//
//		if (null != _dictRoom && null != _dictRoom [_roomID])
//		{
//			_dictRoom [_roomID].Dispose ();
//			_dictRoom [_roomID]=null;
//		}
//		_dictRoom.Clear();
//		//		_instance = null;
//	}
//
//	public void ReStartGame()
//	{
//		if (null != _dictRoom)
//		{
//			if (null != _dictRoom [_roomID])
//			{
//				_dictRoom [_roomID].ReStartGame ();
//			}
//		}
//	}
//
//}
//
