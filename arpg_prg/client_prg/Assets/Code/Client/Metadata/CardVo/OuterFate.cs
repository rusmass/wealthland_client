using System;

namespace Metadata
{
    /// <summary>
    ///  外圈命运卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class OuterFate : Template
	{                    
		// card name 卡牌名字
		public string title;

		// 卡牌路径
		public string cardPath;

		// card infor 描述
		public string desc;

		// handle type of fate 0 no sale(plus)  1 sale  3 nosale(multy) 命运的类型 	0不卖的，1是买的，3是倍数变化的 
		public int fateType;

		// whether handler money 0 no  1  , yes 是否是处理现金的，0是不处理 1，是处理
		public int isHandle_peymeny;

		// money number    处理资金的话，是多少钱卖出
		public float payment;

		// 1 handle single   2 handler a kinds of cards  处理的范围   1是单个处理  2处理一类
		public int handleRange;

		// fate for card 需要处理的种类 id 或者一类 
		public int relateID;

		//whether handler income  0 no , 1 yes 是否处理非劳务收入 0 不处理  1处理 
		public int isHandle_income;

		// hand income type 1 opearete a number  , 2 to be a num  1加减乘一个数  2变到某个数
		public int handler_income_type;

		// income number  非劳务收入的变化的值
		public float handler_income_number;

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

