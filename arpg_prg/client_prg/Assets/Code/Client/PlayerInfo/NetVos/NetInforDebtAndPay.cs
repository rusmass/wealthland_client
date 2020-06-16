using System;
using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// 网络吧游戏中，负债和支出的数据
    /// </summary>
	public class NetInforDebtAndPay
	{
		public NetInforDebtAndPay ()
		{
		}

		/// <summary>
		/// The basic pay list. 基本支出
		/// </summary>
		public List<PaybackVo> basicPayList=new List<PaybackVo>();
		/// <summary>
		/// The new add pay list. 新增支出
		/// </summary>
		public List<PaybackVo> newAddPayList = new List<PaybackVo>();

		/// <summary>
		/// The basic debt list.基本负债
		/// </summary>
		public List<PaybackVo> basicDebtList = new List<PaybackVo>();

		/// <summary>
		/// The new add debt list.新增的负债
		/// </summary>
		public List<PaybackVo> newAddDebtList=new List<PaybackVo>();

	}
}

