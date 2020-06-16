using System;
using Metadata;

namespace Client.UI
{
    /// <summary>
    /// 内圈投资界面UI
    /// </summary>
	public class UIInvestmentCardController:UIController<UIInvestmentCardWindow,UIInvestmentCardController>
	{
		/// <summary>
		/// 窗口预设，独立使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/investment.ab";
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
        /// 网络版放弃投资卡牌
        /// </summary>
		public void NetQuitCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.investment);
			}

            if(normalQuit()==false)
            {
                playerInfor.Settlement._investmentIntegral += cardData.quitScore;
            }
		}

		public void NetBuyCard(int rollPoint=0)
		{
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyInvestmentCard , cardData.id, (int)SpecialCardType.investment, 1,rollPoint);
			}
		}

        /// <summary>
        /// 是否可以正常的放弃
        /// </summary>
        /// <returns></returns>
        private bool normalQuit()
        {
            var normal = false;

            if(playerInfor.playerID==PlayerManager.Instance.HostPlayerInfo.playerID)
            {
                if (playerInfor.totalMoney + cardData.payment  < 0)
                {
                    normal = true;
                }else
                {
                    if(playerInfor.totalIncome>playerInfor.TargetIncome)
                    {
                        normal = true;
                    }
                }
            }
            return normal;
        }


		/// <summary>
		/// Handlers the card data.  如果遇到掷色子的 如果是npc随机判断下数字 处理大于小于的逻辑     本人的，点击确定，掷色子，判断得分
		/// </summary>
		/// <returns><c>true</c>, if card data was handlered, <c>false</c> otherwise.</returns>
		public bool HandlerCardData()
		{
			var canGet = false;
			if (null != cardData) 
			{
                var turnIndex =Array.IndexOf(PlayerManager.Instance.Players,playerInfor) ;// Client.Unit.BattleController.Instance.CurrentPlayerIndex;
                var heroInfor =playerInfor;// PlayerManager.Instance.Players[turnIndex];

				if (heroInfor.totalMoney + cardData.payment * this.castRate< 0)
				{	
					if (PlayerManager.Instance.HostPlayerInfo.playerID == playerInfor.playerID)
					{
						MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
					}				
					return canGet;
				}
				else
				{
					canGet = true;

					if (PlayerManager.Instance.HostPlayerInfo.playerID!= playerInfor.playerID)
					{
						crapNum = UnityEngine.Random.Range (1,6);

						if (GameModel.GetInstance.isPlayNet == true)
						{
							crapNum = GameModel.GetInstance.innerCardRollPoint;
						}
					}


					var tmpIncome = 0f;

					if (cardData.isDice != 0)
					{
						var isRollSuccess = false;

						if (cardData.disc_condition == 1)
						{
							if (crapNum >= cardData.disc_number)
							{
								tmpIncome = cardData.income;
								isRollSuccess = true;
							}
						}
						else if(cardData.disc_condition==2)
						{
							if (crapNum <= cardData.disc_number)
							{
								tmpIncome = cardData.income;
								isRollSuccess = true;
							}
						}

                        //if (GameModel.GetInstance.isPlayNet == false)
                        //{
                        //	if (isRollSuccess == true)
                        //	{
                        //		MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.rollSuccessInvestment,heroInfor.playerName,crapNum.ToString(),cardData.income.ToString()));
                        //	}
                        //	else
                        //	{
                        //		MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.rollFail,heroInfor.playerName,crapNum.ToString()));
                        //	}
                        //}
                        if (PlayerManager.Instance.HostPlayerInfo.playerID != playerInfor.playerID )//== false
                        {
                            //if (GameModel.GetInstance.isPlayNet == false)
                            //{
                            //    if (isRollSuccess == true)
                            //    {
                            //        MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.rollSuccessInvestment, heroInfor.playerName, crapNum.ToString(), cardData.income.ToString()));
                            //    }
                            //    else
                            //    {
                            //        MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.rollFail, heroInfor.playerName, crapNum.ToString()));
                            //    }
                            //}
                        }
                        else
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overInnerInvestment);
                        }
                    }
					else
					{
						tmpIncome = cardData.income;
                        if (PlayerManager.Instance.HostPlayerInfo.playerID != playerInfor.playerID )//== false
                        {
                            //MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.investmentGetMoney3, heroInfor.playerName, cardData.title, tmpIncome.ToString(), (-cardData.payment * this.castRate).ToString()), null, true);

                        }
                        else
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overInnerInvestment);
                        }

                    }

					heroInfor.PlayerIntegral += cardData.rankScore;
                    heroInfor.Settlement._investmentNum += 1;
                    heroInfor.Settlement._investmentIntegral += cardData.rankScore;

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment * this.castRate;
						heroInfor.innerFlowMoney += tmpIncome;	
						heroInfor.investFlow += tmpIncome;
						heroInfor.totalPayment-=cardData.payment * this.castRate;
					}



					if (tmpIncome != 0)
					{
						var flowRecordVo = new InforRecordVo ();
						flowRecordVo.title = cardData.title;
						flowRecordVo.num = cardData.income;
						heroInfor.AddInnerFlowInfor (flowRecordVo);						
					}

                   
				}

				if (GameModel.GetInstance.isPlayNet == false)
				{
					var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
					if(null!=battleController)
					{
						if (PlayerManager.Instance.HostPlayerInfo.playerID == playerInfor.playerID) 
						{					
							battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
							battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
							battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,heroInfor.TargetIncome);
							battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
						}
						else
						{
							battleController.SetPersonInfor (heroInfor,turnIndex,false);
						}
					}
				}
			}

			return canGet;
		}

		/// <summary>
		/// Determines whether this instance has money buy craps. 是否有钱购买其它的筛子
		/// </summary>
		/// <returns><c>true</c> if this instance has money buy craps; otherwise, <c>false</c>.</returns>
		public bool HasMoneyBuyCraps()
		{
			//var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
            var heroInfor =this.playerInfor;// PlayerManager.Instance.Players[turnIndex];
			if (heroInfor.totalMoney + cardData.payment * this.castRate <0)
			{
				Console.WriteLine ("余额不足了");
				if (PlayerManager.Instance.HostPlayerInfo.playerID == playerInfor.playerID)
				{
					MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
				}
				return false;
			}
			return true;
		}

        public bool RobotJudge()
        {
            if(this.castRate<=1)
            {
                return true;
            }
            else
            {
                if(playerInfor.totalMoney>Math.Abs(cardData.payment) * this.castRate *2)
                {
                    return true;
                }
            }
            return false;
        }

        public int crapNum = 0;

        /// <summary>
        /// 卡牌信息
        /// </summary>
        public Investment cardData;	

        /// <summary>
        /// 花费的钱的倍率，轮到自己倍率是1，否则是2
        /// </summary>
        public float castRate=1;

        /// <summary>
        /// 玩家信息
        /// </summary>
        public PlayerInfo playerInfor;

       
        
		public override void Tick(float deltaTime)
		{
			var window = _window as UIInvestmentCardWindow;
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
    }
}

