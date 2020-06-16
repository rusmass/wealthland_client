using System;

namespace Client
{
	public class BorrowVo
	{
		// 贷款次数
		/// <summary>
		/// The times. 贷款次数
		/// </summary>
		public int times=0;

		/// <summary>
		/// The bankborrow. 银行贷款
		/// </summary>
		public int bankborrow=0;

		/// <summary>
		/// The bankdebt.银行贷款的负债
		/// </summary>
		public int bankdebt=0;

		/// <summary>
		/// The cardborrow. 
		/// </summary>
		public int cardborrow=0;

		/// <summary>
		/// The carddebt.信用卡的借贷
		/// </summary>
		public int carddebt=0;

		/// <summary>
		/// The bank rate.显示银行的概率
		/// </summary>
		public string bankRate="";

		/// <summary>
		/// The card rate. 显示卡牌的汇率
		/// </summary>
		public string cardRate="";

	}
}

