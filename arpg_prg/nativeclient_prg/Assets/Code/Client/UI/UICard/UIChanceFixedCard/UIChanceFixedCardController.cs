﻿using System;
using Metadata;

namespace Client.UI
{
    /// <summary>
    /// 小机会固定资产卡牌UI
    /// </summary>
	public class UIChanceFixedCardController:UIController<UIChanceFixedCardWindow,UIChanceFixedCardController>
	{
		/// <summary>
		/// 窗口资源 重复使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/opportunitycard.ab";
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


		public void QuitCard()
		{
            var heroInfor =this.playerInfor;// PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			if (null != cardData)
			{
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.quitChanceCard2,heroInfor.playerName,cardData.title),null,true);

			}

            if(this.normalQuit()==false)
            {                
                heroInfor.Settlement._smallIntegral += cardData.quitScore;
            }
		}

		public void NetQuitCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.fixedChance);
			}

		}

		public void NetBuyCard()
		{			
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyFixedCard , cardData.id, (int)SpecialCardType.fixedChance);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool normalQuit()
        {
            var normal = false;

            if (playerInfor.totalMoney + cardData.payment < 0)
            {
                normal = true;
            }

            return normal;
        }

		public bool HandlerCardData()
		{
			var canGet = false;
			if(null!=cardData)
			{
				var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
                var heroInfor =this.playerInfor;// PlayerManager.Instance.Players[turnIndex];

				if (heroInfor.totalMoney+cardData.payment<0) 
				{
					if (PlayerManager.Instance.IsHostPlayerTurn () == true) 
					{
						MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);									
					}
					return canGet;
				}
				else
				{
					canGet = true;

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment;
						if (cardData.scoreType==(int)CardManager.ScoreType.TimeScore) 
						{
							heroInfor.timeScore += cardData.scoreNumber;
							if (cardData.scoreNumber != 0) 
							{
								var recordInfor = new InforRecordVo ();
								recordInfor.title = cardData.title;
								recordInfor.num = cardData.scoreNumber;
								heroInfor.AddTimeScoreInfor (recordInfor);
							}
						}
						else if(cardData.scoreType==(int)CardManager.ScoreType.QualityScore)
						{
							heroInfor.qualityScore+=cardData.scoreNumber;
							if (cardData.scoreNumber != 0)
							{
								var recordInfor = new InforRecordVo ();
								recordInfor.title = cardData.title;
								recordInfor.num = cardData.scoreNumber;
								heroInfor.AddQualityScoreInfor (recordInfor);
							}

						}

						heroInfor.totalPayment-=cardData.payment;
						heroInfor.totalDebt -= cardData.mortgage;
						heroInfor.totalIncome += cardData.income;				

						if(cardData.belongsTo==(int)CardManager.BalacneKind.House)
						{
							heroInfor.housePayment -= cardData.payment;
							heroInfor.houseDebt -= cardData.mortgage;
						}
						else if(cardData.belongsTo==(int)CardManager.BalacneKind.Antique)
						{
							heroInfor.antiquePayment -= cardData.payment;
						}
						else if(cardData.belongsTo==(int)CardManager.BalacneKind.Company)
						{
							heroInfor.companyPayment -= cardData.payment;
							heroInfor.companyDebt -= cardData.mortgage;
						}
					}

					//MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.buyChanceCard2,heroInfor.playerName,cardData.title),null,true);
					heroInfor.PlayerIntegral += cardData.rankScore;
                    heroInfor.Settlement._smallIntegral += cardData.rankScore;
                    heroInfor.Settlement._smallOpportunitiesNum += 1;

                    heroInfor.chanceFixedCardList.Add (cardData);
					heroInfor.AddCapticalData ();                   
				}

                if (PlayerManager.Instance.IsHostPlayerTurn() == true)
                {
                    MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardSmallFixed);
                }
                else
                {
                    if (GameModel.GetInstance.isPlayNet == false)
                    {
                        MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.buyChanceCard2, heroInfor.playerName, cardData.title), null, true);
                    }
                }
                
                if (GameModel.GetInstance.isPlayNet == false)
				{
					var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
					if (null != battleController)
					{
						if (PlayerManager.Instance.IsHostPlayerTurn ()) 
						{
							battleController.SetQualityScore ((int)heroInfor.qualityScore);
							battleController.SetTimeScore ((int)heroInfor.timeScore);
							battleController.SetNonLaberIncome ((int)heroInfor.totalIncome,(int)heroInfor.MonthPayment);
							battleController.SetCashFlow ((int)heroInfor.totalMoney);										
						}
						else
						{
							battleController.SetPersonInfor (heroInfor,turnIndex);
						}
					}
				}
			}	

			return canGet;
		}

		public override void Tick(float deltaTime)
		{
			var window = _window as UIChanceFixedCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}


        private bool _isOnlyShow = false;


        /// <summary>
        /// 判断玩家是否只是展示卡牌信息
        /// </summary>
        public bool IsOnlyShow
        {
            get
            {
                return _isOnlyShow;
            }

            set
            {

                _isOnlyShow = value;
            }
        }





        /// <summary>
        /// 卡牌信息
        /// </summary>
		public ChanceFixed cardData;
        /// <summary>
        /// 玩家信息
        /// </summary>
        public PlayerInfo playerInfor;
	}
}

