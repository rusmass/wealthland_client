using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using LitJson;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using Client;

using System.Timers;

public  class NetWorkScript 
{    
    private static NetWorkScript script;
	private readonly string endSymbol= "$_";
    private Socket _clientSocket;
    private Socket clientSocket;
    //private Socket clientSocket {
    //    get {
    //        if (_clientSocket == null) {
    //            if (null != messages)
    //            {
    //                messages.Clear();
    //                messages.TrimExcess();
    //            }

    //            try
    //            {

    //                isConnect = true;
    //                //创建socket连接对象SocketType.Dgram
    //                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //                //			clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
    //                //			clientSocket.SendTimeout		= 40000;
    //                //			clientSocket.ReceiveTimeout 	= 10000;
    //                //			clientSocket.NoDelay			= true;           

    //                //正式版本,绑定域名
    //                IPHostEntry hostInfo = Dns.GetHostEntry(hostUrl);
    //                IPAddress address = hostInfo.AddressList[0];

    //                //测试版本，绑定ip地址
    //                //IPAddress address = IPAddress.Parse(hostUrl);
    //                //Console.Error.WriteLine("____:"+ address);

    //                IPEndPoint ep = new IPEndPoint(address, port);
    //                //socket.Connect(host, port);
    //                _clientSocket.Connect(ep);
    //                //连接后开始从服务器读取网络消息
    //                //_clientSocket.BeginReceive(readM, 0, 1024, SocketFlags.None, ReceiveCallBack, this.clientSocket);

    //                isTickedOut = false;
    //                Debug.Log("Connect error:_init");
    //            }
    //            catch (Exception e)
    //            {
    //                //连接失败 打印异常
    //                Debug.Log("Connect error:" + e.Message);
    //                lostNetType = 0;
    //                hasConnect = true;
    //            }
    //        }
    //        return _clientSocket;
    //    }
    //    set {
    //        _clientSocket = value;
    //    }


    //}
    
    //public static string hostUrl = "192.168.1.31"; 
    public static string hostUrl = "192.168.1.31";
    public static int port = 8056;//8099;//8066 8067 8068苹果商城的 8069 6.5内测版本的
    //public static int port = 8100;
    private byte[] readM = new byte[1024];

	private ByteArray ioBuff=new ByteArray();
	private int dataSize;
    private List<SocketModel> messages = new List<SocketModel>();    
    
	private string dataStr="";
	private System.Timers.Timer _haertBeatTimer;

	/// <summary>
	/// The type of the lost net. 判断掉线的类型，如果是断网0弹版，如果是心跳机制断网1，直接连接
	/// </summary>
	public int lostNetType = 0;

    /// <summary>
    /// 链接是否是被其他链接顶替了
    /// </summary>
    public bool isTickedOut = false;
  
	/// <summary>
	/// Gets the instance.   获取连接对象
	/// </summary>
	/// <returns>The instance.</returns>
    public static NetWorkScript getInstance() {
        if (script == null) {
            //第一次调用的时候 创建单例对象 并进行初始化操作
            script = new NetWorkScript();
        }
        
        return script;
    }	

	private bool isConnect=false;

    /// <summary>
    /// 初始化socket，链接服务器，接收数据
    /// </summary>
    public void init() {

        if(null!=messages)
        {
            messages.Clear();
            messages.TrimExcess();
        }
    

        try
        {
			if(isConnect==true)
			{
				return;
			}
			isConnect = true;
			//创建socket连接对象SocketType.Dgram
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //			clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //			clientSocket.SendTimeout		= 40000;
            //			clientSocket.ReceiveTimeout 	= 10000;
            //			clientSocket.NoDelay			= true;           

            //正式版本,绑定域名
            IPHostEntry hostInfo = Dns.GetHostEntry(hostUrl);
            IPAddress address = hostInfo.AddressList[0];

            //测试版本，绑定ip地址
            //IPAddress address = IPAddress.Parse(hostUrl);
            //Console.Error.WriteLine("____:"+ address);

            IPEndPoint ep = new IPEndPoint(address,port);           
            //socket.Connect(host, port);
			clientSocket.Connect(ep);
            //连接后开始从服务器读取网络消息
			clientSocket.BeginReceive(readM, 0, 1024, SocketFlags.None, ReceiveCallBack,this.clientSocket);

            isTickedOut = false;
            Debug.Log("Connect error:_init");
        }
        catch (Exception e) {
            //连接失败 打印异常
            Debug.Log("Connect error:"+e.Message);
			lostNetType = 0;
			hasConnect = true;
        }
    }

    /// <summary>
    /// 开启心跳机制，非游戏中是计时器计时，在游戏中是ui绑定计时
    /// </summary>
	public void _StartHeartBeat()
	{
        if (null == _haertBeatTimer)
        {
            _haertBeatTimer = new System.Timers.Timer(7000);
            _haertBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(_HeartBeatHandler); //到达时间的时候执行事件；   
            _haertBeatTimer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            _haertBeatTimer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
        }
        if (GameModel.GetInstance.heartBeatState == 0)
        {
            _haertBeatTimer.Start();
        }
    }

    /// <summary>
    /// 停止timer发送心跳包
    /// </summary>
    public void _EndHeartBeat()
	{
		if (null != _haertBeatTimer)
		{
			_haertBeatTimer.Stop ();
		}
	}
    /// <summary>
    /// 用timer发送心跳包
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
	public void _HeartBeatHandler(object obj, System.Timers.ElapsedEventArgs e)
	{
		if (IsSocketConnect ())
		{			
			SendHeartBeat ();
			if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState||GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNetChooseState)
			{
				GameModel.GetInstance.heartBeatState = 1;
				_EndHeartBeat ();
			}
		}
	}

	/// <summary>
	/// Starts to game. 进入房间
	/// </summary>
	public void startToNetGame()
	{
		Console.WriteLine ("请求进入匹配房间");
	}

	/// <summary>
	/// Exits to game.退出房间
	/// </summary>
	public void exitToNetGame()
	{
		Console.WriteLine ("请求退出匹配房间");
	}

	/// <summary>
	/// Starts the width robot. 申请机器人加入
	/// </summary>
	public void startWidthRobot()
	{
        //Console.WriteLine ("请求机器人加入游戏");
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();

        headerData["type"] = Protocol.Game_Math_NeedRobotGame;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
        sendData["header"] = headerData;

        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

	/// <summary>
	/// Agrees to re connet game. 同意重新连入游戏
	/// </summary>
	public void AgreeToReConnetGame(string roomId,int state)
	{
		if (GameModel.GetInstance.isReconnecToGame == 2)
		{
			PlayerManager.Instance.HostPlayerInfo.isReconectGame = false;
		}

		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData bodyData = new JsonData ();

		headerData ["type"] = Protocol.Game_AgreeReConnect;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		bodyData ["roomId"] = roomId;
		bodyData ["state"] = state;

		sendData ["header"] = headerData;
		sendData ["body"] = bodyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Refuses to re connect game. 拒绝重新连入游戏
	/// </summary>
	public void RefuseToReConnectGame(string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData bodyData = new JsonData ();

		headerData ["type"] = Protocol.Game_RefuseReConnect;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		bodyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = bodyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 重新进入游戏后，完成初始化数据
    /// </summary>
	public void ReConnectInited()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData bodyData = new JsonData ();

		headerData ["type"] = Protocol.Game_ReConnectedInited;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		bodyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = bodyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 注销游戏。没法接收收到的数据
    /// </summary>
    /// <param name="playerid"></param>
	public void LogOutGame(string playerid)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_LogOut;
		headerData ["playerId"] = playerid;
		sendData ["header"] = headerData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}
    /// <summary>
    /// 活跃排行榜
    /// </summary>
    public void GameRank_Active()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_Active_Rank;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		sendData ["header"] = headerData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}


    public void GetGameRecordData()
    {        
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();

        headerData["type"] = Protocol.Game_GameRecordInfor;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
        sendData["header"] = headerData;

        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

    /// <summary>
    /// 获取对战记录详情
    /// </summary>
    public void GetRecordDetail(string codeId)
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        JsonData bodyData = new JsonData();

        bodyData["roomCode"] = codeId;
        sendData["body"] = bodyData;

        headerData["type"] = Protocol.Game_GameRecoredDetail;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
        sendData["header"] = headerData;  

        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }


	/// <summary>
	/// Modifies the player infor. 修改人物信息
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="gender">Gender.</param>
	/// <param name="headImg">Head image.</param>
	public void ModifyPlayerInfor(string name , int gender , string headImg , string playerId )
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData bodyData = new JsonData ();

		headerData ["type"] = Protocol.Game_ModifyData;
		headerData ["playerId"] = playerId;

		sendData ["header"] = headerData;

		bodyData ["name"] = name;
		bodyData ["gender"] = gender;
		bodyData ["headImg"] = headImg;

		sendData ["body"] = bodyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}



	/// <summary>
	/// Connets the server. 服务期握手请求
	/// </summary>
	/// <param name="playerid">Playerid.</param>
	public void ConnetServer(string playerid)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData bodyData = new JsonData ();

		headerData ["type"] = Protocol.WOSHOU;
		headerData ["playerId"] = playerid;

		bodyData ["clientStatus"] = GameModel.loginStatus;

		sendData ["body"] = bodyData;
		sendData ["header"] = headerData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sends the heart beat. 发送心跳包
	/// </summary>
	public void SendHeartBeat()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.HEART_BEAT;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		sendData ["header"] = headerData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 同意放弃游戏
    /// </summary>
    public void AgreeQuitGame()
	{		
		JsonData sendData = new JsonData ();
		JsonData badyData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_AgreeQuitGame;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 拒绝放弃游戏
    /// </summary>
    public void RefuseQuitGame()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();


		headerData ["type"] = Protocol.Game_RefuseQuitGame;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;

		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Nets the select role.选择角色
	/// </summary>
	/// <param name="value">Value.</param>
	public void NetSelectRole(int value)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SelectRole;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData ["roleId"] = value;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();


		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Nets the cancle select role.取消选择
	/// </summary>
	/// <param name="vlaue">Vlaue.</param>
	public void NetCancleSelectRole(int vlaue)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_CancleSelect;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData ["roleId"] = vlaue;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Nets the sure select. 确认选择
	/// </summary>
	public void NetSureSelect()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SureSelect;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}


	/// <summary>
	/// Nets the auto select role.倒计时0，自动选人
	/// </summary>
	public void NetAutoSelectRole()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_AutoSelect;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 游戏大厅和游戏结束界面时获取分享数据
    /// </summary>
    public void GetNormalShareData()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetNornormalShareInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		sendData ["header"] = headerData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 获取房间界面，分享的数据
    /// </summary>
    public void GetRoomShareData()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		headerData ["type"] = Protocol.Game_GetRoomShareInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		sendData ["header"] = headerData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 提交梦想板的接口
    /// </summary>
    public void GetShareDreamData()
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        headerData["type"] = Protocol.Game_ShareDream;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

        sendData["header"] = headerData;
        var msg = sendData.ToJson();

        msg += endSymbol;
        sendMessage(msg);
    }


	/// <summary>
	/// Games the holl chat broad. 游戏大厅聊天
	/// </summary>
	/// <param name="chatStr">Chat string.</param>
	public void GameHollChatBroad(string chatStr)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GameHollChat;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["msg"] = chatStr;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Rooms the chat broad. 房间聊天
	/// </summary>
	/// <param name="chatStr">Chat string.</param>
	public void RoomChatBroad(string chatStr)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_RoomChat;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["msg"] = chatStr;
		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Requests the game. 请求匹配游戏
	/// </summary>
	public void RequestMatchGame(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_Match;
		headerData ["playerId"] = playerId;

		sendData ["header"] = headerData;
		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Determines whether this instance cancle match game the specified playerId. 取消匹配排队
	/// </summary>
	/// <returns><c>true</c> if this instance cancle match game the specified playerId; otherwise, <c>false</c>.</returns>
	/// <param name="playerId">Player identifier.</param>
	public void CancleMatchGame(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();

		headerData ["type"] = Protocol.Game_Match_Cancle;
		headerData ["playerId"] = playerId;

		sendData ["header"] = headerData;
		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 匹配模式，进入游戏
    /// </summary>
    /// <param name="playerId"></param>
    public void MatchEnterGame(string playerId)
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        JsonData badyData = new JsonData();

        headerData["type"] = Protocol.Game_Match_EnterGame;
        headerData["playerId"] = playerId;
        badyData["roomId"] = GameModel.GetInstance.curRoomId;

        sendData["header"] = headerData;
        sendData["body"] = badyData;

        //sendData["header"] = headerData;
        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

    /// <summary>
    /// Requests the room game. 好友匹配模式
    /// </summary>
    /// <param name="playerId">Player identifier.</param>
    public void RequestRoomGame(string playerId)
	{
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();

        headerData["type"] = Protocol.Game_Friend_Createroom;
        headerData["playerId"] = playerId;

        sendData["header"] = headerData;
        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

	/// <summary>
	/// Requests the enter room. 进入房间
	/// </summary>
	public void RequestEnterRoom(string playerId,string roomId)
	{
		//{"header":{"type":5001,"playerId":"111"},"body":{"roomId":"房间号"}}$_
		GameModel.GetInstance.ShowNetLoading ();
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_Friend_EnterRoom;
		headerData ["playerId"] = playerId;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 发起借款的请求
    /// </summary>
    public void ReadyBorrowFried()
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        JsonData badyData = new JsonData();

        headerData["type"] = Protocol.Game_ReadyBorrowFriend;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
        badyData["roomId"] = GameModel.GetInstance.curRoomId;

        sendData["header"] = headerData;
        sendData["body"] = badyData;
        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

    /// <summary>
    /// 向玩家借钱
    /// </summary>
    public void BorrowFriendMoney(string targetId,int money,float rate=1)
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        JsonData badyData = new JsonData();

        headerData["type"] = Protocol.Game_BorrowFriendMoney;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

        badyData["roomId"] = GameModel.GetInstance.curRoomId;
        badyData["targetPlayerId"] = targetId;
        badyData["cash"] = money;
        badyData["rate"] = rate;

        sendData["header"] = headerData;
        sendData["body"] = badyData;
     
        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }

    /// <summary>
    /// 同意借钱
    /// </summary>
    public void AgreeBorrowedMoney(string targetId,int money,float rate)
    {
        JsonData sendData = new JsonData();
        JsonData headerData = new JsonData();
        JsonData badyData = new JsonData();

        headerData["type"] = Protocol.Game_BorrowFriendInfor;
        headerData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

        badyData["roomId"] = GameModel.GetInstance.curRoomId;
        badyData["targetPlayerId"] = targetId;
        badyData["cash"] = money;
        badyData["rate"] = rate;

        sendData["header"] = headerData;
        sendData["body"] = badyData;
       
        var msg = sendData.ToJson();
        msg += endSymbol;
        sendMessage(msg);
    }


	/// <summary>
	/// Requests the exit room. 退出房间
	/// </summary>
	public void RequestExitRoom(string playerId , string roomId)
	{
		//请求对战：{“header":{"type":5002,"playerId":111},"body":{"roomId":"475567"}}$_
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_Friend_ExitRoom;
		headerData ["playerId"] = playerId;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    ///  房间匹配了准备
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="roomId"></param>
    public void RequestReadyInRoom(string playerId , string roomId)
	{
		//{“header":{"type":5003,"playerId":"111"},"body":{"roomId":"288266"}}$_
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_Friend_ReadyRoom;
		headerData ["playerId"] = playerId;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Requests the initlaizi game. 游戏初始化完成
	/// </summary>
	/// <param name="playId">Play identifier.</param>
	/// <param name="roomid">Roomid.</param>
	public void RequestInitlaiziGame(string playerId , string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_initlaize;
		headerData ["playerId"] = playerId;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Requests the roll craps.请求掷骰子
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="roomId">Room identifier.</param>
	public void RequestRollCraps(string playerId , string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

        Console.Error.WriteLine("掷筛子接口");

		headerData ["type"] = Protocol.Game_RollCraps;
		headerData ["playerId"] = playerId;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Gets the borrow infor. 获取结款信息
	/// </summary>
	public void GetBorrowInfor()
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetBorrowInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Gets the player target infor.获取人物目标信息
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void GetPlayerTargetInfor(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetHeroTargetInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData["targetPlayerId"]=playerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Gets the player balance and income. 获取人物资产信息
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void GetPlayerBalanceAndIncome(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetAssetsIncomeInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData["targetPlayerId"]=playerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Gets the player debt and pay infor. 获取人物资产和负债信息
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void GetPlayerDebtAndPayInfor(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetPlayerDebtAndPayInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData["targetPlayerId"]=playerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}
		
	/// <summary>
	/// Gets the player infor sale record. 获取人物出售记录信息
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void GetPlayerInforSaleRecord(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetPlayerSaleRecordInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData["targetPlayerId"]=playerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Gets the player infor check infor. 获取人物结算面板信息
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	public void GetPlayerInforCheckInfor(string playerId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_GetCheckDataInfor;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = GameModel.GetInstance.curRoomId;
		badyData["targetPlayerId"]=playerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sends the card. 向后台发送卡牌请求
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="cardId">Card identifier.</param>
	/// <param name="cardType">Card type.</param>
	public void SendCard(string roomId,int cardId , int cardType)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SelectCard;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = cardId;
		badyData ["cardType"] = cardType;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();		
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Buies the card. 购买卡牌的参数
	/// </summary>
	/// <param name="portType">Port type.</param>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="cardId">Card identifier.</param>
	/// <param name="cardType">Card type.</param>
	/// <param name="cardNum">Card number.</param>
	/// <param name="cardRollPoint">Card roll point.</param>
	/// <param name="riskFreeChoise">If set to <c>true</c> risk free choise.</param>
	public void BuyCard(int portType , string roomId,int cardId , int cardType,int cardNum,int cardRollPoint=0,bool riskFreeChoise=false)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = portType;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = cardId;
		badyData ["cardType"] = cardType;
		badyData ["cardNumber"] = cardNum;
		badyData ["freeChoice"] = riskFreeChoise;
		badyData ["point"] = cardRollPoint;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

    /// <summary>
    /// 放弃卡牌
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="cardId"></param>
    /// <param name="cardType"></param>
	public void QuitCard(string roomId,int cardId , int cardType)
	{
		if (GameModel.GetInstance.isReconnecToGame == 1)
		{
			//GameModel.GetInstance.isReconnecToGame = 2;
			//NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
		}

		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_QuitCard;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = cardId;
		badyData ["cardType"] = cardType;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sures the give child card. 生孩子卡牌
	/// </summary>
	/// <param name="portType">Port type.</param>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="cardId">Card identifier.</param>
	/// <param name="cardType">Card type.</param>
	/// <param name="cardNum">Card number.</param>
	/// <param name="ifFine">If fine.</param>
	public void SureGiveChildCard(int portType , string roomId,int cardId , int cardType,int cardNum,int ifFine)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = portType;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = cardId;
		badyData ["cardType"] = cardType;
		badyData ["cardNumber"] = cardNum;
		badyData ["ifFine"] = ifFine;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sales the card. 出售资产卡牌
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="currentCardId">Current card identifier.</param>
	/// <param name="_cardList">Card list.</param>
	public void SaleFateFixedCard(string roomId , int currentCardId ,int curCadtype ,List<NetSaleCardVo> _cardList)
	{
		if (GameModel.GetInstance.isReconnecToGame == 1)
		{
			//GameModel.GetInstance.isReconnecToGame = 2;
			//NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
		}

		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SaleFixedCard;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = currentCardId;
		var dataList = new JsonData ();
		for (var i = 0; i < _cardList.Count; i++)
		{
			var tmpdata = new JsonData ();
			var tmpSaleVo=_cardList[i];
			tmpdata ["cardId"] =tmpSaleVo.cardId;
			tmpdata ["cardType"] = tmpSaleVo.cardType;
			dataList.Add (tmpdata);
		}
		badyData ["cardInfo"] = dataList;
		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}
	/// <summary>
	/// Sales the card. 出售股票卡牌
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="currentCardId">Current card identifier.</param>
	/// <param name="_cardList">Card list.</param>
	public void SaleChanceShareCard(string roomId , int currentCardId ,int curCadtype ,List<NetSaleCardVo> _cardList)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SaleChanceShareCard;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["cardId"] = currentCardId;
		var dataList = new JsonData ();
		for (var i = 0; i < _cardList.Count; i++)
		{
			var tmpdata = new JsonData ();
			var tmpSaleVo=_cardList[i];
			tmpdata ["cardId"] =tmpSaleVo.cardId;
			tmpdata ["cardType"] = tmpSaleVo.cardType;
            tmpdata["cardNumber"] = tmpSaleVo.cardNumber;
			dataList.Add (tmpdata);
		}
		badyData ["cardInfo"] = dataList;
		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}


	/// <summary>
	/// Sends the single end. 单人回合结束
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	public void Send_SingleRoundEnd(string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SingleRoundEnd;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();
		Debug.LogWarning("一个人的回合结束了---"+((int)Protocol.Game_SingleRoundEnd).ToString());

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sends the multi end. 多人回合结束
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	public void Send_MultiRoundEnd(string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_MultiRoundEnd;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		int isCheck = 0;
		if (PlayerManager.Instance.IsHostPlayerTurn ())
		{
			isCheck = 1;
		}

		var curPlayer = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];

		badyData ["status"] = isCheck;
		badyData ["nowPlayerId"]=curPlayer.playerID;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();
		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sends the game enter inner finished. 发送游戏晋级到内圈的接口
	/// </summary>
	public void Send_GameEnterInnerFinished(string roomId)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();
		headerData ["type"] = Protocol.Game_UpdateInnerFished;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
		badyData ["roomId"] = roomId;
		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Sends the red pocket. 发送红包
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="money">Money.</param>
	public void Send_RedPocket(string roomId , int money)
	{
		if (GameModel.GetInstance.isReconnecToGame == 1)
		{
			//GameModel.GetInstance.isReconnecToGame = 2;
			//NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
		}
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_SendRedPackeg;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["money"] = money;
		badyData ["receivePlayerId"] = GameModel.GetInstance.NetGameReceivedRedPlauerId;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Games the borrow money. 贷款 loanType 贷款类型 0银行打款  1信用卡贷款  2银行贷款+信用卡贷款
	/// </summary>
	public void Game_BorrowMoney(string roomId ,List<BorrowVo> _data,int loanType )
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_BorrowMoney;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;

		var tmpData = new JsonData ();
		for (var i = 0; i < _data.Count; i++)
		{			
			var borrowData=_data[i];
			tmpData["bankMoney"]=borrowData.bankborrow;
			tmpData["creditMoney"]=borrowData.cardborrow;
		}
		badyData ["loanInfo"] = tmpData;
		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Games the pay back money. 网络版本还款
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="_data">Data.</param>
	public void Game_PayBackMoney(string roomId , List<PaybackVo> _data)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_PayBackMoney;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		var tmpData = new JsonData ();
		for (var i = 0; i < _data.Count; i++)
		{
			var paybackData=_data[i];
			var tmpstr = new JsonData();
			tmpstr ["repayType"] = paybackData.netType;
			tmpstr ["repayName"] = paybackData.title;
			tmpData.Add(tmpstr);
		}
		badyData ["repayInfo"]= tmpData;
		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
	/// Games the buy ensurance. 购买保险
	/// </summary>
	/// <param name="roomId">Room identifier.</param>
	/// <param name="money">Money.</param>
	public void Game_BuyEnsurance(string roomId , int money)
	{
		JsonData sendData = new JsonData ();
		JsonData headerData = new JsonData ();
		JsonData badyData = new JsonData ();

		headerData ["type"] = Protocol.Game_BuyInsurance;
		headerData ["playerId"] = GameModel.GetInstance.myHandInfor.uuid;

		badyData ["roomId"] = roomId;
		badyData ["money"] = money;

		sendData ["header"] = headerData;
		sendData ["body"] = badyData;
		var msg = sendData.ToJson ();

		msg += endSymbol;
		sendMessage (msg);
	}

	/// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="message"></param>
	private void sendMessage(string message)
	{
        try
        {
            //Encoding.UTF8.GetBytes(message.ToCharArray()).ToCharArray()
            //socket.Send(Encoding.Unicode.GetBytes(message.ToCharArray()));
            //string msg = message.TrimEnd('_').TrimEnd('$');
            //var m= WWW.EscapeURL(msg)+"$_";
            var tmpbytes = Encoding.UTF8.GetBytes(message);
			Debug.Log("发送消息-------"+message);
            //Debug.Log("发送消息-------"+ m);
         
			clientSocket.Send(tmpbytes,tmpbytes.Length,SocketFlags.None);
			//socket.Send(arr.Buffer);
        }
		catch (Exception e)
		{
			//连接失败 打印异常
			Debug.LogError("socket send error:"+e.Message);
			if (null != clientSocket)
			{
				lostNetType = 0;
				clientSocket.Close ();
			}
			clientSocket = null;
			return;
		}
    }

    /// <summary>
    /// 这是读取服务器消息的回调--当有消息过来的时候BgenReceive方法会回调此函数
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
	{        
		int readCount = 1110;
		try
		{
			//读取消息长度
			readCount = clientSocket.EndReceive(ar);//调用这个函数来结束本次接收并返回接收到的数据长度。 
			if(readCount==0)
			{

				throw new Exception("被后台踢除");
			}

			//byte[] buffer = new byte[1024];  //buffer大小，此处为1024
			//int receivedSize=socket.Receive(buffer);
			//string rawMsg=Encoding.ASCII.GetString(readM,0,receivedSize);//System.Text.Encoding.Default.GetString(buffer, 0, receivedSize);

			byte[] bytes = new byte[readCount];//创建长度对等的bytearray用于接收
			//Buffer.BlockCopy(readM, 0, bytes, 0, readCount);//拷贝读取的消息到 消息接收数组
			ioBuff.WriteBytes(bytes);			
			string rawMsg=System.Text.Encoding.UTF8.GetString(readM,0,readCount);			
			dataStr+=rawMsg;
			_isReceiveMessage = true;
			onData();
            //消息读取完成
			//}
		}
		catch (Exception e)//出现Socket异常就关闭连接 
		{
			Debug.Log ("lllll Receive errero------"+e.Message);
			if (null != clientSocket)
			{
				lostNetType = 1;
				clientSocket.Close();//这个函数用来关闭客户端连接 
			}		
			clientSocket=null;
			dataStr="";
			return;
		}
		clientSocket.BeginReceive(readM, 0, 1024, SocketFlags.None, ReceiveCallBack,readM);

	}

    /// <summary>
    /// 处理data数据
    /// </summary>
	public void onData()
	{
	    //datastr 是空没有数据，不做处理
		if (dataStr == "")
		{
			return;
		}

        if (dataStr.Contains (this.endSymbol) == false)
		{
			return;//datastr 不是一条完整的数据，返回空
		}	
		//string[] tmpList = Regex.Split (dataStr, "$_");// dataStr.Split ();
		bool useLast = (dataStr.Substring(dataStr.Length-2,2)=="$_");
		//string[] tmpList=dataStr.Split(new char[2]{'$','_'},2,StringSplitOptions.RemoveEmptyEntries); 
		//string[] tmpList=Regex.Split(dataStr,"$_",RegexOptions.IgnoreCase);
		string[] tmpList = Regex.Split (dataStr,"\\$_");

		for (var i = 0; i < tmpList.Length-1; i++)
		{
			if (tmpList [i] != "")
			{
				messages.TrimExcess ();
				Debug.Log ("------"+i.ToString() +"------"+ tmpList[i].ToString());
				var tmpModel = chengDataToModel (tmpList [i]);
				messages.Add(tmpModel);
			}
		}

		if (useLast == true)
		{	
			dataStr = "";
		}
		else 
		{
			dataStr = tmpList [tmpList.Length - 1];
		}
		Debug.Log ("接收信息-----------------"+messages[messages.Count-1]);
		_isReceiveMessage = false;        
    }

	private bool _isReceiveMessage=false;

	public bool IsReceiveMessage
	{
		get
		{
			return _isReceiveMessage;
		}
	}  

    public List<SocketModel> getList()
	{
        return messages;
    }

    /// <summary>
    /// 把json数据转化成socketmodel
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
	private SocketModel  chengDataToModel(string data)
	{
		JsonData ss = JsonMapper.ToObject (data);
		var tmpModel = new SocketModel ();	

		tmpModel.type =int.Parse(ss["header"]["type"].ToString());
		tmpModel.message = data;
		return tmpModel;
	}
    
    /// <summary>
    /// 关闭网络连接
    /// </summary>
	public void CloseNet()
	{
        isConnect = false;
        hasConnect = false;
        if (null != clientSocket)
		{
			clientSocket.Close();//这个函数用来关闭客户端连接 
		}

		_EndHeartBeat ();
		clientSocket=null;		
		dataStr="";
		
	}

	/// <summary>
	/// Determines whether this instance is socket connect. 是否是连接
	/// </summary>
	/// <returns><c>true</c> if this instance is socket connect; otherwise, <c>false</c>.</returns>
	public bool IsSocketConnect()
	{
		var isconnect = false;
		if (null != clientSocket)
		{
			if (clientSocket.Connected == true)
			{
				isconnect = true;
			}
		}
		return isconnect;
	}

	/// <summary>
	/// The has connect.判断是否是已经连接上
	/// </summary>
	public bool hasConnect=false;

}

	/*
	 * StringBuilder sb = new StringBuilder();             //这个是用来保存：接收到了的，但是还没有结束的消息
	public void ReceiveMessage(object state)            //这个函数会被以线程方式运行
	{
		Socket socket = (Socket)state;
		while(true)
		{
			byte[] buffer = new byte[receiveBufferSize];  //buffer大小，此处为1024
			int receivedSize=socket.Receive(buffer);
			string rawMsg=System.Text.Encoding.Default.GetString(buffer, 0, receivedSize);
			int rnFixLength = terminateString.Length;   //这个是指消息结束符的长度，此处为 
			for(int i=0;i<rawMsg.Length;)               //遍历接收到的整个buffer文本
			{
				if (i <= rawMsg.Length - rnFixLength)
				{
					if (rawMsg.Substring(i, rnFixLength) != terminateString)//非消息结束符，则加入sb
					{
						sb.Append(rawMsg[i]);
						i++;
					}
					else
					{
						this.OnNewMessageReceived(sb.ToString());//找到了消息结束符，触发消息接收完成事件
						sb.Clear();
						i += rnFixLength;
					}   
				}
				else
				{
					sb.Append(rawMsg[i]);
					i++;
				}
			}
		}
	}

	//这个组件的使用方法：
	//复制代码 代码如下:

	A2DTcpClient client = new A2DTcpClient("127.0.0.1", 5000);
	client.NewMessageReceived += new MessageReceived(client_NewMessageReceived);
	client.Connect();
	client.Send("HELLO");
	client.Close();


	static void client_NewMessageReceived(string msg)
	{
		Console.WriteLine(msg);
	}*/
/*public class NetSession
{
	public void Connect()
	{
		Connect (_OnConnected);
	}

	private void Connect(Action OnConnected)
	{
		var sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
		sock.SendBufferSize    	= 8192;
		sock.ReceiveBufferSize	= 8192;
		sock.SendTimeout		= 2000;
		sock.ReceiveTimeout 	= 2000;
		sock.NoDelay			= true;

		sock.BeginConnect("127.0.0.1", 8501, _ConnectCallback, OnConnected);
		_sock = sock;
	}
	public void DisConnect()
	{
		if (null != _sock) 
		{
			_sock.Close ();
			_sock = null;
		}
	}
	private void _OnConnected ()
	{

	}
	private void _ConnectCallback (IAsyncResult result)
	{
		try
		{
			var sock = _sock;
			sock.EndConnect(result);

			if (!sock.Connected)
			{
				Console.Error.WriteLine("[_ConnectCallback()] Unable to connect to host.");
				return;
			}

			var OnConnected = result.AsyncState as Action;
			Loom.QueueOnMainThread(()=> OnConnected());

			var netThread = new Thread(_NetworkThread);
			netThread.Start();
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine ("[NetSession._ConnectCallback] Error :" + ex.ToStringEx());
		}
	}
	private void _NetworkThread ()
	{
		try
		{
			var sock = _sock;
			sock.Blocking = false;

			var readBuffer = new byte[4096];
			var osReadBuffer = new OctetsStream();
			var osWriteBuffer = new OctetsStream();

			while(sock.Connected)
			{
				_PollOut(sock, osWriteBuffer);
				_PollIn(sock, osReadBuffer, readBuffer);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine (ex.ToStringEx());
		}
	}
	private void _PollOut (Socket sock, OctetsStream osWriteBuffer)
	{
		if (_osSendBuffer.readableBytes() > 0)
		{
			lock (_sendLocker)
			{
				osWriteBuffer.append(_osSendBuffer);
				_osSendBuffer.clear();
			}
		}

		if (osWriteBuffer.readableBytes() == 0)
		{
			return;
		}

		if (!sock.Poll(_pollWaitTime, SelectMode.SelectWrite))
		{
			return;
		}

		var sendNumber = sock.Send(osWriteBuffer.array(), 0, osWriteBuffer.size(), SocketFlags.None);
		if (sendNumber > 0)
		{
			osWriteBuffer.erase (0, sendNumber);
		}
		else
		{
			throw new SocketException((int) SocketError.Fault);
		}
	}
	private void _PollIn (Socket sock, OctetsStream osReadBuffer, byte[] readBuffer)
	{
		if (!sock.Poll(_pollWaitTime, SelectMode.SelectRead))
		{
			return;
		}

		var readLength = sock.Receive(readBuffer, readBuffer.Length, SocketFlags.None);
		if (readLength == 0)
		{
			throw new SocketException((int) SocketError.ConnectionReset);
		}

		NetUtil.Decode(osReadBuffer, readBuffer, readLength);
	}
	public void DoAction<T>(object param = null)
	{
		var action = _actionFactory.Create<T> ();
		var bytes = action.GetRequestMsg (param);

		_osSendBuffer.append (bytes);
	}
	private Socket _sock;
	private int _pollWaitTime = 1000;
	private readonly object _sendLocker = new object();
	private readonly OctetsStream _osSendBuffer = new OctetsStream();
	private static readonly ActionFactory _actionFactory = ActionFactory.Instance;
}

*/