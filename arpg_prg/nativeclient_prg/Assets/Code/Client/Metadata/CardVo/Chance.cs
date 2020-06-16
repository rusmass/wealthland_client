using System;

namespace Metadata
{
    /// <summary>
    ///  机会卡牌
    /// </summary>
    public partial class Chance
	{

		// kaid of kind
		public int belongsTo;

		public int id;

		//card name
		public string title;

		//card image path
		public string cardPath;

		public string cardIntroduce;

		//card infor 
		public string desc;

		// coefficient 
		public int baseNumber;

		// show coast
		public string coast;

		// price range
		public string sale;

		// pay 
		public float payment;

		// profit show
		public string profit;

		//need pay when sale
		public float mortgage;

		// 1 timeScore 2 qualityScore
		public int scoreType;

		// socres
		public int scoreNumber;

		//extend income with card
		public float income=0;

		///////// 股票
		public int cash_belongsTo;


		public string cash_title;

		public string cash_cardPath;

		// infor
		public string cash_desc;

		// code 
		public string cash_ticketCode;

		// name of ticket
		public string cash_ticketName;

		// pay of card 
		public int cash_payment;

		// show the rate of trun
		public string cash_returnRate;

		//show 
		public string cash_shareOut;

		// score for quality
		public int cash_qualityScore;

		// infor of qualityScore
		public string cash_qualityDesc;

		// income without labor
		public int cash_income;

		// show the rangeprice
		public string cash_priceRagne;

		public int cash_shareNum;
	}
}

