using System;
using System.Collections.Generic;
using Metadata;
using UnityEngine;
using Server;
using Client.UI;

namespace Client
{
    public class PlayerInfo
    {
        /// <summary>
        /// 判断玩家能否进内圈
        /// </summary>
        /// <returns></returns>
        public bool CanEnterInner()
        {  		
			var canInner=false;
			var _controller = UIControllerManager.Instance.GetController<GameTipBoardWindowController> ();
			var battlecontroller = UIControllerManager.Instance.GetController<UIBattleController> ();

			if (GameModel.GetInstance.isPlayNet == false)
			{
				if (CurrentIncome >= TargetIncome)
				{
					if (bankIncome > 0 || creditIncome > 0)
					{
						if(bankIncome>0)
						{
							if (battlecontroller.IsPackbackActive () == false)
							{
								_controller.gameTip = "您有未还银行贷款，不能进入内圈";
								_controller.setVisible (true);
							}					
							return false;
						}
						if(creditIncome>0)
						{
							if (PlayerManager.Instance.IsHostPlayerTurn () == true)
							{							
								if (battlecontroller.IsPackbackActive () == false)
								{
									_controller.gameTip="您有未还信用卡贷款，不能进入内圈";
									_controller.setVisible (true);
								}
							}
							return false;
						}
					}
					else
					{
						canInner = true;
					}
				}
			}
			else if(GameModel.GetInstance.isPlayNet == true)
			{
				if (netIsCanEnter == 0)
				{
					canInner = true;
					bankIncome = 0;
					creditIncome = 0;
				}
				else
				{
					canInner = false;
					if (netIsCanEnter == 2)
					{
						if (PlayerManager.Instance.IsHostPlayerTurn () == true)
						{
							if (battlecontroller.IsPackbackActive () == false)
							{
								_controller.gameTip="您有未还信用卡贷款，不能进入内圈";
								_controller.setVisible (true);
							}
						}
					}
				}
			}
            if(canInner)
            {
                GameTimerManager.Instance.EnterInnerTime(this.playerID);
            }
			return canInner ;
        }

		/// <summary>
        /// 游戏能否获得胜利
        /// </summary>
        /// <returns></returns>
        public bool CanInnerSuccess()
        {
			var cansuccess = false;
			if (GameModel.GetInstance.isPlayNet == false)
			{
                if (null != PlayerManager.Instance)
                {
                    if (null != PlayerManager.Instance.HostPlayerInfo)
                    {
                        if (playerID != PlayerManager.Instance.HostPlayerInfo.playerID)
                        {
                            return false;
                        }
                    }
                }
                //TargetIncome
                if (CurrentIncome >= TargetIncome && qualityScore >= targetQualityScore && timeScore >= targetTimeScore)
				{
					if (bankIncome > 0)
					{
						var _controller = UIControllerManager.Instance.GetController<GameTipBoardWindowController> ();

						var battlecontroller = UIControllerManager.Instance.GetController<UIBattleController> ();

						if (battlecontroller.IsPackbackActive ()==false)
						{
							_controller.gameTip="您有未还银行贷款，不能胜利";
							_controller.setVisible (true);
						}				
						//MessageHint.Show ("您有未还银行贷款，不能胜利");
						return false;
					}
					else
					{
						cansuccess = true;
					}
				}
			}
			else
			{
				if (netIsSuccess == 0)
				{
					cansuccess = true;
					bankIncome = 0;
					creditIncome = 0;
                    _isSuccess = true;
                    if(_isShowSuccessTip==true)
                    {
                        _isShowSuccessTip = false;
                        var tip = string.Format("恭喜{0}进入核心圈!",this.playerName);
                        MessageHint.Show(tip);
                    }
                    return cansuccess;
				}
				else
				{
					cansuccess = false;
					if (netIsSuccess == 2)
					{
						var _controller = UIControllerManager.Instance.GetController<GameTipBoardWindowController> ();
						var battlecontroller = UIControllerManager.Instance.GetController<UIBattleController> ();
						if (PlayerManager.Instance.IsHostPlayerTurn () == true)
						{
                            if (PlayerManager.Instance.HostPlayerInfo.playerID == this.playerID)
                            {
                                if (battlecontroller.IsPackbackActive() == false)
                                {
                                    _controller.gameTip = "您有未还银行贷款，不能胜利";
                                    _controller.setVisible(true);
                                }
                            }							
						}
						return false;
					}
				}
			}
            if (cansuccess)
            {
                GameTimerManager.Instance.SuceessTime();
            }
            return cansuccess;
        }

        /// <summary>
        /// 进入内圈
        /// </summary>
        public void EnterInner()
        {
			if (GameModel.GetInstance.isPlayNet == false)
			{
				totalIncome *= 100;
				qualityScore *= 10;
				timeScore *= 100;

				totalMoney = totalIncome;
				innerFlowMoney = totalIncome;

				_innerInitIncome =(int)totalIncome;
                PlayerManager.Instance.InnerPlayerNumber++;
			}
			TargetIncome = 200000;
			if (GameModel.GetInstance.isPlayNet == true)
			{
				_innerInitIncome =(int) netInitInnerFlowMoney;
			}          

			totalIncome = 0;

			cashFlow = 0;

			bankIncome = 0;
			creditIncome = 0;

			isEnterInner = true;         

			borrowList.Clear ();

			// 清空不在需要的列表shu
			opportCardList.Clear ();
			shareCardList.Clear ();
			chanceFixedCardList.Clear ();

			// 基本负债还款列表
			basePayList.Clear();
			saleRecordList.Clear();

			// 资产界面列表
			capitalList.Clear ();
			// 负债界面列表
//			debtList.Clear ();	

			timeScoreList.Clear ();
			qualityScoreList.Clear ();

			lastCheckDay = checkOutCount;            
        }

        /// <summary>
        /// 设置人物初始信息
        /// </summary>
        /// <param name="data"></param>
        public void SetPlayerInitData(PlayerInitData data)
		{
			playerID =  data.id.ToString();
            careerID = data.id.ToString();
            headName = data.headPath;
			playerName = data.playName;
			cashFlow = data.cashFlow;
			career = data.careers;

			initAdditionalDebt = data.additionalPay;
			initCardLoan =  data.cardPay;
			initCarLoan = data.carPay;
			initEducationLoan = data.educationPay;
			initHouseMortgages = data.housePay;
			initOtherSpend = data.nessPay;
			initTax = data.taxPay;

			fixedAdditionalDebt = data.fixAdditionalDebt;
			fixedCardLoan = data.fixCardDebt;
			fixedCarLoan = data.fixCarDebt;
			fixedHouseMortgages = data.fixHouseDebt;
			fixedEducation = data.fixEducationDebt;

			_initAge = data.initAge;
			_initData = data;

			modelPath = data.modelPath;
			playerImgPath = data.playerImgPath;
			oneChildPrise = data.oneChildPrise;

			playerSex = data.playerSex;

			if (GameModel.GetInstance.isPlayNet == false)
			{
				if(data.housePay>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="住房抵押贷款:";
					tmp.borrow = data.fixHouseDebt;
					tmp.debt = data.housePay;
					tmp.basetype = (int)BaseDebtType.HouseDebt;
					basePayList.Add(tmp);
				}

				if(data.educationPay>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="教育贷款:";
					tmp.borrow = data.fixEducationDebt;
					tmp.debt = data.educationPay;
					tmp.basetype = (int)BaseDebtType.EducationDebt;
					basePayList.Add(tmp);
				}

				if(data.carPay>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="购车贷款:";
					tmp.borrow = data.fixCarDebt;
					tmp.debt = data.carPay;
					tmp.basetype = (int)BaseDebtType.CarDebt;
					basePayList.Add(tmp);
				}

				if(data.cardPay>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="信用卡:";
					tmp.borrow = data.fixCardDebt;
					tmp.debt = data.cardPay;
					tmp.basetype = (int)BaseDebtType.CardDebt;
					basePayList.Add(tmp);
				}

				if(data.additionalPay>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="额外负债:";
					tmp.borrow = data.fixAdditionalDebt;
					tmp.debt = data.additionalPay;
					tmp.basetype =(int)BaseDebtType.AdditionDebt;
					basePayList.Add(tmp);
				}
			}

//			var template = MetadataManager.Instance.GetTemplateTable<ChanceFixed> ();//测试命运卡牌需要用到
//			var it = template.GetEnumerator ();	
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as ChanceFixed;
//				if(value.id==40002 || value.id==20003 ||value.id == 20001)
//				{
//					chanceFixedCardList.Add (value);
//				}
//			}	

//			var template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();//测试命运卡牌需要用到
//			var it = template.GetEnumerator ();	
//			while (it.MoveNext ()) 
//			{				
//				var value = it.Current.Value as ChanceShares;
//				if (value.id == 30001)
//				{
//					value.shareNum = 2000;
//					shareCardList.Add (value);
//					break;
//				}
//			}	

			totalMoney=cashFlow + totalIncome + innerFlowMoney-initCardLoan-initCarLoan-initEducationLoan-initHouseMortgages-initOtherSpend-initAdditionalDebt-initTax;
			UptatePaymentData ();

            //totalMoney = 50000;
            //测试用
            //totalIncome = 10000;
           // totalMoney = 1000000;
            //timeScore = 10;
            //qualityScore = 10;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetInitHouseDebt(int value)
        {

        }

        public void SetInitCarDebt(int value)
        {

        }

        public void SetInitCardDebt(int value)
        {

        }

        public void SetInitEducDebt(int value)
        {

        }
        

             

        /// <summary>
        /// 人物当前的负债  包括原始的基础负债 和 银行贷款 信用卡透支
        /// </summary>
        /// <returns></returns>
        public int GetTotalDebt()
		{			
			return (fixedAdditionalDebt+fixedCardLoan+fixedCarLoan+fixedEducation+fixedHouseMortgages+Mathf.FloorToInt(bankIncome)+Mathf.FloorToInt(creditIncome));
		}

		public void UptatePaymentData()
		{
			totalPayment = initCardLoan + initCarLoan + initEducationLoan + initHouseMortgages + initOtherSpend + initAdditionalDebt + initTax;
			_monthPayment = initCardLoan + initCarLoan + initEducationLoan + initHouseMortgages + initOtherSpend + initAdditionalDebt + initTax;
		}

		public float GetTotalBorrowBank()
		{
			var tmpmoney = (totalIncome + cashFlow + innerFlowMoney - MonthFixedPay) * 10;
			if(isEnterInner==true)
			{
				tmpmoney = (totalIncome + cashFlow + innerFlowMoney) * 100;
			}
			return tmpmoney;
		}

		public float GetTotalBorrowCard()
		{
			return (totalIncome + cashFlow + innerFlowMoney - MonthFixedPay) * 3;
		}

		public string playerImgPath;

		public float totalMoney
		{
			get
			{
				return _totalMoney;
			}
			set
			{
				_totalMoney = value;
				if (_totalMoney > 10000)
				{
					if(_isGetOpportunityTarget==false)
					{
						_isGetOpportunityTarget = true;
						//MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.getOpportChance1,playerName),null,false);
					}
				}
			}
		}

        /// <summary>
        /// 掷筛子的回合数
        /// </summary>
		public int RoundNumber
		{
			get
			{ 
				return _roundNum;
			}
			set
			{
				_roundNum = value;
			}
		}

        /// <summary>
        /// 获取总的年龄
        /// </summary>
		public int totalAge
		{
			get
			{
				return _initAge + this._totalRound;
			}
		}

        /// <summary>
        /// 添加掷筛子数
        /// </summary>
        /// <param name="value"></param>
        public void AddRollPoint(int value)
        {
            if(isEnterInner==false)
            {
                this._outerRollPoint += value;
            }
            else
            {
                this._innerRollPoint += value;
            }

            this._outerRound = (_outerRollPoint / _outerCubeNumber)*4;//外圈25
            this._innerRound = (_innerRollPoint / _innerCubeNumber)*2;//内圈19
            this._totalRound = this._outerRound + this._innerRound;
        }

        /// <summary>
        /// 是否是显示游戏圈数提醒
        /// </summary>
        /// <returns></returns>
        public bool IsTipGameRound()
        {
            var istip = false;

            if(_totalRound>0 && _totalRound!=_oldTotalRound&&_totalRound%1==0)
            {
                istip = true;
            }
            return istip;
        }


        /// <summary>
        /// 刷新旧的玩家圈数
        /// </summary>
        public void UpdateOldRound()
        {
            this._oldTotalRound = this._totalRound;
        }

        /// <summary>
        /// 游戏进度的权重 在外圈是0--1，内圈是2--5；
        /// </summary>
        /// <returns></returns>
        public float GameProgress
        {
            get
            {
                var tmpValue = 0f;
                if (isEnterInner == false)
                {
                    tmpValue = CurrentIncome / this.TargetIncome;
                    if (tmpValue > 1)
                    {
                        tmpValue = 1;
                    }
                }
                else
                {
                    var progressIncome = (CurrentIncome / TargetIncome) >= 1 ? 1 : (CurrentIncome / TargetIncome);
                    var progressQuality = (qualityScore / targetQualityScore) >= 1 ? 1 : (qualityScore / targetQualityScore);
                    var progressTime = (timeScore / targetTimeScore)>1?1: (timeScore / targetTimeScore);
                    tmpValue = progressIncome + progressQuality + progressTime + 2;
                }
                return tmpValue;
            }
            
        }

        /// <summary>
        /// 获取转圈的圈数
        /// </summary>
        public int CircleNumber()
        {
            //var outerNum = (_outerRollPoint/ _outerCubeNumber);//外圈25
            //var innerNum = (_innerRollPoint / _innerCubeNumber);//内圈19
            return this._outerRound + this._innerRound;// outerNum+innerNum;
        }

        /// <summary>
        /// 玩家游戏中的外圈数
        /// </summary>
        private int _outerRound;
        /// <summary>
        /// 内圈数
        /// </summary>
        private int _innerRound;
        /// <summary>
        /// 旧的圈数
        /// </summary>
        private int _oldTotalRound=0;
        /// <summary>
        /// 总的圈数
        /// </summary>
        private int _totalRound=0;

        /// <summary>
        /// 外圈筛子数总和
        /// </summary>
        private int _outerRollPoint=0;
        /// <summary>
        /// 内圈筛子数总和
        /// </summary>
        private int _innerRollPoint = 0;
        /// <summary>
        /// 外圈砖块的个数
        /// </summary>
        private readonly int _outerCubeNumber = MetadataManager.Instance.GetTemplateTable<StageOuterPoint>().Count;
        /// <summary>
        /// 内圈砖块的个数
        /// </summary>
        private readonly int _innerCubeNumber = MetadataManager.Instance.GetTemplateTable<StageInnerPoint>().Count;



        public void AddCapticalData()
		{
			if (isEnterInner == false)
			{
				var capitcal = new CapitalVo ();
				capitcal.age = totalAge;

				var tmpCaptical = 0f;

				for (var i = 0; i < shareCardList.Count; i++)
				{
					var tmpShare = shareCardList [i];
					tmpCaptical += Mathf.Abs (tmpShare.payment) + Mathf.Abs (tmpShare.income);
				}

				for (var i = 0; i < chanceFixedCardList.Count; i++)
				{
					var tmpFiexd=chanceFixedCardList[i];
					tmpCaptical += Mathf.Abs (tmpFiexd.payment) + Mathf.Abs(tmpFiexd.mortgage);
				}

				for (var i = 0; i < opportCardList.Count; i++)
				{
					var tmpOppotr = opportCardList [i];
					tmpCaptical += Mathf.Abs (tmpOppotr.payment) + Mathf.Abs (tmpOppotr.mortgage);
				}

				if(tmpCaptical>0)
				{
					capitcal.captical = tmpCaptical;
					capitalList.Add (capitcal);
				}			
			}
		}
		public bool isThreeRoll=false;

		// 内圈胜利条件
		public int targetTimeScore=1000;
		public int targetQualityScore=100;

        /// <summary>
        /// 流动资金比现有资金多出20w，动态设置的
        /// </summary>
        public int TargetIncome
		{
			get
			{
                //return 100;
				if (isEnterInner == false)
				{
					return (int)MonthPayment;
				}
				return _targetIncome;
			}

			set 
			{
				_targetIncome = value;			
			}
		}

		public float CurrentIncome
		{
			get
			{
				if (isEnterInner == false)
				{
					return (totalIncome + innerFlowMoney); 
				}
				return (totalIncome + innerFlowMoney)-_innerInitIncome;
			}
		}

		public float InnerInitIncome
		{
			get
			{
				return _innerInitIncome;
			}
		}

		public bool CanGetOpportunity
		{
			get
			{
				return (_totalMoney>=10000);
			}
		}

		private int _targetIncome=200000;
		private int _innerInitIncome=0;

		// 人物初始年龄
		private int _initAge = 20;

		private int _roundNum = 0;

		// 判断是否是获得大机会的目标了
		private bool _isGetOpportunityTarget=false;

		public string playerID;

        /// <summary>
        ///  显示玩家职业的ID，不同的id。对应不同音效
        /// </summary>
        public string careerID;

        


        public string playerName; //玩家名称
        public string career; //玩家职业名称

        /// <summary>
        /// 结账日时发的工资 
        /// </summary>
        public int cashFlow = 0;
		public string headName = "share/texture/head/touxiang2.ab";
        
		private float _totalMoney;//总现金 工资+总的收入

        /// <summary>
        /// 初始的每月还款
        /// </summary>
		public int initHouseMortgages=0; // 
        /// <summary>
        /// 每月的购车支出
        /// </summary>
		public int initCarLoan=0;  // 初始的购车贷款
        /// <summary>
        /// 初始的信用卡贷款
        /// </summary>
		public int initCardLoan=0; // 

        /// <summary>
        /// 初始每月的额外支出
        /// </summary>
		public int initAdditionalDebt=0; // 
        /// <summary>
        /// 每月的教育贷款利息
        /// </summary>
		public int initEducationLoan=0;
        
        /// <summary>
        /// 每月的必备支出
        /// </summary>
		public int initOtherSpend=0; // 初始的其它贷款
		public int initTax=0; // 税金用于支出

		public int fixedHouseMortgages=0;
		public int fixedCarLoan=0;
		public int fixedCardLoan=0;

        /// <summary>
        /// 每月的额外负债
        /// </summary>
		public int fixedAdditionalDebt=0;
		public int fixedEducation=0;

		public float totalIncome; // 总非劳务收入   卖房收入， 卖股票收入 ，卖企业收入 ， 直接收入

		public float netInitInnerFlowMoney=0f;

		/// <summary>
		/// The is off line befor game.是否是在进入游戏等待游戏开始时掉线
		/// </summary>
		public bool isOffLineBeforGame=false;

		/// <summary>
		/// The is reconect game. 人物重连游戏
		/// </summary>
		public bool isReconectGame=false;

		public float innerFlowMoney; // 内圈流动资金 
		public float timeScore; // 时间积分
		public float qualityScore; // 品质积分

		// 投资获得的流动资金
		public float investFlow;
		// 有钱有闲获得的流动资金
		public float relaxFlow;

		// 是否是进入了内圈
		public bool isEnterInner=false;

        /// <summary>
        /// 是否是获得胜利了
        /// </summary>
        public bool _isSuccess = false;

        private bool _isShowSuccessTip = true;


        /// <summary>
        /// 返回是否是进核心圈了
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }

            set
            {
                _isSuccess = value;
                if(_isSuccess==true)
                {
                    if (_isShowSuccessTip == true)
                    {
                        _isShowSuccessTip = false;
                        var tip = string.Format("恭喜{0}进入核心圈!", this.playerName);
                        MessageHint.Show(tip);
                    }
                }
            }
        }

        /// <summary>
        /// 是否是在职的（判断玩家是否事业）
        /// </summary>
		public bool isEmployment=true;

		/// <summary>
		/// The player sex.角色的性别 男是1，女是0；
		/// </summary>
		public int playerSex=0;

		public float creditIncome; // 信用卡贷款
		public float bankIncome; // 银行贷款 


		public List<BorrowVo> borrowList=new List<BorrowVo>();

		/// <summary>
		/// The net infor balance and income. 网络版 资产和收入的数据
		/// </summary>
		public NetInforBoardBalanceAndIncomeVo netInforBalanceAndIncome=new NetInforBoardBalanceAndIncomeVo();

		/// <summary>
		/// The net infor check vo. 网络版人物信息面板结算面板
		/// </summary>
		public NetInforBoardCheckVo netInforCheckVo = new NetInforBoardCheckVo ();

		//借款面板数据
		public int netRecordAlreadyBorrow = 0;
		public int netRecordCanBorrow=0;
		public int netRecordLimitBorrow = 0;


		public int netBorrowBoardBankCanBorrow=0;
		public int netBorrowBoardBankTotalBorrow = 0;
		public int netBorrowBoradBankTotalDebt   = 0;
		public int netBorrowBoardBankLoanLimit = 0;

		public int netBorrowBoardCardCanBorrow=0;
		public int netBorrowBoardCardTotalBorrow = 0;
		public int netBorrowBoardCardTotalDebt   =0;
		public int netBorrowBoardCardLoanLimit = 0;

		/// <summary>
		/// The net is can enter. 0 可以进入内圈  1、费劳务收入小于总支出 2、费劳务收入>总支出 但是有贷款
		/// </summary>
		public int netIsCanEnter=-1;

		/// <summary>
		/// The net is success. 0 可以胜利  1、不满足条件 2、可以胜利但是贷款未还
		/// </summary>
		public int netIsSuccess = -1;


		public List<PaybackVo> paybackList = new List<PaybackVo> ();
		public List<PaybackVo> basePayList = new List<PaybackVo> ();
		public NetInforDebtAndPay netInforDebtAndPay=new NetInforDebtAndPay();

		public void AddBasePayVoForShow()
		{
			var addOther = true;
			var addTax = true;
			var addChild = true;

			for (var i = 0; i < basePayList.Count; i++)
			{
				var tmpVo = basePayList [i];
				if (tmpVo.basetype ==(int) BaseDebtType.OtherDebt)
				{
					addOther = false;
				}

				if (tmpVo.basetype == (int)BaseDebtType.InitTxtDebt)
				{
					addTax = false;
				}

				if (tmpVo.basetype ==(int)BaseDebtType.GiveChild)
				{
					addChild = false;
				}

			}
			if (addOther == true)
			{
				if (this.initOtherSpend > 0)
				{
					var tmp = new PaybackVo ();
					tmp.title="其它支出:";
					tmp.borrow = initOtherSpend;
					tmp.debt = initOtherSpend;
					tmp.basetype = (int)BaseDebtType.OtherDebt;
					basePayList.Add (tmp);
					Console.WriteLine ("增加其它支出的金额,"+initOtherSpend.ToString());
				}
			}

			if (addTax == true)
			{
				if (this.initTax > 0)
				{
					var tmp = new PaybackVo ();
					tmp.title="税金:";
					tmp.borrow = initTax;
					tmp.debt = initTax;
					tmp.basetype = (int)BaseDebtType.InitTxtDebt;
					basePayList.Add (tmp);
					Console.WriteLine ("增加税金支出的金额,"+initTax.ToString());
				}
			}

			if (addChild == true)
			{
				if (childNum > 0)
				{
					var tmpV = new PaybackVo ();
					tmpV.basetype =(int) BaseDebtType.GiveChild;
					tmpV.debt =Mathf.FloorToInt(childNum * oneChildPrise);
					tmpV.title = string.Format ("小孩支出（{0}个宝宝）",childNum);
					basePayList.Add(tmpV);

					Console.WriteLine ("增加孩子的个数,"+childNum.ToString());
				}
			}
		}

		public void RemoveBasePayVoForShow()
		{
			for (var i = basePayList.Count-1; i >=0 ; i--)
			{
				var tmpVo = basePayList [i];
				var isRemove = false;
				if (tmpVo.basetype ==(int) BaseDebtType.OtherDebt)
				{
					isRemove = true;
				}
				else if (tmpVo.basetype == (int)BaseDebtType.InitTxtDebt)
				{
					isRemove = true;
				}
				else if (tmpVo.basetype == (int)BaseDebtType.GiveChild)
				{
					isRemove = true;
				}

				if(isRemove==true)
				{
					basePayList.Remove (tmpVo);
				}
			}			
		}



		public int borrowbankTimes=0;
		public int borrowcardTimes=0;

		// 暂时未用到 非劳务收入大于总支出 ， 是大于每个月的总支出 
		public float totalPayment; // 总支出  买房支出 ， 买股票支出 ，买企业支出 ， 其它的支出 。卖房的支出，卖股票的支出 ，卖企业支出

		/// <summary>
		/// The net total payment.网络版里每月的总支出
		/// </summary>
		public float netTotalPayment = 0;

		/// <summary>
		/// The net total debt.总总收入
		/// </summary>
		public float netTotalTake = 0;

		/// <summary>
		/// The net check day number.网络版结账日金额
		/// </summary>
		public float netCheckDayNum = 0;

		/// <summary>
		/// The net player age. 人物的年龄
		/// </summary>
		public int netPlayerAge=0;

		/// <summary>
		/// The net balance money. 
		/// </summary>
		public int netTotalBalanceMoney=0;

		/// <summary>
		/// The net total debt money. 总负债
		/// </summary>
		public int netTotalDebtMoney = 0;

		public float MonthPayment
		{
			get
			{	
				var tmpPay = 0f;
				if (GameModel.GetInstance.isPlayNet == false)
				{
					tmpPay= _monthPayment + childNum * oneChildPrise + bankLoans + creditDebt;
					if(isEnterInner==true)
					{
						tmpPay = bankLoans;
					}
				}
				else
				{
					tmpPay = netTotalPayment;
				}				
				return tmpPay;
			}
		}
        

		// 每个月的固定支出
		public float MonthFixedPay
		{
			get
			{				
				return _monthPayment;
			}
		}



		private float _monthPayment;

		// 暂时没有用到
		public float housePayment; // 买房支出
		public float sharesPayment; // 股票支出
		public float companyPayment; //企业支出 
		public float otherPayment; // 其它支出
		public float antiquePayment;


		public float totalDebt; //总的负债

		public float houseDebt; // 房产负债
		public float companyDebt; // 企业负债
		public float creditDebt; // 信用卡负债
		public float bankLoans; // 银行贷款
		public float otherDebt; // 其它的负债

//		public float maxDebt = 0; //负债的最大值
//		public float extendDebt;  //额外的负债
//		public float maxCapitcal=10; //最多的资产值

		// 生孩子计算
		public int childNum; // 养孩子数
		public float oneChildPrise=240;
		public int childNumMax=3;
		// 结算日计数
		public int checkOutCount=0;



		private PlayerInitData _initData=null;

		public PlayerInitData InitData
		{
			get
			{
				return _initData;
			}
		}


		public void AddTimeScoreInfor(InforRecordVo value)
		{
			var index = timeScoreList.Count+1;
			value.index = index;
			timeScoreList.Add (value);
		}

		public void AddQualityScoreInfor(InforRecordVo value)
		{
			var index = qualityScoreList.Count + 1;
			value.index = index;
			qualityScoreList.Add (value);
		}

		public void AddInnerFlowInfor(InforRecordVo value)
		{
			var index = flowScoreList.Count + 1;
			value.index = index;
			flowScoreList.Add (value);
		}

		/// <summary>
		/// The round postion. 人物在圆盘上的位置，在网络版掉线重连时候需要做
		/// </summary>
		public int roundPostion=0;

		/// <summary>
		/// 更改积分
		/// </summary>
		/// <value>The player integral.</value>
		public int PlayerIntegral
		{
			get
			{
				return _integral;	
			}

			set
			{
				var updatePlayer = false;
				if (_integral != value)
				{
					updatePlayer = true;
				}
				_integral = value;

				var tmplitter = "littere";
				var tmppercent = 0f;

				if(_integral>=361)
				{
					tmplitter = "littera";
					tmppercent = 1;
//					tmppercent=(float)(value-80)/(100-80);
				}
				else if(_integral<361 && _integral>=241)
				{
					tmplitter = "litterb";
					tmppercent=(float)(value-241)/(360-241);
				}
				else if(_integral<241 && _integral>=141)
				{
					tmplitter = "litterc";
					tmppercent=(float)(value-141)/(240-141);
				}
				else if(_integral<141 && _integral>=61)
				{
					tmplitter = "litterd";
					tmppercent=(float)(value-61)/(140-61);
				}
				else if(_integral<61 && _integral>=0)
				{
					tmplitter = "littere";
					tmppercent=(float)value/60;
				}

//				if (updatePlayer == true)
//				{
//					//刷新人物信息
//					var battleControlelr = UIControllerManager.Instance.GetController<UIBattleController> ();
//					if(currentLetter != tmplitter)
//					{
//						currentLetter = tmplitter;
//						//调用数据
//						if (battleControlelr.getVisible ())
//						{
//							battleControlelr.UpdateLetterLevel (playerID,currentLetter);
//						}
//					}
//
//					if (battleControlelr.getVisible ())
//					{
//						battleControlelr.UpdateLettlePercent (playerID,tmppercent);
//					}
//						
//				}
			}
		}

		//private int[] _maxArr = new int[]{30,20,9,10,9,20};
		public string currentLetter = "littere";
		//private int _lettleLevel=0;

		// 人物模型的路径
		public string modelPath;

		/// <summary>
		/// The net target time score.  目标时间积分
		/// </summary>
		public int netTargetTimeScore=0;

		/// <summary>
		/// The net target quality score. 目标品质积分
		/// </summary>
		public int netTargetQualityScore=0;

		/// <summary>
		/// The net target cash flow score. 内圈目标流动资金
		/// </summary>
		public int netTargetCashFlowScore=0;

		public List<InforRecordVo> timeScoreList = new List<InforRecordVo> ();
		public List<InforRecordVo> qualityScoreList = new List<InforRecordVo> ();
		public List<InforRecordVo> flowScoreList = new List<InforRecordVo> ();
		public List<SaleRecordVo> saleRecordList = new List<SaleRecordVo> ();


		// 负债统计和资产统计
//		public List<PlayerDebtInfor> debtList=new List<PlayerDebtInfor>();
		public List<CapitalVo> capitalList = new List<CapitalVo> ();
		public List<ChanceShares> shareCardList = new List<ChanceShares> ();
		public List<Opportunity> opportCardList = new List<Opportunity> ();
		public List<ChanceFixed> chanceFixedCardList = new List<ChanceFixed> ();

		// 买保险的记录
		public List<int> InsuranceList = new List<int> ();

		/// <summary>
		/// Determines whether this instance is train tip.  判断是否是显示教练话术，当最近的显示，当最近的一次显示大于3的时候，显示一次
		/// </summary>
		/// <returns><c>true</c> if this instance is train tip; otherwise, <c>false</c>.</returns>
		public bool IsTrainTip()
		{
			var isTrain = false;

			if (isEnterInner == false)
			{
				if (checkOutCount-lastCheckDay>=outerCheckNum)
				{
					lastCheckDay = checkOutCount;
					isTrain = true;
				}
			}
			else				
			{
				if (checkOutCount - lastCheckDay >= innerCheckNun)
				{
					lastCheckDay = checkOutCount;
					isTrain = true;
				}
			}		
			return isTrain;
		}

        /// <summary>
        /// 玩家结算信息数据
        /// </summary>
        public SettlementData Settlement
        {
            get
            {
                return this._settleMentData;
            }
        }
        
		private const int outerCheckNum = 3;
		private const int innerCheckNun = 2;

        private int lastCheckDay=0;

		public bool isNetOnline=true;
		//积分
		private int _integral = 0;
        /// <summary>
        /// 结算信息的数据
        /// </summary>
        private SettlementData _settleMentData=new SettlementData();

       

    }
}