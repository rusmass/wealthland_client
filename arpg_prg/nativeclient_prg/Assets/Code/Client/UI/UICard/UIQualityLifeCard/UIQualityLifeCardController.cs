using System;
using Metadata;
namespace Client.UI
{
    /// <summary>
    /// 内圈品质生活卡牌
    /// </summary>
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
				CardManager.Instance.NetQuitCard (cardData.id,(int)SpecialCardType.qualityLife);
			}

            if(this.normalQuit()==false)
            {
                playerInfor.Settlement._qualityIntegral += cardData.quitScore;
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

        /// <summary>
        /// 是否是可以正常放弃
        /// </summary>
        /// <returns></returns>
        private bool normalQuit()
        {
            var normal = false;

            if((playerInfor.totalMoney + cardData.payment< 0) ||( playerInfor.timeScore + cardData.timeScore < 0))
            {
                normal = true;
            }
            else
            {
                if(playerInfor.qualityScore>playerInfor.targetQualityScore)
                {
                    normal = true;
                }
            }

            return normal;
        }

        /// <summary>
        /// 处理卡牌数据
        /// </summary>
        /// <returns></returns>
		public int HandlerCardData()
		{
			var canGet = 0;
			if(null != cardData)
			{
                var heroTurn =Array.IndexOf(PlayerManager.Instance.Players,playerInfor) ;// Client.Unit.BattleController.Instance.CurrentPlayerIndex;
                var heroInfor =playerInfor;// PlayerManager.Instance.Players[heroTurn];

				if (heroInfor.totalMoney + cardData.payment * this.castRate < 0) {
					if (PlayerManager.Instance.HostPlayerInfo.playerID== playerInfor.playerID)
					{
						MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);			
					}
					return canGet;
				} 
				else if(heroInfor.timeScore + cardData.timeScore<0)
				{
					if (PlayerManager.Instance.HostPlayerInfo.playerID == playerInfor.playerID)
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
                    heroInfor.Settlement._qualityNum += 1;
                    heroInfor.Settlement._qualityIntegral += cardData.rankScore;

                    if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.totalMoney += cardData.payment*this.castRate;				
						heroInfor.qualityScore += cardData.qualityScore;
						heroInfor.timeScore += cardData.timeScore;
						heroInfor.totalPayment -= cardData.payment*this.castRate;
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
                   
                    //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.qualityGetSocre4,heroInfor.playerName,cardData.title,(-cardData.payment).ToString(),cardData.timeScore.ToString(),cardData.qualityScore.ToString()),null,true);
                    if (PlayerManager.Instance.HostPlayerInfo.playerID==playerInfor.playerID)
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overInnerQuality);
                    }
                    else
                    {
                       // MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.qualityGetSocre4, heroInfor.playerName, cardData.title, (-cardData.payment*this.castRate).ToString(), cardData.timeScore.ToString(), cardData.qualityScore.ToString()), null, true);

                    }
                }
				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
				if(null!=battleController)
				{
					if (PlayerManager.Instance.HostPlayerInfo.playerID==playerInfor.playerID)
					{	
						battleController.SetQualityScore ((int)heroInfor.qualityScore, heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore, heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome, heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney, heroInfor.TargetIncome);
					}
					else
					{
						battleController.SetPersonInfor (heroInfor,heroTurn,false);
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

        /// <summary>
        /// 判断机器人是否是可以购买卡牌
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
                if((playerInfor.totalMoney>Math.Abs(cardData.payment)*castRate *2) &&( playerInfor.qualityScore<playerInfor.targetQualityScore))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 卡牌数据
        /// </summary>
		public QualityLife cardData;

        /// <summary>
        ///玩家信息数据
        /// </summary>
        public PlayerInfo playerInfor;
        /// <summary>
        /// 花费的金币的倍数，如果是自己出发的卡牌，倍数为1倍，否则倍数为两倍
        /// </summary>
        public float castRate=1;


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

