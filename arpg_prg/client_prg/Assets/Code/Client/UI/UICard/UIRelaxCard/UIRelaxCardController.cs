using System;
using Metadata;
using UnityEngine;
namespace Client.UI
{
	public class UIRelaxCardController:UIController<UIRelaxCardWindow,UIRelaxCardController>
	{
		/// 窗口预设，独立使用
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/relaxcard.ab";
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
				CardManager.Instance.NetQuitCard (cardData.id,(int) SpecialCardType.richRelax);
			}
		}

		/// <summary>
		/// Nets the buy card. 网络版本购买
		/// </summary>
		public void NetBuybanCard()
		{
			if (null != cardData)
			{
				CardManager.Instance.NetBuyCard (Protocol.Game_BuyRelaxCard , cardData.id,(int)SpecialCardType.richRelax);
			}
		}

		public bool HandlerCardData()
		{
			var canGet = false;
			if(null !=cardData)
			{
				var heroTurn=Client.Unit.BattleController.Instance.CurrentPlayerIndex;
				var heroInfor=PlayerManager.Instance.Players[heroTurn];

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

					heroInfor.PlayerIntegral += cardData.rankScore; 

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment;
						heroInfor.innerFlowMoney += cardData.income;
						heroInfor.timeScore += cardData.timeScore;
						heroInfor.totalPayment -= cardData.payment;
					}

					heroInfor.relaxFlow += cardData.income;
					heroInfor._richleisureNum += 1;

					if (cardData.timeScore != 0)
					{
						var timeRecord = new InforRecordVo ();
						timeRecord.title = cardData.title;
						timeRecord.num = cardData.timeScore;
						heroInfor.AddTimeScoreInfor (timeRecord);
					}

					if (cardData.income != 0)
					{
						var flowRecordVo = new InforRecordVo ();
						flowRecordVo.title = cardData.title;
						flowRecordVo.num = cardData.income;
						heroInfor.AddInnerFlowInfor (flowRecordVo);						
					}

                    if(PlayerManager.Instance.IsHostPlayerTurn()==false)
                    {
                        if (cardData.timeScore > 0)
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.timeAndMoneyGateMoneyAndScore4, heroInfor.playerName, cardData.title, (-cardData.payment).ToString(), cardData.income.ToString(), cardData.timeScore.ToString()), null, true);
                        }
                        else
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.timeAndMondeyGetMoney3, heroInfor.playerName, cardData.title, (-cardData.payment).ToString(), cardData.income.ToString()), null, true);
                        }
                    }
                    else
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overInnerRelax);
                    }				
				}

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
						battleController.SetPersonInfor (heroInfor,heroTurn);
					}
				}
			}
			return canGet;
		}

		public Relax cardData;

		public override void Tick(float deltaTime)
		{
			var window = _window as UIRelaxCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}
	}
}

