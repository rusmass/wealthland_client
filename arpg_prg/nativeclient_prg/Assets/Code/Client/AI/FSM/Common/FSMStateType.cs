using System;

namespace Client.UnitFSM
{
    /// <summary>
    /// 状态机状态的类型
    /// </summary>
	public enum FSMStateType
    {
        /// <summary>
        /// 默认状态
        /// </summary>
		None,
        /// <summary>
        /// 站立
        /// </summary>
		StayState,//
        /// <summary>
        /// 掷筛子
        /// </summary>
		RollState,//
        /// <summary>
        /// 行走
        /// </summary>
		WalkState,//
        /// <summary>
        /// 选择卡牌
        /// </summary>
        SelectState,//
        /// <summary>
        /// 放弃
        /// </summary>
		GiveUpState,//
        /// <summary>
        /// 升级
        /// </summary>
        UpGradeState,//
        /// <summary>
        /// 胜利
        /// </summary>
        SuccessState,//
    }
}

