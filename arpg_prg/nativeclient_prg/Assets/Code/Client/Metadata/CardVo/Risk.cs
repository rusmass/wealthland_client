using System;

namespace Metadata
{
    /// <summary>
    /// 外圈风险卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial  class Risk :Template
	{
        /// <summary>
        /// 风险类型 1 cost money only  2 extended choice
        /// </summary>
		public int riskTpye;

		/// <summary>
        /// 卡牌名称
        /// </summary>
		public string title;
        /// <summary>
        /// 卡牌路径
        /// </summary>
		public string cardPath;

		/// <summary>
        /// 卡牌的信息
        /// </summary>
		public string desc;

        /// <summary>
        /// 卡牌提示信息的类型
        /// </summary>
        public int tipType=0;

        /// <summary>
        /// 花费的价格
        /// </summary>
        public float payment;

        /// <summary>
        /// 自由选择的文字说明
        /// </summary>
        public string desc2;

		/// <summary>
        /// 自由选择的花费
        /// </summary>
		public float payment2;

        /// <summary>
        /// 奖励积分的类型 0 none  1 timescore   2 qualityscore
        /// </summary>
        public int scoreType;

        /// <summary>
        /// 积分奖励的说明，时间积分还是品质积分
        /// </summary>
        public string scoreName;

        ///奖励的积分数 
        public int score;

        /// <summary>
        ///  infor of score 积分说明的话术
        /// </summary>
        public string score_desc;

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		public int rankScore=0;


	}
}

