using System;

namespace Client
{
    /// <summary>
    ///  枚举，负债的类型
    /// </summary>
	public enum BaseDebtType
    {
        /// <summary>
        /// 默认枚举
        /// </summary>
		None = 100000,
        /// <summary>
        /// 房屋负债
        /// </summary>
		HouseDebt, //
        /// <summary>
        /// 教育负债
        /// </summary>
		EducationDebt,//
        /// <summary>
        /// 购车负债
        /// </summary>
		CarDebt,// 
        /// <summary>
        /// 信用卡负债
        /// </summary>
		CardDebt,//
        /// <summary>
        /// 额外负债
        /// </summary>
		AdditionDebt,//
        /// <summary>
        /// 其他
        /// </summary>
		OtherDebt,//
        /// <summary>
        /// 初始
        /// </summary>
		InitTxtDebt,//
        /// <summary>
        /// 生孩子
        /// </summary>
		GiveChild//
    }
}


