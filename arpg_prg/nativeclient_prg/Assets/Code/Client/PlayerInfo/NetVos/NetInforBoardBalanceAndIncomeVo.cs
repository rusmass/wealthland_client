using System;
using System.Collections.Generic;

namespace Client
{
	public class NetInforBoardBalanceAndIncomeVo
	{
		public NetInforBoardBalanceAndIncomeVo ()
		{
		}

		/// <summary>
		/// The total balance money. 总的资产收入
		/// </summary>
//		public int totalBalanceMoney=0;	//var totalBalanceMoney = balanceInfor["assetTotalMoney"]; //"assetTotalMoney": 0,
		 //var bigChanceList=balanceInfor["bigChances"];
		 //var smallFixedList=balanceInfor["smallChances"];
		 //var chanceShareList=balanceInfor["stocks"];

		//var incomeInfor=backbody["data"]["roleIncomeInfo"];
		public string laborTxt=""; //var laborTxt= incomeInfor["laborIncome"]["name"];
		public int  laoorMoney =0; //var laborMoney=incomeInfor["laborIncome"]["money"];
		public float  totalIncome = 0; //var totoalIncome=incomeInfor["totalIncome"];
		public float  totalNonLaborIncome = 0;// var totalNonLaborIncome=incomeInfor["totalNonLaborIncome"];
		public List<InforRecordVo> nonIncomeList=new List<InforRecordVo>() ; //= incomeInfor ["nonLaborIncomeList"];

	}
}

