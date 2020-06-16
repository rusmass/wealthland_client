using System;

namespace Metadata
{
    /// <summary>
    ///  内圈命运卡牌
    /// </summary>
    [ExportAttribute(ExportFlags.ExportRaw)]
	public partial class InnerFate:Template
	{
		//card name
		public string title;

		public string cardPath;

		// card infor 
		public string desc;

		/// type of fate 1 dice   2 insurance  3 loss
		public int fateType;

		// method of count payment  1 plus  2 mulitiply;
		public int paymenyMethod;

		// type of paymeny 1 money  2 income 3 timeScore 4 quality
		public int paymenyType;

		// payment 
		public float paymeny;

		//fate for card by id
		public int relateID;

		// prise num by dice
		public float dice_prise;

		// 1 bigger  2 less
		public int dice_condition;

		// target dice number;
		public int dice_number;

		// prise type 1 money  2 income 3 timeScore 4 quality
		public int dice_prise_type;

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

