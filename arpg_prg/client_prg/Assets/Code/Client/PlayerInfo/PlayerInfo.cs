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
        /// 是否能进内圈
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
								//						MessageHint.Show ("您有未还信用卡贷款，不能进入内圈");
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
			return canInner ;
        }

        /// <summary>
        /// 游戏是否胜利 23101 1264 744
        /// </summary>
        /// <returns></returns>
        public bool CanInnerSuccess()
        {
			var cansuccess = false;

			if (GameModel.GetInstance.isPlayNet == false)
			{
                //TargetIncome
                if (CurrentIncome >= 0 && qualityScore >= targetQualityScore && timeScore >= targetTimeScore)
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
						//					MessageHint.Show ("您有未还银行贷款，不能胜利");
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
							if (battlecontroller.IsPackbackActive ()==false)
							{
								_controller.gameTip="您有未还银行贷款，不能胜利";
								_controller.setVisible (true);
							}		
						}

						return false;
					}
				}
			}
			return cansuccess;
        }

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
//			if (GameModel.GetInstance.isPlayNet == true)
//			{
//				if (playerID == PlayerManager.Instance.HostPlayerInfo.playerID)
//				{
//					var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
//					controller.EnterInner ();
//				}
//			}
        }

		// 设置人物初始信息
		public void SetPlayerInitData(PlayerInitData data)
		{
			playerID =  data.id.ToString();
            careerID = data.id.ToString();
            headName = data.headPath;
			playerName = data.playName;
			cashFlow = data.cashFlow;
			career = data.careers;

			initAdditionalDebt = data.additionalDebt;
			initCardLoan =  data.cardDebt;
			initCarLoan = data.carLoan;
			initEducationLoan = data.educationLoan;
			initHouseMortgages = data.houseMortgages;
			initOtherSpend = data.otherSpend;
			initTax = data.fixTax;

			fixedAdditionalDebt = data.fixAdditionalDebt;
			fixedCardLoan = data.fixCardDebt;
			fixedCarLoan = data.fixCarLoan;
			fixedHouseMortgages = data.fixHouseMortgages;
			fixedEducation = data.fixEducationLoan;

			_initAge = data.initAge;
			_initData = data;

			modelPath = data.modelPath;
			playerImgPath = data.playerImgPath;
			oneChildPrise = data.oneChildPrise;

			playerSex = data.playerSex;

			if (GameModel.GetInstance.isPlayNet == false)
			{
				if(data.houseMortgages>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="住房抵押贷款:";
					tmp.borrow = data.fixHouseMortgages;
					tmp.debt = data.houseMortgages;
					tmp.basetype = (int)BaseDebtType.HouseDebt;
					basePayList.Add(tmp);
				}

				if(data.educationLoan>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="教育贷款:";
					tmp.borrow = data.fixEducationLoan;
					tmp.debt = data.educationLoan;
					tmp.basetype = (int)BaseDebtType.EducationDebt;
					basePayList.Add(tmp);
				}

				if(data.carLoan>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="购车贷款:";
					tmp.borrow = data.fixCarLoan;
					tmp.debt = data.carLoan;
					tmp.basetype = (int)BaseDebtType.CarDebt;
					basePayList.Add(tmp);
				}

				if(data.cardDebt>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="信用卡:";
					tmp.borrow = data.fixCardDebt;
					tmp.debt = data.cardDebt;
					tmp.basetype = (int)BaseDebtType.CardDebt;
					basePayList.Add(tmp);
				}

				if(data.additionalDebt>0)
				{
					var tmp = new PaybackVo ();
					tmp.title="额外负债:";
					tmp.borrow = data.fixAdditionalDebt;
					tmp.debt = data.additionalDebt;
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

            //测试用
            totalIncome = 10000;
            totalMoney = 10000;
            timeScore = 10;
            qualityScore = 10;
        }

		// 人物当前的负债  包括原始的基础负债 和 银行贷款 信用卡透支
		public int GetTotalDebt()
		{			
			return (fixedAdditionalDebt+fixedCardLoan+fixedCarLoan+fixedEducation+fixedHouseMortgages+Mathf.FloorToInt(bankIncome)+Mathf.FloorToInt(creditIncome));
		}

		public void UptatePaymentData()
		{
			totalPayment = initCardLoan + initCarLoan + initEducationLoan + initHouseMortgages + initOtherSpend + initAdditionalDebt + initTax;
			_monthPayment = initCardLoan + initCarLoan + initEducationLoan + initHouseMortgages + initOtherSpend + initAdditionalDebt + initTax;
		}

        /// <summary>
        /// 总的可以借钱数
        /// </summary>
        /// <returns></returns>
		public float GetTotalBorrowBank()
		{
			var tmpmoney = (totalIncome + cashFlow + innerFlowMoney - MonthFixedPay) * 10;
			if(isEnterInner==true)
			{
				tmpmoney = (totalIncome + cashFlow + innerFlowMoney) * 100;
			}
			return tmpmoney;
		}

        /// <summary>
        /// 信用卡贷款数
        /// </summary>
        /// <returns></returns>
		public float GetTotalBorrowCard()
		{
			return (totalIncome + cashFlow + innerFlowMoney - MonthFixedPay) * 3;
		}

        /// <summary>
        ///  玩家形象的图片路径
        /// </summary>
		public string playerImgPath;

        /// <summary>
        /// 总的现金数
        /// </summary>
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
        /// 年龄
        /// </summary>
		public int Age
		{
			get
			{ 
				return _age;
			}
			set
			{
				_age = value;
			}
		}

		public int totalAge
		{
			get
			{
				return _initAge + _age;
			}
		}

        /// <summary>
        /// 每次掷筛子，增加负债信息
        /// </summary>
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

        /// <summary>
        /// 是否是掷三个筛子
        /// </summary>
		public bool isThreeRoll=false;

        /// <summary>
        /// 内圈胜利条件
        /// </summary>
        public int targetTimeScore=1000;
		public int targetQualityScore=100;
		// 流动资金比现有资金多出20w，动态设置的
		public int TargetIncome
		{
			get
			{
				if (isEnterInner == false)
				{
					return (int)MonthPayment;
				}

                return _targetIncome; // 10; 
            }

			set 
			{
				_targetIncome = value;			
			}
		}

        /// <summary>
        /// 当前的分劳务收入
        /// </summary>
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

        /// <summary>
        /// 内圈初始非劳务收入
        /// </summary>
		public float InnerInitIncome
		{
			get
			{
				return _innerInitIncome;
			}
		}

        /// <summary>
        /// 是否是显示大机会
        /// </summary>
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

		private int _age = 0;

		// 判断是否是获得大机会的目标了
		private bool _isGetOpportunityTarget=false;

		public string playerID;

        /// <summary>
        ///  显示玩家职业的ID，不同的id。对应不同音效
        /// </summary>
        public string careerID;


		public string playerName; //玩家名称
        public string career; //玩家职业名称
        public int cashFlow = 0; //结账日时发的工资 
		public string headName = "share/texture/head/touxiang2.ab";
        
		private float _totalMoney;//总现金 工资+总的收入

		public int initHouseMortgages; // 初始的住房抵押贷款
		public int initCarLoan;  // 初始的购车贷款
		public int initCardLoan; // 初始的信用卡贷款
		public int initAdditionalDebt; // 初始的额外支出
		public int initEducationLoan; // 初始的教育贷款
		public int initOtherSpend; // 初始的其它贷款
		public int initTax; // 税金用于支出

		public int fixedHouseMortgages;
		public int fixedCarLoan;
		public int fixedCardLoan;
		public int fixedAdditionalDebt;
		public int fixedEducation;



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
		/// The net is can enter. 0 可以进入内圈  1、费劳务收入<总支出 2、费劳务收入>总支出 但是有贷款
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

        /// <summary>
        /// 每月的支出
        /// </summary>
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

		private int[] _maxArr = new int[]{30,20,9,10,9,20};
		public string currentLetter = "littere";
		private int _lettleLevel=0;

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

		private const int outerCheckNum = 3;
		private const int innerCheckNun = 2;
		private int lastCheckDay=0;

		public bool isNetOnline=true;

		//积分
		private int _integral = 0;

		//情商指数
		/// <summary>
		/// 有钱有闲计数
		/// </summary>
		public int _richleisureNum=0;	
		/// <summary>
		/// The richleisure total number.遇到有钱有闲的总次数
		/// </summary>
		public int _richleisureTotalNum=0;

		/// <summary>
		/// 品质生活计数
		/// </summary>
		public int _qualityNum=0;
		/// <summary>
		/// The quality total number.品质生活的总次数
		/// </summary>
		public int _qualityTotalNum=0;


		/// <summary>
		/// 生孩子计数
		/// </summary>
		public int _childNum;
		/// <summary>
		/// 慈善计数
		/// </summary>
		public int _charityNum;
		/// <summary>
		/// The charity total number. 慈善的总次数
		/// </summary>
		public float _charityTotalNum=0;

		//灵性指数
		/// <summary>
		/// 健康管理计数
		/// </summary>
		public int _healthNum;

		/// <summary>
		/// The health total number. 健康管理总的次数
		/// </summary>
		public float _healthTotalNum=0;

		/// <summary>
		/// 学习进修计数
		/// </summary>
		public int _learnNum;
		/// <summary>
		/// The learn total number  进修学习总次数
		/// </summary>
		public float _learnTotalNum=0;

		//逆境指数
		/// <summary>
		///失业计数
		/// </summary>
		public int _unemploymentNum;
		/// <summary>
		///审计计数
		/// </summary>
		public int _auditNum;
		public float _auditTotalNum=0;

		/// <summary>
		/// 离婚计数
		/// </summary>
		public int _divorceNum;

		/// <summary>
		/// The divorce total number.离婚的总次数
		/// </summary>
		public float _divorceTotalNum=0;

		/// <summary>
		/// The money loss. 金融风暴次数
		/// </summary>
		public int _moneyLoss = 0;

		public float _moneyLossTotalNum = 0;

		//财商指数
		/// <summary>
		/// 大机会计数
		/// </summary>
		public int _bigOpportunitiesNum=0;

		/// <summary>
		/// The big opportunities total number. 大机会总次数
		/// </summary>
		public float _bigOpportunitiesTotalNum=0;


		/// <summary>
		/// 小机会计数
		/// </summary>
		public int _smallOpportunitiesNum;
		/// <summary>
		/// The small opportunities total number. 小机会总次数
		/// </summary>
		public float _smallOpportunitiesTotalNum=0;

		/// <summary>
		/// 投资计数
		/// </summary>
		public  int _investmentNum;
		/// <summary>
		/// The investment total number.投资总次数
		/// </summary>
		public  float _investmentTotalNum=0;
		/// <summary>
		/// 结算计数
		/// </summary>
		public  int _settlementNum;

		/// <summary>
		/// The settle outer number. 外圈结账日
		/// </summary>
		public int _settleOuterNum=0;

		/// <summary>
		/// The settle inner number. 内圈结账日
		/// </summary>
		public int _settleInnerNum = 0;

		/// <summary>
		/// The buy care number. 购买保险的次数
		/// </summary>
		public int _buyCareNum = 0;

		/// <summary>
		/// The sale nums. 成交的次数
		/// </summary>
		public int _saleNums=0;


		/// <summary>
		/// 灵性积分
		/// </summary>
		public int spiritualIntegral;

		/// <summary>
		/// 逆境积分
		/// </summary>
		public int adversityIntegral;

		/// <summary>
		/// 财商积分
		/// </summary>
		public int financialIntegral;
		/// <summary>
		/// 大机会积分
		/// </summary>
		public int _bigIntegral;
		/// <summary>
		/// 小机会积分
		/// </summary>
		public int _smallIntegral;
		/// <summary>
		/// 投资积分
		/// </summary>
		public int _investmentIntegral;

		/// <summary>
		/// 情商积分
		/// </summary>
		public int eqIntegral;

    }
}
