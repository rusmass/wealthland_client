using System;

namespace Metadata
{
    /// <summary>
    /// 有钱有闲卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class Relax :Template
	{
		// card name
		public string title;

		public string cardPath;

		// card infor
		public string desc;

		// pay money
		public float payment;

		// show profit
		public string profit;

		// flow income 
		public float income;

		// score of time ;
		public float timeScore;

	
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

