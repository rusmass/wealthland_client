using System;
using UnityEngine;

namespace Metadata
{
    /// <summary>
    ///  外圈大机会卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class Opportunity:Template
	{
		// kind of card
		public int belongsTo;

		//name
		public string title;


		public string cardPath;

		//infor
		public string desc;

		//coefficient 
		public int baseNumber;

		//cost show 
		public string cost;

		//sale show
		public string sale;

		// kouchu
		public float payment;

		// profit show
		public string profit;

		// need pay when sale 
		public float mortgage;

		// extended income with card 
		public float income;

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

