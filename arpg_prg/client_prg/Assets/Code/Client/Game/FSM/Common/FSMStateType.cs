using System;

namespace Client.GameFSM
{
    /// <summary>
    ///  状态机状态类型
    /// </summary>
	public enum FSMStateType
	{
        /// <summary>
        /// 默认状体  0
        /// </summary>
		None,
        /// <summary>
        /// 开始
        /// </summary>
		StartState,

		UnpackState,
        /// <summary>
        /// 加载
        /// </summary>
		DownloadState,
        /// <summary>
        /// 登录
        /// </summary>
		LoginState,
        /// <summary>
        /// 选择角色
        /// </summary>
		SelecRoleState,
        /// <summary>
        /// 加载
        /// </summary>
		LoadingState,
        /// <summary>
        /// 游戏大厅
        /// </summary>
		GameHallState,
        /// <summary>
        /// 网络版选择角色
        /// </summary>
		NetSelectRoleEvent,
	}
}

