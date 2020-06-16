using System;

namespace Metadata
{
    /// <summary>
    ///  小机会股票卡牌数据
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class ChanceShares:Template
	{
		// kind of card
		public int belongsTo;

		// name of card 
		public string title;

		public string cardPath;

		// infor
		public string desc;

		// code 
		public string ticketCode;

		// name of ticket
		public string ticketName;

		// pay of card 
		public int payment;

		// show the rate of trun
		public string returnRate;

		//show 
		public string shareOut;

		// score for quality
		public int qualityScore;

		// infor of qualityScore
		public string qualityDesc;

		// income without labor
		public int income;

		// show the rangeprice
		public string priceRagne;

		public int shareNum;

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		public int rankScore = 0;


        /// <summary>
        /// 放弃卡牌对应的积分
        /// </summary>
        public int quitScore = 0;


    }
}

