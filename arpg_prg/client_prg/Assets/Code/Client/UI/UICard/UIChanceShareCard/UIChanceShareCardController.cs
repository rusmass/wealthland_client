using System;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
	public class UIChanceShareCardController:UIController<UIChanceShareCardWindow,UIChanceShareCardController>
	{
		/// <summary>
		/// 窗口资源  独立使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/chanceshares.ab";
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
			var heroInfor = playerInfor;
			if (null != cardData)
			{
				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.quitChanceCard2,heroInfor.playerName,cardData.title),null,true);
			}
//			NetQuitGame ();
		}

		public void NetQuitGame()
		{
			if (GameModel.GetInstance.isPlayNet == true)
			{
				if (null != cardData)
				{
					CardManager.Instance.NetQuitCard ( cardData.id,(int) SpecialCardType.sharesChance);
//					NetWorkScript.getInstance ().QuitCard (GameModel.GetInstance.curRoomId,);
				}
			}
		}

		/// <summary>
		/// Nets the buy card. 购买股票卡牌
		/// </summary>
		/// <param name="nums">Nums.</param>
		public void NetBuyCard(int nums=1)
		{
			if (null != cardData)
			{
				if (GameModel.GetInstance.isReconnecToGame == 1)
				{
					GameModel.GetInstance.isReconnecToGame = 2;
					NetWorkScript.getInstance ().AgreeToReConnetGame (GameModel.GetInstance.curRoomId,1);
				}

				CardManager.Instance.NetBuyCard (Protocol.Game_BuyChanceShareCard , cardData.id, (int)SpecialCardType.sharesChance, nums);
			}
		}

		public void NetSaleCard(List<NetSaleCardVo> _list)
		{
			if (null != cardData)
			{
				CardManager.Instance.NetSaleChanceShareCard (cardData.id, (int)SpecialCardType.sharesChance, _list);
			}
		}

		// 遍历提示
		public bool IsNonToSale()
		{
			var _isNonToSale = true;

			for(var i=0;i<_changeVoList.Count;i++)
			{
				var valueVo = _changeVoList [i];
				if (valueVo.changeNum != 0)
				{
					_isNonToSale = false;
					break;
				}
			}
			return _isNonToSale;
		}

		// 是否有足够的钱购买
		public bool IsCanBuyShare(ChanceShares shares,int num)
		{
			var canbuy = true;
			var heroInfor=playerInfor;
			var tmpMoney=0f;
			for (var i = 0; i < _changeVoList.Count; i++)
			{
				var tmpVo=_changeVoList[i];
				if(tmpVo.shareData.id==shares.id)
				{
					tmpMoney += num * tmpVo.shareData.payment;
				}
				else
				{
					tmpMoney += tmpVo.changeNum * tmpVo.shareData.payment;
				}

			}

			if (heroInfor.totalMoney + tmpMoney < 0)
			{
				canbuy = false;
				if (PlayerManager.Instance.IsHostPlayerTurn () == true)
				{
					MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
				}

			}

			return canbuy;
		}

		public void HandlerChangeCardData(ChangeShareVo valuess)
		{
			_netSaleList.Clear();
			var heroInfor=playerInfor;
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;

			heroInfor.PlayerIntegral += cardData.rankScore;

            if (playerInfor.playerID == PlayerManager.Instance.HostPlayerInfo.playerID)
            {
                if (_isBuyShare == false)
                {
                    MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardSellShare);
                }
                else
                {
                    MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardSmallShare);
                }
            }

            

            

            for (var k = 0; k < _changeVoList.Count; k++)
			{
				var isAddCard = true;
				var value=_changeVoList[k];

				if (GameModel.GetInstance.isPlayNet == false)
				{
					heroInfor.totalMoney += value.changeMoney;
				}

				value.shareData.shareNum += value.changeNum;
				if (value.shareData.shareNum <= 0)
				{
					value.shareData.shareNum = 0;
					isAddCard = false;
				}

				for (var i =heroInfor.shareCardList.Count-1; i >=0 ; i--)
				{
					var tmpValue=heroInfor.shareCardList[i];

					if (tmpValue.id == value.shareData.id)
					{
						if (_isBuyShare == false)
						{
							tmpValue.shareNum = value.shareData.shareNum;
							if (GameModel.GetInstance.isPlayNet == true)
							{
								var tmpsale = new NetSaleCardVo ();
								tmpsale.cardId = tmpValue.id;
								tmpsale.cardNumber = Math.Abs (value.changeNum);
								tmpsale.cardType = (int)SpecialCardType.sharesChance;
								_netSaleList.Add (tmpsale);                                
							}
                        } 
						else
						{
							tmpValue.shareNum += value.shareData.shareNum;
						}

						if (tmpValue.shareNum <= 0) 
						{
							
							if (GameModel.GetInstance.isPlayNet == false)
							{
								heroInfor.shareCardList.Remove (tmpValue);
								heroInfor.totalIncome += value.changeNum * tmpValue.income;
							}
						}

						// 记录卖股票记录
						if (_isBuyShare == false)
						{
							var tmpVo = new SaleRecordVo ();
							tmpVo.title = tmpValue.title;
							tmpVo.price = -tmpValue.payment;
							tmpVo.number = value.changeNum;
							tmpVo.mortage = -1;
							tmpVo.saleMoney = -cardData.payment;
							tmpVo.income = tmpValue.income;
							tmpVo.quality = tmpValue.qualityScore;
							tmpVo.getMoney = ( cardData.payment -tmpValue.payment) * value.changeNum;
							playerInfor.saleRecordList.Add (tmpVo);
							playerInfor._saleNums += 1;
						}

						if (GameModel.GetInstance.isPlayNet == false)
						{
							heroInfor.qualityScore += value.changeNum * value.shareData.qualityScore;
						}

						if (cardData.qualityScore != 0)
						{
							var recordInfor = new InforRecordVo ();
							recordInfor.title =value.shareData.title ;
							recordInfor.num = value.changeNum * value.shareData.qualityScore;;
							heroInfor.AddQualityScoreInfor (recordInfor);
						}

						isAddCard = false;
						break;
					}
				}

				if(isAddCard==true)
				{

					if (GameModel.GetInstance.isPlayNet == false)
					{
						heroInfor.shareCardList.Add (value.shareData);

						heroInfor.totalIncome += value.changeNum * value.shareData.income;
						heroInfor.qualityScore += value.changeNum * value.shareData.qualityScore;

					}

					if (cardData.qualityScore != 0)
					{
						var recordInfor = new InforRecordVo ();
						recordInfor.title =value.shareData.title ;
						recordInfor.num = value.changeNum * value.shareData.qualityScore;
						heroInfor.AddQualityScoreInfor (recordInfor);
					}
				}
			}


			if (GameModel.GetInstance.isPlayNet == false)
			{
				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
				if(null!=battleController)
				{
					if (PlayerManager.Instance.HostPlayerInfo.playerID == heroInfor.playerID) 
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

		public bool HasSameTypeShare()
		{
//			var heroInfor=PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
			var heroInfor=playerInfor;
			var hasShare = false;

			for (var i = 0; i < heroInfor.shareCardList.Count; i++)
			{
				var tmpvalue=heroInfor.shareCardList[i];


				if(_carddata.ticketCode==tmpvalue.ticketCode)
				{
					hasShare = true;
					break;
				}
			}

			return hasShare;
		}

		public bool HandlerCardData()
		{
			var canGet = false;
			if (null != cardData) 
			{
				
				var heroInfor=playerInfor;
				var turnIndex = 0;

				for (var i = 0; i < PlayerManager.Instance.Players.Length; i++)
				{
					if (heroInfor.playerID == PlayerManager.Instance.Players [i].playerID)
					{
						turnIndex = i;
						Console.WriteLine ("当前人物的索引值，"+i.ToString());
						break;
					}
				}

				var tmpShareNumbers =  cardData.shareNum ;

				if (PlayerManager.Instance.IsHostPlayerTurn () == false)
				{
					tmpShareNumbers = 1;
				}

				if(heroInfor.totalMoney + cardData.payment* tmpShareNumbers <0)
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

					MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.buyChanceCard2,heroInfor.playerName,cardData.title),null,true);

					heroInfor.totalMoney += cardData.payment * tmpShareNumbers;

					heroInfor.qualityScore+=cardData.qualityScore *tmpShareNumbers ;

					if (cardData.qualityScore != 0)
					{
						var recordInfor = new InforRecordVo ();
						recordInfor.title =cardData.title ;
						recordInfor.num = cardData.qualityScore * tmpShareNumbers;
						heroInfor.AddQualityScoreInfor (recordInfor);
					}

					heroInfor.totalPayment-=cardData.payment *tmpShareNumbers ;

					Console.WriteLine (string.Format("{0}的非劳务收入是{1}",heroInfor.playerName,heroInfor.totalIncome));

					heroInfor.totalIncome += cardData.income * tmpShareNumbers;
					heroInfor.sharesPayment -= cardData.payment *tmpShareNumbers ;

					Console.WriteLine (string.Format("{0}的非劳务收入是{1}",heroInfor.playerName,heroInfor.totalIncome));

					var addNewShare = true;
					for (var i = 0; i < heroInfor.shareCardList.Count; i++)
					{
						var tmpShare=heroInfor.shareCardList[i];
						if(tmpShare.id==cardData.id)
						{
							addNewShare = false;
							tmpShare.shareNum += tmpShareNumbers;
							break;
						}
					}
					if(addNewShare==true)
					{						
						cardData.shareNum = tmpShareNumbers;
						heroInfor.shareCardList.Add (cardData);	
					}						

					if (heroInfor.qualityScore > 0)
					{						
//						MessageHint.Show (string.Concat(heroInfor.playerName,"获得品质积分",cardData.qualityScore));

					}

					heroInfor.AddCapticalData ();
					heroInfor._smallOpportunitiesNum += 1;
				}


				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();					
				if(null!=battleController)
				{
					if (PlayerManager.Instance.HostPlayerInfo.playerID == heroInfor.playerID) 
					{
						battleController.SetQualityScore ((int)heroInfor.qualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.totalIncome,(int)heroInfor.MonthPayment);
						battleController.SetCashFlow ((int)heroInfor.totalMoney);
					}
					else 
					{
						battleController.SetPersonInfor (heroInfor,turnIndex,false);
					}
				}
			}

			return canGet;
		}


		public List<ChangeShareVo> GetDataList()
		{
//			//测试用数据
//			var template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
//			var it = template.GetEnumerator ();
//			var _shares = new List<ChangeShareVo> ();
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as ChanceShares;
//				var changeVo = new ChangeShareVo ();
//				changeVo.shareData = value;
//				_shares.Add (changeVo);
//				//SetSharesData (_lbsahres,value);			
//			}
//			return _shares;
			return _changeVoList;
		}

		public ChangeShareVo GetValueByIndex(int index)
		{
//			//测试用数据
//			var template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
//			var it = template.GetEnumerator ();
//			var _shares = new List<ChangeShareVo> ();
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as ChanceShares;
//				var changeVo = new ChangeShareVo ();
//				changeVo.shareData = value;
//				_shares.Add (changeVo);
//				//SetSharesData (_lbsahres,value);			
//			}
			var values = _changeVoList;

			if (null != values && index < values.Count)
			{
				return values[index];
			}

			return null;
		}
	

		// 初始话 股票的信息
		private void _InitChangeShareData()
		{
			_changeVoList.Clear ();

			var tmpChange = new ChangeShareVo ();
			var heroInfor=playerInfor;

			//			var template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
			//			var it = template.GetEnumerator ();
			//			var _shares = new List<ChangeShareVo> ();
			//			while (it.MoveNext ()) 
			//			{				
			//				var value = it.Current.Value as ChanceShares;
			//				var changeVo = new ChangeShareVo ();
			//				changeVo.shareData = value;
			//				_shares.Add (changeVo);
			//			}

			for (var i = 0; i < heroInfor.shareCardList.Count; i++)
			{
				var tmpvalue=heroInfor.shareCardList[i];
				if(_carddata.ticketCode==tmpvalue.ticketCode)
				{
					tmpChange = new ChangeShareVo ();
//					tmpvalue.payment = _carddata.payment;
					tmpChange.saleMoney = _carddata.payment;
					tmpChange.shareData = tmpvalue;
					_changeVoList.Add (tmpChange);
				}
			}
		}


		public void IsBuyShare(bool value)
		{
			_isBuyShare = value;

			if (value == false)
			{
				_InitChangeShareData();
			}
			else
			{
				_changeVoList.Clear ();

				var tmpChange = new ChangeShareVo ();

				var tmpShareData = new ChanceShares ();
				tmpShareData.belongsTo = _carddata.belongsTo;
				tmpShareData.cardPath = _carddata.cardPath;
				tmpShareData.desc = _carddata.desc;
				tmpShareData.id = _carddata.id;
				tmpShareData.income = _carddata.income;
				tmpShareData.payment = _carddata.payment;
				tmpShareData.priceRagne = _carddata.priceRagne;
				tmpShareData.qualityDesc = _carddata.qualityDesc;
				tmpShareData.qualityScore = _carddata.qualityScore;
				tmpShareData.returnRate = _carddata.returnRate;
				tmpShareData.shareNum = 0;
				tmpShareData.shareOut = _carddata.shareOut;
				tmpShareData.ticketCode = _carddata.ticketCode;
				tmpShareData.ticketName = _carddata.ticketName;
				tmpShareData.title = _carddata.title;


				tmpChange.shareData = tmpShareData;
				_changeVoList.Add (tmpChange);
			}

		}

		/// <summary>
		/// Updates the player money. 刷新人物金币
		/// </summary>
		public void UpdatePlayerMoney()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIChanceShareCardWindow).updatePlayerMoney ();
			}
		}
	
		public override void Tick(float deltaTime)
		{
			var window = _window as UIChanceShareCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

		private bool _isBuyShare=false;

		private List<ChangeShareVo> _changeVoList = new List<ChangeShareVo> ();
		private ChanceShares _carddata;
		public PlayerInfo playerInfor;

		public List<NetSaleCardVo> _netSaleList = new List<NetSaleCardVo> ();

		public ChanceShares cardData
		{
			get
			{
				return _carddata;
			}
			set
			{
				_carddata = value;
			}
		}
	}
}

