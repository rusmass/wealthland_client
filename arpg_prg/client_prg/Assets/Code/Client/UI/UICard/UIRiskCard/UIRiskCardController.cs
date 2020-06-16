using System;
using Metadata;
using UnityEngine;
namespace Client.UI
{
	public class UIRiskCardController:UIController<UIRiskCardWindow,UIRiskCardController>
	{
		/// <summary>
		/// 窗口预设，独立使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/riskcard.ab";
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

		/// <summary>
		/// Nets the buy card.
		/// </summary>
		public void NetBuyCard(int selectNum=0)
		{
			if (null != cardData)
			{
				if (isSlect == true)
				{
					selectNum = 1;
				}
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyRiskCard , cardData.id,(int)SpecialCardType.risk,1,selectNum,isSlect);
			}
		}

		public void HandlerCardData()
		{			
			if (null != cardData) 
			{
				// 遇到风险，必定会扣钱的 ，如果钱不足，就不能后买自由选择项目
				var heroTurn = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[heroTurn];
				var tmppayment=cardData.payment;

//				if (isSlect == true) 
//				{					
					
//				}
//				if(heroInfor.totalMoney<0)
//				{					
//					MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
//				}		

				if (GameModel.GetInstance.isPlayNet == true)
				{
					if (PlayerManager.Instance.IsHostPlayerTurn () == false)
					{
						if (GameModel.GetInstance.innerCardRollPoint > 0)
						{
							isSlect = true;
						}
					}
				}


				if (isSlect == true )
				{
					if (heroInfor.totalMoney + tmppayment + cardData.payment2 >= 0)
					{
						tmppayment += cardData.payment2;

						if(cardData.score>0)
						{
							if (cardData.scoreType == (int)CardManager.ScoreType.TimeScore) 
							{
								heroInfor.timeScore += cardData.score;		

								if (cardData.score != 0)
								{
									var timeRecord = new InforRecordVo ();
									timeRecord.title = cardData.title;
									timeRecord.num = cardData.score;
									heroInfor.AddTimeScoreInfor (timeRecord);
								}
							}
							else
							{
								heroInfor.qualityScore += cardData.score;
								if (cardData.score != 0)
								{
									var recordInfor = new InforRecordVo ();
									recordInfor.title = cardData.title;
									recordInfor.num = cardData.score;
									heroInfor.AddQualityScoreInfor (recordInfor);
								}
							}						
						}				
					}
					else
					{
						MessageHint.Show (string.Format("{0}的金币不足，不能购买自由选择项",heroInfor.playerName));
					}

				}	

				heroInfor.PlayerIntegral += cardData.rankScore; 
				heroInfor.totalMoney += tmppayment;

                if(PlayerManager.Instance.IsHostPlayerTurn()==false)
                {
                    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.riskCoast3, heroInfor.playerName, cardData.title, (-tmppayment).ToString()), null, true);

                }
                else
                {
                    MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardRisk);
                }


                heroInfor.otherPayment += tmppayment;

				if (cardData.id == 10045)
				{
					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.isEmployment = false;
					}
					heroInfor._unemploymentNum += 1;
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
						battleController.SetPersonInfor (heroInfor,heroTurn);
					}
				}

			}
		}

		/// <summary>
		/// Ises the can select free. 是否是可以自由选择
		/// </summary>
		/// <returns><c>true</c>, if can select free was ised, <c>false</c> otherwise.</returns>
		public bool isCanSelectFree()
		{
			var canFree = false;
			if (null != cardData)
			{
				var heroTurn = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[heroTurn];
				if (heroInfor.totalMoney + cardData.payment + cardData.payment2 >= 0)
				{
					canFree = true;
				}
			}			
			return canFree;
		}


		public Boolean isSlect=false;
		public Risk cardData;

        public override void Tick(float deltaTime)
        {
            var window = _window as UIRiskCardWindow;
            if (null != window && getVisible())
            {
                window.Tick(deltaTime);
            }
        }
	}
}

