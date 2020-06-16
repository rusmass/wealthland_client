using System;

namespace Server.UnitFSM
{
    /// <summary>
    /// 状态机状态类型
    /// </summary>
    public enum FSMStateType
	{
		None,
		StayState,
		RollState,
		WalkState,
        SelectState,
        GiveUpState,
        UpGradState,
        SuccessState,
	}
}

