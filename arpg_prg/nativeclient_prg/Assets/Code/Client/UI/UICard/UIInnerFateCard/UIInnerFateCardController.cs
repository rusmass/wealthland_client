using System;
using Metadata;


namespace Client.UI
{
    /// <summary>
    /// 内圈命运卡牌
    /// </summary>
	public class UIInnerFateCardController:UIController<UIInnerFateCardWindow,UIInnerFateCardController>
	{
		/// <summary>
		/// 窗口预设  在  重复使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get 
			{
				return "prefabs/ui/scene/innerfate.ab";				
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
				CardManager.Instance.NetQuitCard (cardData.id,(int) SpecialCardType.inFate);
			}
		}

		public void NetBuyCard(int rollNum=0)
		{
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyInnerFateCard , cardData.id,(int)SpecialCardType.inFate,1,rollNum);
			}
		}

		public bool HandlerCardData()
		{
			var canHandle = false;
			if (null != cardData) 
			{
				var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[turnIndex];

                var isHostTurn = PlayerManager.Instance.IsHostPlayerTurn();

				if (cardData.fateType == 1) 
				{
					if (heroInfor.totalMoney + cardData.paymeny<0)
					{
						Console.WriteLine ("余额不足了");

						if (isHostTurn == true)
						{
							MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
						}					
						canHandle = false;
						return canHandle;
					}

					var tmppayment=cardData.paymeny;
					if (cardData.paymenyMethod == 1) 
					{						
						tmppayment = cardData.paymeny;
					}
					else if(cardData.paymenyMethod==2)
					{						
						tmppayment =-heroInfor.totalMoney * cardData.paymeny;
					}

					heroInfor.PlayerIntegral += cardData.rankScore;
                    heroInfor.Settlement._innerFateIntegral += cardData.rankScore;

					if (isHostTurn == false)
					{
						crapNum = UnityEngine.Random.Range (1,6);
						if (GameModel.GetInstance.isPlayNet == true)
						{
							crapNum = GameModel.GetInstance.innerCardRollPoint;
						}
					}

					var isRollSuccess = false;

					if (cardData.dice_condition == 1)
					{
						if (crapNum >= cardData.dice_number)
						{
							tmppayment += cardData.dice_prise;
							isRollSuccess = true;
						}
					}
					else if(cardData.dice_condition ==2)
					{
						if (crapNum <= cardData.dice_number)
						{
							tmppayment += cardData.dice_prise;
							isRollSuccess = true;
						}
					}

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += tmppayment;
						heroInfor.totalPayment -= tmppayment;
					}
				
					canHandle = true;

					if (isRollSuccess == true)
					{
						MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.rollSuccessInnerfate,heroInfor.playerName,crapNum.ToString(),cardData.dice_prise.ToString()));
					}
					else
					{
						MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.rollFail,heroInfor.playerName,crapNum.ToString()));
					}
                    //MessageHint.Show (string.Concat( heroInfor.playerName,"失去金币",tmppayment));										
				}
				else if (cardData.fateType==2)
				{
					//保险
					if (heroInfor.totalMoney + cardData.paymeny<0)
					{
						Console.WriteLine ("余额不足了");
						if (isHostTurn == true)
						{
							MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
						}

						canHandle = false;
						return canHandle;
					}
					else
					{
						heroInfor.PlayerIntegral += cardData.rankScore;

                        heroInfor.Settlement._innerFateIntegral += cardData.rankScore;

						if (GameModel.GetInstance.isPlayNet == false)
						{
							heroInfor.totalMoney+=cardData.paymeny;
							heroInfor.totalPayment -= cardData.paymeny;
						}
                        if (isHostTurn == true)
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overInnerFate);
                        }
                        else
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.innerFateSafe2, heroInfor.playerName, cardData.title), null, true);

                        }
                        Console.WriteLine ("失去钱",cardData.paymeny);
						canHandle = true;						
                        heroInfor.InsuranceList.Add (cardData.id);
					}
				}
				else if(cardData.fateType==3)
				{
					//赔付金额

					if (heroInfor.InsuranceList.Count > 0) 
					{
						heroInfor.InsuranceList.RemoveAt (0);

                        if(isHostTurn)
                        {
                            MessageTips.Show(GameTipManager.Instance.GetRandomCare());
                        }
						//MessageHint.Show (string.Concat(heroInfor.playerName,"因为保险避免了损失"),null,true);
						canHandle = true;
						return canHandle;
					}

					if (heroInfor.totalMoney <= 0)
					{
						return true;
					}

					var tmppayment=cardData.paymeny;
					if (cardData.paymenyMethod == 1) 
					{						
						tmppayment = cardData.paymeny;
					}
					else if(cardData.paymenyMethod==2)
					{
						var tmpfix = cardData.paymeny;
						//离婚
						if (cardData.id == 90006)
						{
							if (heroInfor.playerSex == 1)
							{
								tmpfix = 1;
							}
							else
							{
								tmpfix = 0.5f;
							}

                            heroInfor.Settlement._divorceNum += 1;
						}												
						tmppayment =-heroInfor.totalMoney *tmpfix ;
					}

					if (heroInfor.totalMoney + tmppayment < 0)
					{
						Console.WriteLine ("余额不足了");
						canHandle = false;
						return canHandle;
					}
					else
					{
						if(cardData.id == 90009)
						{
                            // 审计
                            heroInfor.Settlement._auditNum += 1;
						}

						if (cardData.id == 90004)
						{
                            heroInfor.Settlement._moneyLoss += 1;
						}

						heroInfor.PlayerIntegral += cardData.rankScore;
                        heroInfor.Settlement._innerFateIntegral += cardData.rankScore;

						if (GameModel.GetInstance.isPlayNet == false)
						{
							heroInfor.totalMoney += tmppayment;
							heroInfor.totalPayment -= tmppayment;
							Console.WriteLine ("失去钱",cardData.paymeny);	
						}					
						canHandle = true;
						//MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.innerFateLose2,heroInfor.playerName,cardData.title),null,true);
                        if(isHostTurn==true)
                        {
                            MessageTips.Show(GameTipManager.Instance.GetRandomInnerFate());
                        }
                    }
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
							battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,(int)heroInfor.TargetIncome);
							battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
						}
						else 
						{
							battleController.SetPersonInfor (heroInfor,turnIndex);
						}
					}
				}			
			}
			return canHandle;
		}


		public bool HasMoneyBuyCraps()
		{
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
			var heroInfor=PlayerManager.Instance.Players[turnIndex];
			if (heroInfor.totalMoney + cardData.paymeny<0)
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


		public override void Tick(float deltaTime)
		{
			var window = _window as UIInnerFateCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

		public InnerFate cardData;
		public int crapNum=1;


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

