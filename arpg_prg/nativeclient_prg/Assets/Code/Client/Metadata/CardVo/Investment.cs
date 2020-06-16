using System;

namespace Metadata
{
    /// <summary>
    ///  内圈投资卡牌
    /// </summary>
    [ExportAttribute(ExportFlags.ExportRaw)]
	public partial class Investment : Template
	{
		//card name
		public string title;

		public string cardPath;

		//card infor
		public string desc;

		//card 
		public float payment;

		// show profit
		public string profit;

		// income 
		public float income;

		//whether use dice  0 no , 1 yes
		public int isDice;

		//1 bigger  2 less
		public int disc_condition;

		//target desc number
		public int disc_number;

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

