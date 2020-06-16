using System;

namespace Metadata
{
    /// <summary>
    /// 教练系统话术
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class GameTips:Config
	{
		public string newTip1;
		public string newTip2;
		public string newTip3;
		public string newTip4;
		public string newTip5;
		public string newTip6;

		public string oldTip1;
		public string oldTip2;
		public string oldTip3;
		public string oldTip4;
		public string oldTip5;
		public string oldTip6;
		public string oldTip7;
		public string oldTip8;

		public string oldMoreTip1;
		public string oldMoreTip2;
		public string oldMoreTip3;
		public string oldMoreTip4;
		public string oldMoreTip5;
		public string oldMoreTip6;

		/// <summary>
		/// The enter tip. 进入内圈的总结s
		/// </summary>
		public string enterTip;

		public string innerTip1;
		public string innerTip2;
		public string innerTip3;
		public string innerTip4;
		public string innerTip5;
		public string innerTip6;
		public string innerTip7;
		public string innerTip8;
		public string innerTip9;
		public string innerTip10;
		public string innerTip11;
		public string innerTip12;
		public string innerTip13;
		public string innerTip14;
		public string innerTip15;

		public string enterResult1;
		public string enterResult2;
		public string enterResult3;

        /// <summary>
        /// 风险卡牌结束后评语
        /// </summary>
        public string overOuterCardRisk;
        /// <summary>
        /// 外圈资产结束评语
        /// </summary>
        public string overOuterCardOuerFate;

        /// <summary>
        /// 小机会资产结束评语
        /// </summary>
        public string overOuterCardSmallFixed;
        /// <summary>
        /// 小机会股票结束评语
        /// </summary>
        public string overOuterCardSmallShare;

        /// <summary>
        /// 卖出小机会股票
        /// </summary>
        public string overOuterCardSellShare;

        /// <summary>
        /// 大机会结束评语
        /// </summary>
        public string overOuterCardChallenge;
        /// <summary>
        /// 慈善事业结束评语
        /// </summary>
        public string overOuterCardCharity;
        /// <summary>
        /// 进修学习结束评语
        /// </summary>
        public string overOuterCardStudy;
        /// <summary>
        /// 外圈健康管理结束话语
        /// </summary>
        public string overOuterCardHealth;
        /// <summary>
        /// 外圈结账日结束话语
        /// </summary>
        public string overOuterCardCheckOut;
        /// <summary>
        /// 外圈生孩子结束话术
        /// </summary>
        public string overOuterGiveChild;
        /// <summary>
        /// 外圈超生的结束话术
        /// </summary>
        public string overOuterMoreChild;
        /// <summary>
        /// 外圈发红包结束话术
        /// </summary>
        public string overOuterSendRed;

        /// <summary>
        /// 内圈有钱有闲结束话术
        /// </summary>
        public string overInnerRelax;
        /// <summary>
        /// 内圈品质生活结束话术
        /// </summary>
        public string overInnerQuality;
        /// <summary>
        /// 内圈投资结束话术
        /// </summary>
        public string overInnerInvestment;

        /// <summary>
        /// 内圈命运结束话术
        /// </summary>
        public string overInnerFate;

        /// <summary>
        /// 内圈进修学习结束话术
        /// </summary>
        public string overInnerStudy;
        /// <summary>
        /// 内圈结账日结束话术
        /// </summary>
        public string overInnerCheckOut;

        /// <summary>
        /// 内圈健康管理结束话术
        /// </summary>
        public string overInnerHealth;

	}
}

