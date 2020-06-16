using System;

namespace Client
{
    /// <summary>
    ///  个人信息页面类型
    /// </summary>
	public enum HeroInforType
	{
        /// <summary>
        /// 默认值
        /// </summary>
		Null,
        /// <summary>
        /// 玩家基本信息
        /// </summary>
		HeroInfor,//
        /// <summary>
        /// 玩家目标信息
        /// </summary>
		TargetInfor,//
        /// <summary>
        /// 资产和收入信息
        /// </summary>
		BalanceIncomeInfor,//
        /// <summary>
        /// 负债信息
        /// </summary>
		DebtInfor,//
        /// <summary>
        /// 卖出信息
        /// </summary>
		SaleInfor,//
        /// <summary>
        /// 结算信息
        /// </summary>
		CheckOutInfor//
    }
}

