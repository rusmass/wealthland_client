using System;

namespace Metadata
{
    /// <summary>
    ///  小机会固定资产卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class ChanceFixed:Template
	{
		// kaid of kind
		public int belongsTo;

		//card name
		public string title;

		public string cardPath;

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

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		public int rankScore=0;

        /// <summary>
        /// 放弃卡牌对应的积分
        /// </summary>
        public int quitScore = 0;


    }
}

