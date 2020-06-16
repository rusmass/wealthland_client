using System;

namespace Metadata
{
    /// <summary>
    /// 外圈风险卡牌
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial  class Risk :Template
	{

		// risk type 1 cost money only  2 extended choice
		public int riskTpye;

		// card name
		public string title;

		public string cardPath;

		// card infor 
		public string desc;

		// coast money
		public float payment;

		//infor of extended choice
		public string desc2;

		//extend paymeny
		public float payment2;

		// type of score 0 none  1 timescore   2 qualityscore
		public int scoreType;

		//score name 
		public string scoreName;

		// scores 
		public int score;

		// infor of score;
		public string score_desc;

		/// <summary>
		/// The rank score.排名积分
		/// </summary>
		public int rankScore=0;


	}
}

