using System;

namespace Metadata
{
    /// <summary>
    /// 特殊卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class SpecialCard:Template
	{
		public string title;

		public string desc;

		public string cardPath;

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

