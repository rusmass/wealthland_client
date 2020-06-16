using System;

namespace Metadata
{
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class SubtitleCount:Config
	{
        /// <summary>
        ///  购买机会卡牌
        /// </summary>
        public string buyChanceCard2;
        /// <summary>
        /// 出售机会卡牌
        /// </summary>
        public string quitChanceCard2;
        /// <summary>
        /// 出售命运卡牌 获得金币
        /// </summary>
        public string salseFateCard3;
        /// <summary>
        /// 放弃命运卡牌
        /// </summary>
        public string quitFateCard1;
        /// <summary>
        /// 股票一股变两股
        /// </summary>
        public string effectShareTwoInOne2;
        /// <summary>
        /// 股票两股变一股
        /// </summary>
        public string effectShareOneInTwo2;
        /// <summary>
        /// 缩减非劳务收入
        /// </summary>
        public string effectMutipleReduceIncome3;
        /// <summary>
        /// 命运非劳务收入增加
        /// </summary>
        public string effectIncreaseIncone3;
        /// <summary>
        /// 命运卡牌非劳务减少
        /// </summary>
        public string effectReduceIncome3;
        /// <summary>
        /// 非劳务收入减少
        /// </summary>
        public string riskCoast3;
        /// <summary>
        /// 结账日结算
        /// </summary>
        public string getCheckOutMoney2;
        /// <summary>
        /// 生孩子介绍
        /// </summary>
        public string getChildDesc2;

        /// <summary>
        /// 参加慈善事业
        /// </summary>
        public string involveCharity2;
        /// <summary>
        /// 参加健康管理
        /// </summary>
        public string involveHealth2;
        /// <summary>
        /// 参与进修学习
        /// </summary>
        public string involeStudy2;
        /// <summary>
        /// 投资获得金币
        /// </summary>
        public string timeAndMondeyGetMoney3;
        /// <summary>
        /// 投资获得金币和时间积分
        /// </summary>
        public string timeAndMoneyGateMoneyAndScore4;
        /// <summary>
        /// 品质生活，消耗金币和时间积分获得品质积分
        /// </summary>
        public string qualityGetSocre4;
        /// <summary>
        /// 都投资增加现金 支付现金
        /// </summary>
        public string investmentGetMoney3;
        /// <summary>
        /// 金币>10000获得大机会卡牌
        /// </summary>
        public string getOpportChance1;

        /// <summary>
        /// 内圈命运  购买股份损失
        /// </summary>
        public string innerFateShareLose2;
        /// <summary>
        /// 内圈命运 购买股份大赚
        /// </summary>
        public string innerFateShareGet2;
        /// <summary>
        /// 内圈命运 购买保险
        /// </summary>
        public string innerFateSafe2;
        /// <summary>
        /// 内圈命运 损失
        /// </summary>
        public string innerFateLose2;


        /// <summary>
        /// 资金不足
        /// </summary>
        public string lackOfGold;

        /// <summary>
        /// 机会卡牌
        /// </summary>
        public string cardChance;
        /// <summary>
        /// 命运卡牌
        /// </summary>
		public string cardFate;
        /// <summary>
        /// 风险卡牌
        /// </summary>
		public string cardRisk;
        /// <summary>
        /// 结账日卡牌
        /// </summary>
        public string cardCheckOut;
        /// <summary>
        /// 生孩子卡牌
        /// </summary>
        public string cardGetChild;
        /// <summary>
        /// 慈善事业卡牌
        /// </summary>
        public string cardCharity;
        /// <summary>
        /// 健康管理卡牌
        /// </summary>
        public string cardHealth;
        /// <summary>
        /// 进修学习卡牌
        /// </summary>
        public string cardStudy;
        /// <summary>
        /// 有钱有闲卡牌
        /// </summary>
        public string cardTimeAndMoney;
        /// <summary>
        /// 品质生活卡牌
        /// </summary>
        public string cardQualityLife;
        /// <summary>
        /// 自由选择卡牌
        /// </summary>
        public string cardFreeChoice;
        /// <summary>
        /// 投资卡牌
        /// </summary>
        public string cardInvestment;
        /// <summary>
        /// 大机会卡牌
        /// </summary>
		public string cardOpportunity;

        /// <summary>
        /// 时间积分不足
        /// </summary>
        public string lackOfTimeScore;

		public string AllEffect;
        /// <summary>
        /// 用户受命运影响
        /// </summary>
        public string userIsEffectedByFate;

        /// <summary>
        /// 点击小猪弹提示
        /// </summary>
        public string pigCashflow;
        /// <summary>
        /// 点击小猪弹非劳务收入提示
        /// </summary>
        public string pigIncome;
        //点击小猪弹时间提示
        public string pigTimetip;
        /// <summary>
        /// 点击小猪弹品质积分提示
        /// </summary>
        public string pigQualitytip;

        /// <summary>
        /// 提示进入什么卡牌
        /// </summary>
        public string enterCard;

		/// <summary>
		/// 当轮到npc玩的时候，玩家不能查看别人信息
		/// </summary>
		public string noChoiceToCheck;

		public string moreChildForBoard;
        /// <summary>
        /// 超生的提示话
        /// </summary>
        public string moreChildForTip;

        /// <summary>
        /// 投资掷色子成功
        /// </summary>
        public string rollSuccessInvestment;

        /// <summary>
        /// 内圈命运掷色子成功
        /// </summary>
        public string rollSuccessInnerfate;

        /// <summary>
        /// 遇到掷色子卡牌，失败
        /// </summary>
        public string rollFail;
	}
}

