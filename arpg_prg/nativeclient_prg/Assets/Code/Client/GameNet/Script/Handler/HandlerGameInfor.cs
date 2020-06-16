using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using Client.UI;

namespace Client
{
	public partial class MessageManager
	{
		/// <summary>
		/// Handlers the game borrow infor. 处理人物结款信息接口
		/// </summary>
		/// <param name="model">Model.</param>
		private void _HandlerGameBorrowInfor(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 

			var playerId = backhead["playerId"].ToString();//玩家的id

			//var portType =int.Parse(backhead["type"].ToString());

			if (stat1 == 0)
			{
				var data = backbody ["data"];

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
				_updatePlayerShowInfor (heroInfor);

				var borrowController = UIControllerManager.Instance.GetController<Client.UI.UIBorrowWindowController> ();
				if (borrowController.getVisible ())
				{
					borrowController.UpdateBorrowInfor ();
				}
				else
				{
					borrowController.playerInfor = PlayerManager.Instance.HostPlayerInfo;
					borrowController.setVisible (true);

					if (GameModel.GetInstance.borrowBoardTime > 0)
					{
						borrowController.SetTime (GameModel.GetInstance.borrowBoardTime);
					}
				}

			}
		}

		/// <summary>
		/// Handlers the normal share data. 处理游戏分享信息
		/// </summary>
		/// <param name="model">Model.</param>
		private void HandlerNormalShareData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0			

			if (stat1 == 0)
			{				
				var data = backbody ["data"];

				var shareTitle =data["title"].ToString();
				var shareTxt = data["txt"].ToString();
				var shareImgUrl = data["address"].ToString();
				var shareWebUrl = data["weburl"].ToString();

				ShareContentInfor.Instance.SetShareContent (shareTitle, shareTxt, shareImgUrl, shareWebUrl);

				var _shareboardController = UIControllerManager.Instance.GetController<UIShareBoardWindowController> ();
				_shareboardController.setVisible (true);
			}
		}

		/// <summary>
		/// Handlers the room share data. 处理游戏分享房间信息
		/// </summary>
		/// <param name="model">Model.</param>
		private void HandlerRoomShareData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 			

			if (stat1 == 0)
			{
				var data = backbody ["data"];
				var shareTitle =data["title"].ToString();
				var shareTxt = data["txt"].ToString();
				var shareImgUrl = data["address"].ToString();
				var shareWebUrl = data["weburl"].ToString();

				ShareContentInfor.Instance.SetShareRoomContent (shareTitle, shareTxt, shareImgUrl, shareWebUrl);

				ShareContentInfor.Instance.setShareRoomTxt (GameModel.GetInstance.curRoomId);
				MBGame.Instance.ShareWeiChat (ShareContentInfor.Instance.roomFightContent);

               

            }
		}

        /// <summary>
        /// 处理梦想板的数据
        /// </summary>
        /// <param name="model"></param>
        private void HandlerDreamShareData(SocketModel model)
        {
            var backMessage = JsonMapper.ToObject(model.message);
            var backbody = backMessage["body"];
            var stat1 = int.Parse(backbody["status"].ToString()); // 返回的状态  0 			

            if (stat1 == 0)
            {
                var data = backbody["data"];
                var shareTitle = data["title"].ToString();
                var shareTxt = data["txt"].ToString();
                var shareImgUrl = data["address"].ToString();
                var shareWebUrl = data["weburl"].ToString();

                ShareContentInfor.Instance.SetShareDream(shareTitle, shareTxt, shareImgUrl, shareWebUrl);
                //ShareContentInfor.Instance.setShareRoomTxt(GameModel.GetInstance.curRoomId);
                //MBGame.Instance.ShareWeiChat(ShareContentInfor.Instance.roomFightContent);

                var webController = UIControllerManager.Instance.GetController<UINativeWebController>();
                webController.SetTargetUrl(shareWebUrl);
                webController.setVisible(true);
            }
        }


        /// <summary>
        /// Handlers the player target data.处理获取目标信息
        /// </summary>
        /// <param name="model">Model.</param>
        private void HandlerPlayerTargetData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id			

			if (stat1 == 0)
			{
				var data= backbody["data"]["integralRecord"];

				playerId = backbody ["data"] ["targetPlayerId"].ToString ();

				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				player.netTargetTimeScore = int.Parse (data["timeTotalIntegral"].ToString());
				player.netTargetQualityScore = int.Parse (data["qualityTotalIntegral"].ToString());

				if(((IDictionary)data).Contains ("flowCashTotalIntegral"))
				{
					player.netTargetCashFlowScore = int.Parse (data["flowCashTotalIntegral"].ToString());
					var cashFlowScoreList=data["flowCashIntegral"];
					if (cashFlowScoreList.IsArray)
					{
						player.flowScoreList.Clear ();
						for (var i = 0; i < cashFlowScoreList.Count; i++)
						{
							var inforRecordVo = new InforRecordVo ();

							var tmpData = cashFlowScoreList[i];
							inforRecordVo.index = i+1;
							inforRecordVo.title = tmpData ["name"].ToString ();
							inforRecordVo.num =float.Parse(tmpData ["integral"].ToString ());
							player.flowScoreList.Add (inforRecordVo);
						}
					}
				}
				var timeScoreList=data["timeIntegral"];
				if (timeScoreList.IsArray)
				{
					player.timeScoreList.Clear ();
					for (var i = 0; i < timeScoreList.Count; i++)
					{
						var inforRecordVo = new InforRecordVo ();

						var tmpData = timeScoreList[i];
						inforRecordVo.index = i+1;
						inforRecordVo.title = tmpData ["name"].ToString ();
						inforRecordVo.num =float.Parse(tmpData ["integral"].ToString ());
						player.timeScoreList.Add (inforRecordVo);
					}
				}

				var qualityScoreList = data["qualityIntegral"];
				if (qualityScoreList.IsArray)
				{
					player.qualityScoreList.Clear ();

					for (var i = 0; i < qualityScoreList.Count; i++)
					{
						var inforRecordVo = new InforRecordVo ();

						var tmpData = qualityScoreList[i];
						inforRecordVo.index = i+1;
						inforRecordVo.title = tmpData ["name"].ToString ();
						inforRecordVo.num = float.Parse(tmpData ["integral"].ToString ());
						player.qualityScoreList.Add (inforRecordVo);
					}
				}
				GameModel.GetInstance.hasLoadTarget=true;
				var _totalInforController = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				_totalInforController.NetShowTargetBoard ();
			}
		}

		/// <summary>
		/// Handlers the balance and income data. 处理资产和费劳务收入的信息
		/// </summary>
		/// <param name="model">Model.</param>
		private void HandlerBalanceAndIncomeData(SocketModel model)
		{
			/*
 "body": {
        "data": {
            "roleHaveAssetInfo": {
                "assetTotalMoney": 0,
                "bigChances": [],
                "smallChances": [
                    {
                        "cardIntegral": 7,
                        "cost": "65000",
                        "downPayment": -5000,
                        "id": 20001,
                        "instructions": "\\u3000\\u3000政府查封的房产中有优质的3室2厅的居室出售。房产维护良好,租户稳定。可以自己接受这笔生意,也可以卖给其他玩家。",
                        "integralNumber": 1,
                        "integralType": 2,
                        "investmentIncome": "67%",
                        "mortgageLoan": -60000,
                        "name": "待售公寓3室2厅",
                        "nonLaborIncome": 280,
                        "number": 1,
                        "path": "share/atlas/battle/card/fixedcard1/card_d_34.ab",
                        "sellPrice": "65000-150000",
                        "type": 1
                    }
                ],
                "stocks": []
            },
            "roleIncomeInfo": {
                "laborIncome": {
                    "money": 2000,
                    "name": "工资"
                },
                "nonLaborIncomeList": [],
                "totalIncome": 0,
                "totalNonLaborIncome": 0
            }
        },
        "status": 0
    },
    "header": {
        "attachment": {},
        "playerId": "ddac7397-c73e-44a1-ba71-13130cacd947",
        "type": 6003
    }*/
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001

			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id			

			if (stat1 == 0)
			{
				playerId = backbody ["data"] ["targetPlayerId"].ToString ();
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				var balanceInfor = backbody ["data"] ["roleHaveAssetInfo"];
               
                var bigChanceList=balanceInfor["bigChances"];
				if (bigChanceList.IsArray)
				{
					player.opportCardList.Clear ();
					for (var i = 0; i < bigChanceList.Count; i++)
					{
						var tmpbigData = bigChanceList [i];
						if (((IDictionary)tmpbigData).Contains ("id") == true)
						{
							var bigcard = HandlerJsonToCardVo.ToOpportunityCard (tmpbigData);
							player.opportCardList.Add (bigcard);
						}
					}
				}
				var smallFixedList=balanceInfor["smallChances"];
				if (smallFixedList.IsArray)
				{
					player.chanceFixedCardList.Clear ();
					for (var i = 0; i < smallFixedList.Count; i++)
					{
						var tmpbigData = smallFixedList [i];

						if (((IDictionary)tmpbigData).Contains ("id") == true)
						{
							var fixedcard = HandlerJsonToCardVo.ToFixedChanceCard (tmpbigData);
							player.chanceFixedCardList.Add (fixedcard);
						}
					}
				}
				var chanceShareList=balanceInfor["stocks"];
				if (chanceShareList.IsArray)
				{
					player.shareCardList.Clear ();
					for (var i = 0; i < chanceShareList.Count; i++)
					{
						var chanceData = chanceShareList[i];
						if (((IDictionary)chanceData).Contains ("id") == true)
						{
							var chanceCard = HandlerJsonToCardVo.ToChanceSharesCard (chanceData);
							player.shareCardList.Add (chanceCard);
						}					
					}
				}

				var incomeInfor=backbody["data"]["roleIncomeInfo"];
				player.netInforBalanceAndIncome.laborTxt= incomeInfor["laborIncome"]["name"].ToString();

				player.netInforBalanceAndIncome.laoorMoney =int.Parse(incomeInfor["laborIncome"]["money"].ToString());
				player.netInforBalanceAndIncome.totalIncome=float.Parse(incomeInfor["totalIncome"].ToString());
				player.netInforBalanceAndIncome.totalNonLaborIncome = float.Parse(incomeInfor["totalNonLaborIncome"].ToString());
			    var nonIncomeList = incomeInfor ["nonLaborIncomeList"];
				if (nonIncomeList.IsArray)
				{
					player.netInforBalanceAndIncome.nonIncomeList.Clear ();
					for (var i = 0; i < nonIncomeList.Count; i++)
					{
						var tmpData = nonIncomeList[i];
						var recordVo =new InforRecordVo ();
						recordVo.index = i + 1;
						recordVo.title = tmpData ["name"].ToString ();
						recordVo.num   =float.Parse(tmpData["money"].ToString());
						player.netInforBalanceAndIncome.nonIncomeList.Add (recordVo);
					}
				}

				GameModel.GetInstance.hasLoadBalanceAndIncome=true;

				var totalInfor = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				totalInfor.NetShowBalanceAndIncomeBaord ();
			}
		}

		/// <summary>
		/// Gets the debt and pay infor. 获取负债和支出的信息
		/// </summary>
		private void HandlerPlayerDebtAndPayData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id
			if (stat1 == 0)
			{
				/*
{"body":
{"data":{
"roleAddNewSpendInfo":{},
"roleBasicSpendInfo":{"住房抵押贷款":{"money":500,"name":"住房抵押贷款"},"教育贷款":{"money":0,"name":"教育贷款"},"购车贷款":{"money":100,"name":"购车贷款"},"信用卡":{"money":75,"name":"信用卡"},"额外负债":{"money":65,"name":"额外负债"},"其他支出":{"money":720,"name":"其他支出"},"税金":{"money":570,"name":"税金"}},
"roleBasicDebtInfo":[{"debtInterest":500,"debtMoney":50000,"debtName":"住房抵押贷款"},{"debtInterest":0,"debtMoney":0,"debtName":"教育贷款"},{"debtInterest":100,"debtMoney":5000,"debtName":"购车贷款"},{"debtInterest":75,"debtMoney":2500,"debtName":"信用卡"},{"debtInterest":65,"debtMoney":1250,"debtName":"额外负债"}],
"roleAddNewDebtInfo":[{"debtInterest":100,"debtMoney":1000,"debtName":"一个球"}]},"status":0},"header":{"attachment":{},"playerId":"ddac7397-c73e-44a1-ba71-13130cacd947","type":6004}}


				*/
				playerId = backbody ["data"] ["targetPlayerId"].ToString ();
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				var data = backbody["data"];
				var basicPayData = data ["roleBasicSpendInfo"];
				if (basicPayData.IsArray)
				{
					player.netInforDebtAndPay.basicPayList.Clear ();
					for (var i = 0; i < basicPayData.Count; i++)
					{
						var tmpVo = new PaybackVo ();
						var tmpdata = basicPayData[i];
						tmpVo.title=tmpdata["name"].ToString();
						tmpVo.debt=int.Parse(tmpdata["money"].ToString());
						player.netInforDebtAndPay.basicPayList.Add (tmpVo);
					}
				}

				var newAddPayData=data["roleAddNewSpendInfo"];
				if (newAddPayData.IsArray == true)
				{
					player.netInforDebtAndPay.newAddPayList.Clear ();
					for (var i = 0; i < newAddPayData.Count; i++)
					{
						var tmpvo = new PaybackVo ();
						var tmpdata=newAddPayData[i];
						tmpvo.title = tmpdata ["name"].ToString ();
						tmpvo.debt = int.Parse (tmpdata["money"].ToString());
						player.netInforDebtAndPay.newAddPayList.Add (tmpvo);
					}
				}


				var basicDebtData= data["roleBasicDebtInfo"];
				if (basicDebtData.IsArray == true)
				{
					player.netInforDebtAndPay.basicDebtList.Clear ();
					for (var i = 0; i < basicDebtData.Count; i++)
					{
						var tmpdata=basicDebtData[i];
						var tmpvo = new PaybackVo ();

						tmpvo.title = tmpdata["debtName"].ToString();
						tmpvo.borrow =int.Parse(tmpdata["debtMoney"].ToString());
						tmpvo.debt =int.Parse(tmpdata["debtInterest"].ToString());

						player.netInforDebtAndPay.basicDebtList.Add (tmpvo);
					}
				}

				var newAddDebtData = data ["roleAddNewDebtInfo"];
				if (newAddDebtData.IsArray==true)
				{
					player.netInforDebtAndPay.newAddDebtList.Clear ();
					for (var i = 0; i < newAddDebtData.Count; i++)
					{
						var tmpdata=newAddDebtData[i];
						var tmpvo = new PaybackVo ();

						tmpvo.title = tmpdata["debtName"].ToString();
						tmpvo.borrow =int.Parse(tmpdata["debtMoney"].ToString());
						tmpvo.debt =int.Parse(tmpdata["debtInterest"].ToString());

						player.netInforDebtAndPay.newAddDebtList.Add (tmpvo);
					}
				}

				GameModel.GetInstance.hasLoadDebtAndPay = true;
				var totalInfor = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				totalInfor.NetShowDebtAndPayBoard ();
			}			
		}

		/// <summary>
		/// Gets the player sale record. 获得出售记录的信息
		/// </summary>
		private void HandlerPlayerSaleRecordData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id
			
			if (stat1 == 0)
			{
				playerId = backbody ["data"] ["targetPlayerId"].ToString ();
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);

				var data= backbody["data"];
				var recoredList = data ["roleSellAssetRecord"] ["roleSellAssetRecord"];

				if (recoredList.IsArray == true)
				{
					player.saleRecordList.Clear ();
					for (var i = 0; i < recoredList.Count; i++) 
					{
						var tmpdata = recoredList[i];
						var tmpRecord = new SaleRecordVo ();
						tmpRecord.title=tmpdata["name"].ToString();
						tmpRecord.price =float.Parse(tmpdata ["price"].ToString ());
						tmpRecord.number = int.Parse (tmpdata["number"].ToString());
						tmpRecord.income = -float.Parse (tmpdata["nonLaborIncome"].ToString());
						tmpRecord.mortage = float.Parse (tmpdata["loan"].ToString());
						tmpRecord.quality = float.Parse (tmpdata["qualityIntegral"].ToString());
						tmpRecord.getMoney = float.Parse (tmpdata["net"].ToString());
						tmpRecord.saleMoney = float.Parse (tmpdata["sellPrice"].ToString());
						player.saleRecordList.Add (tmpRecord); 
					}
				}
				GameModel.GetInstance.hasLoadSaleInfor = true;
				var totalInfor = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				totalInfor.NetShowSaleInforBoard ();
			}
		}

		/// <summary>
		/// Gets the player check infor. 或得人物结算面板信息
		/// </summary>
		private void HandlerPlayerCheckData(SocketModel model)
		{
			var backMessage = JsonMapper.ToObject (model.message);
			var backbody = backMessage["body"];
			var backhead = backMessage["header"];// playerid  , type6001
			var stat1 =int.Parse(backbody["status"].ToString()); // 返回的状态  0 
			var playerId = backhead["playerId"].ToString();//玩家的id			

			if (stat1 == 0)
			{
				/*
                 {"body":
                   {"data":
                     {"totalIncome":2000,"totalSpend":1205,"closingDateMoney":795},"status":0},"header":{"attachment":{},"playerId":"4d100592-a8a6-4f7c-9d1e-cf55330d2678","type":6006}}
				*/
				playerId = backbody ["data"] ["targetPlayerId"].ToString ();
				var player = PlayerManager.Instance.GetPlayerInfo (playerId);
				var data=backbody["data"];
				player.netInforCheckVo.totalIncome = float.Parse (data ["totalIncome"].ToString ());
				player.netInforCheckVo.totalPay = float.Parse (data["totalSpend"].ToString());
				player.netInforCheckVo.checkMoney = float.Parse (data["closingDateMoney"].ToString());

				GameModel.GetInstance.hasLoadCheck = true;
				var totalinfor = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				totalinfor.NetShowCheckInforBoard ();
			}
		}
	}
}

