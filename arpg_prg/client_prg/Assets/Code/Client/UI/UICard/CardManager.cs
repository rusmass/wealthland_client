using System;
using UnityEngine;
using Metadata;
using System.Collections;
using System.Collections.Generic;



namespace Client.UI
{
	public class CardManager
	{
		public enum BalacneKind:int // 资产类型
		{			
			House=1, // 房产
			Shares=2, // 股票
			Antique=3, // 古玩
			Company=4 // 公司企业
		}


		public enum ScoreType:int // 时间积分 和 品质积分 分类
		{
			TimeScore=1,   // 时间积分
			QualityScore=2 // 品质积分
		}


		/// <summary>
		/// Opens the card.显示卡牌的逻辑
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void OpenCard(int id)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				//Console.Error.WriteLine ("就是进入看卡牌了怎么滴-----");
//				id=(int)SpecialCardType.GiveChildType;
				NetCardManager.Instance.OpenCard (id);
				return;
			}

			var player = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			var isHostPlayerTurn=PlayerManager.Instance.IsHostPlayerTurn();

			if ((int)SpecialCardType.SelectChance == id)
			{
				//如果是选择大机会，小机会的选择本地显示
				var controller = Client.UIControllerManager.Instance.GetController<UIChanceSelectController> ();
				controller.setVisible (true);
				return;
			}

			if ((int)SpecialCardType.SelectInnerFree == id)
			{
				//自由选择的界面
				var controller = Client.UIControllerManager.Instance.GetController<UIInnerSelectFreeWindowController> ();
				controller.setVisible(true);
				return;
			}

			//健康管理
			if ((int)SpecialCardType.HealthType == id || (int)SpecialCardType.InnerHealthType == id )// 健康
			{				
			//	Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialhealth);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialhealth);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;			
				controller.SetCardId(id);

				player._healthTotalNum += 1;

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardHealth),()=>{
					if (isHostPlayerTurn== true)
					{
						controller.setVisible (true);
					}
					else
					{						
						controller.HandlerCardData ();
						Client.Unit.BattleController.Instance.Send_RoleSelected (1);
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();
				},false);

				return;
			}

			//慈善事业 如果是联网游戏，显示玩家走到的位置，卡牌的类型，如果是本人打开卡牌，进行操作，选择购买开牌还是放弃卡牌，向后台发送信息，切换下一个玩家
			// 但是不会结算。                                          如果不是本人，不会打开开牌，后台发送玩家信息后再处理卡牌
			if ((int)SpecialCardType.CharityType == id)
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialcharitycard);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialcharitycard);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;	
				controller.SetCardId(id);
				player._charityTotalNum += 1;
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardCharity),()=>{
					if(isHostPlayerTurn==true)
					{
						controller.setVisible (true);
					}
					else
					{
						controller.HandlerCardData ();
						Client.Unit.BattleController.Instance.Send_RoleSelected (1);
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();
				},false);

				return;
			}

			if ((int)SpecialCardType.StudyType == id || (int)SpecialCardType.InnerStudyType==id) //  进修学习
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialstudy);

				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialstudy);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;				
				controller.SetCardId(id);

				player._learnTotalNum += 1;
			
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardStudy), () => {
					if(isHostPlayerTurn==true)
					{
						controller.setVisible (true);
					}
					else
					{						
						controller.HandlerCardData ();
						Client.Unit.BattleController.Instance.Send_RoleSelected (1);
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();

				},false);

				return;
			}

			if ((int)SpecialCardType.CheckDayType == id ||(int)SpecialCardType.InnerCheckDayType==id) // 结账日
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.closing_date);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.closing_date);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;			
				controller.SetCardId(id);
			
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardCheckOut), () => {
					if(isHostPlayerTurn==true)
					{
						player._settlementNum += 1;

						if(player.isEnterInner==true)
						{
							player._settleInnerNum+=1;
						}
						else
						{
							player._settleOuterNum+=1;
						}
                        Audio.AudioManager.Instance.Tip_CheckDay(player.careerID);
						controller.setVisible (true);
					}
					else
					{						
						controller.HandlerCardData ();
						Client.Unit.BattleController.Instance.Send_RoleSelected (1);
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();

				},false);

				return;
			}

            //生孩子
            if ((int)SpecialCardType.GiveChildType == id)
            {
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialchild);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;	
				controller.SetCardId(id);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardGetChild),()=>{
//					if(player.childNum>=player.childNumMax)
//					{
//						player.childNum = player.childNumMax;
//						MessageHint.Show(string.Format("孩子数已达{0}个，无法生更多的孩子了.",player.childNumMax));
//						Client.Unit.BattleController.Instance.Send_RoleSelected(1);
//					}
//					else
//					{
					if(isHostPlayerTurn==true) //如果是自己掷骰子
					{
						if(player._childNum>=player.childNumMax)
						{
							player._childNum=player.childNumMax;
						}
                        Audio.AudioManager.Instance.Tip_GiveChild(player.careerID);
						controller.setVisible (true);
					}
					else
					{
						//如果是其它人生孩子， 红包
						if(player.childNum<player.childNumMax)
						{
							if(PlayerManager.Instance.HostPlayerInfo.totalMoney<=0)
							{
								Client.Unit.BattleController.Instance.Send_RoleSelected (1);																
							}
							else
							{
								var redController = Client.UIControllerManager.Instance.GetController<UIRedPacketWindowController>();
								redController.player = player;
								redController.OpenPackRedPacket();
								redController.setVisible(true);
							}
							controller.HandlerCardData ();
						}
						else
						{
							controller.HandlerCardData ();
							if(GameModel.GetInstance.isPlayNet==false)
							{
								Client.Unit.BattleController.Instance.Send_RoleSelected (1);	
							}
						}
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();

				},false);

                return;
            }

            //外圈晋级内圈
            if ((int)SpecialCardType.UpGradType == id)
            {
                //TODO
                //1.不可以自由选择是否进入内圈时：界面内部处理，打开界面N秒后(固定时间),发送下面的命令                
				//2.  可以自由选择是否进入内圈时：选择进入内圈发送true，否则 false

//				var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
//				controller.playerInfor = player;

				//2016-11-11 zll add 
				if(isHostPlayerTurn == true)
				{
//					Debug.LogError("sss");
//					var couclusionController = Client.UIControllerManager.Instance.GetController<UIConclusionController>();
//					couclusionController.setTiletText(false);
//					couclusionController.setUpBaseText(player);
//					couclusionController.setVisible (true);
				}
				else
				{
					//var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
					//controller.playerInfor = player;
					//controller.HandlerCardData ();
//					Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				}
                return;
            }

            //当某一玩家胜利时
            if ((int)SpecialCardType.SuccessType == id)
            {
                //				var couclusionController = Client.UIControllerManager.Instance.GetController<UIConclusionController>();
                //				couclusionController.setUpBaseText(PlayerManager.Instance.HostPlayerInfo);
                //				couclusionController.setTiletText(true);	
                //				couclusionController.setVisible (true);

                var entersuccess = Client.UIControllerManager.Instance.GetController<UIEnterSuccessWindowController>();
                entersuccess.playerInfor = player;
                entersuccess.setVisible(true);

                //var overHandler = Client.UIControllerManager.Instance.GetController<UIGameOverWindowController>();
                //overHandler.setVisible(true);
                return;
            }

			// 命运
            if (outerFateList.Contains (id)) 
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_fate);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_fate);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardFate),()=>{

					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowOuterFateCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Fate(player.careerID);
                    }

				},false);

				return;					
			}

			// 固定资产的卡牌
			if (outerChanceFixed.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_little_chance);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardChance),()=>{

					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowChanceFixedCard (id,isHostPlayerTurn,player);

                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Chance(player.careerID);
                    }

				},false);
				return;
			}

			//股票卡牌
			if (outerChanceShares.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_little_chance);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardChance),()=>{

					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowChanceSharesCard (id,isHostPlayerTurn,player);
                    if (isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Chance(player.careerID);
                    }
                },false);

				return;
			}

			//处理大机会卡牌
			if (outerOpportunityList.Contains (id))
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_great_opportunity);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardOpportunity),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowOpportCard (id,isHostPlayerTurn,player);
                    if (isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Chance(player.careerID);
                    }

                },false);

				return;
			}

			//风险处理卡牌
			if (outerRiskList.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_risk);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardRisk),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowRiskCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Risk(player.careerID);
                    }
				},false);
				return;
			}

			//内圈命运卡牌
			if (innerFateList.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_fate);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardFate),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowInnerFateCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Fate(player.careerID);
                    }

				},false);

				return;
			}

			//内圈投资卡牌
			if (innerInvestmentList.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_investment);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardInvestment),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowInnerInvestmentCard (id,isHostPlayerTurn,player);

                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_InvestmentWin(player.careerID);
                    }

				},false);

				return;
			}

			//处理内圈品质生活卡牌
			if (innerQualtyList.Contains (id)) 
			{
//				Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_quality_life);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_quality_life);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardQualityLife),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowQualityLifeCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Quality(player.careerID);
                    }

				},false);

				return;
			}

			//处理有钱有闲卡牌
			if (innerRelaxList.Contains (id)) 
			{
//				Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_free_time);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.inner_ring_free_time);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardTimeAndMoney),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowInnerRelaxCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Relax(player.careerID);
                    }

				},false);
				return;
			}

		}

		private void _CheckOutDay(GameEventArgs evt)
		{
			var playerIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;

			var player =PlayerManager.Instance.Players[playerIndex];

			var isHostPlayerTurn=PlayerManager.Instance.IsHostPlayerTurn();

			if(null!=player)
			{
				var checkoutMoney=(player.cashFlow + player.totalIncome + player.innerFlowMoney-player.MonthPayment);			

				player.totalMoney += checkoutMoney;

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.getCheckOutMoney2,player.playerName,checkoutMoney.ToString()),null,true);
				player.checkOutCount++;

				var battleController = Client.UIControllerManager.Instance.GetController<UIBattleController> ();
				if (null != battleController) 
				{
					if (isHostPlayerTurn) 
					{					
						battleController.SetCashFlow ((int)player.totalMoney);
					}
					else
					{
						battleController.SetPersonInfor (player, playerIndex, false);
					}
				}
			}
		}

		/// <summary>
		/// Nets the quit card. 网络版放弃卡牌
		/// </summary>
		/// <param name="cardId">Card identifier.</param>
		/// <param name="cardType">Card type.</param>
		public void NetQuitCard(int cardId , int cardType)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().QuitCard (GameModel.GetInstance.curRoomId, cardId, cardType);
			}
		}

		/// <summary>
		/// Nets the buy card. 网络版购买卡牌
		/// </summary>
		/// <param name="cardId">Card identifier.</param>
		/// <param name="cardType">Card type.</param>
		/// <param name="cardNumber">Card number.</param>
		public void NetBuyCard(int portType ,  int cardId , int cardType , int cardNumber=1,int cardRollPoint=0,bool riskFreeChoise=false)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().BuyCard (portType,GameModel.GetInstance.curRoomId, cardId, cardType,cardNumber, cardRollPoint,riskFreeChoise);
			}
		}

		/// <summary>
		/// Nets the sale card.网络版出售资产卡牌
		/// </summary>
		public void NetSaleFixedCard(int cardId , int cardType ,List<NetSaleCardVo> _list )
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().SaleFateFixedCard (GameModel.GetInstance.curRoomId, cardId, cardType, _list);
			}
		}

		/// <summary>
		/// Nets the sale chance share card. 出售股票卡牌
		/// </summary>
		/// <param name="cardId">Card identifier.</param>
		/// <param name="cardType">Card type.</param>
		/// <param name="_list">List.</param>
		public void NetSaleChanceShareCard(int cardId , int cardType ,List<NetSaleCardVo> _list)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().SaleChanceShareCard (GameModel.GetInstance.curRoomId, cardId, cardType, _list);
			}
		}


		public void NetBorrowMoney(List<BorrowVo> _netborrowList, int loanType)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().Game_BorrowMoney (GameModel.GetInstance.curRoomId, _netborrowList,loanType);
			}
		}

		public void NetPayBackMoney(List<PaybackVo> _netPackList)
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				NetWorkScript.getInstance ().Game_PayBackMoney (GameModel.GetInstance.curRoomId, _netPackList);
			}
		}

		public void CloseCard(int id)
		{			
//			var tmpRandom = UnityEngine.Random.Range (0, 100);
//			if (tmpRandom <= 60)
//			{
//				return;
//			}
			if (PlayerManager.Instance.HostPlayerInfo.IsTrainTip () == false)
			{
				return;
			}

			Console.WriteLine ("啦啦啦啦啦来了来了关闭窗口"+Client.Unit.BattleController.Instance.CurrentPlayerIndex.ToString());

			// 玩家关闭卡牌界面后
			if (Client.Unit.BattleController.Instance.CurrentPlayerIndex == 1)
			{
				var player = PlayerManager.Instance.HostPlayerInfo;

				var tmpStr = "";

				if (player.isEnterInner == false)
				{
					if ( player.totalAge < 31)
					{
						tmpStr = GameTipManager.Instance.GetYoungOuterTip ();
//						Debug.LogError ("外圈进入游戏31岁之前...."+player.totalAge.ToString()+"......"+tmpStr);
					} 
					else if(player.totalAge>=31 && player.totalAge<45)
					{
						tmpStr = GameTipManager.Instance.GetOldOuterTip ();
//						Debug.LogError("外圈进入游戏31-45岁之间...."+player.totalAge.ToString()+"......"+tmpStr);
					}	
					else
					{
						tmpStr = GameTipManager.Instance.GetMoreOldTip ();
//						Debug.LogError("外圈进入游戏45岁之后...."+player.totalAge.ToString()+"......"+tmpStr);
					}
				} 
				else
				{
					tmpStr = GameTipManager.Instance.GetInnerTip ();
				}

				Console.WriteLine ("hhahhahahahah,,,,,,,,,,,,,,,,,"+tmpStr);

				var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
				controller.OnShowGameTip (tmpStr);
			}
        }


		/// <summary>
		/// Shows the opport card. 处理大机会卡牌
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowOpportCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{			
			var cardController = UIControllerManager.Instance.GetController<UIOpportunityCardController> ();
			var template = MetadataManager.Instance.GetTemplateTable<Opportunity> ();
			var it = template.GetEnumerator ();

			while (it.MoveNext ()) 
			{
				Console.WriteLine (id);
				var value = it.Current.Value as Opportunity;

				if (value.id == id) 
				{	
					cardController.cardData = value;
//					player.PlayerIntegral += value.rankScore; 
					player._bigIntegral += value.rankScore; 
					player._bigOpportunitiesTotalNum += 1;
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				opportunityIDList.Add(id);
			} 
			else
			{				
				cardController.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}
		}

		private void _ShowChanceFixedCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIChanceFixedCardController> ();
			var template = MetadataManager.Instance.GetTemplateTable<ChanceFixed> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as ChanceFixed;
				if (value.id == id) 
				{
					cardController.cardData = value;
//					player.PlayerIntegral += value.rankScore; 
					player._smallIntegral += value.rankScore;
					player._smallOpportunitiesTotalNum += 1;
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				chanceIDList.Add(id);
			} 
			else
			{				
				cardController.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}
		}

		/// <summary>
		/// Shows the chance shares card. 显示股票卡牌的信息
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowChanceSharesCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIChanceShareCardController> ();
			var template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
			var it = template.GetEnumerator ();
			var tmpShare =it.Current.Value as ChanceShares ;
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as ChanceShares;
				if (value.id == id) 
				{
					tmpShare = value;
//					player.PlayerIntegral += value.rankScore; 
					player._smallIntegral += value.rankScore;
					player._smallOpportunitiesTotalNum += 1;
					break;
				}
			}

			var playerArr=PlayerManager.Instance.Players;
			var showBoard = true;
			for (var i = playerArr.Length - 1; i >= 0; i--)
			{
				cardController.playerInfor=playerArr[i];
				cardController.cardData = tmpShare;
				if (i > 0)
				{						
					//如果是其它玩家
					if (playerArr [i].isEnterInner == false)
					{
						if (i == Client.Unit.BattleController.Instance.CurrentPlayerIndex)
						{
							//处理下卡牌
							cardController.HandlerCardData ();
						}
					}	
				}
				else
				{
					if(playerArr[i].isEnterInner==false)
					{
						//如果是自己玩卡牌而且是自己自己掷色子，显示弹板
						if (PlayerManager.Instance.IsHostPlayerTurn () == true)
						{
							cardController.setVisible (true);
							chanceIDList.Add(id);
						}
						else
						{							
							//如果是别人掷色子，但是自己有符合条件的卡牌，继续弹板
							if (cardController.HasSameTypeShare () == false)
							{
								showBoard = false;
							}
							else
							{
								cardController.setVisible (true);
							}						
						}
							
					}
				}
			}

			if (playerArr [0].isEnterInner == true || showBoard==false)
			{
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}

		}

		private void _ShowOuterFateCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIOuterFateCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<OuterFate> ();
			var it = tmplate.GetEnumerator ();
			var tmpOutefate=it.Current.Value as OuterFate;
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as OuterFate;
				if (value.id == id) 
				{
					tmpOutefate=value;
//					player.PlayerIntegral += value.rankScore; 
//					cardController.cardData=tmpOutefate;
					break;
				}
			}

			var playerArr=PlayerManager.Instance.Players;
			var showbaord = true;
			if (GameModel.GetInstance.isPlayNet == false)
			{
				for (var i = playerArr.Length - 1; i >= 0; i--)
				{
					cardController.playerInfor=playerArr[i];
					cardController.cardData = tmpOutefate;
					if (i > 0)
					{						
						//如果是其它人，就处理符合条件的
						if (playerArr [i].isEnterInner == false)
						{
							cardController.HandlerCardData ();
						}
					}
					else
					{
						if (playerArr [i].isEnterInner == false)
						{
							//如果是轮到玩家，本来就是自己，那就显示
							if (PlayerManager.Instance.IsHostPlayerTurn () == true)
							{
								cardController.setVisible (true);
								fateIDList.Add(id);
							}
							else
							{
								//如果不是玩家自己玩，但是显示影响所有人物类型的，需要弹板点击确定

								if (cardController.IsEffectAll() == true)
								{
									cardController.setVisible (true);
								}
								else
								{
									//有卡牌去显示
									if(cardController.IsFateToSale()==true)
									{
										cardController.setVisible (true);
									}
									else
									{
										showbaord = false;
									}
								}														
							}
						}
					}
				}

				if (playerArr [0].isEnterInner == true  || showbaord ==false)
				{					
					Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				}
			}
			else
			{
				//如果是轮到玩家，本来就是自己，那就显示
				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
				{
					cardController.playerInfor=player;
					cardController.cardData = tmpOutefate;
					cardController.setVisible (true);
					fateIDList.Add(id);
				}
			}



		}

		/// <summary>
		/// Shows the quality life card. 品质生活卡牌
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowQualityLifeCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIQualityLifeCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<QualityLife> ();
			var it = tmplate.GetEnumerator ();
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as QualityLife;
				if (value.id == id) 
				{
					cardController.cardData=value;
//					player.PlayerIntegral += value.rankScore; 
					player._qualityTotalNum+=1;
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				qualityLifeIDList.Add(id);
			} 
			else
			{				
				cardController.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}
		}

		//处理内圈命运卡牌  涉及到离婚，审计和金融风暴
		private void _ShowInnerFateCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIInnerFateCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<InnerFate> ();
			var it = tmplate.GetEnumerator ();
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as InnerFate;
				if (value.id == id) 
				{
					cardController.cardData=value;
//					player.PlayerIntegral += value.rankScore; 
					if (value.id == 90006)
					{
						player._divorceTotalNum += 1;
					}				
					if(value.id == 90009)
					{
						// 审计
						player._auditTotalNum += 1;
					}
					if (value.id == 90004)
					{
						player._moneyLossTotalNum += 1;
					}
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				fateIDList.Add(id);
			} 
			else
			{
				if (GameModel.GetInstance.isPlayNet == false)
				{
					cardController.HandlerCardData ();
					Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				}

			}
		}

		private void _ShowInnerRelaxCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIRelaxCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<Relax> ();
			var it = tmplate.GetEnumerator ();
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as Relax;
				if (value.id == id) 
				{
					cardController.cardData = value;
//					player.PlayerIntegral += value.rankScore; 
					player._richleisureTotalNum+=1;
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				relaxkIDList.Add(id);
			} 
			else
			{
				if (GameModel.GetInstance.isPlayNet == false)
				{
					cardController.HandlerCardData ();
					Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				}
			}
		}

		/// <summary>
		/// Shows the inner investment card. 处理内圈投资卡牌    
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowInnerInvestmentCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIInvestmentCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<Investment> ();
			var it = tmplate.GetEnumerator ();

			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as Investment;
				if (value.id == id) 
				{
					cardController.cardData = value;
//					player.PlayerIntegral += value.rankScore; 
					player._investmentIntegral += value.rankScore;
					player._investmentTotalNum += 1;
					break;
				}
			}

			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				investmentIDList.Add(id);
			} 
			else
			{
				if (GameModel.GetInstance.isPlayNet == false)
				{
					cardController.HandlerCardData ();
					Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				}
			}
		}

		/// <summary>
		/// Shows the risk card.处理风险卡牌
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowRiskCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIRiskCardController> ();
			var tmplate = MetadataManager.Instance.GetTemplateTable<Risk> ();
			var it = tmplate.GetEnumerator ();
			while (it.MoveNext ()) 
			{
				var value = it.Current.Value as Risk;
				if (value.id == id) 
				{
					cardController.cardData=value;
//					player.PlayerIntegral += value.rankScore; 
					break;
				}
			}
			if (isHostPlayerTurn == true)
			{
				cardController.setVisible (true);
				riskIDList.Add(id);
			} 
			else
			{				
				cardController.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			}
		}

		private void destoryParticle()
		{
			Particle.Instance.DestroyHeadParticle();
		}

		// 外圈风险 
		public List<int> outerRiskList=new List<int>();
		// 外圈小机会 
		public List<int> outerChanceList=new List<int>();
		// 外圈大机会 
		public List<int> outerOpportunityList = new List<int> ();
        // 外圈命运 
		public List<int> outerFateList = new List<int> ();

		// 外圈房产公司其它 
		public List<int> outerChanceFixed = new List<int> ();
		// 外圈股票 
		public List<int> outerChanceShares = new List<int> ();


		// 投资 
		public List<int> innerInvestmentList =new List<int>();
		// 有钱有闲 
		public List<int> innerRelaxList = new List<int> ();
		// 品质生活 
		public List<int> innerQualtyList = new List<int> ();
		// 内圈命运 
		public List<int> innerFateList = new List<int> ();	 

		//2016-11-15 zll fix
		public List<Risk> outerRiskAllList = new List<Risk>();
		public List<Opportunity> outerOpportunityAllList = new List<Opportunity>();
		public List<Chance> chanceAllList = new List<Chance>();

		public List<Fate> FateAllList = new List<Fate>();
		public List<Investment> innerInvestmentAllList = new List<Investment>();
		public List<QualityLife> innerQualityLifeAllList = new List<QualityLife>();
		public List<Relax> innerRelaxkAllList = new List<Relax>();


		public List<int> riskIDList = new List<int>();
		public List<int> opportunityIDList = new List<int>();
		public List<int> chanceIDList = new List<int>();

		public List<int> fateIDList = new List<int>();
		public List<int> investmentIDList = new List<int>();
		public List<int> qualityLifeIDList = new List<int>();
		public List<int> relaxkIDList = new List<int>();

		public static readonly CardManager Instance = new CardManager();

		private CardManager()
		{			
			var template = MetadataManager.Instance.GetTemplateTable<Risk> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Risk;
				outerRiskList.Add(value.id);
				outerRiskAllList.Add(value);
			}
			outerRiskList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<ChanceFixed> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as ChanceFixed;
				outerChanceFixed.Add (value.id);	
				outerChanceList.Add (value.id);
				GetChanceFixed(value);
			}


			template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
			it = template.GetEnumerator ();

			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as ChanceShares;
				outerChanceShares.Add (value.id);	
				outerChanceList.Add (value.id);
				GetChanceShares(value);
			}
			outerChanceFixed.TrimExcess ();
			outerChanceShares.TrimExcess ();
			outerChanceList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<Opportunity> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Opportunity;
				outerOpportunityList.Add (value.id);
				outerOpportunityAllList.Add(value);

				//Debug.LogError ("当前的卡牌id是："+value.id.ToString());
			}
			outerOpportunityList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<OuterFate> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as OuterFate;
				outerFateList.Add (value.id);	
				GetouterFateValue(value);
			}
			outerFateList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<Investment> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Investment;
				innerInvestmentList.Add(value.id);		
				innerInvestmentAllList.Add(value);
			}
			innerInvestmentList.TrimExcess ();


			template = MetadataManager.Instance.GetTemplateTable<Relax> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Relax;
				innerRelaxList.Add (value.id);
				innerRelaxkAllList.Add(value);
			}

			innerRelaxList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<QualityLife> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as QualityLife;
				innerQualtyList.Add(value.id);
				innerQualityLifeAllList.Add(value);
			}
			innerQualtyList.TrimExcess ();

			template = MetadataManager.Instance.GetTemplateTable<InnerFate> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as InnerFate;
				innerFateList.Add (value.id);
				GetInnerFateValue(value);
			}		 
			innerFateList.TrimExcess ();

			GameEventManager.Instance.SubscribeEvent (GameEvents.CheckDay, _CheckOutDay);
		}
			

		void GetouterFateValue(OuterFate outerFate)
		{
			Fate fateValue = new Fate();
			fateValue.title = outerFate.title;
			fateValue.cardPath = outerFate.cardPath;
			fateValue.cardIntroduce = outerFate.desc;
			fateValue.id = outerFate.id;
			FateAllList.Add(fateValue);
		}

		void GetInnerFateValue(InnerFate innerFate)
		{
			Fate fateValue = new Fate();
			fateValue.title = innerFate.title;
			fateValue.cardPath = innerFate.cardPath;
			fateValue.cardIntroduce = innerFate.desc;
			fateValue.id = innerFate.id;
			FateAllList.Add(fateValue);
		}

		void GetChanceFixed(ChanceFixed chanceFixed)
		{
			Chance chanceValue = new Chance();
			chanceValue.id = chanceFixed.id;
			chanceValue.belongsTo = chanceFixed.belongsTo;
			chanceValue.title = chanceFixed.title;
			chanceValue.cardPath = chanceFixed.cardPath;
			chanceValue.desc = chanceFixed.desc;
			chanceValue.baseNumber = chanceFixed.baseNumber;
			chanceValue.coast = chanceFixed.coast;
			chanceValue.sale = chanceFixed.sale;
			chanceValue.payment = chanceFixed.payment;
			chanceValue.profit = chanceFixed.profit;
			chanceValue.mortgage = chanceFixed.mortgage;
			chanceValue.scoreType = chanceFixed.scoreType;
			chanceValue.scoreNumber = chanceFixed.scoreNumber;
			chanceAllList.Add(chanceValue);
		}

		void GetChanceShares(ChanceShares chanceShares)
		{
			Chance chanceValue = new Chance();
			chanceValue.id = chanceShares.id;
			chanceValue.cash_belongsTo = chanceShares.belongsTo;
			chanceValue.cash_title = chanceShares.title;
			chanceValue.cash_cardPath = chanceShares.cardPath;
			chanceValue.cash_desc = chanceShares.desc;
			chanceValue.cash_ticketCode = chanceShares.ticketCode;
			chanceValue.cash_ticketName = chanceShares.ticketName;
			chanceValue.cash_payment = chanceShares.payment;
			chanceValue. cash_returnRate = chanceShares.returnRate;
			chanceValue.cash_shareOut = chanceShares.shareOut;
			chanceValue.cash_qualityScore = chanceShares.qualityScore;
			chanceValue.cash_qualityDesc = chanceShares.qualityDesc;
			chanceValue.cash_income = chanceShares.income;
			chanceValue.cash_priceRagne = chanceShares.priceRagne;
			chanceValue.cash_shareNum = chanceShares.shareNum;
			chanceAllList.Add(chanceValue);
		}

	}
}
	