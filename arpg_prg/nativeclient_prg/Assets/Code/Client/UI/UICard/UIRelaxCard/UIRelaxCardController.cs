using System;
using Metadata;
using UnityEngine;
namespace Client.UI
{
    /// <summary>
    /// 有钱有闲卡牌
    /// </summary>
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
            var heroInfor =playerInfor;// PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
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

            if(normalQuit()==false)
            {
                playerInfor.Settlement._relaxIntegral += cardData.rankScore;
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

        /// <summary>
        /// 是否可以正常放弃卡牌
        /// </summary>
        /// <returns></returns>
        private bool normalQuit()
        {
            var normal = false;

            if(playerInfor.totalMoney + cardData.payment<0)
            {
                normal = true;
            }
            else
            {
                if(playerInfor.CurrentIncome>playerInfor.TargetIncome ||playerInfor.timeScore>playerInfor.targetTimeScore)
                {
                    normal = true;
                }
            }
            return normal;
        }

		public bool HandlerCardData()
		{
			var canGet = false;
			if(null !=cardData)
			{
                var heroTurn = Array.IndexOf(PlayerManager.Instance.Players, playerInfor); //Client.Unit.BattleController.Instance.CurrentPlayerIndex;

                var heroInfor=this.playerInfor;

				if (heroInfor.totalMoney + cardData.payment*castRate < 0)
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
					
					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment*castRate;
						heroInfor.innerFlowMoney += cardData.income;
						heroInfor.timeScore += cardData.timeScore;
						heroInfor.totalPayment -= cardData.payment*castRate;
					}

					heroInfor.relaxFlow += cardData.income;

                    heroInfor.Settlement._richleisureNum += 1;
                    heroInfor.PlayerIntegral += cardData.rankScore;
                    heroInfor.Settlement._relaxIntegral += cardData.rankScore;

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

                    //if (cardData.timeScore > 0)
                    //{
                    //	MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.timeAndMoneyGateMoneyAndScore4,heroInfor.playerName,cardData.title,(-cardData.payment).ToString(),cardData.income.ToString(),cardData.timeScore.ToString()),null,true);
                    //}
                    //else
                    //{
                    //	MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.timeAndMondeyGetMoney3,heroInfor.playerName,cardData.title,(-cardData.payment).ToString(),cardData.income.ToString()),null,true);
                    //}
                    if (PlayerManager.Instance.HostPlayerInfo.playerID != playerInfor.playerID)
                    {
                        //if (cardData.timeScore > 0)
                        //{
                        //    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.timeAndMoneyGateMoneyAndScore4, heroInfor.playerName, cardData.title, (-cardData.payment*castRate).ToString(), cardData.income.ToString(), cardData.timeScore.ToString()), null, true);
                        //}
                        //else
                        //{
                        //    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.timeAndMondeyGetMoney3, heroInfor.playerName, cardData.title, (-cardData.payment*castRate).ToString(), cardData.income.ToString()), null, true);
                        //}
                    }
                    else
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overInnerRelax);
                    }
                }

				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
				if(null!=battleController)
				{
					if (PlayerManager.Instance.HostPlayerInfo.playerID==playerInfor.playerID) 
				    {					
						battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
					}	
					else
					{
						battleController.SetPersonInfor (heroInfor,heroTurn,false);
					}
				}
			}
			return canGet;
		}

        /// <summary>
        /// 卡牌的数据
        /// </summary>
		public Relax cardData
        {
            get
            {
                return this._cardData;
            }
            set
            {
                this._cardData = value;
                //Console.Error.WriteLine("当前卡牌的花费"+cardData.payment);
                //var tmpCost = _cardData.payment;
                //this._cardData.payment *= _cardData.payment * this.castRate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotJudge()
        {
            if(castRate<=1)
            {
                return true;
            }
            else
            {
                if(playerInfor.totalMoney>Math.Abs(cardData.payment)*castRate *2)
                {
                    return true;
                }
            }

            return false;
        }
        

        private Relax _cardData;

        /// <summary>
        /// 玩家信息
        /// </summary>
        public PlayerInfo playerInfor;

        /// <summary>
        /// 花费金币的倍率，如果自己触发卡牌，倍率是1，如果是其他的话，自己要花费双倍的金币
        /// </summary>
        public float castRate=1;

		public override void Tick(float deltaTime)
		{
			var window = _window as UIRelaxCardWindow;
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

