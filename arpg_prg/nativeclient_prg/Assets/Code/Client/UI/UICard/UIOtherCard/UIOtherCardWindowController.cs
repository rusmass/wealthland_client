﻿using System;
using Metadata;
using UnityEngine;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 特殊类型卡牌UI，生孩子、健康管理、进修学习，慈善事业卡牌
    /// </summary>
	public class UIOtherCardWindowController:UIController<UIOtherCardWindow,UIOtherCardWindowController>
	{
        /// <summary>
        /// 网络版提示话术
        /// </summary>
        public static HealthKnowledge netKnowledge = new HealthKnowledge();

		// 确定按钮的链接
		public static string imgSurePath="share/atlas/battle/newcard/queren.ab";

		/// <summary>
		/// 窗口预设和内圈命运使用相同
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get 
			{
				return "prefabs/ui/scene/uiothercard.ab";				
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


		public void NetQuitCard()
		{
			CardManager.Instance.NetQuitCard (_cardID, _cardID);
		}

		/// <summary>
		/// Nets the buy card. 网络版购买卡牌
		/// </summary>
		public void NetBuyCard()
		{			
//			if (_cardID == (int)SpecialCardType.CheckDayType || _cardID == (int)SpecialCardType.InnerCheckDayType)
//			{
//				NetWorkScript.getInstance ().Send_SingleRoundEnd (GameModel.GetInstance.curRoomId);
//			}
//			else
//			{
//				CardManager.Instance.NetBuyCard (portType,_cardID, _cardID, 1);
//			}
			CardManager.Instance.NetBuyCard (portType,_cardID, _cardID, 1);
		}

		public void HandlerCardData()
		{	
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;

			if ((int)SpecialCardType.GiveChildType == _cardID) 
			{
				var battleController = Client.UIControllerManager.Instance.GetController<UIBattleController> ();
				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (player.childNum >= player.childNumMax)
					{
						var paymoney = (int)(player.totalMoney* 0.1f);
						if (player.totalMoney <=0)
						{
							paymoney=0;					
						}
						player.totalMoney -= paymoney;
						player.totalPayment += paymoney;

                        //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.moreChildForTip,player.playerName,paymoney.ToString()),null,true);
                        if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.moreChildForTip, player.playerName, paymoney.ToString()), null, true);

                        }
                        else
                        {
                            MessageTips.Show(string.Format(GameTipManager.Instance.gameTips.overOuterMoreChild, paymoney.ToString()));
                        }
                        
                        if (null != battleController) 
						{
							if (PlayerManager.Instance.IsHostPlayerTurn ()) 
							{						
								battleController.SetCashFlow ((int)player.totalMoney,player.TargetIncome);
							}
							else
							{
								battleController.SetPersonInfor (player,turnIndex);
							}
						}
						return;
					}
				}

				player.childNum++;
				if (player.childNum > player.childNumMax)
				{
					player.childNum = player.childNumMax;
				}
				else
				{
                    //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.getChildDesc2,player.playerName,player.oneChildPrise),null,true);
                    if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                    {
                        MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.getChildDesc2, player.playerName, player.oneChildPrise), null, true);

                    }
                    else
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overOuterGiveChild);
                    }
                }
				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (null != battleController) 
					{
						if (PlayerManager.Instance.IsHostPlayerTurn ()) 
						{						
							battleController.SetCashFlow ((int)player.totalMoney,player.TargetIncome);
							battleController.SetNonLaberIncome ((int)player.totalIncome,(int)player.MonthPayment);
						}
						else
						{
							battleController.SetPersonInfor (player,turnIndex);
						}
					}
				}

			} 
			else if((int)SpecialCardType.CheckDayType == _cardID ||(int)SpecialCardType.InnerCheckDayType==_cardID)
			{				
				var checkoutMoney=(player.cashFlow + player.totalIncome + player.innerFlowMoney-player.MonthPayment) ;

				if (GameModel.GetInstance.isPlayNet == true)
				{
					checkoutMoney=player.netCheckDayNum;
				}

				player.totalMoney += checkoutMoney;
                //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.getCheckOutMoney2,player.playerName,checkoutMoney.ToString()),null,true);
                if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                {
                    MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.getCheckOutMoney2, player.playerName, checkoutMoney.ToString()), null, true);

                }
                else
                {
                    if ((int)SpecialCardType.CheckDayType == _cardID)
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardCheckOut);
                    }
                    else
                    {
                        MessageTips.Show(GameTipManager.Instance.gameTips.overInnerCheckOut);
                    }
                }

                player.checkOutCount++;
			
				var battleController = Client.UIControllerManager.Instance.GetController<UIBattleController> ();
				if (null != battleController) 
				{
					if(PlayerManager.Instance.IsHostPlayerTurn())
					{
						battleController.SetCashFlow ((int)player.totalMoney,player.TargetIncome);
					}
					else
					{
						battleController.SetPersonInfor (player,turnIndex,false);
					}
				}		
			}		
			else 			
			{
				var paymoney = (int)((player.cashFlow +player.totalIncome + player.innerFlowMoney) * 0.1f);

				if (player.totalMoney - paymoney > 0)
				{
					if (GameModel.GetInstance.isPlayNet == false)
					{
						player.totalMoney -= paymoney;
						player.totalPayment += paymoney;
					}
				

					if ((int)SpecialCardType.CharityType == _cardID)
					{
                        if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.involveCharity2, player.playerName, paymoney.ToString()), null, true);

                        }
                        else
                        {
                            MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardCharity);
                        }
                        player.Settlement._charityNum += 1;
					}
					else if((int)SpecialCardType.HealthType == _cardID||(int)SpecialCardType.InnerHealthType == _cardID)
					{
                        //MessageHint.Show (string.Format (SubTitleManager.Instance.subtitle.involveHealth2, player.playerName, paymoney.ToString ()),null,true);
                        if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.involveHealth2, player.playerName, paymoney.ToString()), null, true);

                        }
                        else
                        {
                            if ((int)SpecialCardType.HealthType == _cardID)
                            {
                                MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardHealth);
                            }
                            else
                            {
                                MessageTips.Show(GameTipManager.Instance.gameTips.overInnerHealth);
                            }                            
                        }
                        if (GameModel.GetInstance.isPlayNet == false)
                        {
                            if (player.isEnterInner == true)
                            {

                                player.timeScore += 10;
                            }
                        }
                        player.Settlement._healthNum += 1;
					}
					else if((int)SpecialCardType.StudyType == _cardID||(int)SpecialCardType.InnerStudyType == _cardID)
					{
                        //MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.involeStudy2,player.playerName,paymoney.ToString()),null,true);
                        if (PlayerManager.Instance.IsHostPlayerTurn() == false)
                        {
                            MessageHint.Show(string.Format(SubTitleManager.Instance.subtitle.involeStudy2, player.playerName, paymoney.ToString()), null, true);

                        }
                        else
                        {
                            if ((int)SpecialCardType.StudyType == _cardID)
                            {
                                MessageTips.Show(GameTipManager.Instance.gameTips.overOuterCardStudy);
                            }
                            else
                            {
                                MessageTips.Show(GameTipManager.Instance.gameTips.overInnerStudy);
                            }                           
                        }

                        if (GameModel.GetInstance.isPlayNet == false)
                        {
                            if (player.isEnterInner == true)
                            {
                                player.qualityScore += 5;
                            }
                        }

                        player.Settlement._learnNum += 1;
					}


					if (GameModel.GetInstance.isPlayNet == false)
					{
						var battleController = Client.UIControllerManager.Instance.GetController<UIBattleController> ();
						if (null != battleController) 
						{
							if (PlayerManager.Instance.IsHostPlayerTurn ()) 
							{						
								battleController.SetCashFlow ((int)player.totalMoney,player.TargetIncome);
                                if(player.isEnterInner)
                                {
                                    battleController.SetQualityScore((int)player.qualityScore, player.targetQualityScore);
                                    battleController.SetTimeScore((int)player.timeScore, player.targetTimeScore);
                                }
                               
							}
							else
							{
								battleController.SetPersonInfor (player,turnIndex);
							}
						}
						player.isThreeRoll = true;
					}

					Console.WriteLine ("ssdfenwdnfweofnsdofnsdf扔三个筛子,"+player.isThreeRoll.ToString());
				}
				else
				{
					if (PlayerManager.Instance.IsHostPlayerTurn () == true)
					{
						MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);
					}
				}
			}
		}

		public bool EnoughMoney()
		{
			var paymoney = (int)((player.cashFlow +player.totalIncome + player.innerFlowMoney) * 0.1f);
			return player.totalMoney >paymoney;
		}

		public void SetCardId(int id)
		{
			_cardID = id;

			var tmpCard = new SpecialCard();

			if (GameModel.GetInstance.isPlayNet == true)
			{
				tmpCard = GameModel.GetInstance.netSpecialCard;
			}
			else
			{
				var tmplate = MetadataManager.Instance.GetTemplateTable<SpecialCard> ();
				var it = tmplate.GetEnumerator ();
				while (it.MoveNext ()) 
				{
					var value = it.Current.Value as SpecialCard;
					if (value.id == id) 
					{
						tmpCard=value;
						//	isGetCard=true;
						break;
					}
				}
			}



			if ((int)SpecialCardType.CharityType==id)
			{
				// 慈善事业
				cardTitle = SubTitleManager.Instance.subtitle.cardCharity;
				cardTitlePath = CardTitlePath.Special_Charity;

				var payMoney = player.cashFlow + player.totalIncome + player.innerFlowMoney;

				cardInfor = string.Format(tmpCard.desc, HandleStringTool.HandleMoneyTostring((int)(payMoney * 0.1f)));
				cardPath = tmpCard.cardPath;

				portType = Protocol.Game_BuyCharityCard;
			}
			else if ((int)SpecialCardType.CheckDayType==id)
			{
				// 结账日

				cardTitle = tmpCard.title;
				cardTitlePath = CardTitlePath.Special_CheckDay;

				var checkoutMoney =(player.cashFlow + player.totalIncome + player.innerFlowMoney - player.MonthPayment);
//				var paymentmoney =HandleStringTool.HandleMoneyTostring( player.MonthPayment);
//				var incomeMoney = HandleStringTool.HandleMoneyTostring(player.cashFlow + player.totalIncome + player.innerFlowMoney);

				if (GameModel.GetInstance.isPlayNet == true)
				{
					checkoutMoney=player.netCheckDayNum;
				}

				var tmpcheckstr = HandleStringTool.HandleMoneyTostring (checkoutMoney);

				cardInfor =string.Format(tmpCard.desc,tmpcheckstr);
				cardPath = tmpCard.cardPath;

				portType = Protocol.Game_BuyCheckDayCard;
			}
			else if ((int)SpecialCardType.InnerCheckDayType==id)
			{
				cardTitle = tmpCard.title;
				cardTitlePath = CardTitlePath.Special_CheckDay;

				var checkoutMoney=(player.cashFlow + player.totalIncome + player.innerFlowMoney-player.MonthPayment) ;
				if (GameModel.GetInstance.isPlayNet == true)
				{
					checkoutMoney=player.netCheckDayNum;
				}

				var tmpcheckstr = HandleStringTool.HandleMoneyTostring (checkoutMoney);

				cardInfor =string.Format(tmpCard.desc,tmpcheckstr,tmpcheckstr);

				cardPath = tmpCard.cardPath;

				portType = Protocol.Game_BuyCheckDayCard;
			}
			else if ((int)SpecialCardType.GiveChildType==id)
			{
				// 生孩子
				cardTitle = SubTitleManager.Instance.subtitle.cardGetChild;
				cardTitlePath = CardTitlePath.Special_GiveChild;

				var tmpChildNum = player.childNum;

				if (GameModel.GetInstance.isPlayNet == false)
				{
					if(tmpChildNum>=player.childNumMax)
					{
						var tmpMoney = (int)(player.totalMoney * 0.1f);

						if (tmpMoney < 0)
						{
							tmpMoney = 0;
						}

						cardPath = tmpCard.cardPath;
						cardInfor =string.Format(SubTitleManager.Instance.subtitle.moreChildForBoard,player.childNumMax.ToString(),tmpMoney.ToString());
						return;
					}
				}
				else
				{
					if (GameModel.GetInstance.isGiveChild == 1)
					{
						var tmpMoney = (int)(player.totalMoney * 0.1f);
						if (tmpMoney < 0)
						{
							tmpMoney = 0;
						}
						cardPath = tmpCard.cardPath;
						cardInfor =string.Format(SubTitleManager.Instance.subtitle.moreChildForBoard,player.childNumMax.ToString(),tmpMoney.ToString());
						return;
					}
				}



				tmpChildNum+=1;

				if (tmpChildNum > player.childNumMax)
				{
					tmpChildNum = player.childNumMax;
				}
				cardInfor =string.Format(tmpCard.desc,tmpChildNum.ToString(),(tmpChildNum * player.oneChildPrise).ToString());
				cardPath = tmpCard.cardPath;

				portType = Protocol.Game_BuyGiveChildCard;
			}
			else if ((int)SpecialCardType.HealthType==id ||(int)SpecialCardType.InnerHealthType==id)
			{
				// 外圈健康管理 内圈健康管理
				cardTitle = SubTitleManager.Instance.subtitle.cardHealth;
				cardTitlePath = CardTitlePath.Special_Health;

				var payMoney = player.cashFlow + player.totalIncome + player.innerFlowMoney;
				cardInfor = string.Format(tmpCard.desc,HandleStringTool.HandleMoneyTostring(((int)(payMoney * 0.1f))));
				cardPath = tmpCard.cardPath;

				portType = Protocol.Game_BuyHealthCard;
			}
			else if ((int)SpecialCardType.InnerStudyType==id ||(int)SpecialCardType.StudyType==id)
			{
				// 进修学习 和外圈进修学习
				cardTitle = SubTitleManager.Instance.subtitle.cardStudy;
				cardTitlePath = CardTitlePath.Special_Study;

				var payMoney = player.cashFlow + player.totalIncome + player.innerFlowMoney;
				Console.WriteLine ("dddddddddddddd"+tmpCard.desc);
				cardInfor = string.Format(tmpCard.desc,HandleStringTool.HandleMoneyTostring((int)(payMoney * 0.1f)));
				cardPath = tmpCard.cardPath;
				portType = Protocol.Game_BuyStudyCard;
			}
		}

		public override void Tick(float deltaTime)
		{
			var window = _window as UIOtherCardWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}

        /// <summary>
        /// 是否会展示知识面板
        /// </summary>
        /// <returns></returns>
        public bool IsHasKnowledge()
        {
            var isknowledge = false;
            if(_cardID ==(int)SpecialCardType.CharityType ||_cardID==(int)SpecialCardType.HealthType|| _cardID == (int)SpecialCardType.InnerHealthType || _cardID == (int)SpecialCardType.StudyType|| _cardID == (int)SpecialCardType.InnerStudyType)
            {
                isknowledge = true;
            }
            return isknowledge;
        }


		private int _cardID;
		public int cardID
		{
			get
			{
				return _cardID;
			}
		}

        /// <summary>
        /// 获取健康知识点
        /// </summary>
        /// <returns></returns>
        public HealthKnowledge GetHealthKnowledge()
        {
            HealthKnowledge _healthData=new HealthKnowledge();
            if(GameModel.GetInstance.isPlayNet==true)
            {
                _healthData = UIOtherCardWindowController.netKnowledge;
                return _healthData;
            }


            if(null==_healthList)
            {
                _healthList = new List<HealthKnowledge>();
                var template = MetadataManager.Instance.GetTemplateTable<HealthKnowledge>();
                var it = template.GetEnumerator();
                while (it.MoveNext())
                {
                    var value = it.Current.Value as HealthKnowledge;                   
                    _healthList.Add(value);
                }
            }            
            _healthData =_healthList[MathUtility.Random(0,_healthList.Count-1)];
            return _healthData;
        }

        /// <summary>
        /// 学习知识点
        /// </summary>
        /// <returns></returns>
        public StudyKnowledge GetStudyKnowledge()
        {
            StudyKnowledge _studyknowledge =new StudyKnowledge();
            if(GameModel.GetInstance.isPlayNet==true)
            {
                _studyknowledge.title = UIOtherCardWindowController.netKnowledge.title;
                _studyknowledge.content = UIOtherCardWindowController.netKnowledge.content;
                return _studyknowledge;
            }


            if(_studyList==null)
            {
                _studyList = new List<StudyKnowledge>();
                var template = MetadataManager.Instance.GetTemplateTable<StudyKnowledge>();
                var it = template.GetEnumerator();
                while (it.MoveNext())
                {
                    var value = it.Current.Value as StudyKnowledge;
                    _studyList.Add(value);
                }
            }

            _studyknowledge = _studyList[MathUtility.Random(0, _studyList.Count - 1)];

            return _studyknowledge;
        }

        /// <summary>
        /// 获取慈善事业知识点
        /// </summary>
        /// <returns></returns>
        public CharityKnowledge GetCharityKnowledge()
        {
            CharityKnowledge _charityknowledge = new CharityKnowledge();
            if(GameModel.GetInstance.isPlayNet == true)
            {
                _charityknowledge.title = UIOtherCardWindowController.netKnowledge.title;
                _charityknowledge.content=UIOtherCardWindowController.netKnowledge.content;
                return _charityknowledge;
            }



            if (_charityList == null)
            {
                _charityList = new List<CharityKnowledge>();
                var template = MetadataManager.Instance.GetTemplateTable<CharityKnowledge>();
                var it = template.GetEnumerator();
                while (it.MoveNext())
                {
                    var value = it.Current.Value as CharityKnowledge;
                    _charityList.Add(value);
                }
            }

            _charityknowledge = _charityList[MathUtility.Random(0, _charityList.Count - 1)];

            return _charityknowledge;
        }

        public string KnowledgeHeadStr()
        {
            var tmpstr = "";

            if(cardID==(int)SpecialCardType.HealthType ||cardID==(int)SpecialCardType.InnerHealthType)
            {
                tmpstr = "健康小达人";
            }
            else if(cardID==(int)SpecialCardType.StudyType||cardID==(int)SpecialCardType.InnerStudyType)
            {
                tmpstr = "嘉许您的用功";
            }
            else if(cardID==(int)SpecialCardType.CharityType)
            {
                tmpstr = "感恩你的付出";
            }
            tmpstr = "嘉许您的用功";
            return tmpstr;
        }

        /// <summary>
        /// 健康知识的列表
        /// </summary>
        private List<HealthKnowledge> _healthList;
        /// <summary>
        /// 学习的知识点列表
        /// </summary>
        private List<StudyKnowledge> _studyList;

        /// <summary>
        /// 慈善事业知识列表
        /// </summary>
        private List<CharityKnowledge> _charityList;

		public PlayerInfo player;
		private int portType=0;

		public string cardPath;
		public string cardInfor;
		public string cardTitle;
		public string cardTitlePath;

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

