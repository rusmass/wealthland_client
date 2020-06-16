using System;
using Metadata;

namespace Client.UI
{
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
			var heroInfor=PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			if (null != cardData)
			{
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.quitChanceCard2,heroInfor.playerName,cardData.title),null,true);
			}
		}

		public void NetQuitCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.investment);
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
		/// Handlers the card data.  如果遇到掷色子的 如果是npc随机判断下数字 处理大于小于的逻辑     本人的，点击确定，掷色子，判断得分
		/// </summary>
		/// <returns><c>true</c>, if card data was handlered, <c>false</c> otherwise.</returns>
		public bool HandlerCardData()
		{
			var canGet = false;
			if (null != cardData) 
			{
				var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[turnIndex];

				if (heroInfor.totalMoney + cardData.payment < 0)
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

					if (PlayerManager.Instance.IsHostPlayerTurn () == false)
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
                        if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                        {
                            if(GameModel.GetInstance.isPlayNet==false)
                            {
                                if (isRollSuccess == true)
                                {
                                    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.rollSuccessInvestment, heroInfor.playerName, crapNum.ToString(), cardData.income.ToString()));
                                }
                                else
                                {
                                    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.rollFail, heroInfor.playerName, crapNum.ToString()));
                                }
                            }                          
                        }
                        else
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overInnerInvestment);
                        }
                    }
					else
					{
						tmpIncome = cardData.income;

						if (PlayerManager.Instance.IsHostPlayerTurn() == false)
						{
							MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.investmentGetMoney3,heroInfor.playerName,cardData.title,tmpIncome.ToString(),(-cardData.payment).ToString()),null,true);

						}
                        else
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overInnerInvestment);
                        }
					}

					heroInfor.PlayerIntegral += cardData.rankScore; 

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment;
						heroInfor.innerFlowMoney += tmpIncome;	
						heroInfor.investFlow += tmpIncome;
						heroInfor.totalPayment-=cardData.payment;
					}



					if (tmpIncome != 0)
					{
						var flowRecordVo = new InforRecordVo ();
						flowRecordVo.title = cardData.title;
						flowRecordVo.num = cardData.income;
						heroInfor.AddInnerFlowInfor (flowRecordVo);						
					}

					heroInfor._investmentNum += 1;
				}

				if (GameModel.GetInstance.isPlayNet == false)
				{
					var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
					if(null!=battleController)
					{
						if (PlayerManager.Instance.IsHostPlayerTurn ()) 
						{					
							battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
							battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
							battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,heroInfor.TargetIncome);
							battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
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

		/// <summary>
		/// Determines whether this instance has money buy craps. 是否有钱购买其它的筛子
		/// </summary>
		/// <returns><c>true</c> if this instance has money buy craps; otherwise, <c>false</c>.</returns>
		public bool HasMoneyBuyCraps()
		{
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
			var heroInfor=PlayerManager.Instance.Players[turnIndex];
			if (heroInfor.totalMoney + cardData.payment<0)
			{
				Console.WriteLine ("余额不足了");
				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
				{
					MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
				}
				return false;
			}
			return true;
		}


		public Investment cardData;
		public int crapNum=0;

		public override void Tick(float deltaTime)
		{
			var window = _window as UIInvestmentCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}
	}
}

