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
	public partial class MessageManager
	{
		/// <summary>
		/// Handlers the start room. 响应进入房间
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerStartRoom(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString());
			if (stat1 == 0)//成功
			{
				var roomid = backbody["data"]["roomId"].ToString ();
				Console.WriteLine ("创建房间成功");
				GameModel.GetInstance.curRoomId=roomid;
				GameModel.GetInstance.roomPlayerInforList.Clear ();
				GameModel.GetInstance.roomPlayerInforList.Add(GameModel.GetInstance.myHandInfor);
				var gameHallController = UIControllerManager.Instance.GetController<UIGameHallWindowController> ();
				gameHallController.ShowFightRoom ();
			}
		}

        /// <summary>
        /// 处理玩家掉线的数据
        /// </summary>
        /// <param name="model"></param>
		private void _HandlerGameLostConnect(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];
			var playerId = backMessage ["header"] ["playerId"].ToString();
			var stat1 =int.Parse(backbody["status"].ToString());

			//			return;
			if (stat1 == 0)
			{					
				if (GameModel.GetInstance.IsPlayingGame == GamePlayingState.GameNetGameState)
				{
					var players = PlayerManager.Instance.Players;
					for (var i = 0; i < players.Length; i++)
					{
						var tmpPlayer = players [i];
						if (null != tmpPlayer)
						{
							if (tmpPlayer.playerID == playerId)
							{
								tmpPlayer.isNetOnline = false;
//								Console.Error.WriteLine ("是否在游戏前掉线------"+tmpPlayer.isOffLineBeforGame);
								if (GameModel.GetInstance.isGameRealStart == 0)
								{
									tmpPlayer.isOffLineBeforGame = true;
//									Console.Error.WriteLine ("游戏前掉线玩家:---------"+tmpPlayer.playerName+"索引值是-----------------"+i.ToString());
									if (GameModel.GetInstance.isReconnecToGame == 1)
									{
										if (i == Client.Unit.BattleController.Instance.CurrentPlayerIndex)
										{
											//										Console.Error.WriteLine ("游戏开始后掉线的玩家--------"+tmpPlayer.playerName+"索引值是------"+i.ToString());
											//Client.Unit.BattleController.Instance.NetGameSeletcResult();
											VirtualServer.Instance.NetGameLostToNextOne();
											VirtualServer.Instance.NetGameLostToSelectNext ();
											VirtualServer.Instance.NetGameLostToUpGrade ();
										}
									}

								}
								else if(GameModel.GetInstance.isGameRealStart==1)
								{
									if (i == Client.Unit.BattleController.Instance.CurrentPlayerIndex)
									{
//										Console.Error.WriteLine ("游戏开始后掉线的玩家--------"+tmpPlayer.playerName+"索引值是------"+i.ToString());
										//Client.Unit.BattleController.Instance.NetGameSeletcResult();
										VirtualServer.Instance.NetGameLostToNextOne();
										VirtualServer.Instance.NetGameLostToSelectNext ();
										VirtualServer.Instance.NetGameLostToUpGrade ();
									}
								}

								var tips = string.Format ("玩家{0}离开游戏", tmpPlayer.playerName);

								MessageHint.Show (tips);

//								if (GameModel.GetInstance.isReconnecToGame == 1)
//								{
//									
//								}

								var battleController = UIControllerManager.Instance.GetController<UIBattleController> ();
								if (battleController.getVisible ())
								{
									battleController.SetHeadGray (i);
//									battleController.setVisible (true);
//									battleController.SetTipRecord (100);
								}

								break;
							}
						}
					}
					var tmpData = backMessage ["body"] ["data"];
					if (((IDictionary)tmpData).Contains ("exit"))
					{
						//判断是否是同意解散房间
						var totalNum = int.Parse (tmpData["numberOfPeople"].ToString ());
						var agreeNum = int.Parse (tmpData["numberOfPeopleAgree"].ToString ());

						var exit = bool.Parse (tmpData["exit"].ToString ());

						var controller = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
						if (controller.getVisible () )
						{
							controller.SetPlayGameNnum (agreeNum,totalNum);
							controller.SetHandlerNum (playerId);

							if (exit == true)
							{
								controller.HandlerExitRoom ();
								GameModel.GetInstance.InitNetGameBackData ();
								netExitGameDeleteCards ();
							}
						}
					}

				}
			}
		}

		/// <summary>
		/// Handlers the enter room.  响应 加入房间
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerEnterRoom(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];

			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0) {
				var baodyDadeArr = backbody ["data"] ["players"];

				var tmpList = new List<PlayerHeadInfor> ();

				if (baodyDadeArr.IsArray == true) {
					for (int i = 0; i < baodyDadeArr.Count; i++) {
						var tmpData = baodyDadeArr [i];

						var playerData = tmpData ["player"];

						var name = playerData ["name"].ToString ();
						var uuid = playerData ["uuid"].ToString ();
						var ready = bool.Parse (tmpData ["ready"].ToString ());
						var headImg = playerData ["headImg"].ToString ();
						var gender = int.Parse (playerData ["gender"].ToString ());  //0 女生   1男生

						var tmpInfor = new PlayerHeadInfor ();
						tmpInfor.headImg = headImg;
						tmpInfor.nickName = name;
						tmpInfor.uuid = uuid;
						tmpInfor.isReady = ready;
						tmpInfor.sex = gender;

						tmpList.Add (tmpInfor);
					}

					GameModel.GetInstance.roomPlayerInforList = tmpList;

					if (GameModel.GetInstance.IsPlayingGame != GamePlayingState.GameNetGameState) {
						var fightRoomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
						if (fightRoomController.getVisible () == true)
						{
							fightRoomController.SetPlayerInfors (tmpList);
						} 
						else 
						{
							fightRoomController.headInforList = tmpList;
							fightRoomController.setVisible (true);
							//						fightRoomController.SetPlayerInfors (tmpList);
							var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController> ();
							gameHall.HideEnterRoomImg ();
						}
					}
				}
			}
			else
			{
				if (stat1 == -1)
				{//没有此房间
					var str = string.Format ("房间号[{0}]错误，没有此房间", GameModel.GetInstance.curRoomId);
					MessageHint.Show (str);
					GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;
				} 
				else if (stat1 == -2)
				{//输入房间人员已满
					MessageHint.Show ("输入房间人员已满");
					GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;
				}
				else
				{
					GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;
				}

				GameModel.GetInstance.HideNetLoading ();

				var gameHallController = UIControllerManager.Instance.GetController<UIGameHallWindowController> ();
				if (gameHallController.getVisible ())
				{
					gameHallController.OnReInitNumTxt ();
				}

			}
		}
        
		/// <summary>
		/// Handlers the exit room. 响应退出房间
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerExitRoom(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];

			var stat1 =int.Parse(backbody["status"].ToString());
//			return;
			if (stat1 == 0)
			{				
				if (null != backMessage ["header"] ["playerId"])
				{

					var tmpStr = backMessage ["header"] ["playerId"].ToString ();

					if (GameModel.GetInstance.myHandInfor.uuid == tmpStr)
					{
						var tmpController = UIControllerManager.Instance.GetController<UIFightroomController> ();
						if (tmpController.getVisible ())
						{
							tmpController.setVisible (false);
						}
						GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;
						return;
					}

					for (int i = 0; i < GameModel.GetInstance.roomPlayerInforList.Count; i++)
					{
						if(null != GameModel.GetInstance.roomPlayerInforList [i])
						{
							if (GameModel.GetInstance.roomPlayerInforList [i].uuid == tmpStr)
							{
								GameModel.GetInstance.roomPlayerInforList [i] = null;
							}
						}
					}
				}

				var fightRoomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
				if(fightRoomController.getVisible()==true)
				{
					fightRoomController.SetPlayerInfors (GameModel.GetInstance.roomPlayerInforList);
				}
			}
		}

		/// <summary>
		/// Handlers the room ready. 响应在房间里准本好
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerRoomReady(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];

			var stat1 =int.Parse(backbody["status"].ToString());


			if (stat1 == 0)
			{			
				var playerId = backMessage["header"]["playerId"].ToString();

				var tmpData = backbody["data"]["player"].ToString();
//				var name = tmpData["name"].ToString();
//				var uuid = tmpData ["uuid"].ToString ();
//				var ready = bool.Parse(tmpData ["ready"].ToString());
//				var headImg = tmpData ["headImg"].ToString ();
//				var gender  = int.Parse(tmpData["gender"].ToString());  //0 女生   1男生

//				var tmpInfor = new PlayerHeadInfor ();
//				tmpInfor.headImg = headImg;
//				tmpInfor.nickName = name;
//				tmpInfor.uuid = uuid;
//				tmpInfor.isReady = ready;
//				tmpInfor.sex = gender;

				for (int i = 0; i < GameModel.GetInstance.roomPlayerInforList.Count; i++)
				{
					if (null != GameModel.GetInstance.roomPlayerInforList [i])
					{
						if (GameModel.GetInstance.roomPlayerInforList [i].uuid == tmpData)
						{
							GameModel.GetInstance.roomPlayerInforList [i].isReady = true;
						}
					}
				}
				var fightRoomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
				if(fightRoomController.getVisible()==true)
				{
					fightRoomController.SetPlayerInfors (GameModel.GetInstance.roomPlayerInforList);
				}

				if (playerId ==GameModel.GetInstance.myHandInfor.uuid)
				{
					fightRoomController.SetSureBtnDisabled ();
				}
//				entergameAction ();
			}
		}

		/// <summary>
		/// Handlers the initlaizi game. 初始化游戏完成
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerInitlaiziGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);

			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString());

			var battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
			var readyController = UIControllerManager.Instance.GetController<UINetGameReadyWindowController> ();

			if (stat1 == 0)//单个初始化完成
			{				
				var playerId = backhead ["playerId"].ToString ();
				readyController.SetReadyPlayerId (playerId);

				if (playerId == PlayerManager.Instance.HostPlayerInfo.playerID)
				{
					battlerController.InitLaiziHideBtn ();

					if (GameModel.GetInstance.isRoomAllReady == true)
					{
						battlerController.InitLaiziStartLiazi ();
						readyController.HideTipHandler ();
					}
					else
					{
						if (GameModel.GetInstance.isReconnecToGame == 0)
						{
							//|| GameModel.GetInstance.isReconnecToGame == 2

							if (readyController.getVisible () == false)
							{
								readyController.setVisible (true);
							}
						}
					}
				}
			}
			else if(stat1==1)//所有的人物都初始化完成
			{	
				if (GameModel.GetInstance.isRoomAllReady == true)
				{
					return;
				}	
				if (GameModel.GetInstance.isReconnecToGame == 0)//|| GameModel.GetInstance.isReconnecToGame == 2
				{
					GameModel.GetInstance.isGameRealStart = 1;
					GameModel.GetInstance.isRoomAllReady = true;

//					Console.Error.WriteLine ("没掉线的玩家初始化完成啦啦啦啦啦啦啦");
				}
				battlerController.InitLaiziStartLiazi ();
				readyController.HideTipHandler ();
			}
		}

		/// <summary>
		/// Handlers the roll craps. 掷色子
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerRollCraps(SocketModel model)
		{
			/*{
			 "body":
			 {
			 "data":{
			 "cardInfo":{
			 "billName":"股票代码",
			 "cardIntegral":6,
			 "dividend":"无分红",
			 "id":30014,
			 "instructions":"\\u3000\\u3000公司经营没有达到预期目标。股价跌至历史低点。只有你才能按此价格购买该股票。\\n每个人都可按此价格出售该股票。",
			 "investmentIncome":"0%",
			 "name":"股票369健康管理公司",
			 "nonLaborIncome":0,
			 "path":"share/atlas/battle/card/chancecard1/card_gupiao_2.ab",
			 "priceScope":"10-30",
			 "qualityIntegral":0,
			 "qualityIntegralInstruction":"",
			 "stockCode":"369",
			 "stockNumber":0,"todayPrice":-10,"type":2},
			 
			 "closingDateMoney":2750,
			 "cardType":-986,
			 "closingDateNumber":0,
			 "point":[6]},
			 "status":0},
			 "header":{"attachment":{},"playerId":"4d100592-a8a6-4f7c-9d1e-cf55330d2678","type":6001}}
			*/

            var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"]["data"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backMessage["body"]["status"].ToString());

			if (stat1 == 0)
			{
				//开牌类型
				var cardType=int.Parse(backbody["cardType"].ToString());
				GameModel.GetInstance.ShowCardType=cardType;
				//卡牌信息

				if (((IDictionary)backbody).Contains ("cardInfo"))
				{
					var cardInfor=backbody["cardInfo"];
					HandlerJsonToCardVo.HandlerCardsDatas (cardType,cardInfor);
//					Console.Error.WriteLine ("初始化卡牌数据喽");
				}			

				if (((IDictionary)backbody).Contains ("ifFine"))
				{
					GameModel.GetInstance.isGiveChild = int.Parse (backbody["ifFine"].ToString());
				}

				var crapsArr=backbody["point"];
				if (crapsArr.IsArray == true)
				{
					var playerId = backhead ["playerId"].ToString ();

					GameModel.GetInstance.curRollPoints.Clear();
					GameModel.GetInstance.curRollPoints.TrimExcess ();
					for (int i = 0; i < crapsArr.Count; i++)
					{
						var nums =int.Parse(crapsArr [i].ToString ());
						GameModel.GetInstance.curRollPoints.Add(nums);
					}

					if (GameModel.GetInstance.curRollPoints.Count == 3)
					{
						for (int i = 0; i < PlayerManager.Instance.Players.Length; i++)
						{
							var targetPlayer = PlayerManager.Instance.Players [i];
							if (targetPlayer.playerID == playerId)
							{
								targetPlayer.isThreeRoll = true;
								break;
							}
						}
					}

					var battlecontroller = UIControllerManager.Instance.GetController<UIBattleController> ();
					battlecontroller.NetSendRequestRoll ();
				}
			}
		}

		/// <summary>
		/// Handlers the send card.处理接受显示卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerSelectChanceCard(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"]["data"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backMessage["body"]["status"].ToString());

			if (stat1 == 0)
			{
				//开牌类型
				var cardType = int.Parse (backbody ["cardType"].ToString ());
				GameModel.GetInstance.ShowCardType = cardType;

				if (((IDictionary)backbody).Contains ("cardInfo")) {
					var cardInfor = backbody ["cardInfo"];
					HandlerJsonToCardVo.HandlerCardsDatas (cardType, cardInfor);
				}			

				NetCardManager.Instance.OpenCard (GameModel.GetInstance.ShowCardType);
			}
			return;
		}

		/// <summary>
		/// Handlers the buy card. 购买卡牌逻辑
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerBuyCard(SocketModel model)
		{
			/*
			{"body":
			{"data":
			{
			"roleDataManageInfo":{"assetTotalMoney":1500,"cash":18795,"cashFlow":18795,"debtTotalMoney":33750,"haveChildNumber":0,"nonLaborIncome":0,"qualityIntegral":1,"timeIntegral":0,"totalIncome":20000,"totalSpending":1205},
			"roleHaveAssetInfo":{
			"bigChances":[],
			"smallChances":[{"cardIntegral":1,"cost":"1500","downPayment":-1500,"id":20003,"instructions":"\\u3000\\u3000在一个交易会上,你发现了宋朝的3件精致玉器,每件售价￥500.可以自己接受这笔生意,也可以卖给其他玩家.","integralNumber":1,"integralType":2,"investmentIncome":"0%","mortgageLoan":0,"name":"稀有的玉石","nonLaborIncome":0,"number":3,"path":"share/atlas/battle/card/fixedcard1/card_d_48.ab","sellPrice":"0-5000/件","type":3}],
			"stocks":[]},
			"roleIncomeInfo":{"bigChanceNonLaborIncome":[],"laborIncome":[20000],"smallChanceNonLaborIncome":[],"stockNonLaborIncome":[]}
			}
			,"status":0},
			"header":{"attachment":{},"playerId":"4d100592-a8a6-4f7c-9d1e-cf55330d2678","type":6003}}
			*/

			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id
			var portType =int.Parse(backhead["type"].ToString());
			if (stat1 != 0)
			{
				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
				{
					
				}
				else
				{
					if (portType == (int)Protocol.Game_BuyChanceShareCard ||portType ==(int)Protocol.Game_BuyOuterFateCard)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}
				return;
			}            
			var data = backbody ["data"];

			JsonData datamanagerInfor = null;
			if (((IDictionary)data).Contains ("roleDataManageInfo"))
			{
				datamanagerInfor=data["roleDataManageInfo"];
			}

			JsonData roleHaveAssets = null;
			if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
			{
				roleHaveAssets=data["roleHaveAssetInfo"];
			}
            
			if (portType ==Protocol.Game_BuyRiskCard)
			{
				if (((IDictionary)data).Contains ("ifSuspended"))
				{
					var unem =int.Parse(data["ifSuspended"].ToString());

//					Console.Error.WriteLine ();

					var isEmployment = true;
					if (unem == 1)
					{
						isEmployment = false;
					}
					PlayerManager.Instance.GetPlayerInfo (playerId).isEmployment=isEmployment;
				}
			}

			if (stat1 == 0)//返回数据成功 购买开牌成功了  aa购买开牌成功了
			{
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				HandlerJsonToCardVo.HandlerPlayerAssetsInfor (player, roleHaveAssets);
				_updatePlayerShowInfor(player);
//				_UpdatePlayerInfor (playerId, datamanagerInfor, roleHaveAssets);

				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
				{
					
				}
				else
				{
					if (portType == (int)Protocol.Game_BuyChanceShareCard ||portType ==(int)Protocol.Game_BuyOuterFateCard)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}

			}
		}

		/// <summary>
		/// Handlers the buy child. 购买生孩子卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerBuyChild(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id
			var portType =int.Parse(backhead["type"].ToString());
			if (stat1 == 0)//生孩子其他人发红包
			{
				GameModel.GetInstance.NetGameReceivedRedPlauerId=playerId;
				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)
				{
					var player = PlayerManager.Instance.GetPlayerInfo (playerId);

					var myplayer = PlayerManager.Instance.HostPlayerInfo;

					if (myplayer.totalMoney > 0)//如果自己的金币>0 ,发红包
					{
						if (GameModel.GetInstance.isReconnecToGame != 1)
						{
							var redController = Client.UIControllerManager.Instance.GetController<UIRedPacketWindowController>();
							redController.player =player;
							redController.OpenPackRedPacket();
							redController.setVisible(true);
						}
						else
						{
							NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId,0);
						}

					}
					else
					{
						NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId,0);
					}
				}

			}
			else if(stat1==1)//超生罚款
			{
				var data = backbody ["data"];
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}

				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				_updatePlayerShowInfor(player);

				NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
			}
		}

		/// <summary>
		/// Handlers the buy assets card. 处理购买资产的卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerBuyAssetsCard(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id
			var portType =int.Parse(backhead["type"].ToString());
			if (stat1 != 0)
			{

				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
				{

				}
				else
				{
					if (portType == (int)Protocol.Game_BuyChanceShareCard)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}

				return;
			}

			var data = backbody ["data"];

			JsonData datamanagerInfor = null;
			if (((IDictionary)data).Contains ("roleDataManageInfo"))
			{
				datamanagerInfor=data["roleDataManageInfo"];
			}

			JsonData roleHaveAssets = null;
			if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
			{
				roleHaveAssets=data["roleHaveAssetInfo"];
			}

			if (stat1 == 0)//返回数据成功 购买开牌成功了  aa购买开牌成功了
			{

				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				HandlerJsonToCardVo.HandlerPlayerAssetsInfor (player, roleHaveAssets);
				_updatePlayerShowInfor(player);

				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
				{

				}
				else
				{
					if (portType == (int)Protocol.Game_BuyChanceShareCard ||portType ==(int)Protocol.Game_BuyOuterFateCard)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}

			}
		}

		/// <summary>
		/// Handlers the buy card. 购买卡牌逻辑
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerBuyInvestmentCard(SocketModel model)
		{
			/*

			{"body":
			{"data":
			{
			"roleDataManageInfo":{"assetTotalMoney":1500,"cash":18795,"cashFlow":18795,"debtTotalMoney":33750,"haveChildNumber":0,"nonLaborIncome":0,"qualityIntegral":1,"timeIntegral":0,"totalIncome":20000,"totalSpending":1205},
			"roleHaveAssetInfo":{
			"bigChances":[],
			"smallChances":[{"cardIntegral":1,"cost":"1500","downPayment":-1500,"id":20003,"instructions":"\\u3000\\u3000在一个交易会上,你发现了宋朝的3件精致玉器,每件售价￥500.可以自己接受这笔生意,也可以卖给其他玩家.","integralNumber":1,"integralType":2,"investmentIncome":"0%","mortgageLoan":0,"name":"稀有的玉石","nonLaborIncome":0,"number":3,"path":"share/atlas/battle/card/fixedcard1/card_d_48.ab","sellPrice":"0-5000/件","type":3}],
			"stocks":[]},
			"roleIncomeInfo":{"bigChanceNonLaborIncome":[],"laborIncome":[20000],"smallChanceNonLaborIncome":[],"stockNonLaborIncome":[]}
			}
			,"status":0},
			"header":{"attachment":{},"playerId":"4d100592-a8a6-4f7c-9d1e-cf55330d2678","type":6003}}
			*/

			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 

			var playerId = backhead["playerId"].ToString();//玩家的id

			var portType =int.Parse(backhead["type"].ToString());
			if (stat1 ==-1)
			{
				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
				{
				}
				else
				{
					if (portType == (int)Protocol.Game_BuyChanceShareCard ||portType ==(int)Protocol.Game_BuyOuterFateCard)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}

				return;
			}



			var data = backbody ["data"];

			JsonData datamanagerInfor = null;
			if (((IDictionary)data).Contains ("roleDataManageInfo"))
			{
				datamanagerInfor=data["roleDataManageInfo"];
			}

			JsonData roleHaveAssets = null;
			if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
			{
				roleHaveAssets=data["roleHaveAssetInfo"];
			}

//			_UpdatePlayerInfor (playerId, datamanagerInfor, roleHaveAssets);

			var player = PlayerManager.Instance.GetPlayerInfo (playerId);
			HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
			HandlerJsonToCardVo.HandlerPlayerAssetsInfor (player, roleHaveAssets);
			_updatePlayerShowInfor (player);

			if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己的开牌，刷新游戏、如果是自己的操作，发送下一句逻辑
			{
			}
			else
			{
				if (portType == (int)Protocol.Game_BuyChanceShareCard ||portType ==(int)Protocol.Game_BuyOuterFateCard)//如果买了股票的要进行数量的操作
				{
					NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
				}
				else 
				{
					NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
				}
			}
		}

		/// <summary>
		/// Updates the player infor. 刷新人物信息
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="dataManager">Data manager.</param>
		/// <param name="assetsData">Assets data.</param>
		/// <param name="incomeData">Income data.</param>
		private void _UpdatePlayerInfor(string playerId , JsonData dataManager , JsonData assetsData = null ,JsonData incomeData=null, JsonData debtInfor=null ,JsonData borrowRecord=null,JsonData borrowBoardInfor=null)
		{
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
			PlayerInfo heroInfor=null;

			for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
			{
				if (PlayerManager.Instance.Players [i].playerID == playerId)
				{
					heroInfor = PlayerManager.Instance.Players [i];

					break;
				}
			}
			HandlerJsonToCardVo.UpdatePlayerInfor (heroInfor,dataManager,assetsData,incomeData,debtInfor,borrowRecord,borrowBoardInfor);

			var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
			if (null != battleController)
			{
				if (PlayerManager.Instance.HostPlayerInfo.playerID ==heroInfor.playerID ) 
				{
					if (heroInfor.isEnterInner == false)
					{
						battleController.SetQualityScore ((int)heroInfor.qualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.totalIncome,(int)heroInfor.MonthPayment);
						battleController.SetCashFlow ((int)heroInfor.totalMoney);		
					}
					else
					{
						battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
					}													
				}
				else
				{
					battleController.SetPersonInfor (heroInfor,turnIndex);
				}
			}
		}
        
		/// <summary>
		/// Updates the player show infor. 刷新人物的显示信息
		/// </summary>
		/// <param name="heroInfor">Hero infor.</param>
		private void _updatePlayerShowInfor(PlayerInfo heroInfor)
		{
			var turnIndex = 0;// Client.Unit.BattleController.Instance.CurrentPlayerIndex;

			for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
			{
				if (PlayerManager.Instance.Players [i].playerID == heroInfor.playerID)
				{
					turnIndex = i;
					break;
				}
			}
			var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
			if (null != battleController)
			{
				if (PlayerManager.Instance.HostPlayerInfo.playerID ==heroInfor.playerID ) 
				{
					if (heroInfor.isEnterInner == false)
					{
						battleController.SetQualityScore ((int)heroInfor.qualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.totalIncome,(int)heroInfor.MonthPayment);
						battleController.SetCashFlow ((int)heroInfor.totalMoney);		
					}
					else
					{
						battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
					}			
//					Console.Error.WriteLine ("是自己的账号啊"+heroInfor.playerID+"sssssssssssss____"+heroInfor.totalMoney.ToString());
				}
				else
				{
					battleController.SetPersonInfor (heroInfor,turnIndex,false);
//					Console.Error.WriteLine ("不是的账号啊"+heroInfor.playerID+"sssssssssssss____"+heroInfor.totalMoney.ToString());
				}
			}
		}

		/// <summary>
		/// Handlers the quit card. 响应放弃卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerQuitCard(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
//			var cardId=int.Parse(backbody["cardId"].ToString()); // 返回的cardid 购买的卡牌
//			var carNum=int.Parse(backbody["cardId"].ToString());
			var cardType=int.Parse(backbody["data"]["cardType"].ToString());

			var playerId = backhead["playerId"].ToString();//玩家的id

			if (stat1 == 0)//返回数据成功
			{
				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是不是自己在操作，不做处理。如果是自己的的操作，返回本轮结束
				{
					
				}
				else
				{
					if (cardType == (int)SpecialCardType.sharesChance ||cardType ==(int)SpecialCardType.outFate)//如果买了股票的要进行数量的操作
					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					}
					else 
					{
						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
					}
				}

			}
		}
		/// <summary>
		/// Handlers the sale card. 响应卖出卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerSaleFateCard(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
//			var cardId=int.Parse(backbody["cardId"].ToString()); // 返回的cardid 购买的卡牌
			//			var carNum=int.Parse(backbody["cardId"].ToString());
//			var cardType=int.Parse(backbody["cardType"].ToString());

			var playerId = backhead["playerId"].ToString();//玩家的id,

			if (stat1 == 0)//返回数据成功
			{
				var data = backbody ["data"];
				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}

				JsonData roleHaveAssets = null;
				if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
				{
					roleHaveAssets=data["roleHaveAssetInfo"];
				}

				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				HandlerJsonToCardVo.HandlerPlayerAssetsInfor (player, roleHaveAssets);
				_updatePlayerShowInfor(player);
//				_UpdatePlayerInfor (playerId, datamanagerInfor, roleHaveAssets);
				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是自己在操作，刷新游戏
				{
//						}
//						for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
//						{
//							var tmpplayer = PlayerManager.Instance.Players [i];
//							if (tmpplayer.playerID == playerId)
//							{
//								/// 处理卖出的卡牌
//								if (cardType == (int)SpecialCardType.outFate)
//								{
//									NetCardHandler.UpdateSaleOuterFate (tmpplayer, cardId, tmpSaleList);
//								}
//								else if(cardType==(int)SpecialCardType.sharesChance)
//								{
//									NetCardHandler.UptateSaleChanceShares (tmpplayer, cardId, tmpSaleList);
//								}
//								break;
//							}
//						}
//					}
				}
				else
				{
//					if (cardType == (int)SpecialCardType.sharesChance ||cardType ==(int)SpecialCardType.outFate)//如果买了股票的要进行数量的操作
//					{
						NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
//					}
//					else 
//					{
//						NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
//					}
				}
			}			
		}

		/// <summary>
		/// Handlers the sale card. 响应卖出股票卡牌
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerSaleShareCard(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 

			var playerId = backhead["playerId"].ToString();//玩家的id,

			if (stat1 == 0)//返回数据成功
			{
				var data = backbody ["data"];

				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}

				JsonData roleHaveAssets = null;
				if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
				{
					roleHaveAssets=data["roleHaveAssetInfo"];
				}

				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				HandlerJsonToCardVo.HandlerPlayerAssetsInfor (player, roleHaveAssets);
				_updatePlayerShowInfor(player);

//				_UpdatePlayerInfor (playerId, datamanagerInfor, roleHaveAssets, roleIncomeInfo);

				if (PlayerManager.Instance.HostPlayerInfo.playerID != playerId)//如果是自己在操作，刷新游戏
				{
					
				}
				else
				{
					NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
				}
			}			
		}

		/// <summary>
		/// Handlers the single round end. 响应单局玩家结束
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerSingleRoundEnd(SocketModel model)
		{
			Debug.Log("要进行下一个回合了啊");

			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString());

			var isEnterData = int.Parse (backbody ["data"] ["ifPromotion"].ToString ());
			var playerId = backhead["playerId"].ToString();

			var tmpPlayer = PlayerManager.Instance.GetPlayerInfo (playerId);
			tmpPlayer.netIsCanEnter = isEnterData;

			GameModel.GetInstance.NetCurrentPlayerId = playerId;

			if (((IDictionary)backbody ["data"]).Contains ("ifSuccess"))
			{
//				datamanagerInfor=data["roleDataManageInfo"];
				tmpPlayer.netIsSuccess = int.Parse (backbody ["data"]["ifSuccess"].ToString());
			}

//			for(var i=0;i<PlayerManager.Instance.Players.Length;i++)
//			{
//				var tmpPlayer = PlayerManager.Instance.Players[i];
//				if (tmpPlayer.playerID == playerId)
//				{
//					tmpPlayer.netIsCanEnter = isEnterData;
//					break;
//				}
//			}
			if (GameModel.GetInstance.isReconnecToGame == 1 && tmpPlayer.netIsSuccess!=0)
			{
				GameModel.GetInstance.isReconnecToGame = 2;
				NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
			}
			else if(tmpPlayer.netIsSuccess==0)
			{
				GameModel.GetInstance.isReconnecToGame = 2;
                //var overHandler = Client.UIControllerManager.Instance.GetController<UIGameOverWindowController> ();
                //overHandler.setVisible (true);
                var player = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
                var entersuccess = Client.UIControllerManager.Instance.GetController<UIEnterSuccessWindowController>();
                entersuccess.playerInfor = player;
                entersuccess.setVisible(true);

                return;
			}

			for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
			{
				var onePlayer = PlayerManager.Instance.Players[i];
				if (null != onePlayer)
				{
					onePlayer.isReconectGame = false;
				}
			}

			Client.Unit.BattleController.Instance.NetGameSeletcResult();
		}

		/// <summary>
		/// Handlers the multi round end. 响应多个玩家结束
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerMultiRoundEnd(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); 

			if (stat1 == 0)
			{
				if (GameModel.GetInstance.isReconnecToGame == 1)
				{
					GameModel.GetInstance.isReconnecToGame = 2;
					NetWorkScript.getInstance ().Send_MultiRoundEnd (GameModel.GetInstance.curRoomId);
					NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
				}
			}
			else if(stat1==1)
			{
				Debug.Log ("多人回合的，进入了下一回合了");//多人回合结束，进入到下一回合，这时候可以选择，切换到到下一个玩家，但是不需要再发多人回合结束
				//进行下一个回合

				var isEnterData = int.Parse (backbody ["data"] ["ifPromotion"].ToString ());
				var playerId = backbody["data"]["nowPlayerId"].ToString();

				GameModel.GetInstance.NetCurrentPlayerId = playerId;

				for(var i=0;i<PlayerManager.Instance.Players.Length;i++)
				{
					var tmpPlayer = PlayerManager.Instance.Players[i];
					if (tmpPlayer.playerID == playerId)
					{
						tmpPlayer.netIsCanEnter = isEnterData;
						break;
					}
				}

				if (GameModel.GetInstance.isReconnecToGame == 1)
				{
					GameModel.GetInstance.isReconnecToGame = 2;
					NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
				}

				for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
				{
					var onePlayer = PlayerManager.Instance.Players[i];
					if (null != onePlayer)
					{
						onePlayer.isReconectGame = false;
					}
				}

				Client.Unit.BattleController.Instance.NetGameSeletcResult();
			}
		}

		/// <summary>
		/// Handlers the enter inner finished. 完成进入内圈的动作
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerEnterInnerFinished(SocketModel model)
		{

			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 

			var playerId = backhead["playerId"].ToString();//玩家的id

			var portType =int.Parse(backhead["type"].ToString());
			var data = backbody ["data"];

			JsonData datamanagerInfor = null;
			if (((IDictionary)data).Contains ("roleDataManageInfo"))
			{
				datamanagerInfor=data["roleDataManageInfo"];
			}

			JsonData roleIncomeInfo = null;
			if (((IDictionary)data).Contains ("roleIncomeInfo"))
			{
				roleIncomeInfo=data["roleIncomeInfo"];
			}

			if (stat1 == 0)//返回数据成功 购买开牌成功了  aa购买开牌成功了
			{
//				_UpdatePlayerInfor (playerId, datamanagerInfor, null, roleIncomeInfo);
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				player.EnterInner ();
				_updatePlayerShowInfor(player);
			}

			Client.Unit.BattleController.Instance.NetGameEnterInnerFinished (true);
		}

		/// <summary>
		/// Handlers the red package. 响应红包接口
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerRedPackage(SocketModel model)
		{
			//
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , 
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0一个人的， 1所有人的

			var playerId = backhead["playerId"].ToString();
			var data = backbody ["data"];

			if (GameModel.GetInstance.isReconnecToGame == 1)
			{
				GameModel.GetInstance.isReconnecToGame = 2;
				NetWorkScript.getInstance ().Send_RedPocket (GameModel.GetInstance.curRoomId, 0);
				NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId, 1);
			}

			if (stat1 == 0)
			{				
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				_updatePlayerShowInfor (player);

			}
			else if(stat1==1)
			{
				var moneydata = backbody["data"]["redEnvelopeInfo"];

				GameModel.GetInstance.NetReadPackageJson = moneydata;

				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				_updatePlayerShowInfor (player);

				if (PlayerManager.Instance.HostPlayerInfo.playerID == player.playerID)
				{
					var redController = Client.UIControllerManager.Instance.GetController<UIRedPacketWindowController>();
					redController.player =player;
					redController.OpenGetRedPacket();	
					redController.setVisible(true);		
				}
                
			}

		}

		/// <summary>
		/// Handlers the barrow money.  响应结款接口 
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerBarrowMoney(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0一个人的， 1所有人的

			var playerId = backhead["playerId"].ToString();

			if (stat1 == 0)
			{
				var data = backbody ["data"];

				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}

				JsonData roleHaveAssets = null;
				if (((IDictionary)data).Contains ("roleHaveAssetInfo"))
				{
					roleHaveAssets=data["roleHaveAssetInfo"];
				}

				JsonData roleIncomeInfo = null;
				if (((IDictionary)data).Contains ("roleIncomeInfo"))
				{
					roleIncomeInfo=data["roleIncomeInfo"];
				}

				JsonData debtInfor = null;
				if (((IDictionary)data).Contains ("roleDebtsInfo"))
				{
					debtInfor=data["roleDebtsInfo"];
				}

				JsonData borrowRecord = null;
				if (((IDictionary)data).Contains ("roleLoanTotalRecordInfo"))
				{
					borrowRecord=data["roleLoanTotalRecordInfo"];
				}

				JsonData borrowBoarInfor = null;
				if (((IDictionary)data).Contains ("roleBankLoanInfo"))
				{
					borrowBoarInfor=data["roleBankLoanInfo"];
				}


				PlayerInfo heroInfor=null;
				for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
				{
					if (PlayerManager.Instance.Players [i].playerID == playerId)
					{
						heroInfor = PlayerManager.Instance.Players [i];
						break;
					}
				}
				HandlerJsonToCardVo.HandlerBorrowInfor (heroInfor, data);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (heroInfor,datamanagerInfor);
				_updatePlayerShowInfor (heroInfor);

				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
				{
					var borrowController = UIControllerManager.Instance.GetController<Client.UI.UIBorrowWindowController> ();
					if (borrowController.getVisible ())
					{
						borrowController.UpdateBorrowInfor ();
					}
				}
//				_UpdatePlayerInfor (playerId, datamanagerInfor, roleHaveAssets, roleIncomeInfo,debtInfor,borrowRecord,borrowBoarInfor);

//				var borrowController = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
//				borrowController.UpdateBorrowInfor ();

//				if (playerId == PlayerManager.Instance.HostPlayerInfo.playerID)
//				{
//					return;
//				}
//
//				var data = backbody ["data"];
//				var borrowList = new List<BorrowVo> ();
//				if (data.IsArray == true)
//				{
//					for (var i = 0; i < data.Count; i++)
//					{
//						var tmpdata = data[i];
//						var borrowvo = Coding<BorrowVo>.decode (tmpdata.ToString());
//						borrowList.Add (borrowvo);
//					}
//				}			
//
//				for (int i = 0; i < PlayerManager.Instance.Players.Length; i++)
//				{
//					var tmpPlayerInfor = PlayerManager.Instance.Players[i];
//					if (tmpPlayerInfor.playerID == playerId)
//					{
//						//处理贷款信息
//						NetCardHandler.UpdateBorrowMoney(tmpPlayerInfor,borrowList);
//						break;
//					}
//				}
			}
		}

		/// <summary>
		/// Handlers the pay back money. 响应还款接口
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerPayBackMoney(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0一个人的， 1所有人的

			var playerId = backhead["playerId"].ToString();

			if (stat1 == 0)
			{
//				if (playerId == PlayerManager.Instance.HostPlayerInfo.playerID)
//				{
//					return;
//				}

				var data = backbody ["data"];
//				var paybackList = new List<PaybackVo> ();
//				if (data.IsArray == true)
//				{
//					for (var i = 0; i < data.Count; i++)
//					{
//						var tmpdata = data[i];
//						var paybackvo = Coding<PaybackVo>.decode (tmpdata.ToString());
//						paybackList.Add (paybackvo);
//					}
//				}

//				for (int i = 0; i < PlayerManager.Instance.Players.Length; i++)
//				{
//					var tmpPlayerInfor = PlayerManager.Instance.Players[i];
//					if (tmpPlayerInfor.playerID == playerId)
//					{
//						//处理还款信息
//						NetCardHandler.UpdatePackBackMoney(tmpPlayerInfor,paybackList);
//						break;
//					}
//				}
				PlayerInfo heroInfor=null;
				for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
				{
					if (PlayerManager.Instance.Players [i].playerID == playerId)
					{
						heroInfor = PlayerManager.Instance.Players [i];
						break;
					}
				}
				HandlerJsonToCardVo.HandlerBorrowInfor (heroInfor, data);

				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}

				HandlerJsonToCardVo.HandlerPlayerDataInfor (heroInfor,datamanagerInfor);
				_updatePlayerShowInfor (heroInfor);

				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
				{
					var borrowController = UIControllerManager.Instance.GetController<Client.UI.UIBorrowWindowController> ();
					if (borrowController.getVisible ())
					{
						borrowController.UpdateBorrowInfor ();
					}
				}

			}
		}

		/// <summary>
		/// Handlers the buy insurance. 响应购买保险接口
		/// </summary>
		/// <param name="model">Model.</param>
		public void _HandlerBuyInsurance(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0一个人的

			var playerId = backhead["playerId"].ToString();


			var buycardController = UIControllerManager.Instance.GetController<UIBuyCareWindowController> ();
		
			if (stat1 == 0)
			{
//				if (playerId == GameModel.GetInstance.myHandInfor.uuid)
//				{
//					return;
//				}
//				var money = int.Parse (backbody["money"].ToString());
//
//				for (int i = 0; i < PlayerManager.Instance.Players.Length; i++)
//				{
//					var tmpPlayerInfor = PlayerManager.Instance.Players[i];
//					if (tmpPlayerInfor.playerID == playerId)
//					{
//						//处理购买保险
//						NetCardHandler.UpdateBuyEnsurance(tmpPlayerInfor,money);
//						break;
//					}
//				}

				var data = backbody ["data"];

				JsonData datamanagerInfor = null;
				if (((IDictionary)data).Contains ("roleDataManageInfo"))
				{
					datamanagerInfor=data["roleDataManageInfo"];
				}
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				HandlerJsonToCardVo.HandlerPlayerDataInfor (player,datamanagerInfor);
				_updatePlayerShowInfor(player);

                player.InsuranceList.Add(90001);

				buycardController.setVisible (false);

				if (playerId == GameModel.GetInstance.myHandInfor.uuid)
				{
					MessageHint.Show ("购买保险成功");
				}

				var borrowController = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
				borrowController.UpdateBorrowBoardMoney ();

			}
			else if(stat1==1)
			{
				if (playerId == GameModel.GetInstance.myHandInfor.uuid)
				{
					MessageHint.Show ("您已经购买保险,不能重复购买");
				}

				buycardController.setVisible (false);
			}
		}

		/// <summary>
		/// Handlers the room enter game. 响应进入游戏
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerRoomEnterGame(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)//通讯成功
			{
				var baodyDadeArr= backbody["data"]["roles"];
				if (baodyDadeArr.IsArray == true)
				{
					List<PlayerInfo> players = new List<PlayerInfo> ();

					for (int i = 0; i < baodyDadeArr.Count; i++)
					{
						var tmpData = baodyDadeArr[i];
						var playerJson = tmpData["player"];
						var name = playerJson["name"].ToString();
						var uuid = playerJson["uuid"].ToString ();				

						var playerInitdata = new PlayerInitData();

						var roleJson=tmpData["role"]; //roleBankLoanInfo 银行借贷信息  roleBasicInfo 基础信息   roleDataManageInfo 数据  roleDebtsInfo负债   roleHaveAssetInfo资产   roleLoanTotalRecordInfo 结款记录
						var roleBasicData=roleJson["roleBasicInfo"];//
						var roleModelInfor=roleJson["roleModelInfo"];//

//						var roleTotalInterest=roleJson["rolePayoutInfo"]["basicPayout"];
//						var roleTotalLoan=roleJson["roleDebtsInfo"];

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


						//"roleDebtsInfo":{"addNewDebts":{},"basicDebts":{"education":0,"car":5000,"additional":1250,"credit":2500,"house":25000}},

//						playerInitdata.fixHouseMortgages=int.Parse( roleTotalLoan["house"].ToString());//: 166000,
//
//						playerInitdata.fixEducationLoan=int.Parse( roleTotalLoan["education"].ToString());
//						// 购车贷款 用于展示
//						playerInitdata.fixCarLoan=int.Parse(roleTotalLoan["car"].ToString());//: 18750;
//						// 信用卡贷款 用于展示
//						playerInitdata.fixCardDebt=int.Parse(roleTotalLoan["credit"].ToString());//: 27500,;
//						// 额外负债 用于展示 
//						playerInitdata.fixAdditionalDebt=int.Parse(roleTotalLoan["additional"].ToString());//: 1250,;


//						// 税金用于用于支出
//						playerInitdata.fixTax=int.Parse(roleTotalInterest["tax"].ToString());//: 2950,;
//						// 每月要还的房子抵押贷款
//						playerInitdata.houseMortgages=int.Parse(roleTotalInterest["house"].ToString());//: 1650,;
//						// 每月要还的教育贷款
//						if (((IDictionary)roleTotalInterest).Contains ("education"))
//						{
//							playerInitdata.educationLoan=int.Parse(roleTotalInterest["education"].ToString());
//						}
//						else
//						{
//							playerInitdata.educationLoan = 0;
//						}
//						// 每月要偿还的购车贷款
//						playerInitdata.carLoan=int.Parse(roleTotalInterest["car"].ToString());//: 370,;
//						// 每月的信用卡还款
//						playerInitdata.cardDebt=int.Parse(roleTotalInterest["credit"].ToString());//: 850,;
//						// 每月的额外支出
//						playerInitdata.additionalDebt=int.Parse(roleTotalInterest["additional"].ToString());//: 60,;
//						// 其它支出 
//						playerInitdata.otherSpend=int.Parse(roleTotalInterest["other"].ToString());//: 2750;
												

						var playerInfor = new PlayerInfo ();
						playerInfor.SetPlayerInitData (playerInitdata);
						playerInfor.playerID = uuid;
						playerInfor.playerName = name;

						var isBreakLine = bool.Parse(tmpData["roleIfBreak"].ToString());

						if (isBreakLine == true)
						{
							playerInfor.isNetOnline = false;
						}
						else
						{
							playerInfor.isNetOnline = true;
						}


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
//						if (((IDictionary)roleJson).Contains ("roleIncomeInfo"))
//						{
//							roleIncomeInfo=roleJson["roleIncomeInfo"];
//						}

						//roleDebtsInfo负债  
						JsonData debtInfor = null;
//						if (((IDictionary)roleJson).Contains ("roleDebtsInfo"))
//						{
//							debtInfor=roleJson["roleDebtsInfo"];
//						}

						//roleLoanTotalRecordInfo 结款记录
						JsonData borrowRecord = null;
//						if (((IDictionary)roleJson).Contains ("roleLoanTotalRecordInfo"))
//						{
//							borrowRecord=roleJson["roleLoanTotalRecordInfo"];
//						}

						//roleBankLoanInfo 银行借贷信息    
						JsonData borrowBoarInfor = null;
//						if (((IDictionary)roleJson).Contains ("roleBankLoanInfo"))
//						{
//							borrowBoarInfor=roleJson["roleBankLoanInfo"];
//						}

						HandlerJsonToCardVo.UpdatePlayerInfor (playerInfor,datamanagerInfor,roleHaveAssets,roleIncomeInfo,debtInfor,borrowRecord,borrowBoarInfor);
						players.Add (playerInfor);
					}

					GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
					GameModel.GetInstance.isGameRealStart = 0;

					PlayerManager.Instance.SetNetPlayerInfor (reRankPlayer(players));
					entergameAction ();
				}
			}
		}

		/// <summary>
		/// Ons the get select handler data.处理选择人物信息的数据
		/// </summary>
		/// <param name="model">Model.</param>
		private void _OnGetSelectHandlerData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)
			{//通讯成功


				var baodyDadeArr = backbody ["data"]["roles"];
				if (baodyDadeArr.IsArray == true) {
//					List<PlayerInfo> players = new List<PlayerInfo> ();
					List<PlayerInitData> playerList=new List<PlayerInitData>();

					for (int i = 0; i < baodyDadeArr.Count; i++) {
						var roleBasicData = baodyDadeArr [i];							

						var playerInitdata = new PlayerInitData ();

						//"id": 100005,
						//"name": "拽文",
						//						"professional": "司机",
						//						"age": 20,
						//						"bankSavings": 950,
						//						"wage": 3200
						//						"introduction": "爱抽烟，车技精湛，拉着领导全国跑，梦想当个赛车手。",
						//						"gender": 1,
						//						"headImg": "share/texture/head/siji.ab",
						//						"personImg": "share/atlas/battle/role/role_5.ab",
						//						"model": 4,
						//						"modelSave": "prefabs/character/03.ab",
						//						"houseLoan": 50000,
						//						"educationLoan": 0,
						//						"carLoan": 5000,
						//						"creditCard": 2500,
						//                      "additionalDebt": 1250,
						//						"tax": 570,
						//						"houseInterest": 500,
						//						"educationInterest": 0,
						//						"carInterest": 100,
						//						"creditCardInterest": 75,
						//						"additionalInterest": 65,
						//						"otherInterest": 720,

//						"haveChild": 140,

						playerInitdata.id = int.Parse (roleBasicData ["id"].ToString ());//: 100001,
						playerInitdata.playName = roleBasicData ["name"].ToString ();//: "道客藤",
						playerInitdata.oneChildPrise = float.Parse (roleBasicData ["haveChild"].ToString ());//: 480,;
						playerInitdata.careers = roleBasicData ["professional"].ToString ();//: "医生",
						playerInitdata.initAge = int.Parse (roleBasicData ["age"].ToString ());//: 20,
						playerInitdata.fixBankSaving = int.Parse (roleBasicData ["bankSavings"].ToString ());//: 500,
						playerInitdata.cashFlow = int.Parse (roleBasicData ["wage"].ToString ());//: 12000
						playerInitdata.infor = roleBasicData ["introduction"].ToString ();//: "外科专家，救治过千人以上，参与多次抢救行动。",;
						/// <summary>
						/// The player sex. 角色的性别
						/// </summary>
						playerInitdata.playerSex = int.Parse (roleBasicData ["gender"].ToString ());//: 0,;

						//////////
						playerInitdata.headPath = roleBasicData ["headImg"].ToString ();//: "share/texture/head/yisheng.ab",;
						playerInitdata.playerImgPath = roleBasicData ["personImg"].ToString ();//: "share/atlas/battle/role/role_1.ab";
						playerInitdata.modelResID = int.Parse (roleBasicData ["model"].ToString ());//: 0,
						// 加载模型路径
						playerInitdata.modelPath = roleBasicData ["modelSave"].ToString ();//: "prefabs/character/01.ab",;;


						//"roleDebtsInfo":{"addNewDebts":{},"basicDebts":{"education":0,"car":5000,"additional":1250,"credit":2500,"house":25000}},

						playerInitdata.fixHouseMortgages = int.Parse (roleBasicData ["houseLoan"].ToString ());//: 166000,
						playerInitdata.fixEducationLoan = int.Parse (roleBasicData ["educationLoan"].ToString ());
						// 购车贷款 用于展示
						playerInitdata.fixCarLoan = int.Parse (roleBasicData ["carLoan"].ToString ());//: 18750;
						// 信用卡贷款 用于展示
						playerInitdata.fixCardDebt = int.Parse (roleBasicData ["creditCard"].ToString ());//: 27500,;
						// 额外负债 用于展示 
						playerInitdata.fixAdditionalDebt = int.Parse (roleBasicData ["additionalDebt"].ToString ());//: 1250,;


						// 税金用于用于支出
						playerInitdata.fixTax = int.Parse (roleBasicData ["tax"].ToString ());//: 2950,;
						// 每月要还的房子抵押贷款
						playerInitdata.houseMortgages = int.Parse (roleBasicData ["houseInterest"].ToString ());//: 1650,;
						// 每月要还的教育贷款
						if (((IDictionary)roleBasicData).Contains ("educationInterest")) {
							playerInitdata.educationLoan = int.Parse (roleBasicData ["educationInterest"].ToString ());
						} else {
							playerInitdata.educationLoan = 0;
						}
						// 每月要偿还的购车贷款
						playerInitdata.carLoan = int.Parse (roleBasicData ["carInterest"].ToString ());//: 370,;
						// 每月的信用卡还款
						playerInitdata.cardDebt = int.Parse (roleBasicData ["creditCardInterest"].ToString ());//: 850,;
						// 每月的额外支出
						playerInitdata.additionalDebt = int.Parse (roleBasicData ["additionalInterest"].ToString ());//: 60,;
						// 其它支出 
						playerInitdata.otherSpend = int.Parse (roleBasicData ["otherInterest"].ToString ());//: 2750;

						playerList.Add (playerInitdata);
					}

					List<NetChooseRoleInfor> tmpRightList = new List<NetChooseRoleInfor> ();
					for (var i = 0; i < playerList.Count; i++)
					{
						var tmpRightData = new NetChooseRoleInfor ();
						var tmpRoomPlayer = GameModel.GetInstance.roomPlayerInforList [i];
//						var tmpInitdata = playerList [i];
						tmpRightData.nickName=tmpRoomPlayer.nickName;
						tmpRightData.playerId = tmpRoomPlayer.uuid;
						tmpRightList.Add (tmpRightData);
					}

					GameModel.GetInstance.ShowNetLoading ();

					GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetChooseState;

					var chooseRoleController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
					chooseRoleController.SetInitData(playerList);
					chooseRoleController.SetRightPlayerInfors (tmpRightList);
					chooseRoleController.setVisible (true);

					var roomcontroller = UIControllerManager.Instance.GetController<UIFightroomController> ();
					if (roomcontroller.getVisible ())
					{
						roomcontroller.setVisible (false);
					}
				}


			}
		}

        /// <summary>
        /// 网络版选择角色数据
        /// </summary>
        /// <param name="model"></param>
		private void _NetSelecRoletHandler(SocketModel model)
		{
			//{"body":{"data":{"roleId":100002},"status":0},"header":{"attachment":{},"playerId":"ddac7397-c73e-44a1-ba71-13130cacd947","type":5006}}

			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)
			{
				//通讯成功
				var tmpNetVo=new NetChooseRoleInfor();
				tmpNetVo.careerId = int.Parse (backbody ["data"] ["roleId"].ToString ());
				tmpNetVo.playerId = backhead ["playerId"].ToString ();
				var netSelectController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
				netSelectController.SetSelectInfor (tmpNetVo);
			}
		}

        /// <summary>
        /// 取消选择某个角色
        /// </summary>
        /// <param name="model"></param>
		private void _NetCancleSelecRoletHandler(SocketModel model)
		{
			
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)
			{
				//通讯成功
				var tmpNetVo=new NetChooseRoleInfor();
				tmpNetVo.careerId = int.Parse (backbody ["data"] ["roleId"].ToString ());
				tmpNetVo.playerId = backhead ["playerId"].ToString ();
				var netSelectController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
				netSelectController.SetCancleInfor (tmpNetVo);
			}
		}

        /// <summary>
        ///  处理确定选择角色的数据
        /// </summary>
        /// <param name="model"></param>
		private void _NetSelectRoleSureHandler(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];//  
			var stat1 =int.Parse(backbody["status"].ToString());

			if (stat1 == 0)
			{
				var playerId = backhead ["playerId"].ToString ();
				var netSelectController = UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
				netSelectController.SetReadyImg (playerId);
			}
		}




//		public PlayerInitData SelectRole(int roleId)
//		{
//			var template = MetadataManager.Instance.GetTemplateTable<PlayerInitData> ();
//			var it = template.GetEnumerator ();
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as PlayerInitData;
//				if (value.id == roleId)
//				{
//					return value;
//				}
//			}
//			return null;
//		}


		/// <summary>
		/// Res the rank player.对后台返回的玩家数据进行排序，自己坐在最前方
		/// </summary>
		/// <returns>The rank player.</returns>
		/// <param name="datalist">Datalist.</param>
		private List<PlayerInfo> reRankPlayer(List<PlayerInfo> datalist)
		{
			int myIndex = 0;

			var tmpList = new List<PlayerInfo> ();

			var tmpuid = datalist [0].playerID;

			for (int i = 0; i < datalist.Count; i++)
			{
				if (datalist [i].playerID == GameModel.GetInstance.myHandInfor.uuid)
				{
					myIndex = i;
					break;
				}
			}

			for(int i = myIndex ; i< datalist.Count ; i++)
			{
				tmpList.Add (datalist[i]);
			}

			for (int i = 0; i < myIndex; i++)
			{
				tmpList.Add (datalist[i]);
			}

			for (int i = 0; i < tmpList.Count; i++)
			{
				if (tmpList [i].playerID == tmpuid)
				{
					GameModel.GetInstance.curStartIndex = i;
					break;
				}
			}
			return tmpList;
		}

		/// <summary>
		/// Entergames the action. 进入游戏场景
		/// </summary>
		private void entergameAction()
		{
//			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
//			controller.setVisible (true);
//			controller.LoadBattleNetUI();

			var controller = Client.UIControllerManager.Instance.GetController<UILoadingNetGameWindowController>();
			controller.setVisible (true);
			controller.LoadBattleNetUI();

			var selectRoleController = Client.UIControllerManager.Instance.GetController<UIChooseRoleNetWindowController> ();
			selectRoleController.setVisible (false);

			var controller2 = Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>(); 
			controller2.setVisible(true);

			var gamehallController = Client.UIControllerManager.Instance.GetController<UIGameHallWindowController>();
			gamehallController.setVisible (false);

//			var roomcontroller = Client.UIControllerManager.Instance.GetController<UIFightroomController> ();
//			roomcontroller.setVisible (false);
		}
	}
}




