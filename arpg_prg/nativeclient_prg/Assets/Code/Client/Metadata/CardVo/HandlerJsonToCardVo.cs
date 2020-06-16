using System;
using Metadata;
using LitJson;
using Client;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 处理后台返回的数据
/// </summary>
public class HandlerJsonToCardVo
{
	public HandlerJsonToCardVo ()
	{
	}

    /// <summary>
    ///  更新玩家信息
    /// </summary>
    /// <param name="player"></param>
    /// <param name="dataManager"></param>
    /// <param name="assetsData"></param>
    /// <param name="incomeData"></param>
    /// <param name="debtInfor"></param>
    /// <param name="borrowRecordData"></param>
    /// <param name="borrowBarodInfor"></param>
    public static void UpdatePlayerInfor(PlayerInfo player , JsonData dataManager , JsonData assetsData = null ,JsonData incomeData=null,JsonData debtInfor=null,JsonData borrowRecordData=null,JsonData borrowBarodInfor=null)
	{
		if (null != dataManager)
		{
			HandlerPlayerDataInfor (player,dataManager);
//			player.totalMoney = int.Parse (dataManager["cash"].ToString());//现金
//			player.netTotalBalanceMoney = int.Parse (dataManager["assetTotalMoney"].ToString());//资产总额..人物信息
//			player.totalDebt =  int.Parse (dataManager["debtTotalMoney"].ToString());//负债总额 ..人物信息
//
//			player.netTotalPayment = int.Parse (dataManager["totalSpending"].ToString()); //总的支出
//
//			player.netTotalTake = int.Parse (dataManager["totalIncome"].ToString());   //总收入
//
//			player.innerFlowMoney =  int.Parse (dataManager["cashFlow"].ToString());      //内圈流动现金
//
//			player.netCheckDayNum = float.Parse (dataManager["closingDateMoney"].ToString());//结账日的金额
//
//			player.totalIncome  =  int.Parse (dataManager["nonLaborIncome"].ToString());//费劳务收入
//
//			player.netTotalDebtMoney = int.Parse (dataManager["debtTotalMoney"].ToString());
//
//			player.qualityScore = int.Parse (dataManager ["qualityIntegral"].ToString());         //品质积分
//			player.timeScore =  int.Parse (dataManager["timeIntegral"].ToString());  //时间积分
//			player.childNum =  int.Parse (dataManager["haveChildNumber"].ToString());
		}

		if (null != assetsData)
		{
			HandlerPlayerAssetsInfor (player,assetsData);
		}

		if (null != incomeData)
		{
			
		}

		if (null != debtInfor)
		{   
//			HandlerDebtList (player, debtInfor);
		}

		if (null != borrowRecordData)
		{
			var borrowRecord = borrowRecordData["loanRecords"];
			if (borrowRecord.IsArray)
			{
				player.borrowList.Clear();
				for (var i = 0; i < borrowRecord.Count; i++)
				{
					var tmpdata = borrowRecord[i];
					var tmpvo = new BorrowVo();
					tmpvo.times = i + 1;
					tmpvo.bankborrow = int.Parse( tmpdata["bankLoan"].ToString());
					tmpvo.bankdebt=int.Parse(tmpdata["bankInterest"].ToString());
					tmpvo.bankRate = tmpdata ["bankInterestRate"].ToString ();

					tmpvo.cardborrow = int.Parse (tmpdata["creditLoan"].ToString());
					tmpvo.carddebt = int.Parse (tmpdata["creditInterest"].ToString());
					tmpvo.cardRate = tmpdata["creditInterestRate"].ToString();
					player.borrowList.Add (tmpvo);
				}
			}

			player.netRecordAlreadyBorrow = int.Parse (borrowRecordData["totalLoan"].ToString());
			player.netRecordCanBorrow = int.Parse (borrowRecordData["canLoan"].ToString());
			player.netRecordLimitBorrow = int.Parse (borrowRecordData["ceiling"].ToString());
		}

		// 处理还款信息
//		HandlerBorrowInfor(player,borrowBarodInfor);
	}

    /// <summary>
    /// Handlers the player assets infor.处理玩家资产数据
    /// </summary>
    /// <param name="player">Player.</param>
    /// <param name="assetManaget">Asset managet.</param>
    public static void HandlerPlayerAssetsInfor(PlayerInfo player , JsonData assetsData)
	{
		if (null != assetsData)
		{
			var bigchanceList = assetsData["bigChances"];
			if (bigchanceList.IsArray == true)
			{
				List<Opportunity> opportunityList = new List<Opportunity> ();
				for (var i = 0; i < bigchanceList.Count; i++)
				{
					var tmpdata=  bigchanceList[i];
					if (((IDictionary)tmpdata).Contains ("id")==true)
					{
						var opportunityCard = ToOpportunityCard (tmpdata);
						opportunityList.Add (opportunityCard);
					}
				}
				player.opportCardList = opportunityList;
			}

			var sharechanceList = assetsData["stocks"];
			if (sharechanceList.IsArray == true)
			{
				List<ChanceShares> shareList = new List<ChanceShares> ();
				for (var i = 0; i < sharechanceList.Count; i++)
				{
					var tmpdata=  sharechanceList[i];
					if (((IDictionary)tmpdata).Contains ("id") == true)
					{						
						var chanceshare = ToChanceSharesCard (tmpdata);
						shareList.Add (chanceshare);
					}
				}
				player.shareCardList = shareList;
			}

			var fixedchanceList = assetsData["smallChances"];
			if (fixedchanceList.IsArray == true)
			{
				List<ChanceFixed> fixedList = new List<ChanceFixed> ();
				for (var i = 0; i < fixedchanceList.Count; i++)
				{
					var tmpdata = fixedchanceList[i];
					if (((IDictionary)tmpdata).Contains ("id") == true)
					{
						var fixedvo = ToFixedChanceCard (tmpdata);
						fixedList.Add (fixedvo);
					}
				}
				player.chanceFixedCardList = fixedList;
			}
		}

	}

    /// <summary>
    ///  处理玩家，金币，流动现金，品质积分，时间积分数据
    /// </summary>
    /// <param name="player"></param>
    /// <param name="dataManager"></param>
	public static void HandlerPlayerDataInfor(PlayerInfo player ,JsonData dataManager=null)
	{
		if (null != dataManager)
		{
			player.totalMoney = int.Parse (dataManager["cash"].ToString());//现金
			player.netTotalBalanceMoney = int.Parse (dataManager["assetTotalMoney"].ToString());//资产总额..人物信息
			player.totalDebt =  int.Parse (dataManager["debtTotalMoney"].ToString());//负债总额 ..人物信息

			player.netTotalPayment = int.Parse (dataManager["totalSpending"].ToString()); //总的支出

			player.netTotalTake = int.Parse (dataManager["totalIncome"].ToString());   //总收入

			var cashFlow = float.Parse (dataManager["cashFlow"].ToString()); 

			if (((IDictionary)dataManager).Contains ("initCashFlow"))
			{
				var initFlow = float.Parse (dataManager ["initCashFlow"].ToString ());
				player.netInitInnerFlowMoney = initFlow;
				cashFlow += initFlow;
			}
			player.innerFlowMoney = cashFlow;     //内圈流动现金

			player.netCheckDayNum = float.Parse (dataManager["closingDateMoney"].ToString());//结账日的金额

			player.totalIncome  =  int.Parse (dataManager["nonLaborIncome"].ToString());//费劳务收入

			player.netTotalDebtMoney = int.Parse (dataManager["debtTotalMoney"].ToString());

			player.qualityScore = int.Parse (dataManager ["qualityIntegral"].ToString());         //品质积分
			player.timeScore =  int.Parse (dataManager["timeIntegral"].ToString());  //时间积分
			player.childNum =  int.Parse (dataManager["haveChildNumber"].ToString());
		}
	}

	/// <summary>
	/// Handlers the borrow infor.处理可以借款的信息
	/// </summary>
	/// <param name="borrowInfor">Borrow infor.</param>
	public static void HandlerBorrowInfor(PlayerInfo player ,JsonData borrowBarodInfor=null)
	{
		if (null != borrowBarodInfor)
		{
			/*
			    "bankCanLoan": 27500,              
                "bankTotalInterest": 0,
                "bankTotalLoan": 0,
                "bankLoanLimit": 27500,
                "creditCanLoan": 8250,
                "creditLoanLimit": 8250,
                "creditTotalInterest": 0,
                "creditTotalLoan": 0,
                "totalCanLoan": 35750,
                "totalLoan": 0,
                "totalLoanLimit": 35750
			*/

			var boInfor = borrowBarodInfor["roleLoanInfo"];

			player.netBorrowBoardBankCanBorrow = int.Parse(boInfor ["bankCanLoan"].ToString());//: 33700,银行可贷款
			player.netBorrowBoardBankTotalBorrow = int.Parse(boInfor["bankTotalLoan"].ToString());//: 212,银行累计利息
			player.netBorrowBoradBankTotalDebt =  int.Parse(boInfor["bankTotalInterest"].ToString());//: 2121,银行累计贷款
			player.netBorrowBoardBankLoanLimit = int.Parse(boInfor["bankLoanLimit"].ToString());

			player.netBorrowBoardCardCanBorrow = int.Parse(boInfor["creditCanLoan"].ToString());//: 10110,信用卡可贷款
			player.netBorrowBoardCardTotalDebt =  int.Parse(boInfor["creditTotalInterest"].ToString());//: 0,信用卡累计利息
			player.netBorrowBoardCardTotalBorrow = int.Parse(boInfor["creditTotalLoan"].ToString());//: 信用卡累计贷款 
			player.netBorrowBoardCardLoanLimit = int.Parse(boInfor["creditLoanLimit"].ToString());

			player.netRecordAlreadyBorrow = int.Parse (boInfor["totalLoan"].ToString());
			player.netRecordCanBorrow = int.Parse (boInfor["totalCanLoan"].ToString());
			player.netRecordLimitBorrow = int.Parse (boInfor["totalLoanLimit"].ToString());


			if (((IDictionary)borrowBarodInfor).Contains ("roleBasicDebtInfo"))
			{
				var basicDebt = borrowBarodInfor["roleBasicDebtInfo"];//basicLoanList

				if (basicDebt.IsArray)
				{
					player.basePayList.Clear ();

					if (player.isEnterInner == false)
					{
						for (var i = 0; i < basicDebt.Count; i++)
						{
							var tmpdata = basicDebt[i];
							var tmpvo = new PaybackVo ();
							tmpvo.title = tmpdata["debtName"].ToString();
							tmpvo.borrow=int.Parse(tmpdata["debtMoney"].ToString());
							tmpvo.debt  =int.Parse(tmpdata["debtInterest"].ToString());
							tmpvo.netType = 0;
							player.basePayList.Add (tmpvo);
						}
					}
				}
			}

			if (((IDictionary)borrowBarodInfor).Contains ("roleAddNewDebtInfo"))
			{
				player.paybackList.Clear ();
				var bankloanList=borrowBarodInfor["roleAddNewDebtInfo"];
				if (bankloanList.IsArray)
				{
					for (var i = 0; i < bankloanList.Count; i++)
					{
						var tmpdata = bankloanList[i];
						var tmpvo = new PaybackVo ();
						tmpvo.title = tmpdata["debtName"].ToString();
						tmpvo.borrow=int.Parse(tmpdata["debtMoney"].ToString());
						tmpvo.debt  =int.Parse(tmpdata["debtInterest"].ToString());
						tmpvo.netType = 1;
						player.paybackList.Add (tmpvo);
					}
				}	
			}

			if (((IDictionary)borrowBarodInfor).Contains ("roleLoanRecordInfo"))
			{
				var baordInfor=borrowBarodInfor["roleLoanRecordInfo"];
				var borrowRecord=baordInfor["loanRecords"];

				if (borrowRecord.IsArray)
				{
					player.borrowList.Clear();
					for (var i = 0; i < borrowRecord.Count; i++)
					{
						var tmpdata = borrowRecord[i];
						var tmpvo = new BorrowVo();
						tmpvo.times = i + 1;
						tmpvo.bankborrow = int.Parse( tmpdata["bankLoan"].ToString());
						tmpvo.bankdebt=int.Parse(tmpdata["bankInterest"].ToString());
						tmpvo.bankRate = tmpdata ["bankInterestRate"].ToString ();

						tmpvo.cardborrow = int.Parse (tmpdata["creditLoan"].ToString());
						tmpvo.carddebt = int.Parse (tmpdata["creditInterest"].ToString());
						tmpvo.cardRate = tmpdata["creditInterestRate"].ToString();
						player.borrowList.Add (tmpvo);
					}
				}
			}		

		}
	}

	/// <summary>
	/// Handlers the cards datas.根据不同的卡牌类型，处理不同的卡牌数据
	/// </summary>
	public static void HandlerCardsDatas(int cardType , JsonData value)
	{
		switch (cardType)
		{
		case (int)SpecialCardType.bigChance:
			GameModel.GetInstance.netOpportunityCard = ToOpportunityCard (value);
			break;
		case (int)SpecialCardType.CharityType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.CheckDayType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard(value);
			break;
		case (int)SpecialCardType.fixedChance:
			GameModel.GetInstance.netFixedChancCard = ToFixedChanceCard (value);
			break;
		case (int)SpecialCardType.GiveChildType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.HealthType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.inFate:
			GameModel.GetInstance.netInnerFateCard = ToInnerFateCard (value);
			break;
		case (int)SpecialCardType.InnerCheckDayType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.InnerHealthType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.InnerStudyType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		case (int)SpecialCardType.investment:
			GameModel.GetInstance.netInvestmentCard = ToInvestmentCard (value);
			break;
		case (int)SpecialCardType.outFate:
			GameModel.GetInstance.netOuterFateCard = ToOuterFateCard (value);
			break;

		case (int)SpecialCardType.qualityLife:
			GameModel.GetInstance.netQualityCard = ToQualityLifeCard (value);
			break;
		case (int)SpecialCardType.richRelax:
			GameModel.GetInstance.netRelaxCard = ToRelaxCard (value);
			break;
		case (int)SpecialCardType.risk:
			GameModel.GetInstance.netRiskCard = ToRiskCard (value);
			break;

		case (int)SpecialCardType.sharesChance:
			GameModel.GetInstance.netChanceShareCard = ToChanceSharesCard (value);
			break;
		case (int)SpecialCardType.StudyType:
			GameModel.GetInstance.netSpecialCard = ToSpecialCard (value);
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// Tos the fixed chance. 根据数据返回小机会固定资产卡牌
	/// </summary>
	/// <returns>The fixed chance.</returns>
	/// <param name="value">Value.</param>
	public static ChanceFixed ToFixedChanceCard(JsonData value)
	{
		var cardVo=new ChanceFixed();

		cardVo.id = int.Parse (value["id"].ToString());
		// kaid of kind
		cardVo.belongsTo=int.Parse(value["type"].ToString());
		//card name
		cardVo.title=value["name"].ToString();

		cardVo.cardPath=value["path"].ToString();

		//card infor 
		cardVo.desc=value["instructions"].ToString();

		// coefficient 
		cardVo.baseNumber=int.Parse(value["number"].ToString());

		// show coast
		cardVo.coast=value["cost"].ToString();

		// price range
		cardVo.sale=value["sellPrice"].ToString();

		// pay 
		cardVo.payment=float.Parse(value["downPayment"].ToString());

		// profit show
		cardVo.profit=value["investmentIncome"].ToString();

		//need pay when sale
		cardVo.mortgage=float.Parse(value["mortgageLoan"].ToString());

		// 1 timeScore 2 qualityScore
		cardVo.scoreType=int.Parse(value["integralType"].ToString());

		// socres
		cardVo.scoreNumber=int.Parse(value["integralNumber"].ToString());

		//extend income with card
		cardVo.income=float.Parse(value["nonLaborIncome"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(value["cardIntegral"].ToString());

		return cardVo;
	}

	/// <summary>
	/// Tos the chance shares. 根据数据返回股票卡牌数据
	/// </summary>
	/// <returns>The chance shares.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static ChanceShares ToChanceSharesCard(JsonData jsonValue)
	{
		var cardVo = new ChanceShares ();

		Console.Warning.WriteLine (jsonValue.ToString());

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		// kind of card
		cardVo.belongsTo = int.Parse(jsonValue["type"].ToString());

		// name of card 
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		// infor
		cardVo.desc=jsonValue["instructions"].ToString();

		// code 
		cardVo.ticketCode=jsonValue["stockCode"].ToString();

		// name of ticket
		cardVo.ticketName=jsonValue["billName"].ToString();

		// pay of card 
		cardVo.payment=int.Parse(jsonValue["todayPrice"].ToString());

		// show the rate of trun
		cardVo.returnRate=jsonValue["investmentIncome"].ToString();

		//show 
		cardVo.shareOut=jsonValue["dividend"].ToString();

		// score for quality
		cardVo.qualityScore=int.Parse(jsonValue["qualityIntegral"].ToString());

		// infor of qualityScore
		cardVo.qualityDesc=jsonValue["qualityIntegralInstruction"].ToString();

		// income without labor
		cardVo.income=int.Parse(jsonValue["nonLaborIncome"].ToString());

		// show the rangeprice
		cardVo.priceRagne=jsonValue["priceScope"].ToString();

		cardVo.shareNum = int.Parse (jsonValue["stockNumber"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore = int.Parse(jsonValue["cardIntegral"].ToString());


		return cardVo;
	}

	/// <summary>
	/// Tos the inner fate. 根据数据返回内圈命运的接口
	/// </summary>
	/// <returns>The inner fate.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static InnerFate ToInnerFateCard(JsonData jsonValue)
	{
		var cardVo = new InnerFate ();


		cardVo.id = int.Parse (jsonValue["id"].ToString());

		//card name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		// card infor 
		cardVo.desc=jsonValue["instructions"].ToString();

		/// type of fate 1 dice   2 insurance  3 loss
		cardVo.fateType=int.Parse(jsonValue["type"].ToString());

		// method of count payment  1 plus  2 mulitiply;
		cardVo.paymenyMethod=int.Parse(jsonValue["payAlgorithm"].ToString());

		// type of paymeny 1 money  2 income 3 timeScore 4 quality
		cardVo.paymenyType=int.Parse(jsonValue["payType"].ToString());

		// payment 
		cardVo.paymeny=float.Parse(jsonValue["payNumber"].ToString());

		//fate for card by id
		cardVo.relateID=int.Parse(jsonValue["correlationId"].ToString());

		// prise num by dice
		cardVo.dice_prise=float.Parse(jsonValue["diceRewardMoney"].ToString());

		// 1 bigger  2 less
		cardVo.dice_condition=int.Parse(jsonValue["diceCondition"].ToString());

		// target dice number;
		cardVo.dice_number=int.Parse(jsonValue["diceNumber"].ToString());

		// prise type 1 money  2 income 3 timeScore 4 quality
		cardVo.dice_prise_type=int.Parse(jsonValue["diceRewardType"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;
	}

	/// <summary>
	/// Tos the investment. 根据数据反馈投资卡牌
	/// </summary>
	/// <returns>The investment.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static Investment ToInvestmentCard(JsonData jsonValue)
	{

		var cardVo = new Investment ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());


		//card name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		//card infor
		cardVo.desc=jsonValue["instructions"].ToString();

		//card 
		cardVo.payment=float.Parse(jsonValue["investmentPay"].ToString());

		// show profit
		cardVo.profit=jsonValue["investmentIncome"].ToString();

		// income 
		cardVo.income=float.Parse(jsonValue["flowCash"].ToString());

		//whether use dice  0 no , 1 yes
		cardVo.isDice=int.Parse(jsonValue["ifRollDice"].ToString());

		//1 bigger  2 less
		cardVo.disc_condition=int.Parse(jsonValue["diceConditions"].ToString());

		//target desc number
		cardVo.disc_number=int.Parse(jsonValue["diceNumber"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;		
	}

	/// <summary>
	/// Tos the opportunity. 根据信息返回大机会卡牌
	/// </summary>
	/// <returns>The opportunity.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static Opportunity ToOpportunityCard(JsonData jsonValue)
	{

		var cardVo = new Opportunity ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		// kind of card
		cardVo.belongsTo= int.Parse(jsonValue["type"].ToString());

		//name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		//infor
		cardVo.desc=jsonValue["instructions"].ToString();

		//coefficient 
		cardVo.baseNumber=int.Parse(jsonValue["number"].ToString());

		//cost show 
		cardVo.cost=jsonValue["cost"].ToString();

		//sale show
		cardVo.sale=jsonValue["sellPrice"].ToString();

		// kouchu
		cardVo.payment=float.Parse(jsonValue["downPayment"].ToString());

		// profit show
		cardVo.profit=jsonValue["investmentIncome"].ToString();

		// need pay when sale 
		cardVo.mortgage=float.Parse(jsonValue["mortgageLoan"].ToString());

		// extended income with card 
		cardVo.income=float.Parse(jsonValue["nonLaborIncome"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;
	}

	/// <summary>
	/// Tos the outer fate. 根据数据返回命运开牌
	/// </summary>
	/// <returns>The outer fate.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static OuterFate ToOuterFateCard(JsonData jsonValue)
	{
		var cardVo = new OuterFate ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		// card name 卡牌名字
		cardVo.title=jsonValue["name"].ToString();

		// 卡牌路径
		cardVo.cardPath=jsonValue["path"].ToString();

		// card infor 描述
		cardVo.desc=jsonValue["instructions"].ToString();

		// handle type of fate 0 no sale(plus)  1 sale  3 nosale(multy) 命运的类型 	0不卖的，1是买的，3是倍数变化的 
		cardVo.fateType=int.Parse(jsonValue["processType"].ToString());

		// whether handler money 0 no  1  , yes 是否是处理现金的，0是不处理 1，是处理
		cardVo.isHandle_peymeny=int.Parse(jsonValue["ifProcessMoney"].ToString());

		// money number    处理资金的话，是多少钱卖出
		cardVo.payment=float.Parse(jsonValue["processMoney"].ToString());

		// 1 handle single   2 handler a kinds of cards  处理的范围   1是单个处理  2处理一类
		cardVo.handleRange=int.Parse(jsonValue["processWay"].ToString());

		// fate for card 需要处理的种类 id 或者一类 
		cardVo.relateID=int.Parse(jsonValue["relevanceId"].ToString());

		//whether handler income  0 no , 1 yes 是否处理非劳务收入 0 不处理  1处理 
		cardVo.isHandle_income=int.Parse(jsonValue["ifProcessNonLobarIncome"].ToString());

		// hand income type 1 opearete a number  , 2 to be a num  1加减乘一个数  2变到某个数
		cardVo.handler_income_type=int.Parse(jsonValue["nonLobarIncomeChangeType"].ToString());

		// income number  非劳务收入的变化的值
		cardVo.handler_income_number=float.Parse(jsonValue["nonLobarIncomeChange"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());


		return cardVo;
	}

	/// <summary>
	/// Tos the quality life. 根据数据返回品质生活
	/// </summary>
	/// <returns>The quality life.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static QualityLife ToQualityLifeCard(JsonData jsonValue)
	{
		var cardVo = new QualityLife ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		//card name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		//card infor
		cardVo.desc=jsonValue["instructions"].ToString();

		// num 
		cardVo.payment=float.Parse(jsonValue["consumeMoney"].ToString());

		// score of time;
		cardVo.timeScore=float.Parse(jsonValue["timeIntegral"].ToString());

		// score of time
		cardVo.qualityScore=float.Parse(jsonValue["qualityIntegral"].ToString());

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;
	}

	/// <summary>
	/// Tos the relax. 根据数据，返回有钱有闲
	/// </summary>
	/// <returns>The relax.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static Relax ToRelaxCard(JsonData jsonValue)
	{
		var cardVo = new Relax ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		// card name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		// card infor
		cardVo.desc=jsonValue["instructions"].ToString();

		// pay money
		cardVo.payment=float.Parse(jsonValue["investmentPay"].ToString());

		// show profit
		cardVo.profit=jsonValue["investmentIncome"].ToString();

		// flow income 
		cardVo.income=float.Parse(jsonValue["flowCash"].ToString());

		// score of time ;
		cardVo.timeScore=float.Parse(jsonValue["timeIntegral"].ToString());


		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;
	}

	/// <summary>
	/// Tos the risk. 根据数据返回风险卡牌
	/// </summary>
	/// <returns>The risk.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static Risk ToRiskCard(JsonData jsonValue)
	{
//		Risk cardVo = Risk ();
		var cardVo = new Risk ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		// risk type 1 cost money only  2 extended choice
		cardVo.riskTpye=int.Parse(jsonValue["type"].ToString());

		// card name
		cardVo.title=jsonValue["name"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

		// card infor 
		cardVo.desc=jsonValue["instructionsOne"].ToString();

		// coast money
		cardVo.payment=float.Parse(jsonValue["moneyOne"].ToString());

		//infor of extended choice
		cardVo.desc2=jsonValue["instructionsTwo"].ToString();

		//extend paymeny
		cardVo.payment2=float.Parse(jsonValue["moneyTwo"].ToString());

		// type of score 0 none  1 timescore   2 qualityscore
		cardVo.scoreType=int.Parse(jsonValue["integralType"].ToString());

		//score name 
		cardVo.scoreName=jsonValue["integralName"].ToString();

		// scores 
		cardVo.score=int.Parse(jsonValue["integralValue"].ToString());

		// infor of score;
		cardVo.score_desc=jsonValue["integralInstruction"].ToString();

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;		
	}

	/// <summary>
	/// Tos the special card. 根据数据，返回特殊卡牌
	/// </summary>
	/// <returns>The special card.</returns>
	/// <param name="jsonValue">Json value.</param>
	public static SpecialCard ToSpecialCard(JsonData jsonValue)
	{
		var cardVo = new SpecialCard ();

		cardVo.id = int.Parse (jsonValue["id"].ToString());

		cardVo.title=jsonValue["name"].ToString();

		cardVo.desc=jsonValue["instructions"].ToString();

		cardVo.cardPath=jsonValue["path"].ToString();

        if(((IDictionary)jsonValue).Contains("tipData"))
        {
            var tipData = jsonValue["tipData"];

            var tipType = tipData["type"].ToString();
            var tipContent = tipData["content"].ToString();
            Client.UI.UIOtherCardWindowController.netKnowledge.title = tipType;
            Client.UI.UIOtherCardWindowController.netKnowledge.content = tipContent;
            
        }

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		cardVo.rankScore=int.Parse(jsonValue["cardIntegral"].ToString());

		return cardVo;
	}
}

	
