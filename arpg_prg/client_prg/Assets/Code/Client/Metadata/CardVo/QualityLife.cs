using System;

namespace Metadata
{
    /// <summary>
    ///  内圈品质生活卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class QualityLife :Template
	{
		//card name
		public string title;

		public string cardPath;

		//card infor
		public string desc;

		// num 
		public float payment;

		// score of time;
		public float timeScore;

		// score of time
		public float qualityScore;

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

