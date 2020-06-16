using System;
using UnityEngine;
using Metadata;
using System.Collections;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 网络模式下，显示不同卡牌类型的逻辑
    /// </summary>
	public class NetCardManager
	{
		/// <summary>
		/// Opens the card.显示卡牌的逻辑
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void OpenCard(int id)
		{
			if (GameModel.GetInstance.isReconnecToGame == 1)
			{
				return;
			}           

			var player = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			var isHostPlayerTurn=PlayerManager.Instance.IsHostPlayerTurn();

            if(isHostPlayerTurn==false)
            {
                var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
                var cardTitle = "";
                if ((int)SpecialCardType.HealthType == id || (int)SpecialCardType.InnerHealthType == id)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardHealth;
                }
                else if((int)SpecialCardType.CharityType == id)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardCharity;
                }
                else if ((int)SpecialCardType.StudyType == id || (int)SpecialCardType.InnerStudyType == id)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardStudy;
                }
                else if((int)SpecialCardType.CheckDayType == id || (int)SpecialCardType.InnerCheckDayType == id)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardCheckOut;
                }
                else if((int)SpecialCardType.GiveChildType == id)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardGetChild;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.outFate)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardFate;// GameModel.GetInstance.netOuterFateCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.fixedChance)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardChance;// GameModel.GetInstance.netFixedChancCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.sharesChance)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardChance;// GameModel.GetInstance.netChanceShareCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.bigChance)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardOpportunity;// GameModel.GetInstance.netOpportunityCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.risk)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardRisk;// GameModel.GetInstance.netRiskCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.inFate)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardFate;// GameModel.GetInstance.netInnerFateCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.investment)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardInvestment;// GameModel.GetInstance.netInvestmentCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.qualityLife)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardQualityLife;// GameModel.GetInstance.netQualityCard.title;
                }
                else if(GameModel.GetInstance.ShowCardType == (int)SpecialCardType.richRelax)
                {
                    cardTitle = SubTitleManager.Instance.subtitle.cardTimeAndMoney;// GameModel.GetInstance.netRelaxCard.title;
                }

                if (cardTitle!="")
                {
                    battleController.ShowControllerBoard(player.playerName, cardTitle);
                }

            }

            if (player.isNetOnline == false)
			{
				VirtualServer.Instance.NetGameLostToSelectNext();
				return;
			}

			if ((int)SpecialCardType.SelectChance == id)
			{
				//如果是选择大机会，小机会的选择本地显示
				if (isHostPlayerTurn == true)
				{
					var controller = Client.UIControllerManager.Instance.GetController<UIChanceSelectController> ();
					controller.setVisible (true);
				}
				return;
			}

			if ((int)SpecialCardType.SelectInnerFree == id)
			{
				//自由选择的界面
				if (isHostPlayerTurn == true)
				{
					var controller = Client.UIControllerManager.Instance.GetController<UIInnerSelectFreeWindowController> ();
					controller.setVisible(true);
				}
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

                player.Settlement._healthTotalNum += 1;

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardHealth),()=>{
					if (isHostPlayerTurn== true)
					{
                        controller.IsOnlyShow = false;
					}
					else
					{
                        controller.IsOnlyShow = true;						
					}
                    controller.setVisible(true);
                    specialEffects.setHeadImageDisappear();
					destoryParticle();
				},false);

				return;
			}
			 
			if ((int)SpecialCardType.CharityType == id)
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialcharitycard);
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialcharitycard);

				var controller =Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
				controller.player = player;	
				controller.SetCardId(id);
                player.Settlement._charityTotalNum += 1;
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardCharity),()=>{
					if(isHostPlayerTurn==true)
					{
                        controller.IsOnlyShow = false;
					}
					else
					{
                        controller.IsOnlyShow = true;
					}
                    controller.setVisible(true);

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

                player.Settlement._learnTotalNum += 1;

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardStudy), () => {
					if(isHostPlayerTurn==true)
					{
                        controller.IsOnlyShow = false;
					}
					else
					{
                        controller.IsOnlyShow = true;
					}
                    controller.setVisible(true);

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
                        controller.IsOnlyShow = false;
                        player.Settlement._settlementNum += 1;
						if(player.isEnterInner==true)
						{
                            player.Settlement._settleInnerNum+=1;
						}
						else
						{
                            player.Settlement._settleOuterNum+=1;
						}
						
                        Audio.AudioManager.Instance.Tip_CheckDay(player.careerID);
					}
					else
					{
                        controller.IsOnlyShow = true;
                    }
                    controller.setVisible(true);
                    specialEffects.setHeadImageDisappear();
					destoryParticle();

				},false);

				return;
			}

			//生孩子
			if ((int)SpecialCardType.GiveChildType == id)
			{
				//Particle.Instance.AddHeadParticle(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.specialchild);

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
                        Console.Warning.WriteLine("轮到自己生孩了来来来啦啦啦啦啦啦了{0}", isHostPlayerTurn.ToString());
      //                  if (player._childNum>=player.childNumMax)
						//{
						//	player._childNum=player.childNumMax;
						//}
                        Audio.AudioManager.Instance.Tip_GiveChild(player.careerID);
                        controller.IsOnlyShow = false;
						controller.setVisible (true);
					}
					specialEffects.setHeadImageDisappear();
					destoryParticle();
				},false);
				return;
			}

			//外圈晋级内圈
			if ((int)SpecialCardType.UpGradType == id)
			{
				if(isHostPlayerTurn == true)
				{
				}
				else
				{
					//var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
					//controller.playerInfor = player;
					//controller.HandlerCardData ();
				}
				return;
			}

			//当某一玩家胜利时
			if ((int)SpecialCardType.SuccessType == id)
			{
				var overHandler = Client.UIControllerManager.Instance.GetController<UIGameOverWindowController> ();
				overHandler.setVisible (true);
				return;
			}

			// 命运
			if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.outFate) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_fate);
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard, player.playerName,SubTitleManager.Instance.subtitle.cardFate),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowOuterFateCard (id,isHostPlayerTurn,player);

                    if(isHostPlayerTurn==true)
                    {
                        Audio.AudioManager.Instance.Tip_Fate(player.careerID);
                    }

				},false);
				return;					
			}

			// 固定资产的卡牌
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.fixedChance)// outerChanceFixed.Contains (id)) 
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
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.sharesChance) //outerChanceShares.Contains (id)) 
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_little_chance);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardChance),()=>{

					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowChanceSharesCard (id,isHostPlayerTurn,player);
                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Chance(player.careerID);
                    }
				},false);
				return;
			}

			//处理大机会卡牌
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.bigChance)// outerOpportunityList.Contains (id))
			{
				var specialEffects =Client.UIControllerManager.Instance.GetController<UISpecialeffectsWindowController>();
				specialEffects.setHeadImage(Room.Instance.getCurrentPlay().getPlayPos(),Particle.StatusParticle.outer_ring_great_opportunity);

				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.enterCard,player.playerName,SubTitleManager.Instance.subtitle.cardOpportunity),()=>{
					destoryParticle();
					specialEffects.setHeadImageDisappear();
					_ShowOpportCard (id,isHostPlayerTurn,player);

                    if(isHostPlayerTurn)
                    {
                        Audio.AudioManager.Instance.Tip_Chance(player.careerID);
                    }
				},false);
				return;
			}
			//风险处理卡牌
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.risk)  //outerRiskList.Contains (id)) 
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
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.inFate)// innerFateList.Contains (id)) 
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
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.investment) ///innerInvestmentList.Contains (id)) 
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
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.qualityLife)  //innerQualtyList.Contains (id)) 
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
			if (GameModel.GetInstance.ShowCardType==(int)SpecialCardType.richRelax) //innerRelaxList.Contains (id)) 
			{
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

		/// <summary>
		/// Shows the opport card. 处理大机会卡牌
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="isHostPlayerTurn">If set to <c>true</c> is host player turn.</param>
		/// <param name="player">Player.</param>
		private void _ShowOpportCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{			
			var cardController = UIControllerManager.Instance.GetController<UIOpportunityCardController> ();
            cardController.playerInfor = player;
			var value = GameModel.GetInstance.netOpportunityCard;
			cardController.cardData = value;
            //player.Settlement._bigIntegral += value.rankScore;
            player.Settlement._bigOpportunitiesTotalNum += 1;

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;				
				CardManager.Instance.opportunityIDList.Add(id);
			} 
			else
			{
                cardController.IsOnlyShow = true;
                //cardController.HandlerCardData ();
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);

            }
            cardController.setVisible(true);
        }

		private void _ShowChanceFixedCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIChanceFixedCardController> ();
			var value = GameModel.GetInstance.netFixedChancCard;
            cardController.playerInfor = player;
            cardController.cardData = value;
            
            player.Settlement._smallOpportunitiesTotalNum += 1;

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;				
				CardManager.Instance.chanceIDList.Add(id);
			} 
			else
			{
                cardController.IsOnlyShow = true;
                //cardController.HandlerCardData ();
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);
            }
            cardController.setVisible(true);
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
			var tmpShare =GameModel.GetInstance.netChanceShareCard;
            //					player.PlayerIntegral += value.rankScore; 
            //player.Settlement._smallIntegral += tmpShare.rankScore;
            player.Settlement._smallOpportunitiesTotalNum += 1;

			var playerArr=PlayerManager.Instance.Players;
			var showBoard = true;

            cardController.playerInfor = player;
            cardController.cardData = tmpShare;
            //cardController.setVisible(true);
            CardManager.Instance.chanceIDList.Add(id);

            if (PlayerManager.Instance.IsHostPlayerTurn() == true)
            {
                cardController.playerInfor = player;
                cardController.cardData = tmpShare;
                cardController.IsOnlyShow = false;
                cardController.setVisible(true);
                CardManager.Instance.chanceIDList.Add(id);
            }
            else
            {
                for (var i = playerArr.Length - 1; i >= 0; i--)
                {
                    cardController.playerInfor = playerArr[i];
                    cardController.cardData = tmpShare;
                    if (i > 0)
                    {
                    }
                    else
                    {
                        if (playerArr[i].isEnterInner == false)
                        {
                            if (cardController.HasSameTypeShare() == false)
                            {
                                showBoard = false;
                                NetWorkScript.getInstance().QuitCard(GameModel.GetInstance.curRoomId, tmpShare.id, (int)SpecialCardType.sharesChance);//后面打开
                            }
                            else
                            {
                                cardController.IsOnlyShow = false;
                                cardController.setVisible(true);
                            }
                        }
                        else
                        {
                            showBoard = false;
                            NetWorkScript.getInstance().QuitCard(GameModel.GetInstance.curRoomId, tmpShare.id, (int)SpecialCardType.sharesChance);//后面打开						
                        }
                    }
                }
            }

            if (playerArr [0].isEnterInner == true || showBoard==false)
			{
                cardController.IsOnlyShow = true;
                cardController.setVisible(true);
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);				
            }
		}

		private void _ShowOuterFateCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIOuterFateCardController> ();
			var tmpOutefate=GameModel.GetInstance.netOuterFateCard;

			var playerArr=PlayerManager.Instance.Players;
			var showbaord = true;

			if (GameModel.GetInstance.isPlayNet == true)
			{
                cardController.playerInfor = player;
                cardController.cardData = tmpOutefate;
                //cardController.setVisible(true);
                CardManager.Instance.fateIDList.Add(id);

                //如果是轮到玩家，本来就是自己，那就显示
                if (PlayerManager.Instance.IsHostPlayerTurn() == true)
                {   
                    
                    cardController.playerInfor = player;
                    cardController.cardData = tmpOutefate;
                    cardController.IsOnlyShow = false;
                    cardController.setVisible(true);
                    CardManager.Instance.fateIDList.Add(id);
                }
                else
                {
                    for (var i = playerArr.Length - 1; i >= 0; i--)
                    {
                        cardController.playerInfor = playerArr[i];
                        cardController.cardData = tmpOutefate;
                        if (i > 0)
                        {

                        }
                        else
                        {
                            if (playerArr[i].isEnterInner == false)
                            {
                                //如果是轮到玩家，本来就是自己，那就显示
                                if (cardController.IsEffectAll() == true)
                                {
                                    cardController.IsOnlyShow = false;
                                    cardController.setVisible(true);
                                }
                                else
                                {
                                    //有卡牌去显示
                                    if (cardController.IsFateToSale() == true)
                                    {
                                        cardController.IsOnlyShow = false;
                                        cardController.setVisible(true);
                                    }
                                    else
                                    {                                       
                                        showbaord = false;
                                        NetWorkScript.getInstance().QuitCard(GameModel.GetInstance.curRoomId, tmpOutefate.id, (int)SpecialCardType.outFate);//后面打开
                                    }
                                }
                            }
                            else
                            {
                                NetWorkScript.getInstance().QuitCard(GameModel.GetInstance.curRoomId, tmpOutefate.id, (int)SpecialCardType.outFate);//后面打开
                            }
                        }
                    }
                }

                if (playerArr [0].isEnterInner == true  || showbaord ==false)
				{
                    cardController.IsOnlyShow = true;
                    cardController.setVisible(true);
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

			var value =GameModel.GetInstance.netQualityCard;
			cardController.cardData=value;
            player.Settlement._qualityTotalNum+=1;
            cardController.playerInfor = player;
            cardController.castRate = 1;

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;
				CardManager.Instance.qualityLifeIDList.Add(id);
			} 
			else
			{
                var isOnlyShow = true;
                //if (PlayerManager.Instance.IsAllEnterInner() == true)
                //{
                //    isOnlyShow = false;
                //}
                cardController.IsOnlyShow = isOnlyShow;
                //cardController.HandlerCardData ();
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);				
            }
            cardController.setVisible(true);
        }

        /// <summary>
        /// 处理内圈命运卡牌  涉及到离婚，审计和金融风暴
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isHostPlayerTurn"></param>
        /// <param name="player"></param>
        private void _ShowInnerFateCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIInnerFateCardController> ();

			var value = GameModel.GetInstance.netInnerFateCard;
			cardController.cardData=value;
			if (value.id == 90006)
			{
                player.Settlement._divorceTotalNum += 1;
			}				

			if(value.id == 90009)
			{
                // 审计
                player.Settlement._auditTotalNum += 1;
			}

			if (value.id == 90004)
			{
                player.Settlement._moneyLossTotalNum += 1;
			}

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;
                CardManager.Instance.fateIDList.Add(id);
			} 
			else
			{
                cardController.IsOnlyShow = true;
				//cardController.HandlerCardData ();
				//Client.Unit.BattleController.Instance.Send_RoleSelected (1);
            }
            cardController.setVisible(true);
        }

        /// <summary>
        /// 显示有钱有闲卡牌
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isHostPlayerTurn"></param>
        /// <param name="player"></param>
		private void _ShowInnerRelaxCard(int id,bool isHostPlayerTurn,PlayerInfo player)
		{
			var cardController = UIControllerManager.Instance.GetController<UIRelaxCardController> ();
			var value = GameModel.GetInstance.netRelaxCard;
			cardController.cardData = value;
            cardController.castRate = 1;
            player.Settlement._richleisureTotalNum+=1;
            cardController.playerInfor = player;

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;
				CardManager.Instance.relaxkIDList.Add(id);
			} 
			else
			{
                var isOnlyShow = true;
                //if (PlayerManager.Instance.IsAllEnterInner() == true)
                //{
                //    isOnlyShow = false;
                //}
                cardController.IsOnlyShow = isOnlyShow;
                //cardController.HandlerCardData ();
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);

            }
            cardController.setVisible(true);
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
			var value = GameModel.GetInstance.netInvestmentCard;
			cardController.cardData = value;
            cardController.playerInfor = player;
            cardController.castRate = 1;

            //player.Settlement._investmentIntegral += value.rankScore;
            player.Settlement._investmentTotalNum += 1;

			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;
				CardManager.Instance.investmentIDList.Add(id);
			} 
			else
			{
                var isOnlyShow = true;
                //if(PlayerManager.Instance.IsAllEnterInner()==true)
                //{
                //    isOnlyShow = false;
                //}
                cardController.IsOnlyShow = isOnlyShow;
                // cardController.HandlerCardData ();
                //Client.Unit.BattleController.Instance.Send_RoleSelected (1);
            }
            cardController.setVisible(true);
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
			var value = GameModel.GetInstance.netRiskCard;
			cardController.cardData = value;
			if (isHostPlayerTurn == true)
			{
                cardController.IsOnlyShow = false;				
				CardManager.Instance.riskIDList.Add(id);
			} 
			else
			{
                cardController.IsOnlyShow = true;
					//cardController.HandlerCardData ();
					//Client.Unit.BattleController.Instance.Send_RoleSelected (1);				
			}
            cardController.setVisible(true);
        }

        /// <summary>
        /// 关闭展示卡牌信息UI
        /// </summary>
        public void CloseShowCard()
        {
            //健康管理
            if ((int)SpecialCardType.HealthType == GameModel.GetInstance.ShowCardType || (int)SpecialCardType.InnerHealthType == GameModel.GetInstance.ShowCardType)// 健康
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
                if(controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }
            if ((int)SpecialCardType.CharityType == GameModel.GetInstance.ShowCardType)
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
                if(controller.getVisible())
                {
                    controller.setVisible(false);
                }               
                return;
            }

            if ((int)SpecialCardType.StudyType == GameModel.GetInstance.ShowCardType || (int)SpecialCardType.InnerStudyType == GameModel.GetInstance.ShowCardType) //  进修学习
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
                if(controller.getVisible())
                {
                    controller.setVisible(false);
                }                
                return;
            }
            if ((int)SpecialCardType.CheckDayType == GameModel.GetInstance.ShowCardType || (int)SpecialCardType.InnerCheckDayType == GameModel.GetInstance.ShowCardType) // 结账日
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();
                if(controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //生孩子
            //if ((int)SpecialCardType.GiveChildType == GameModel.GetInstance.ShowCardType)
            //{
            //    var controller = Client.UIControllerManager.Instance.GetController<UIOtherCardWindowController>();                
            //    return;
            //}
            // 命运
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.outFate)
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOuterFateCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            // 固定资产的卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.fixedChance)// outerChanceFixed.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIChanceFixedCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }
            //股票卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.sharesChance) //outerChanceShares.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIChanceShareCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //处理大机会卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.bigChance)// outerOpportunityList.Contains (id))
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIOpportunityCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }
            //风险处理卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.risk)  //outerRiskList.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIRiskCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //内圈命运卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.inFate)// innerFateList.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIInnerFateCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //内圈投资卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.investment) ///innerInvestmentList.Contains (id)) 
			{
                var controller = Client.UIControllerManager.Instance.GetController<UIInvestmentCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //处理内圈品质生活卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.qualityLife)  //innerQualtyList.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIQualityLifeCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }

            //处理有钱有闲卡牌
            if (GameModel.GetInstance.ShowCardType == (int)SpecialCardType.richRelax) //innerRelaxList.Contains (id)) 
            {
                var controller = Client.UIControllerManager.Instance.GetController<UIRelaxCardController>();
                if (controller.getVisible())
                {
                    controller.setVisible(false);
                }
                return;
            }
        }

		private void destoryParticle()
		{
			Particle.Instance.DestroyHeadParticle();
		}

		private static NetCardManager _instance;

		public static NetCardManager Instance
		{
			get
			{
				if (null == _instance)
				{
					_instance = new NetCardManager ();
				}

				return _instance;
			}
		}

		public NetCardManager()
		{			
		}

	}
}