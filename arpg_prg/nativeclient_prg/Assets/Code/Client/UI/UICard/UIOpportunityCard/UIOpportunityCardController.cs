using System;
using Metadata;
namespace Client.UI
{
	
	/// <summary>
    /// 外圈大机会卡牌UI
    /// </summary>
	public class UIOpportunityCardController:UIController<UIOpportunityCardWindow,UIOpportunityCardController>
	{
		/// <summary>
		/// 窗口资源，重复使用
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
            var heroInfor =playerInfor;// PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			if (null != cardData)
			{
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.quitChanceCard2,heroInfor.playerName,cardData.title),null,true);
			}
		}

		/// <summary>
		/// Nets the quit card. 网络版放弃卡牌
		/// </summary>
		public void NetQuitCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetQuitCard (cardData.id,(int) SpecialCardType.bigChance);
			}

            if(this.normalQuit()==false)
            {
                playerInfor.Settlement._bigIntegral += cardData.quitScore;
            }
		}

		/// <summary>
		/// Nets the buy card. 网络版本购买
		/// </summary>
		public void NetBuyCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyOpportunityCard , cardData.id,(int)SpecialCardType.bigChance,1);
			}
		}

        /// <summary>
        /// 是否可以正常放弃卡牌
        /// </summary>
        /// <returns></returns>
        private bool normalQuit()
        {
            var normal = false;
            if(playerInfor.totalMoney + cardData.payment < 0)
            {
                normal = true;
            }
            return normal;
        }

		public bool HandlerCardData()
		{
			var canGet = false;

			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
            var heroInfor =this.playerInfor;//PlayerManager.Instance.Players[turnIndex];

			if(heroInfor.totalMoney+cardData.payment<0)
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
				heroInfor.totalMoney += cardData.payment;	
				heroInfor.totalPayment-=cardData.payment;
				heroInfor.totalDebt -= cardData.mortgage;
				heroInfor.totalIncome += cardData.income;

				//人物评分
				heroInfor.PlayerIntegral += cardData.rankScore;
                heroInfor.Settlement._bigOpportunitiesNum += 1;
                heroInfor.Settlement._bigIntegral += cardData.rankScore;//购买 xx  ，放弃xxx

                //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.buyChanceCard2,heroInfor.playerName,cardData.title),null,true);
                if (PlayerManager.Instance.IsHostPlayerTurn())
                {
                    MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardChallenge);
                }
                else
                {
                    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.buyChanceCard2, heroInfor.playerName, cardData.title), null, true);
                }

                if (cardData.belongsTo==(int)CardManager.BalacneKind.House)
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
			
				heroInfor.opportCardList.Add(cardData);
				heroInfor.AddCapticalData ();

              
			}

			var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
			if(null!=battleController)
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

			return canGet;
		}

		public override void Tick(float deltaTime)
		{
			var window = _window as UIOpportunityCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

        /// <summary>
        /// 大机会卡牌数据
        /// </summary>
		public Opportunity cardData;

        /// <summary>
        /// 玩家信息
        /// </summary>
        public PlayerInfo playerInfor;

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

    }
}

