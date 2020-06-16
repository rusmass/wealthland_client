using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using LitJson;
using Client.UI;
using Metadata;
using Client;

namespace Client
{
    /// <summary>
    /// 处理后台返回数据的逻辑
    /// </summary>
	public partial class MessageManager{

		private static MessageManager _script;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
		public static MessageManager getInstance() {
			if (_script == null) {
				//第一次调用的时候 创建单例对象 并进行初始化操作
				_script = new MessageManager();
				_script.init();
			}
			return _script;
		}

		// Use this for initialization
		public void init () {
		}

		private float _heartBeatTime = 6f;
		private float _heartBeatInitTime=6f;

		private float _heartSendCount=0;

        /// <summary>
        /// 未引用
        /// </summary>
		public void ResetSendTime()
		{
			_heartSendCount = 0;
		}

        /// <summary>
        /// Update is called once per frame 刷新数据
        /// </summary>
        /// <param name="deltatime"></param>
        public void tick(float deltatime)
        {
            if(NetWorkScript.getInstance().isTickedOut==true)
            {
               return;
            }


			if (NetWorkScript.getInstance ().IsSocketConnect() == true)
			{
				if (NetWorkScript.getInstance ().IsReceiveMessage == false)
				{
					List<SocketModel> list = NetWorkScript.getInstance().getList();
					if (list.Count > 0)
					{
						SocketModel model = list[0];
						if (model != null)
						{
							OnMessage(model);
							list.Remove(model);
						}
					}
				}
			}           

			if (NetWorkScript.getInstance ().hasConnect==true)
			{
				if (NetWorkScript.getInstance ().IsSocketConnect () == false)
				{
                    //Console.Error.WriteLine("sssss----------ssss");

					NetWorkScript.getInstance ().CloseNet ();
//					NetWorkScript.getInstance ().init ();
//					NetWorkScript.getInstance ().ConnetServer (GameModel.GetInstance.curRoomId);
                   
                    if(NetWorkScript.getInstance().isTickedOut==false)
                    {
                        Console.Error.WriteLine("是被踢出了吗：" + NetWorkScript.getInstance().isTickedOut.ToString());
                        _isShowConnectBoard = true;
                    }

                    var tmpList = NetWorkScript.getInstance().getList();
                    if (null != tmpList && tmpList.Count > 0)
                    {
                        for (var i = 0; i < tmpList.Count; i++)
                        {
                            var tmpModel = tmpList[i];
                            if(tmpModel.type==Protocol.TickedOutLine)
                            {
                                _isShowConnectBoard = false;                      
                                _HandleTickedOut(tmpModel);
                                break;
                            }
                        }
                    }
                    leftTime = 0;
                    _heartBeatTime = _heartBeatInitTime;
                    if(_isShowConnectBoard == true)
                    {
                        GameModel.GetInstance.ShowNetLoading();
                    }					
				}

                //_heartBeatTime -= deltatime;
                //if (_heartBeatTime <= 0)
                //{
                //    //Console.Error.WriteLine("开始发送心跳包包");
                //    _heartBeatTime = _heartBeatInitTime;
                //    NetWorkScript.getInstance().SendHeartBeat();
                //}

                //心跳机制，依赖于ui计时，方便测试用 如果是在游戏界面就开启ui心跳机制，如果是在其他界面就用计时器倒计时
                //如果切换状态应该先发一个心跳包，
                if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState || GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetChooseState)
                {
                    //如果是在游戏过程中，时游ui绑定计时器，就是开始加载
                    if (GameModel.GetInstance.heartBeatState == 1)
                    {
                        _heartBeatTime -= deltatime;
                        if (_heartBeatTime <= 0)
                        {
                            _heartBeatTime = _heartBeatInitTime;
                            NetWorkScript.getInstance().SendHeartBeat();
                        }
                    }
                }
                else
                {
                    if (GameModel.GetInstance.heartBeatState == 1)
                    {
                        //如果不在游戏界面了，当前还想计时器心跳，就想后台发送星跳，暂停ui心跳，开启计时器心跳
                        _heartBeatTime = _heartBeatInitTime;
                        NetWorkScript.getInstance().SendHeartBeat();
                        GameModel.GetInstance.heartBeatState = 0;
                        NetWorkScript.getInstance()._StartHeartBeat();
                    }
                }
            }           

			if (_isShowConnectBoard == true)
			{
				leftTime += deltatime;
				if (leftTime >= 1)
				{
					_isShowConnectBoard = false;
					leftTime = 0;
					//如果在单机游戏过程中，掉线不进行处理，返回主界面后进行连接
					if (GameModel.GetInstance.isPlayNet == false&&GameModel.GetInstance.IsPlayingGame==1)
					{
                        //ReConnectNet();
                    }
                    else
					{
                        Console.Error.WriteLine(NetWorkScript.getInstance().isTickedOut.ToString());
                        
                        if (NetWorkScript.getInstance().lostNetType == 0)
                        {
                            var controller = UIControllerManager.Instance.GetController<UIGameSimpleTipController>();
                            controller.SetSureTip("您已经断开连接，请重新连接", ReConnectNet);
                            controller.setVisible(true);
                        }
                        else
                        {
                            ReConnectNet();
                        }

                        //var controller = UIControllerManager.Instance.GetController<UIGameSimpleTipController>();
                        //controller.SetSureTip("您已经断开连接，请重新连接", ReConnectNet);
                        //controller.setVisible(true);
                    }
				}
			}
		}
        /// <summary>
        /// 是否是显示重连
        /// </summary>
        private bool _isShowConnectBoard=false;

        /// <summary>
        /// 是否是被后台踢除
        /// </summary>
        private bool _isTickedOut = false;

		private float leftTime = 0;

        /// <summary>
        /// 重新开始连接
        /// </summary>
		private void ReConnectNet()
		{
			NetWorkScript.getInstance ().init ();
			NetWorkScript.getInstance ().ConnetServer (GameModel.GetInstance.myHandInfor.uuid);
		}

        /// <summary>
        /// 接收后台的数据，根据不同的接口处理不同的数据
        /// </summary>
        /// <param name="model"></param>
		private void OnMessage(SocketModel model)
		{
			if (null == model)
			{
				Console.Error.WriteLine ("有空的gamemodel了，请查看---------------------------------------------");
				return;
			}
			Console.WriteLine ("~~~~~~~~~~~~~执行啦：："+model.type.ToString()+"kkakakakkak"+model.message);

			switch(model.type)
			{
			case Protocol.WOSHOU:
				_HandleWoShow (model);
				break;

                case Protocol.TickedOutLine:
                    _HandleTickedOut(model);
                    break;

			case Protocol.HEART_BEAT:
				_HandleHeatBeat (model);
				break;		
			case Protocol.Game_Tips:
				_HandleGameTips (model);
				break;

			case Protocol.Game_Active_Rank:
				_HandlerRankActive (model);
				break;

			case Protocol.Game_LogOut:
                _HandlerLogOut(model);
                break;
			case Protocol.Game_ModifyData:
				_HandlModifyData (model);
				break;
			case Protocol.Game_Friend_Createroom:
				//_HandlerRoomEnterGame (model);
				_HandlerStartRoom (model);
				break;

			case Protocol.Game_Friend_EnterRoom:
				_HandlerEnterRoom (model);
				break;

			case Protocol.Game_Friend_ExitRoom:
				_HandlerExitRoom (model);
				break;
                case Protocol.Game_LostInRoom:
                    _HandlerExitRoom(model);
                    break;
            case Protocol.Game_Friend_ReadyRoom:
				_HandlerRoomReady (model);
				break;
            case Protocol.Game_Match_EnterGame:
                _HandlerCatchRoomReady(model);
                break;
			case Protocol.Game_Friend_EnterGameScene:
				_HandlerRoomEnterGame (model);
				break;

			case Protocol.Game_ReceiveSelectRoleInfor:
				_OnGetSelectHandlerData (model);
				break;

			case Protocol.Game_SelectRole:
				_NetSelecRoletHandler (model);
				break;

			case Protocol.Game_CancleSelect:
				_NetCancleSelecRoletHandler (model);
				break;
			case Protocol.Game_SureSelect:
				_NetSelectRoleSureHandler (model);
				break;

			case Protocol.Game_GetNornormalShareInfor:
				HandlerNormalShareData (model);
				break;
			case Protocol.Game_GetRoomShareInfor:
                    HandlerRoomShareData(model);
				break;
                case Protocol.Game_ShareDream:
                    HandlerDreamShareData(model);
                break;

			case Protocol.Game_initlaize:
				_HandlerInitlaiziGame (model);
				break;


			case Protocol.Game_GetBorrowInfor:
				_HandlerGameBorrowInfor (model);
				break;
			case Protocol.Game_GetHeroTargetInfor:
				HandlerPlayerTargetData (model);
				break;
			case Protocol.Game_GetAssetsIncomeInfor:
				HandlerBalanceAndIncomeData (model);
				break;
			case Protocol.Game_GetPlayerDebtAndPayInfor:
				HandlerPlayerDebtAndPayData (model);
				break;
			case Protocol.Game_GetPlayerSaleRecordInfor:
				HandlerPlayerSaleRecordData (model);
				break;
			case Protocol.Game_GetCheckDataInfor:
				HandlerPlayerCheckData (model);
				break;

			case Protocol.Game_RollCraps:
				_HandlerRollCraps (model);
				break;

			case Protocol.Game_SelectCard:
				_HandlerSelectChanceCard (model);
				break;

			case Protocol.Game_BuyFixedCard:
				_HandlerBuyAssetsCard (model);
				break;
			case Protocol.Game_BuyChanceShareCard:
				_HandlerBuyAssetsCard (model);
				break;
			case Protocol.Game_BuyOpportunityCard:
				_HandlerBuyAssetsCard (model);
				break;
			case Protocol.Game_BuyRiskCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyRelaxCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyQualityCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyStudyCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyHealthCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyOuterFateCard:
				_HandlerBuyCard (model);
				break;			
			case Protocol.Game_BuyInvestmentCard:
//				_HandlerBuyCard (model);
				_HandlerBuyInvestmentCard (model);
				break;
			case Protocol.Game_BuyInnerFateCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyCheckDayCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyCharityCard:
				_HandlerBuyCard (model);
				break;
			case Protocol.Game_BuyGiveChildCard:
				_HandlerBuyChild (model);
				break;


			case Protocol.Game_QuitCard:
				_HandlerQuitCard (model);
				break;


			case Protocol.Game_SaleFixedCard:
				_HandlerSaleFateCard (model);
				break;

			case Protocol.Game_SaleChanceShareCard:
				_HandlerSaleShareCard (model);
				break;

			case Protocol.Game_SingleRoundEnd:
				_HandlerSingleRoundEnd (model);
				break;
			case Protocol.Game_MultiRoundEnd:
				_HandlerMultiRoundEnd(model);
				break;

			case Protocol.Game_SendRedPackeg:
				_HandlerRedPackage (model);
				break;

			case Protocol.Game_BorrowMoney:
				_HandlerBarrowMoney (model);
				break;
			case Protocol.Game_PayBackMoney:
				_HandlerPayBackMoney (model);
				break;
			case Protocol.Game_BuyInsurance:
				_HandlerBuyInsurance (model);
				break;
			case Protocol.Game_UpdateInnerFished:
				_HandlerEnterInnerFinished (model);
				break;

			case Protocol.Game_GameHollChat:
				_HandleGameHallChatData (model);
				break;
			case Protocol.Game_RoomChat:
				_HandlerRoomChatData (model);
				break;
			case Protocol.Game_AgreeQuitGame:
				_HandlerAgreeExitGame (model);
				break;
			case Protocol.Game_RefuseQuitGame:
				_HandlerRefuseExitGame (model);
				break;

			case Protocol.Game_ChooseRoleLost:
				_HandlerGameChooseLost (model);
				break;

			case Protocol.Game_DisconnectInReady:
                _HandlerGameLostConnectReady(model);
				//_HandlerGameLostConnect (model);
				break;
            case Protocol.Game_Disconnect:
                _HandlerGameLostConnect(model);
                break;


			case Protocol.Game_ReConnectToGame:
				_HandlerReConnectToGame (model);
				break;

			case Protocol.Game_AgreeReConnect:
				_HandlerAgreeReConnectGame (model);
				break;
			case Protocol.Game_RefuseReConnect:
				_HandlerRefuseReConnectGame (model);
				break;			
				
			case Protocol.Game_ReConnectedInited:
				_HandlerReConnectInited (model);
				break;
            case Protocol.Game_ReConnectedFresh:
                _HandlerReConnectFresh(model);
                break;
            case Protocol.Gane_Match_OK:
                _HandlerMathGameIn(model);
                break;
                case Protocol.Game_ReadyBorrowFriend:
                    this._ReadyBorrowFriendHandler(model);
                    break;
                case Protocol.Game_BorrowFriendMoney:
                    this._BrrowFriendHandler(model);
                    break;
                case Protocol.Game_BorrowFriendInfor:
                    this._BorrowFriendInforHander(model);
                    break;

                case Protocol.Game_GameRecordInfor:
                    _HandlerGameReadData(model);
                    break;

                case Protocol.Game_GameRecoredDetail:
                    this._HandlerRecordDetail(model);
                    break;
            default:			
				break;			
			}
		}

       
        /// <summary>
        /// 获取玩家对战详情
        /// </summary>
        /// <param name="model"></param>
        private void _HandlerRecordDetail(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var state =int.Parse(backbody["status"].ToString());

            if (state == 0)
            {
                var detailArr = backbody["data"]["gameRecords"];
                if (detailArr.IsArray == true)
                {
                    List<DetailVo> detailList = new List<DetailVo>();
                    for(var i=0;i<detailArr.Count;i++)
                    {
        //                public string headPath;
        //public string name;
        //public string pro;
        //public string creat;
        //public string use;
        //public string mgr;
        //public string beyond;
        //public string all;

                        var tmpVo = new DetailVo();
                        var tmpObj = detailArr[i];

                        tmpVo.name = tmpObj["name"].ToString();
                        tmpVo.pro = tmpObj["occupation"].ToString();
                        tmpVo.headPath = tmpObj["job"].ToString();
                        tmpVo.creat = tmpObj["createWealth"].ToString();
                        tmpVo.use = tmpObj["useOfWealth"].ToString();
                        tmpVo.mgr = tmpObj["manageWealth"].ToString();
                        tmpVo.beyond = tmpObj["surpassWealth"].ToString();
                        tmpVo.all = tmpObj["comprehensiveScore"].ToString();
                        tmpVo.level = int.Parse(tmpObj["inOrOut"].ToString());



                        detailList.Add(tmpVo);
                    }

                    var detailController = UIControllerManager.Instance.GetController<UIPersonGameRecodeController>();
                    detailController.detailList = detailList;
                    detailController.setVisible(true);
                }
            }

        }


        /// <summary>
        /// 处理个人中心游戏数据的逻辑
        /// </summary>
        /// <param name="model"></param>
        private void _HandlerGameReadData(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var state = int.Parse(backbody["status"].ToString());

            if (state == 0)
            {
                var gameTotalRecord = backbody["data"]["gameStatistics"];
                int failNums =int.Parse(gameTotalRecord["failScreenings"].ToString());
                int winNums = int.Parse(gameTotalRecord["winScreenings"].ToString());
                int totalNums = failNums + winNums;
                string winRate = gameTotalRecord["winrate"].ToString();
                string avrage = gameTotalRecord["whenLong"].ToString();

               

                TotalGameRecordVo vo = new TotalGameRecordVo(totalNums,winRate,avrage);
                var inforController = UIControllerManager.Instance.GetController<UIPersonalController>();
                inforController.TotalGameRecord=vo;

                if (totalNums > 0)
                {
                    var recordList = backbody["data"]["gameRecord"];
                    List<ListData> recordListData = new List<ListData>();
                    if (recordList.IsArray)
                    {
                        for (var i = 0; i < recordList.Count; i++)
                        {
                            var tmpObj = recordList[i];
                            var tmpData = new ListData();

                            if (((IDictionary)tmpObj).Contains("createTime"))
                            {
                                var dateStr = (tmpObj["createTime"].ToString()).Split(new char[] { ' ' });
                                tmpData.date = dateStr[0];
                            }

                            tmpData.headPath = tmpObj["job"].ToString();
                            tmpData.levelState =int.Parse(tmpObj["inOrOut"].ToString());
                            tmpData.money = tmpObj["flowCash"].ToString();
                            tmpData.quality = tmpObj["qualityIntegral"].ToString();
                            if (((IDictionary)tmpObj).Contains("roomCode"))
                            {
                                tmpData.roomCode = tmpObj["roomCode"].ToString();
                            }
                            tmpData.time = tmpObj["whenLong"].ToString();
                            tmpData.timeCount = tmpObj["timeIntegral"].ToString();
                            recordListData.Add(tmpData);
                        }
                        inforController.GetGameDataList = recordListData;
                    }
                }
                inforController.setVisible(true);
            }
        }

        /// <summary>
        /// 准备好友借款的处理
        /// </summary>
        /// <param name="model"></param>
        private void _ReadyBorrowFriendHandler(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var state = int.Parse(backbody["status"].ToString());
            var playerId = backMessage["header"]["playerId"].ToString();

            //"data": {
            //    "players": [
            //        "f32d3180-65c5-4efa-bfff-9ac184df5593",
            //        "c172db27-b7e7-4fc4-a2c4-36c1350cb47f",
            //        "4d3a74d1-9dc5-47f6-bf76-4a126cbc40b1",
            //        "e4e88db1-5ed0-48f5-a2fe-cc893d1b10df"
            //]
            if (state==0)
            {
                List<string> tmpArr = new List<string>();
                var playerIdArr = backbody["data"]["players"];
                if(playerIdArr.IsArray)
                {
                    for (var i = 0; i < playerIdArr.Count;i++)
                    {
                        var tmpId = playerIdArr[i].ToString();
                        if(tmpId!=playerId)
                        {
                            tmpArr.Add(tmpId);
                            
                        }
                    }
                    if(tmpArr.Count>0)
                    {
                        var _controller = UIControllerManager.Instance.GetController<UIBorrowFriendController>();
                        _controller.PlayerIDArrs = tmpArr;
                        _controller.setVisible(true);
                    }
                    else
                    {
                        MessageHint.Show("当前玩家已离线，暂不能向他人借贷");
                    }

                   
                }                
            }          
        }
        
        /// <summary>
        /// 向好友借款
        /// </summary>
        /// <param name="model"></param>
        private void _BrrowFriendHandler(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var state = int.Parse(backbody["status"].ToString());
            if(state==0)
            {
                var _borrowcontroller = UIControllerManager.Instance.GetController<UIBorrowFriendController>();
                _borrowcontroller.setVisible(false);
            }
            else if(state==1)
            {
                var _controller = UIControllerManager.Instance.GetController<UIBorrowFriendTipController>();
                var data = backbody["data"];

                //var tmpRate =float.Parse(data["rate"].ToString());
                var tmpCash = int.Parse(data["cash"].ToString());
                var tmpId = data["playerId"].ToString();

                var tmpPlayer = PlayerManager.Instance.GetPlayerInfo(tmpId);
                var tmpName = tmpPlayer.playerName;

                _controller.PlayerName = tmpName;
                _controller.HeadPath = tmpPlayer.headName;
                _controller.TargetPlayerID = tmpId;
                _controller.TargetMoney = tmpCash;

                _controller.setVisible(true);
            }

            
        }

        /// <summary>
        /// 好友借款之后的返回信息，刷新玩家的数据
        /// </summary>
        /// <param name="socket"></param>
        private void _BorrowFriendInforHander(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];

            var data = backbody["data"];

            var tmpPlayerIds = data["playerIds"];
            if(tmpPlayerIds.IsArray==true)
            {
                for(var i=0;i<tmpPlayerIds.Count;i++)
                {
                    var tmpID = tmpPlayerIds[i].ToString();
                    var dataManager = data[tmpID];
                    var player = PlayerManager.Instance.GetPlayerInfo(tmpID);
                    HandlerJsonToCardVo.HandlerPlayerDataInfor(player, dataManager);
                    _updatePlayerShowInfor(player);
                }
            }

            if(((IDictionary)backbody["data"]).Contains("info"))
            {
                var tipInfo = backbody["data"]["info"].ToString();
                MessageHint.Show(tipInfo);
            }
            
            //JsonData datamanagerInfor = null;
            //if (((IDictionary)data).Contains("roleDataManageInfo"))
            //{
            //    datamanagerInfor = data["roleDataManageInfo"];

                //var player = PlayerManager.Instance.GetPlayerInfo(playerId);
                //HandlerJsonToCardVo.HandlerPlayerDataInfor(player, datamanagerInfor);
                //HandlerJsonToCardVo.HandlerPlayerAssetsInfor(player, roleHaveAssets);
                //_updatePlayerShowInfor(player);
            //}
        }

        /// <summary>
        /// 匹配进入游戏成功
        /// </summary>
        /// <param name="model"></param>
        private void _HandlerMathGameIn(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);

            var backbody = backMessage["body"];

            var stat1 = int.Parse(backbody["status"].ToString());

            if (stat1 == 0)
            {
                var baodyDadeArr = backbody["data"]["players"];
                var matchData = backbody["data"]["match_players"];
                var readyStatus = backbody["data"]["readyStatus"];
                var tmpList = new List<PlayerHeadInfor>();

                var waitControlelr = UIControllerManager.Instance.GetController<UITipWaitingWindowController>();
                if(waitControlelr.getVisible())
                {
                    waitControlelr.setVisible(false);
                }

                GameModel.GetInstance.curRoomId = backbody["data"]["roomId"].ToString();

                if (baodyDadeArr.IsArray == true)
                {
                    for (int i = 0; i < baodyDadeArr.Count; i++)
                    {
                        var playerData = matchData[baodyDadeArr[i].ToString()];
                        //var playerData = tmpData ["player"];
                        var name = playerData["name"].ToString();
                        var uuid = "";
                        if(((IDictionary)playerData).Contains("id"))
                        {
                            uuid = playerData["id"].ToString();
                        }
                        else
                        {
                            uuid = playerData["robotId"].ToString();
                        }
                        var ready = bool.Parse(readyStatus[uuid].ToString());
                        var headImg = playerData["avatar"].ToString();
                        var gender = int.Parse(playerData["gender"].ToString());  //0 女生   1男生

                        var tmpInfor = new PlayerHeadInfor();
                        tmpInfor.headImg = headImg;
                        tmpInfor.nickName = name;
                        tmpInfor.uuid = uuid;
                        tmpInfor.isReady = ready;
                        tmpInfor.sex = gender;

                        tmpList.Add(tmpInfor);
                    }

                    GameModel.GetInstance.roomPlayerInforList = tmpList;

                    if (GameModel.GetInstance.IsPlayingGame != GamePlayingState.GameNetGameState)
                    {
                        var fightRoomController = UIControllerManager.Instance.GetController<UIFightCatchController>();
                        if (fightRoomController.getVisible() == true)
                        {
                            fightRoomController.SetPlayerInfors(tmpList);
                        }
                        else
                        {
                            fightRoomController.headInforList = tmpList;
                            fightRoomController.setVisible(true);
                            //fightRoomController.SetPlayerInfors (tmpList);
                            //var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController>();
                            //gameHall.HideEnterRoomImg();
                        }
                    }
                }
            }
        }

        private void _HandlerLogOut(SocketModel model)
        {
           
        }
        /// <summary>
        /// 收到从新连接成功后，处理重连玩家显示的状态
        /// </summary>
        /// <param name="model"></param>
		private void _HandlerReConnectInited(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id


			if (stat1 == 0)
			{
				//通讯成功
				var playersArr = PlayerManager.Instance.Players;
				for (var i = 0; i < playersArr.Length; i++)
				{
					var tmpPlayer = playersArr[i];
					if (null != tmpPlayer)
					{
						if (tmpPlayer.playerID == playerId)
						{
							tmpPlayer.isNetOnline = true;
							tmpPlayer.isReconectGame = true;
							var uibattleController = UIControllerManager.Instance.GetController<UIBattleController> ();
							uibattleController.SetHeadBright (i);
							break;
						}
					}
				}
				GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
//				if(((IDictionary)backbody).Contains("data"))
//				{
//					if (((IDictionary)backbody["data"]).Contains ("playerId"))
//					{
//						var firstPlayerId = backbody["data"]["playerId"].ToString();
//						var playerArr = PlayerManager.Instance.Players;
//						for (var i = 0; i < playerArr.Length; i++)
//						{
//							var tmpplayer = playerArr [i];
//							if (null != tmpplayer)
//							{
//								if (tmpplayer.playerID == firstPlayerId)
//								{
//									var index = i - 1;
//									if (i < 0)
//									{
//										i += playerArr.Length;
//									}
//
//									Console.Error.WriteLine ("sssssssssss-----"+tmpplayer.playerID+"-------"+index.ToString());
//
//
//									GameModel.GetInstance.NetCurrentPlayerId = playerArr [index].playerID;
//									break;
//								}
//							}
//						}
//						VirtualServer.Instance.NetGameLostToNextOne();
//						VirtualServer.Instance.NetGameLostToSelectNext ();
//						VirtualServer.Instance.NetGameLostToUpGrade ();
////						VirtualServer.Instance.NetGameLostToNextOne ();
//					}
//				}
			}
		}

		/// <summary>
		/// Handlers the agree re connect game.同意加入游戏
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerAgreeReConnectGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)//通讯成功
			{
                //角色信息
				var baodyDadeArr= backbody["data"]["roleInfoData"];
                //玩家信息
                var playerArr= backbody["data"]["roleOrder"];
                //玩家位置信息
                var tablePositions = backbody["data"]["placeByTurntables"];
                //内圈外圈的情况
                var outOrIns = backbody["data"]["outOrIns"];
                //玩家名称
                var playerNames = backbody["data"]["playerNames"];
                //玩家掉钱情况
                var roleIsBreaks = backbody["data"]["roleIsBreaks"];

                if (playerArr.IsArray == true)
				{
					List<PlayerInfo> players = new List<PlayerInfo> ();
					for (var i = 0; i < playerArr.Count; i++)
					{
						var tmpPlayerId=playerArr[i].ToString();
						var tmpData = baodyDadeArr[tmpPlayerId];
						
						var name = playerNames[tmpPlayerId].ToString();
						var uuid = tmpPlayerId;			

						var playerInitdata = new PlayerInitData();

						var roleJson= tmpData; //roleBankLoanInfo 银行借贷信息  roleBasicInfo 基础信息   roleDataManageInfo 数据  roleDebtsInfo负债   roleHaveAssetInfo资产   roleLoanTotalRecordInfo 结款记录
						var roleBasicData=roleJson["roleBasicInfo"];//
						var roleModelInfor=roleJson["roleModelInfo"];//

						playerInitdata.id=int.Parse(roleBasicData["id"].ToString());//: 100001,
						playerInitdata.playName=roleBasicData["name"].ToString();//: "道客藤",
						playerInitdata.oneChildPrise=float.Parse(roleBasicData["giveChildMoney"].ToString());//: 480,;
						playerInitdata.careers=roleBasicData["professional"].ToString();//: "医生",
						playerInitdata.initAge=int.Parse(roleBasicData["age"].ToString());//: 20,
						playerInitdata.fixBankSaving=int.Parse(roleBasicData["bankSavings"].ToString());//: 500,
						playerInitdata.cashFlow =int.Parse(roleBasicData["wage"].ToString());//: 12000
						playerInitdata.infor=roleBasicData["introduction"].ToString();//: "外科专家，救治过千人以上，参与多次抢救行动。",;
						/// <summary>
						/// The player sex. 角色的性别
						/// </summary>
						playerInitdata.playerSex=int.Parse(roleBasicData["gender"].ToString());//: 0,;

						playerInitdata.headPath=roleModelInfor["headImgInfo"].ToString();//: "share/texture/head/yisheng.ab",;
						playerInitdata.playerImgPath=roleModelInfor["personImgInfo"].ToString();//: "share/atlas/battle/role/role_1.ab";
						playerInitdata.modelResID=int.Parse(roleModelInfor["modelId"].ToString());//: 0,
						// 加载模型路径
						playerInitdata.modelPath=roleModelInfor["modelPath"].ToString();//: "prefabs/character/01.ab",;;

						PlayerInfo playerInfor = null;
						//声明了新的人物信息设置
						var hasPlayer=false;
						for(var k=0;k<PlayerManager.Instance.Players.Length;k++)
						{
							playerInfor=PlayerManager.Instance.Players[k];
							if (null != playerInfor)
							{
								if (playerInfor.playerID == uuid)
								{
									hasPlayer = true;
									break;
								}
							}
						}

						if (hasPlayer == false)
						{
//							Console.Error.WriteLine ("要新建角色信息了");
							playerInfor= new PlayerInfo ();
							playerInfor.SetPlayerInitData (playerInitdata);
							playerInfor.playerID = uuid;
							playerInfor.playerName = name;
						}
									

						var tmpPoint = int.Parse (tablePositions[tmpPlayerId].ToString());
						var isOuter = int.Parse (outOrIns[tmpPlayerId].ToString ());
						var isBreakLine = int.Parse(roleIsBreaks[tmpPlayerId].ToString());

						if (isBreakLine == 0)
						{
							playerInfor.isNetOnline = false;
						}
						else
						{
							playerInfor.isNetOnline = true;
						}

						if (isOuter == 0)
						{
							playerInfor.isEnterInner = false;
							playerInfor.roundPostion = tmpPoint;
						}
						else
						{
							playerInfor.isEnterInner = true;
							playerInfor.roundPostion = tmpPoint;

						}

//						Console.Error.WriteLine ("玩家的姓名："+playerInfor.playerName + "------玩家的砖块位置："+playerInfor.roundPostion.ToString());

						// roleDataManageInfo 数据   
						JsonData datamanagerInfor = null;
						if (((IDictionary)roleJson).Contains ("roleDataManageInfo"))
						{
							datamanagerInfor=roleJson["roleDataManageInfo"];
						}

						//roleHaveAssetInfo资产 
						JsonData roleHaveAssets = null;
						if (((IDictionary)roleJson).Contains ("roleHaveAssetInfo"))
						{
							roleHaveAssets=roleJson["roleHaveAssetInfo"];
						}


						JsonData roleIncomeInfo = null;
						//roleDebtsInfo负债  
						JsonData debtInfor = null;
						//roleLoanTotalRecordInfo 结款记录
						JsonData borrowRecord = null;

						//roleBankLoanInfo 银行借贷信息    
						JsonData borrowBoarInfor = null;

						HandlerJsonToCardVo.UpdatePlayerInfor (playerInfor,datamanagerInfor,roleHaveAssets,roleIncomeInfo,debtInfor,borrowRecord,borrowBoarInfor);

						if (isOuter == 1)
						{							
							playerInfor.EnterInner ();
						}

						players.Add (playerInfor);
					}

                    if (((IDictionary)backbody["data"]).Contains("currentRole"))
                    {
                        GameModel.GetInstance.NetCurrentPlayerId = backbody["data"]["currentRole"].ToString();

                    }
                    else
                    {
                        GameModel.GetInstance.NetCurrentPlayerId = playerArr[1].ToString();
                    }


                    PlayerManager.Instance.SetNetPlayerInfor (reRankPlayer(players));
//					Console.Error.WriteLine("当前的重连状态"+GameModel.GetInstance.isReconnecToGame.ToString());
					if (GameModel.GetInstance.isReconnecToGame == 1)
					{
//						Console.Error.WriteLine("当前的游戏状态"+GameModel.GetInstance.IsPlayingGame.ToString());
						GameModel.GetInstance.isGameRealStart=0;
						if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState)
						{
							var uibattleController = UIControllerManager.Instance.GetController<UIBattleController> ();
							if (uibattleController.getVisible ())
							{																
								uibattleController.InitLaiziHideBtn ();
								uibattleController.InitLaiziStartLiazi ();
							}
							NetWorkScript.getInstance ().ReConnectInited ();
						}
						else
						{
							GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
							entergameAction ();
						}
					}
					else if(GameModel.GetInstance.isReconnecToGame==2)
					{
						if (null != PlayerManager.Instance.Players)
						{
							var playerss = PlayerManager.Instance.Players;
							for (var i = 0; i < playerss.Length; i++)
							{
								var tmpPlayer = playerss [i];
								if (null != tmpPlayer)
								{
									var battleController = UIControllerManager.Instance.GetController<UIBattleController> ();
									if (battleController.getVisible ())
									{
										if (tmpPlayer.isNetOnline == true)
										{
											battleController.SetHeadBright (i);
										}
										else
										{
											battleController.SetHeadGray (i);
										}
									}
								}
							}

						}
						GameModel.GetInstance.isGameRealStart = 1;
					}
				}
			}
		}

        //_HandlerReConnectFresh

        private void _HandlerReConnectFresh(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var stat1 = int.Parse(backbody["status"].ToString());

            if (stat1 == 0)//通讯成功
            {                
                //玩家信息
                var playerArr = backbody["data"]["roleOrder"];
                //玩家位置信息
                var tablePositions = backbody["data"]["placeByTurntables"];
                //内圈外圈的情况
                var outOrIns = backbody["data"]["outOrIns"];               
                //玩家掉钱情况
                var roleIsBreaks = backbody["data"]["roleIsBreaks"];

                if (playerArr.IsArray == true)
                {
                    List<PlayerInfo> players = new List<PlayerInfo>();
                    for (var i = 0; i < playerArr.Count; i++)
                    {
                        var tmpPlayerId = playerArr[i].ToString();                      
                        //var uuid = tmpPlayerId;                       
                        PlayerInfo playerInfor = PlayerManager.Instance.GetPlayerInfo(tmpPlayerId);

                        var tmpPoint = int.Parse(tablePositions[tmpPlayerId].ToString());
                        var isOuter = int.Parse(outOrIns[tmpPlayerId].ToString());
                        var isBreakLine = int.Parse(roleIsBreaks[tmpPlayerId].ToString());

                        if (isBreakLine == 0)
                        {
                            playerInfor.isNetOnline = false;
                        }
                        else
                        {
                            playerInfor.isNetOnline = true;
                        }

                        if (isOuter == 0)
                        {
                            playerInfor.isEnterInner = false;
                            playerInfor.roundPostion = tmpPoint;
                        }
                        else
                        {
                            playerInfor.isEnterInner = true;
                            playerInfor.roundPostion = tmpPoint;

                        }

                        //						Console.Error.WriteLine ("玩家的姓名："+playerInfor.playerName + "------玩家的砖块位置："+playerInfor.roundPostion.ToString());

                        // roleDataManageInfo 数据   
                        JsonData datamanagerInfor = null;
                        if (((IDictionary)backbody["data"]).Contains("roleDataManageInfo"))
                        {
                            datamanagerInfor = backbody["data"]["roleDataManageInfo"][tmpPlayerId];
                        }

                        //roleHaveAssetInfo资产 
                        JsonData roleHaveAssets = null;
                        if (((IDictionary)backbody["data"]).Contains("roleHaveAssetInfo"))
                        {
                            roleHaveAssets = backbody["data"]["roleHaveAssetInfo"][tmpPlayerId];
                        }

                        JsonData roleIncomeInfo = null;
                        //roleDebtsInfo负债  
                        JsonData debtInfor = null;
                        //roleLoanTotalRecordInfo 结款记录
                        JsonData borrowRecord = null;

                        //roleBankLoanInfo 银行借贷信息    
                        JsonData borrowBoarInfor = null;

                        HandlerJsonToCardVo.UpdatePlayerInfor(playerInfor, datamanagerInfor, roleHaveAssets, roleIncomeInfo, debtInfor, borrowRecord, borrowBoarInfor);

                        _updatePlayerShowInfor(playerInfor);

                        if (isOuter == 1)
                        {
                            playerInfor.EnterInner();
                        }

                        players.Add(playerInfor);
                    }

                    GameModel.GetInstance.NetCurrentPlayerId = backbody["data"]["currentRole"].ToString();
                    GameModel.GetInstance.isReconnecToGame = 2;
                    PlayerManager.Instance.SetNetPlayerInfor(reRankPlayer(players));
                    //					Console.Error.WriteLine("当前的重连状态"+GameModel.GetInstance.isReconnecToGame.ToString());
                    if (GameModel.GetInstance.isReconnecToGame == 1)
                    {
                        
                    }
                    else if (GameModel.GetInstance.isReconnecToGame == 2)
                    {
                        if (null != PlayerManager.Instance.Players)
                        {
                            var playerss = PlayerManager.Instance.Players;
                            for (var i = 0; i < playerss.Length; i++)
                            {
                                var tmpPlayer = playerss[i];
                                if (null != tmpPlayer)
                                {
                                    var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
                                    if (battleController.getVisible())
                                    {
                                        if (tmpPlayer.isNetOnline == true)
                                        {
                                            battleController.SetHeadBright(i);
                                        }
                                        else
                                        {
                                            battleController.SetHeadGray(i);
                                        }
                                    }
                                }
                            }

                        }
                        GameModel.GetInstance.isGameRealStart = 1;
                        VirtualServer.Instance.NetGameLostToNextOne();
                        VirtualServer.Instance.NetGameLostToSelectNext();
                        VirtualServer.Instance.NetGameLostToUpGrade();
                    }
                }
            }
        }


        /// <summary>
        /// Handlers the refuse re connect game. 拒绝重新加入游戏
        /// </summary>
        /// <param name="model">Model.</param>
        private void _HandlerRefuseReConnectGame(SocketModel model)
		{
			
		}

		/// <summary>
		/// Handlers the re connect to game. 收到重新连入游戏的请求
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerReConnectToGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			//var backbody = backMessage["body"];
			if (((IDictionary)backMessage ["body"]).Contains("data"))
			{
				GameModel.GetInstance.isReconnecToGame = 0;

				var bodyStatus = int.Parse (backMessage ["body"]["data"]["status"].ToString());
				if (bodyStatus == 0)
				{				

					var data = backMessage ["body"] ["data"];

					var tmpRoomId = data["roomId"].ToString ();

					var showReConnectBoard = false;

					GameModel.GetInstance.reConnectRoomId = tmpRoomId;
					//GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameRoomState || GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetChooseState||
					if (GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameChooseState)
					{
						//放弃重连入游戏
						_ReConnectTipRefuse();
					}
					else if(GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameRoomState || GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetChooseState)
					{
						if (GameModel.GetInstance.curRoomId != tmpRoomId)
						{
							_ReConnectTipRefuse ();
						}
						else
						{
							if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameRoomState)
							{
								var roomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
								roomController.setVisible (false);
							}
							showReConnectBoard = true;
						}
					}
					else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNetGameState && GameModel.GetInstance.isPlayNet==true)
					{
						//直接连接游戏
						_ReConnectTipSure();
					}
					else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNormalState)
					{
						showReConnectBoard = true;
					}

					if (showReConnectBoard == true)
					{
						//弹版提示是否是要重新连接
						var tipController = UIControllerManager.Instance.GetController<UITipSureOrNotController>();
						tipController.SetTip ("游戏对战未结束，是否重新加入游戏", _ReConnectTipSure, _ReConnectTipRefuse);
						tipController.setVisible (true);

						var gonggaoController = UIControllerManager.Instance.GetController<UIGongGaoController> ();
						if (gonggaoController.getVisible ())
						{
							gonggaoController.setVisible (false);
						}
					}
				}
				else
				{
					MessageHint.Show ("游戏已结束，房间已解散");

					if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState)
					{
						var tipController = UIControllerManager.Instance.GetController<UIGameSimpleTipController> ();
						tipController.SetSureTip ("游戏已结束，房间已解散");
						tipController.setVisible (true);

						var gameOverController = UIControllerManager.Instance.GetController<UIGameOverWindowController> ();
						if (gameOverController.getVisible ())
						{
							gameOverController.setVisible (false);
						}

						var quitroomController = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
						if (quitroomController.getVisible ())
						{
							quitroomController.setVisible (false);
						}

						Client.Unit.BattleController.Instance.Dispose();
						VirtualServer.Instance.Dispose ();
						//MessageHint.Dispose ();

						var effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
						effectController.ReInitConttoller ();
						effectController.setVisible (false);		

						var battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
						if (null != battlerController)
						{
							battlerController.setVisible (false);
							battlerController.RestartList ();
						}

						Client.Scenes.SceneManager.Instance.CurrentScene.Unload();
						PlayerManager.Instance.Dispose ();

						GameModel.GetInstance.InitNetGameBackData ();

						var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
						controller.setVisible (true);
						controller.LoadGameHallUI();
					}
				}
			}

//			if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameRoomState || GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetChooseState||GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameChooseState)
//			{
//				//放弃重连入游戏
//				_ReConnectTipRefuse();
//			}
//			else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNetGameState && GameModel.GetInstance.isPlayNet==true)
//			{
//				//直接连接游戏
//				_ReConnectTipSure();
//			}
//			else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNormalState)
//			{
//				//弹版提示是否是要重新连接
//				var tipController = UIControllerManager.Instance.GetController<UITipSureOrNotController>();
//				tipController.SetTip ("游戏对战未结束，是否重新加入游戏", _ReConnectTipSure, _ReConnectTipRefuse);
//				tipController.setVisible (true);
//			}
		}

        /// <summary>
        /// 确定重新连接进入游戏
        /// </summary>
		private void _ReConnectTipSure()
		{
			//isReconnecToGame
			GameModel.GetInstance.isPlayNet=true;

			var gonggaoController = UIControllerManager.Instance.GetController<UIGongGaoController> ();
			if (gonggaoController.getVisible ())
			{
				gonggaoController.setVisible (false);
			}

			GameModel.GetInstance.isReconnecToGame=1;
			GameModel.GetInstance.curRoomId = GameModel.GetInstance.reConnectRoomId;
			NetWorkScript.getInstance().AgreeToReConnetGame(GameModel.GetInstance.reConnectRoomId,0);

			var roomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
			if (roomController.getVisible ())
			{
				roomController.setVisible (false);
			}

		}

        /// <summary>
        /// 拒绝重新连接进入之前的游戏
        /// </summary>
		private void _ReConnectTipRefuse()
		{
			NetWorkScript.getInstance().RefuseToReConnectGame(GameModel.GetInstance.reConnectRoomId);

			var roomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
			if (roomController.getVisible ())
			{
				roomController.setVisible (false);
			}
		}

		/// <summary>
		/// Handlers the game choose lost. 选人界面掉线后返回到房间界面
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerGameChooseLost(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];

			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)
			{
				MessageHint.Show ("有玩家掉线，将返回房间等待");
				var baodyDadeArr= backbody["data"]["players"];
                var readyStatus = backbody["data"]["readyStatus"];
                var tmpList = new List<PlayerHeadInfor>();
                if (baodyDadeArr.IsArray == true)
                {
                    for (int i = 0; i < baodyDadeArr.Count; i++)
                    {
                        var playerData = baodyDadeArr[i];
                        var name = playerData["name"].ToString();
                        var uuid = playerData["id"].ToString();
                        var ready = bool.Parse(readyStatus[uuid].ToString());
                        var headImg = playerData["avatar"].ToString();
                        var gender = int.Parse(playerData["gender"].ToString());  //0 女生   1男生

                        var tmpInfor = new PlayerHeadInfor();
                        tmpInfor.headImg = headImg;
                        tmpInfor.nickName = name;
                        tmpInfor.uuid = uuid;
                        tmpInfor.isReady = ready;
                        tmpInfor.sex = gender;
                        tmpList.Add(tmpInfor);
                    }
                    GameModel.GetInstance.roomPlayerInforList = tmpList;

                    var fightRoomController = UIControllerManager.Instance.GetController<UIFightroomController>();
                    if (fightRoomController.getVisible() == true)
                    {
                        fightRoomController.SetPlayerInfors(tmpList);
                    }
                    else
                    {
                        fightRoomController.headInforList = tmpList;
                        fightRoomController.setVisible(true);
                        var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController>();
                        gameHall.HideEnterRoomImg();
                    }

                    var chooseNetRoleController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController>();
                    if (chooseNetRoleController.getVisible())
                    {
                        chooseNetRoleController.setVisible(false);
                    }
                }  			
			}
		}

        /// <summary>
        /// 处理同意退出房间的请求
        /// </summary>
        /// <param name="model"></param>
        private void _HandlerAgreeExitGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString());

            //1、倒计时过程中，多数人结束了
            //   
            //
            //
            //
			if (stat1 == 0)//单个初始化完成
			{
				if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState)
				{
					var totalNum = int.Parse (backbody ["data"] ["numberOfPeople"].ToString ());
					var agreeNum = int.Parse (backbody ["data"] ["numberOfPeopleAgree"].ToString ());					

					var playerId = backhead ["playerId"].ToString ();
					var controller = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
					if (controller.getVisible () == false)
					{
						controller.setVisible (true);
					}
					controller.SetPlayGameNnum (agreeNum,totalNum);
					controller.SetHandlerNum (playerId);

                    if(((IDictionary)backbody["data"]).Contains("exit"))
                    {
                        var exit = int.Parse(backbody["data"]["exit"].ToString());
                        if (exit == 1)
                        {
                            controller.HandlerExitRoom();
                            GameModel.GetInstance.InitNetGameBackData();
                            netExitGameDeleteCards();
                        }
                    }                   
				}
			}
		}

        /// <summary>
        /// 处理拒绝退出房间的请求
        /// </summary>
        /// <param name="model"></param>
		private void _HandlerRefuseExitGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];
			//var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString());
			if (stat1 == 0)//单个初始化完成
			{
				//var playerId = backhead ["playerId"].ToString ();
                if (((IDictionary)backbody["data"]).Contains("exit"))
                {
                    var exit = int.Parse(backbody["data"]["exit"].ToString());
                    if (exit == 0)
                    {
                        var controller = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController>();
                        if (controller.getVisible())
                        {
                            controller.InitData();
                            controller.setVisible(false);
                        }
                    }
                }
            }
		}
        /// <summary>
        /// 处理游戏大厅聊天
        /// </summary>
        /// <param name="model"></param>
        private void _HandleGameHallChatData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			if (((IDictionary)backMessage).Contains ("body"))
			{
				var body =backMessage ["body"]["data"];
				var head = backMessage ["header"];
				var chatvo=new NetChatVo();
				chatvo.chat = body ["msg"].ToString ();
				chatvo.playerName = body ["name"].ToString ();
				chatvo.playerHead = body ["headImg"].ToString ();
				chatvo.playerId = head ["playerId"].ToString ();

				chatvo.sex = int.Parse(body ["gender"].ToString ());
				chatvo.sendTime =  body["sendTime"].ToString();

				var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController> ();
				gameHall.SetChatWord (chatvo);

				var gameChat = UIControllerManager.Instance.GetController<UIGameHallChatController> ();
				gameChat.AddNewChatLog (chatvo);

			}
		}
        /// <summary>
        /// 处理房间中聊天
        /// </summary>
        /// <param name="model"></param>
        private void _HandlerRoomChatData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			if (((IDictionary)backMessage).Contains ("body"))
			{
				var body =backMessage ["body"]["data"];
				var head = backMessage ["header"];
				var chatvo=new NetChatVo();
				chatvo.chat = body ["msg"].ToString ();
				chatvo.playerName = body ["name"].ToString ();
				chatvo.playerHead = body ["headImg"].ToString ();
				chatvo.playerId = head ["playerId"].ToString ();

				chatvo.sex = int.Parse(body ["gender"].ToString ());
				chatvo.sendTime =body["sendTime"].ToString();

				if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameRoomState)
				{
					var fightRoomChat = UIControllerManager.Instance.GetController<UIFightroomController> ();
					fightRoomChat.AddNewChatLog (chatvo);
				}
				else
				{
					var uibatlle = UIControllerManager.Instance.GetController<UIBattleController> ();
					uibatlle.UpdateChatData (chatvo);
				}

			}
		}
        /// <summary>
        /// 处理修改人物信息
        /// </summary>
        /// <param name="model"></param>
        private void _HandlModifyData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var stat1 =int.Parse(backMessage ["body"] ["status"].ToString());

			if(stat1==0)//成功
			{
				MessageHint.Show ("修改人物信息成功");
				GameModel.GetInstance.UpdatePlayerInfor ();

				var gameHallController = UIControllerManager.Instance.GetController<Client.UI.UIGameHallWindowController> ();
				gameHallController.UpdatePlayerHeadInfor ();

			}
			else if(stat1==-1)// 失败
			{
				MessageHint.Show ("修改人物信息失败");
			}
		}

		private void _HandlLogOut(SocketModel model)
		{

		}

        /// <summary>
        /// 处理活跃度排行榜数据
        /// </summary>
        /// <param name="model"></param>
		private void _HandlerRankActive(SocketModel model)
		{
			var data = JsonMapper.ToObject(model.message);

			var list = new List<GameRankVo> ();

			var status = int.Parse (data ["body"] ["status"].ToString ());

			if (status == 0)
			{
				var tipList = data["body"]["data"]["players"];
				if (tipList.IsArray)
				{
					for (var i = 0; i < tipList.Count; i++)
					{
						var rankVO = new GameRankVo ();
						var tmpData= tipList[i];
						rankVO.headPath=tmpData["headImg"].ToString();
						rankVO.rankTip = tmpData ["game_number"].ToString ();
						rankVO.playerName = tmpData["name"].ToString();
						rankVO.rankIndex = i+1;
						list.Add (rankVO);
					}
				}		

				//var myRankData=data["body"]["data"]["player"];


				GameModel.GetInstance.gameActiveRankList = list;

				var controller = UIControllerManager.Instance.GetController<Client.UI.UIGameRankWindowController> ();
				controller.activeRankList = GameModel.GetInstance.gameActiveRankList;
				controller.setVisible (true);
			}
		}
        /// <summary>
        /// 处理链接游戏，握手成功后，处理状态
        /// </summary>
        /// <param name="model"></param>
        private void _HandleWoShow(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var stat1 =int.Parse(backMessage ["body"] ["status"].ToString());

			if (stat1 == 0)
			{
				Console.WriteLine ("握手成功");
				GameModel.loginStatus = 1;
				NetWorkScript.getInstance ().hasConnect=true;

				GameModel.GetInstance.isReconnecToGame = 0;
				GameModel.GetInstance.NetCurrentPlayerId = "";

				NetWorkScript.getInstance ()._StartHeartBeat ();

				GameModel.GetInstance.HideNetLoading ();

				HttpRequestManager.GetInstance ().GetGameNotice();

              

				if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNormalState)
				{
                    //Console.Error.WriteLine("当前的游戏状态：" + GameModel.GetInstance.IsPlayingGame);
                    var waitcontorller = UIControllerManager.Instance.GetController<UITipWaitingWindowController>();
                    if (waitcontorller.getVisible())
                    {
                        waitcontorller.setVisible(false);
                    }

                  
                }
				else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameRoomState)
				{
					var controller = UIControllerManager.Instance.GetController<UIFightroomController> ();
					if (controller.getVisible ())
					{
						controller.setVisible (false);
					}

                    var fightRoomController = UIControllerManager.Instance.GetController<UIFightCatchController>();
                    if (fightRoomController.getVisible() == true)
                    {
                        fightRoomController.setVisible(false);
                    }

                    if (GameModel.GetInstance.isPlayNet == true)
					{
						if ("" != GameModel.GetInstance.curRoomId&&GameModel.GetInstance.playNetMode==0)
						{
							NetWorkScript.getInstance ().RequestEnterRoom (GameModel.GetInstance.myHandInfor.uuid,GameModel.GetInstance.curRoomId);
						}
					}
				}
				else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameWaitState)
				{
					var tmpController = UIControllerManager.Instance.GetController<UITipWaitingWindowController> ();
					if (tmpController.getVisible ())
					{
						tmpController.setVisible (false);
					}

                   
				}
				else if(GameModel.GetInstance.IsPlayingGame==GamePlayingState.GameNetGameState)
				{
                    VirtualServer.Instance.ReStartGame();
                    Client.Unit.BattleController.Instance.ReStartGame();
                    PlayerManager.Instance.ReStartGame();
                    Client.Scenes.SceneManager.Instance.CurrentScene.RestartGame();
                    MessageHint.Dispose();
                    GameModel.GetInstance.NetCurrentPlayerId = Client.PlayerManager.Instance.Players[2].playerID;
                    Client.Unit.BattleController.Instance.Send_StartGame();
                    //_battleController.Send_StartGame();
                    netExitGameDeleteCards ();
				}
				else if(GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetChooseState)
				{
					
					var tmpController = UIControllerManager.Instance.GetController<UITipWaitingWindowController> ();
					if (tmpController.getVisible ())
					{
						tmpController.setVisible (false);
					}

					var netChooseRoleController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
					if (netChooseRoleController.getVisible ())
					{
						netChooseRoleController.setVisible (false);
					}

					if (GameModel.GetInstance.curRoomId != "")
					{
						NetWorkScript.getInstance ().RequestEnterRoom (GameModel.GetInstance.myHandInfor.uuid, GameModel.GetInstance.curRoomId);
					}
				}

			}
			else if(stat1==1)
			{
				Console.WriteLine ("握手失败");
			}
//			_HandleGameTips(model);
		}


        public void _HandleTickedOut(SocketModel model)
        {
            //UnityEngine.GameObject
            Console.Error.WriteLine("被踢出出去了啦啦啦啦啦");
            NetWorkScript.getInstance().isTickedOut = true;
            NetWorkScript.getInstance().CloseNet();

            NetWorkScript.getInstance().getList().Clear();

            var tmpData = JsonMapper.ToObject(model.message);
            var body = tmpData["body"];
            var tmpStr = "您的账号在其他设备登录，请及时修改密码，保障账户安全！";
            if(((IDictionary)body).Contains("tip"))
            {
                tmpStr = body["tip"].ToString();
            }
            var controller = UIControllerManager.Instance.GetController<UIGameSimpleTipController>();
            controller.SetSureTip(tmpStr, backToStartGuid);
            controller.setVisible(true);

        }

        /// <summary>
        /// 返回到游戏引导页
        /// </summary>
        private void backToStartGuid()
        {
            var tmpScene = GameObject.FindObjectsOfType(typeof(Canvas));

            if(Game.Instance.GetCurrentFsmState()== GameFSM.FSMStateType.GameHallState)
            {
                GameModel.GetInstance.DeleteGamehallScene();
            }
            else if(Game.Instance.GetCurrentFsmState()==GameFSM.FSMStateType.LoadingState)
            {
               // Console.Error.WriteLine("清楚游戏场景啦");
               
                Client.Unit.BattleController.Instance.Dispose();
                VirtualServer.Instance.Dispose();
                //MessageHint.Dispose ();

                var effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
                effectController.ReInitConttoller();
                effectController.setVisible(false);
                GameModel.GetInstance.InitNetGameBackData();
                Client.Scenes.SceneManager.Instance.CurrentScene.Unload();
                PlayerManager.Instance.Dispose();
                GameModel.GetInstance.DeleteGameScene();
            }

            Game.Instance.SwitchLoginWindow();          

           // UIControllerManager.Instance.GetController <class( GameModel.GetInstance._startScenes[0]) > ();
        }

        /// <summary>
        /// Nets the exit game delete cards.退出房间，删除游戏界面的卡牌
        /// </summary>
        public void netExitGameDeleteCards()
		{
				var battleController = UIControllerManager.Instance.GetController<UIBattleController> ();
				if (battleController.getVisible())
				{
					battleController.HideCrapBtn ();
				}

				var borrowController = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
				if (borrowController.getVisible ())
				{
					borrowController.setVisible (false);
				}

				var setController = UIControllerManager.Instance.GetController<UIGameSetWindowController> ();
				if (setController.getVisible ())
				{
					setController.setVisible (false);
				}

				var inforController = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				if (inforController.getVisible ())
				{
					inforController.CloseHandler ();
				}

				var quitController = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
				if (quitController.getVisible ())
				{
					quitController.setVisible (false);
				}

				var netReadyController = UIControllerManager.Instance.GetController<UINetGameReadyWindowController> ();
				if (netReadyController.getVisible ()) 
				{
					netReadyController.setVisible (false);
				}

				var conclusionController = UIControllerManager.Instance.GetController<UIConclusionController> ();
				if (conclusionController.getVisible ())
				{
					conclusionController.setVisible (false);
				}

				var enterInnerController = UIControllerManager.Instance.GetController<UIEnterInnerWindowController> ();
				if (enterInnerController.getVisible ())
				{
					enterInnerController.setVisible (false);
				}

				var riskController = UIControllerManager.Instance.GetController<UIRiskCardController> ();
				if (riskController.getVisible ())
				{
					riskController.setVisible (false);
				}

				var chanceShare = UIControllerManager.Instance.GetController<UIChanceShareCardController> ();
				if (chanceShare.getVisible ())
				{
					chanceShare.setVisible (false);
				}

				var chanceFixedContolelr = UIControllerManager.Instance.GetController<UIChanceFixedCardController> ();
				if (chanceFixedContolelr.getVisible ())
				{
					chanceFixedContolelr.setVisible (false);
				}

				var oppoertunityController = UIControllerManager.Instance.GetController<UIOpportunityCardController> ();
				if (oppoertunityController.getVisible ())
				{
					oppoertunityController.setVisible (false);
				}

				var outerFateController = UIControllerManager.Instance.GetController<UIOuterFateCardController> ();
				if (outerFateController.getVisible ())
				{
					outerFateController.setVisible (false);
				}

				var otherController = UIControllerManager.Instance.GetController<UIOtherCardWindowController> ();
				if (otherController.getVisible ())
				{
					otherController.setVisible (false);
				}

				var selectChanceController = UIControllerManager.Instance.GetController<UIChanceSelectController> ();
				if (selectChanceController.getVisible ())
				{
					selectChanceController.setVisible (false);
				}

				var redPackeyController = UIControllerManager.Instance.GetController<UIRedPacketWindowController> ();
				if (redPackeyController.getVisible ())
				{
					redPackeyController.setVisible (false);
				}


				var innerSelectController = UIControllerManager.Instance.GetController<UIInnerSelectFreeWindowController> ();
				if (innerSelectController.getVisible ())
				{
					innerSelectController.setVisible (false);
				}

				var innerfatecontroller = UIControllerManager.Instance.GetController<UIInnerFateCardController> ();
				if (innerfatecontroller.getVisible ())
				{
					innerfatecontroller.setVisible (false);
				}

				var investmentController = UIControllerManager.Instance.GetController<UIInvestmentCardController> ();
				if (investmentController.getVisible ())
				{
					investmentController.setVisible (false);
				}

				var qualityController = UIControllerManager.Instance.GetController<UIInvestmentCardController> ();
				if (qualityController.getVisible ())
				{
					qualityController.setVisible (false);
				}

				var relaxController = UIControllerManager.Instance.GetController<UIRelaxCardController> ();
				if (relaxController.getVisible ())
				{
					relaxController.setVisible (false);
				}

            var buyshare = UIControllerManager.Instance.GetController<UIBuyCareWindowController>();
            if(buyshare.getVisible())
            {
                buyshare.setVisible(false);
            }

            var borrowFriend = UIControllerManager.Instance.GetController<UIBorrowFriendController>();
            if(borrowFriend.getVisible())
            {
                borrowFriend.setVisible(false);
            }

            var judgeBorrow = UIControllerManager.Instance.GetController<UIBorrowFriendTipController>();
            if(judgeBorrow.getVisible())
            {
                judgeBorrow.setVisible(false);
            }
		}

        /// <summary>
        /// 处理心跳状态机制的。目前后台值接收，不返消息
        /// </summary>
        /// <param name="model"></param>
        private void _HandleHeatBeat(SocketModel model)
		{
			//NetWorkScript.getInstance ().SendHeartBeat ();
		}

        /// <summary>
        /// 显示游戏公告信息
        /// </summary>
        /// <param name="model"></param>
		private void _HandleGameTips(SocketModel model)
		{
			//-
			//{"body":{"data":[{"title":"鏄庢棩缁存姢","content":"鏄庢棩鏃╀笂鍏?偣閽熻繘琛岀淮鎶ｋ澶ф?闇瑕佷袱涓?皬鏃惦鍏蜂綋寮鏀炬椂闂畴璇峰叧娉ㄥ畼缃愷"},
			//{"title":"鏄庢棩缁存姢","content":"鏄庢棩鏃╀笂鍏?偣閽熻繘琛岀淮鎶ｋ澶ф?闇瑕佷袱涓?皬鏃惦鍏蜂綋寮鏀炬椂闂畴璇峰叧娉ㄥ畼缃愸"}]},
			//"header":{"attachment":{},"type":1000}}

			//{"body":{"data":[{"title":"鏄庢棩缁存姢","content":"鏄庢棩鏃╀笂鍏?偣閽熻繘琛岀淮鎶ｋ澶ф?闇瑕佷袱涓?皬鏃惦鍏蜂綋寮鏀炬椂闂畴璇峰叧娉ㄥ畼缃愷"},
			//                 {"title":"鏄庢棩缁存姢","content":"鏄庢棩鏃╀笂鍏?偣閽熻繘琛岀淮鎶ｋ澶ф?闇瑕佷袱涓?皬鏃惦鍏蜂綋寮鏀炬椂闂畴璇峰叧娉ㄥ畼缃愸"}]},
			//"header":{"attachment":{},"type":1000}}

			GameModel.GetInstance.gonggaoList.Clear ();
			GameModel.GetInstance.gonggaoList.TrimExcess ();

			var data = JsonMapper.ToObject(model.message);

			var tipList = data["body"]["data"]["notices"];
			if (tipList.IsArray)
			{
				for (int i = 0; i < tipList.Count; i++)
				{
					var tip = tipList[i];

					var title = tip["title"].ToString();
					var content = tip ["content"].ToString ();
					var type =int.Parse(tip ["type"].ToString ());

					var gonggaovo = new GonggaoVo ();
					gonggaovo.title = title;
					gonggaovo.content = content;
					gonggaovo.type = type;
					Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + gonggaovo.title);
					GameModel.GetInstance.gonggaoList.Add (gonggaovo);
				}
			}

			if (GameModel.GetInstance.isFirstInGameHall == true)
			{
				_ShowGonggao ();
			}
		}

		/// <summary>
		/// Shows the gonggao.游戏公告
		/// </summary>
		private void _ShowGonggao()
		{
			if (GameModel.GetInstance.gonggaoList.Count > 0)
			{
				var gonggaocontroller = UIControllerManager.Instance.GetController<Client.UI.UIGongGaoController> ();
				gonggaocontroller.inforList = GameModel.GetInstance.gonggaoList;
				gonggaocontroller.setVisible (true);
			}
		}
	}
}

