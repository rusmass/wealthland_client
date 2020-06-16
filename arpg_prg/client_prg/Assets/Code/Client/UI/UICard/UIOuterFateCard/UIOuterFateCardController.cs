using System;
using Metadata;
using System.Collections.Generic;
using UnityEngine;
﻿using System;
using Metadata;
using System.Collections.Generic;
using UnityEngine;

namespace Client.UI
{
	public class UIOuterFateCardController:UIController<UIOuterFateCardWindow,UIOuterFateCardController>
	{
		/// <summary>dsf
		/// 窗口预设，独立使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get 
			{
				return "prefabs/ui/scene/outerfatecard.ab";				
			}
		}

		protected override void _OnLoad ()
		{
			
		}

		protected override void _OnShow ()
		{
			
		}

		protected override void _OnHide ()
		{
			
		}

		protected override void _Dispose ()
		{
			
		}

		public void  NetQuitCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.outFate);
			}
		}

		public void NetBuyCard ()
		{
			if (null != cardData)
			{
				if (GameModel.GetInstance.isReconnecToGame == 1)
				{
					GameModel.GetInstance.isReconnecToGame = 2;
					NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
				}
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyOuterFateCard , cardData.id, (int)SpecialCardType.outFate);
			}
		}

		public void NetSaleCard(List<NetSaleCardVo> _list)
		{
			if (null != cardData)
			{
				CardManager.Instance.NetSaleFixedCard(cardData.id,(int)SpecialCardType.outFate,_list);
			}
		}

		// 是否是影响全局的
		public bool IsEffectAll()
		{
			var effectall = false;

			if (cardData.handleRange == 2)
			{
				effectall = true;
			}

			return effectall;
		}

		public bool IsFateToSale()
		{
			var isSale = false;
			var heroInfor=playerInfor;

			var cardID = cardData.relateID;
			for (var i = 0; i < heroInfor.chanceFixedCardList.Count; i++)
			{
				var tmpFixed=heroInfor.chanceFixedCardList[i];
				if(tmpFixed.id==cardID)
				{
					isSale = true;
					return isSale;
				}
			}

			for (var i = 0; i < heroInfor.opportCardList.Count; i++)
			{
				var tmpOpport=heroInfor.opportCardList[i];
				if(tmpOpport.id==cardID)
				{
					isSale = true;
					return isSale;
				}
			}

			return isSale;
		}

		public void SaleFiexedData()
		{
			_netSaleList.Clear();
			var playerInfor = PlayerManager.Instance.HostPlayerInfo;
			for (var i = 0; i < _saleFixedList.Count; i++)
			{
				var tmpvalue=_saleFixedList[i];
				if(tmpvalue.isSlected==true)
				{
					for (var k = playerInfor.chanceFixedCardList.Count-1; k >=0 ; k--) 
					{
						var tmpFixed=playerInfor.chanceFixedCardList[k];
						if(tmpvalue.id==tmpFixed.id)
						{
							var getMoney = tmpvalue.saleMoney * tmpvalue.baseNum + tmpvalue.mortgage;

							playerInfor.totalMoney +=getMoney ;
							playerInfor.totalIncome += tmpvalue.income;
							playerInfor.qualityScore += tmpvalue.quality;
							playerInfor.chanceFixedCardList.Remove (tmpFixed);

							if (GameModel.GetInstance.isPlayNet == true)
							{
								var saleVo = new NetSaleCardVo ();
								saleVo.cardId = tmpFixed.id;
								saleVo.cardNumber = 1;
								saleVo.cardType = (int)SpecialCardType.fixedChance;
								_netSaleList.Add (saleVo);
							}

							if (tmpvalue.quality != 0)
							{
								var recordInfor = new InforRecordVo ();
								recordInfor.title = cardData.title;
								recordInfor.num = tmpvalue.quality;
								playerInfor.AddQualityScoreInfor (recordInfor);
							}

							var saleRecord = new SaleRecordVo ();
							saleRecord.title = tmpFixed.title;
							saleRecord.price = Mathf.Abs(tmpFixed.payment);
							saleRecord.number=tmpFixed.baseNumber;
							saleRecord.income=tmpFixed.income;
							saleRecord.mortage=Mathf.Abs(tmpFixed.mortgage);
							saleRecord.quality=tmpFixed.scoreNumber;
							saleRecord.getMoney=getMoney;
							saleRecord.saleMoney=tmpvalue.saleMoney;
							playerInfor.saleRecordList.Add (saleRecord);

							playerInfor._saleNums += 1;

							break;
						}
					}

					for (var k = playerInfor.opportCardList.Count-1; k>=0  ; k--) 
					{
						var tmpFixed=playerInfor.opportCardList[k];
						if(tmpvalue.id==tmpFixed.id)
						{
							var getMoney =tmpvalue.saleMoney*tmpvalue.baseNum + tmpvalue.mortgage ;
							playerInfor.totalMoney += getMoney;
							playerInfor.totalIncome += tmpvalue.income;
							playerInfor.qualityScore += tmpvalue.quality;
							playerInfor.opportCardList.Remove (tmpFixed);

							if (tmpvalue.quality != 0)
							{
								var recordInfor = new InforRecordVo ();
								recordInfor.title = cardData.title;
								recordInfor.num = tmpvalue.quality;
								playerInfor.AddQualityScoreInfor (recordInfor);
							}

							if (GameModel.GetInstance.isPlayNet == true)
							{
								var saleVo = new NetSaleCardVo ();
								saleVo.cardId = tmpFixed.id;
								saleVo.cardNumber = 1;
								saleVo.cardType = (int)SpecialCardType.bigChance;
								_netSaleList.Add (saleVo);
							}

							var saleRecord = new SaleRecordVo ();
							saleRecord.title = tmpFixed.title;
							saleRecord.price = Mathf.Abs(tmpFixed.payment);
							saleRecord.number=tmpFixed.baseNumber;
							saleRecord.income=tmpFixed.income;
							saleRecord.mortage=Mathf.Abs(tmpFixed.mortgage);
							saleRecord.quality=0;
							saleRecord.getMoney=getMoney;
							saleRecord.saleMoney=tmpvalue.saleMoney;
							playerInfor.saleRecordList.Add (saleRecord);

							playerInfor._saleNums += 1;

							break;
						}
					}

				}
			}

			playerInfor.AddCapticalData ();

            if(playerInfor.playerID==PlayerManager.Instance.HostPlayerInfo.playerID)
            {
                MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardOuerFate);
            }

			var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
			if(null!=battleController)
			{
				battleController.SetQualityScore ((int)playerInfor.qualityScore);
				battleController.SetTimeScore ((int)playerInfor.timeScore);
				battleController.SetNonLaberIncome ((int)playerInfor.totalIncome,(int)playerInfor.MonthPayment);
				battleController.SetCashFlow ((int)playerInfor.totalMoney);
			}
		}


		public void QuitCard()
		{
			var heroInfor=playerInfor;
			if (null != cardData)
			{
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.quitFateCard1,heroInfor.playerName),null,true);
			}

		}

		//如果是单个买卖，弹板，随机弹板--关联id 估计id判断是股票还是固定资产
		//根据id显示要卖的东西

		private void _IsFitConditionForAllEffect(PlayerInfo player)
		{
			if (null != cardData)
			{
				var heroInfor = player;
				var fitcondition = false;
				for (var i = heroInfor.shareCardList.Count - 1; i >= 0; i--) {
					var sharecard = heroInfor.shareCardList [i];		

					if (cardData.handleRange == 2) {
						if (sharecard.belongsTo == cardData.relateID || sharecard.id == cardData.relateID) {
							fitcondition = true;
							break;
						}
					}
				}

				if(fitcondition ==false)
				{
					for (var i = heroInfor.chanceFixedCardList.Count - 1; i >= 0; i--) {
						var fixedCard = heroInfor.chanceFixedCardList [i];
						if (cardData.handleRange == 2) {
							if (fixedCard.belongsTo == cardData.relateID || fixedCard.id==cardData.relateID) {
								fitcondition = true;
								break;
							}
						}
					}
				}

				if (fitcondition == false)
				{
					for (var i = heroInfor.opportCardList.Count - 1; i >= 0; i--) {
						var opportuniycard = heroInfor.opportCardList [i];

						if (cardData.handleRange == 2) {
							if (opportuniycard.belongsTo == cardData.relateID || opportuniycard.id==cardData.relateID) {
								fitcondition = true;
								break;
							}
						}
					}
				}

				if(fitcondition==true)
				{
					MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.userIsEffectedByFate,player.playerName),null,true);
				}
				else
				{
					
				}

			}
		}

		// 总的处理卡牌方法，适用与npc和整体变化的处理
		public void HandlerCardData()
		{
			if (null != cardData)
			{
				//当前玩家和索引值
				var heroInfor=playerInfor;
				var heroTurn = 0;

				// 点击确定是添加评分的
				heroInfor.PlayerIntegral += cardData.rankScore;

				for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
				{
					if (heroInfor.playerID == PlayerManager.Instance.Players [i].playerID)
					{
						heroTurn = i;
						break;
					}
				}

				_IsFitConditionForAllEffect (heroInfor);


				var isAddBalance = false;
				var fitcondition_share = false;
				// 处理股票的数据
				for (var i = heroInfor.shareCardList.Count - 1; i >= 0; i--) 
				{
					var sharecard=heroInfor.shareCardList[i];
					var fitcondition = false;
					if (cardData.handleRange == 2) 
					{
						if (sharecard.belongsTo ==cardData.relateID || sharecard.id ==cardData.relateID) 
						{
							fitcondition = true;
							isAddBalance = true;
						}
					}

					if (fitcondition == true)
					{
						fitcondition_share = true;

						// 处理现金
						if(cardData.isHandle_peymeny==1)
						{	
							
							if (cardData.fateType == 3)
							{
								// 成倍增加的 暂时没有
								var tmpPay = sharecard.payment * (cardData.payment-1);
								if(heroInfor.totalMoney +tmpPay<0)
								{									
									break;
								}
								else
								{
									heroInfor.totalMoney +=tmpPay;
								}

							} else 
							{
								if (cardData.payment > 0)
								{
									heroInfor.totalMoney += cardData.payment;
								}									
							}														
					    }


						// 处理非劳务收入
						if (cardData.isHandle_income==1) 
						{
							if (cardData.handler_income_type == 2)
							{  
								// 变成一个数
								var tmpincome = sharecard.income - cardData.handler_income_number;
								heroInfor.totalIncome -= tmpincome;
							}
							else if (cardData.handler_income_type == 1) // 加减乘一个数
							{
								var tmpincome = 0f;

								if(cardData.fateType==3)//倍数变化
								{
									tmpincome= sharecard.income * (cardData.handler_income_number-1);
									heroInfor.totalIncome += tmpincome;
								}
								else 
								{
									tmpincome = cardData.handler_income_number;
									heroInfor.totalIncome += tmpincome;
								}

								if(tmpincome>0)
								{									
									MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.effectShareOneInTwo2,heroInfor.playerName,cardData.title),null,true);
								}
								else
								{									
									MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.effectShareTwoInOne2,heroInfor.playerName,cardData.title),null,true);
								}

							}

						}

						if (cardData.fateType == 1) 
						{//卖
							heroInfor.shareCardList.RemoveAt(i);
							MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.salseFateCard3,heroInfor.playerName,cardData.title,(-cardData.payment).ToString()),null,true);
						}
					}
				}

				var fitcondition_fixChance = false;

				for (var i = heroInfor.chanceFixedCardList.Count-1; i>=0; i--) 
				{
					var fixedCard=heroInfor.chanceFixedCardList[i];

					var fitcondition = false;
					// 处理一类的
					if (cardData.handleRange == 2) 
					{
						if (fixedCard.belongsTo ==cardData.relateID ||fixedCard.id == cardData.relateID) 
						{
							fitcondition = true;
							isAddBalance = true;
						}
					}else if(cardData.handleRange==1)
					{
						// 单个处理
						if(cardData.relateID==fixedCard.id)
						{
							fitcondition = true;
							isAddBalance = true;
						}
					}
					var tmpPay = 0f;
					if (fitcondition == true)
					{
						fitcondition_fixChance = true;

						// 处理金币
						if(cardData.isHandle_peymeny==1)
						{
							// 倍数变化的，一般是全局
							if (cardData.fateType == 3)
							{
								tmpPay= fixedCard.payment * (cardData.payment-1);
							} else 
							{
								if (cardData.payment > 0)
								{
									tmpPay= cardData.payment*fixedCard.baseNumber + fixedCard.mortgage;
								}									
							}

							heroInfor.totalMoney +=tmpPay;
						}

						if (cardData.isHandle_income==1) 
						{
							var tmpincome=fixedCard.income;
							if (cardData.handler_income_type == 2)
							{ // 变成一个数
								tmpincome = cardData.handler_income_number-fixedCard.income ;
								if(tmpincome>0)
								{
									MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,tmpincome.ToString()),null,true);
								}
								else
								{
									MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectReduceIncome3,heroInfor.playerName,cardData.title,(-tmpincome).ToString()),null,true);
								}

							}
							else if (cardData.handler_income_type == 1) // 加减乘一个数
							{
								if(cardData.fateType==3)//倍数变化
								{
									tmpincome = fixedCard.income * (cardData.handler_income_number-1);
									if(tmpincome>0)
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,((cardData.handler_income_number-1)*100).ToString()+"%"),null,true);
									}
									else
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectMutipleReduceIncome3,heroInfor.playerName,cardData.title,((1-cardData.handler_income_number)*100).ToString()+"%"),null,true);
									}
								}
								else 
								{
									tmpincome = cardData.handler_income_number;
									if(tmpincome>0)
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,tmpincome.ToString()),null,true);
									}
									else
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectReduceIncome3,heroInfor.playerName,cardData.title,(-tmpincome).ToString()),null,true);
									}
								}
							}
							heroInfor.totalIncome += tmpincome;
						}

						if (cardData.fateType == 1) 
						{
							//卖							
							heroInfor.chanceFixedCardList.RemoveAt(i);

							var getMoney=cardData.payment*fixedCard.baseNumber + fixedCard.mortgage;

							var saleRecord = new SaleRecordVo ();
							saleRecord.title = fixedCard.title;
							saleRecord.price = Mathf.Abs(fixedCard.payment);
							saleRecord.number=fixedCard.baseNumber;
							saleRecord.income=fixedCard.income;
							saleRecord.mortage=Mathf.Abs(fixedCard.mortgage);
							saleRecord.quality=fixedCard.scoreNumber;
							saleRecord.getMoney=getMoney;
							saleRecord.saleMoney=cardData.payment;
							playerInfor.saleRecordList.Add (saleRecord);

							MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.salseFateCard3,heroInfor.playerName,cardData.title,tmpPay.ToString()),null,true);
						}
					}
				}

				var fitcondition_opportunity = false;

				for (var i = heroInfor.opportCardList.Count-1; i>=0; i--) 
				{
					var opportuniycard=heroInfor.opportCardList[i];

					var fitcondition = false;

					if (cardData.handleRange == 2) 
					{
						if (opportuniycard.belongsTo ==cardData.relateID || opportuniycard.id==cardData.relateID) 
						{
							fitcondition = true;
							isAddBalance = true;
						}
					}else if(cardData.handleRange==1)
					{
						if(cardData.relateID==opportuniycard.id)
						{
							fitcondition = true;
							isAddBalance = true;
						}
					}
					var tmpPay = 0f;

					if (fitcondition == true)
					{
						fitcondition_opportunity = true;

						// 处理命运
						if(cardData.isHandle_peymeny==1)
						{		
							// 全局的倍数变化
							if (cardData.fateType == 3)
							{
								tmpPay= opportuniycard.payment * (cardData.payment-1);
							} else 
							{
								if (cardData.payment > 0)
								{
									tmpPay= cardData.payment * opportuniycard.baseNumber +opportuniycard.mortgage;
								}									
							}								
							heroInfor.totalMoney +=tmpPay;
						}

						if (cardData.isHandle_income==1) 
						{
							var tmpincome=opportuniycard.income;

							if (cardData.handler_income_type == 2)
							{ // 变成一个数
								tmpincome = cardData.handler_income_number-opportuniycard.income ;
								if(tmpincome>0)
								{
									MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,tmpincome.ToString()),null,true);
								}
								else
								{
									MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectReduceIncome3,heroInfor.playerName,cardData.title,(-tmpincome).ToString()),null,true);
								}

							}
							else if (cardData.handler_income_type == 1) // 加减乘一个数
							{
								if(cardData.fateType==3)//倍数变化
								{
									tmpincome = opportuniycard.income * (cardData.handler_income_number-1);
									if(tmpincome>0)
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,((cardData.handler_income_number-1)*100).ToString()+"%"),null,true);
									}
									else
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectMutipleReduceIncome3,heroInfor.playerName,cardData.title,((1-cardData.handler_income_number)*100).ToString()+"%"),null,true);
									}
								}
								else 
								{
									tmpincome = cardData.handler_income_number;
									if(tmpincome>0)
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectIncreaseIncone3,heroInfor.playerName,cardData.title,tmpincome.ToString()),null,true);
									}
									else
									{
										MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.effectReduceIncome3,heroInfor.playerName,cardData.title,(-tmpincome).ToString()),null,true);
									}
								}
							}
							heroInfor.totalIncome += tmpincome;
						}

						if (cardData.fateType == 1) 
						{
							//卖
							MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.salseFateCard3,heroInfor.playerName,cardData.title,tmpPay.ToString()),null,true);
							heroInfor.opportCardList.RemoveAt(i);

							var getMoney = cardData.payment * opportuniycard.baseNumber + opportuniycard.mortgage;

							var saleRecord = new SaleRecordVo ();
							saleRecord.title = opportuniycard.title;
							saleRecord.price = Mathf.Abs(opportuniycard.payment);
							saleRecord.number=opportuniycard.baseNumber;
							saleRecord.income=opportuniycard.income;
							saleRecord.mortage=Mathf.Abs(opportuniycard.mortgage);
							saleRecord.quality=0;
							saleRecord.getMoney=getMoney;
							saleRecord.saleMoney=cardData.payment;
							playerInfor.saleRecordList.Add (saleRecord);

						}

//						if (cardData.fateType != 3)
//						{
//							heroInfor.totalMoney += opportuniycard.mortgage * opportuniycard.baseNumber;
//							heroInfor.totalDebt += opportuniycard.mortgage * opportuniycard.baseNumber;
//
//							if(opportuniycard.belongsTo==(int)CardManager.BalacneKind.House)
//							{
//								heroInfor.houseDebt += opportuniycard.mortgage;
//
//							}
//							else if(opportuniycard.belongsTo==(int)CardManager.BalacneKind.Antique)
//							{
//							}
//							else if(opportuniycard.belongsTo==(int)CardManager.BalacneKind.Company)
//							{								
//								heroInfor.companyDebt += opportuniycard.mortgage;							
//							}
//						}

					}
				}

				if(isAddBalance==true)
				{
					heroInfor.AddCapticalData ();
				}

				if (fitcondition_fixChance == true || fitcondition_opportunity == true || fitcondition_share == true)
				{
					var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
					if(null!=battleController)
					{
						if (heroTurn==0) 
						{
							battleController.SetQualityScore ((int)heroInfor.qualityScore);
							battleController.SetTimeScore ((int)heroInfor.timeScore);
							battleController.SetNonLaberIncome ((int)heroInfor.totalIncome,(int)heroInfor.MonthPayment);
							battleController.SetCashFlow ((int)heroInfor.totalMoney);
						}
						else 
						{
							battleController.SetPersonInfor (heroInfor,heroTurn,false);
						}
					}
				}
				
			}
		}

		public override void Tick(float deltaTime)
		{
			var window = _window as UIOuterFateCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

		public void UpdateTextData()
		{
			var totalMoney = 0f;
			var income = 0f;
			var quality = 0f;
			for (var i = 0; i < _saleFixedList.Count; i++) 
			{
				var value=_saleFixedList[i];
				if(value.isSlected==true)
				{
					totalMoney += value.saleMoney*value.baseNum+value.mortgage ;
					income += value.income;
					quality += value.quality;
				}
			}
			if (null != _window)
			{
				var tmpvalue = _window as UIOuterFateCardWindow;
				tmpvalue.UpdateTxtData (totalMoney.ToString(),income.ToString(),quality.ToString());
			}
		}

		public List<FateSaleFiexdVo> GetDataList()
		{
			return _saleFixedList;
		}

		public FateSaleFiexdVo GetValueByIndex(int index)
		{
			var values = _saleFixedList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}

			return null;
		}

		public OuterFate _cardData;
		private List<FateSaleFiexdVo> _saleFixedList=new List<FateSaleFiexdVo>();

		private void _InitSetFateData()
		{
			_saleFixedList.Clear ();
			_netSaleList.Clear();
		
			var saleVo = new FateSaleFiexdVo ();
			var heroInfor=playerInfor;

//			var template = MetadataManager.Instance.GetTemplateTable<ChanceFixed> ();
//			var it = template.GetEnumerator ();	
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as ChanceFixed;
//				if(value.id==40002 || value.id==20003 ||value.id == 20001)
//				{
//					heroInfor.chanceFixedCardList.Add (value);
//				}
//			}			

//			template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
//			it = template.GetEnumerator ();
//			while(it.MoveNext())
//			{
//				var value = it.Current.Value as ChanceShares;
//
//				if (value.id == 30001)
//				{
//					Console.WriteLine ("添加股票信息");
//					heroInfor.shareCardList.Add (value);
//				}
//
//			}


			for (var i = 0; i < heroInfor.chanceFixedCardList.Count; i++)
			{
				var fixedVo=heroInfor.chanceFixedCardList[i];
				if (fixedVo.id == cardData.relateID)
				{
					saleVo = new FateSaleFiexdVo ();
					saleVo.title = fixedVo.title;
					saleVo.payment = fixedVo.payment;
//					saleVo.mortgage = fixedVo.mortgage * fixedVo.baseNumber ;
					saleVo.mortgage = fixedVo.mortgage ;
					saleVo.saleMoney = cardData.payment;
					saleVo.changeMoney = saleVo.saleMoney * fixedVo.baseNumber + saleVo.mortgage;
					saleVo.income = -fixedVo.income;
					saleVo.quality =  -fixedVo.scoreNumber;
					saleVo.id = fixedVo.id;
					saleVo.baseNum = fixedVo.baseNumber;

					_saleFixedList.Add (saleVo);
				}
			}

			for (var i = 0; i < heroInfor.opportCardList.Count; i++)
			{
				var opprtuniyvo=heroInfor.opportCardList[i];
				if (opprtuniyvo.id == cardData.relateID)
				{
					saleVo = new FateSaleFiexdVo ();
					saleVo.title = opprtuniyvo.title;
					saleVo.payment = opprtuniyvo.payment;
//					saleVo.mortgage = opprtuniyvo.mortgage * opprtuniyvo.baseNumber ;
					saleVo.mortgage = opprtuniyvo.mortgage;
					saleVo.saleMoney = cardData.payment;
					saleVo.changeMoney = saleVo.saleMoney* opprtuniyvo.baseNumber + saleVo.mortgage;
					saleVo.income = -opprtuniyvo.income;
					saleVo.quality =  0;
					saleVo.id = opprtuniyvo.id;
					saleVo.baseNum = opprtuniyvo.baseNumber;
					_saleFixedList.Add (saleVo);
				}
			}
		}


		public PlayerInfo playerInfor;

		public List<NetSaleCardVo> _netSaleList = new List<NetSaleCardVo> ();

		public OuterFate cardData
		{
			get 
			{
				return _cardData;
			}

			set
			{
				_cardData = value;
				_InitSetFateData ();

			}
		}

	}
}

