using System;

namespace Metadata
{
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class SubtitleCount:Config
	{
		// 购买机会卡牌
		public string buyChanceCard2;
		// 出售机会卡牌
		public string quitChanceCard2;
		// 出售命运卡牌 获得金币
		public string salseFateCard3;
		// 放弃命运卡牌
		public string quitFateCard1;
		// 股票一股变两股
		public string effectShareTwoInOne2;
		// 股票两股变一股
		public string effectShareOneInTwo2; 
		// 缩减非劳务收入
		public string effectMutipleReduceIncome3;
		// 命运非劳务收入增加
		public string effectIncreaseIncone3;
		// 命运卡牌非劳务减少
		public string effectReduceIncome3;
		// 非劳务收入减少
		public string riskCoast3;
		// 结账日结算
		public string getCheckOutMoney2;
		// 生孩子介绍
		public string getChildDesc2;

		// 参加慈善事业
		public string involveCharity2;
		// 参加健康管理
		public string involveHealth2;
		// 参与进修学习
		public string involeStudy2;
		// 投资获得金币
		public string timeAndMondeyGetMoney3;
		// 投资获得金币和时间积分
		public string timeAndMoneyGateMoneyAndScore4;
		// 品质生活，消耗金币和时间积分获得品质积分
		public string qualityGetSocre4;
		// 都投资增加现金 支付现金
		public string investmentGetMoney3;
		// 金币>10000获得大机会卡牌
		public string getOpportChance1;

		// 内圈命运  购买股份损失
		public string innerFateShareLose2;
		// 内圈命运 购买股份大赚
		public string innerFateShareGet2;
		// 内圈命运 购买保险
		public string innerFateSafe2;
		// 内圈命运 损失
		public string innerFateLose2;


		// 资金不足
		public string lackOfGold;

		// 机会卡牌
		public string cardChance;
		public string cardFate;
		public string cardRisk;
		// 结账日卡牌
		public string cardCheckOut;
		// 生孩子卡牌
		public string cardGetChild;
		// 慈善事业卡牌
		public string cardCharity;
		// 健康管理卡牌
		public string cardHealth;
		// 进修学习卡牌
		public string cardStudy;
		// 有钱有闲卡牌
		public string cardTimeAndMoney;
		// 品质生活卡牌
		public string cardQualityLife;
		// 自由选择卡牌
		public string cardFreeChoice;
		// 投资卡牌
		public string cardInvestment;

		public string cardOpportunity;

		// 时间积分不足
		public string lackOfTimeScore;

		public string AllEffect;
		// 用户受命运影响
		public string userIsEffectedByFate;

		// 点击小猪弹提示
		public string pigCashflow;
		// 点击小猪弹非劳务收入提示
		public string pigIncome;
		// 点击小猪弹时间提示
		public string pigTimetip;
		// 点击小猪弹品质积分提示
		public string pigQualitytip;

		// 提示进入什么卡牌
		public string enterCard;

		/// <summary>
		/// 当轮到npc玩的时候，玩家不能查看别人信息
		/// </summary>
		public string noChoiceToCheck;

		public string moreChildForBoard;
		// 超生的提示话
		public string moreChildForTip;

		// 投资掷色子成功
		public string rollSuccessInvestment;

		// 内圈命运掷色子成功
		public string rollSuccessInnerfate;

		// 遇到掷色子卡牌，失败
		public string rollFail;
	}
}

