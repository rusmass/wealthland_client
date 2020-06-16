using System;
using Metadata;
namespace Client.UI
{
	public class UIQualityLifeCardController:UIController<UIQualityLifeCardWindow,UIQualityLifeCardController>
	{
		/// <summary>
		/// 窗口预设 独立使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/qualitylifecard.ab";
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
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.qualityLife);
			}
		}

		/// <summary>
		/// Nets the buy card.  购买卡牌
		/// </summary>
		public void NetBuyCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyQualityCard , cardData.id,(int)SpecialCardType.qualityLife);
			}
		}

		public int HandlerCardData()
		{
			var canGet = 0;
			if(null != cardData)
			{
				var heroTurn = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[heroTurn];

				if (heroInfor.totalMoney + cardData.payment < 0)
                {
					if (PlayerManager.Instance.IsHostPlayerTurn () == true)
					{
						MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);			
					}

					return canGet;
				} 
				else if(heroInfor.timeScore + cardData.timeScore<0)
				{
					if (PlayerManager.Instance.IsHostPlayerTurn () == true)
					{
						MessageHint.Show(SubTitleManager.Instance.subtitle.lackOfTimeScore);
					}
			
					canGet = -1;
					return canGet;
				}
				else
				{
					
					canGet = 1;

					heroInfor.PlayerIntegral += cardData.rankScore; 

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment;				
						heroInfor.qualityScore += cardData.qualityScore;
						heroInfor.timeScore += cardData.timeScore;
						heroInfor.totalPayment -= cardData.payment;
					}


					if (cardData.timeScore != 0)
					{
						var timeRecord = new InforRecordVo ();
						timeRecord.title = cardData.title;
						timeRecord.num = cardData.timeScore;
						heroInfor.AddTimeScoreInfor (timeRecord);
					}

					if (cardData.qualityScore != 0)
					{
						var recordInfor = new InforRecordVo ();
						recordInfor.title = cardData.title;
						recordInfor.num = cardData.qualityScore;
						heroInfor.AddQualityScoreInfor (recordInfor);
					}
					heroInfor._qualityNum += 1;

                    if(PlayerManager.Instance.IsHostPlayerTurn())
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overInnerQuality);
                    }
                    else
                    {
                        MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.qualityGetSocre4, heroInfor.playerName, cardData.title, (-cardData.payment).ToString(), cardData.timeScore.ToString(), cardData.qualityScore.ToString()), null, true);

                    }
                }


				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
				if(null!=battleController)
				{
					if (PlayerManager.Instance.IsHostPlayerTurn ())
					{	
						battleController.SetQualityScore ((int)heroInfor.qualityScore, heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore, heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome, heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney, heroInfor.TargetIncome);
					}
					else
					{
						battleController.SetPersonInfor (heroInfor,heroTurn);
					}
				}

			}

			return canGet;
		}

		public override void Tick(float deltaTime)
		{
			var window = _window as UIQualityLifeCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

		public QualityLife cardData;


	}
}

