using System;

namespace GameNet
{
    /// <summary>
    /// 未引用
    /// </summary>
	public class CheckVersionBack
	{
		/// <summary>
		/// The 提示信息.//0 成功，1失败
		/// </summary>
		public int status;
		///提示信息
		public string msg;
		public CheckVersionBackData data;
	}

    /// <summary>
    /// 查看游戏版本，返回的数据
    /// </summary>
	public class CheckVersionBackData
	{
		/// <summary>
		/// The .告诉前台是否需要更新版本，0不更新，1有更新，2强制更新
		/// </summary>
		public int versionCode;
	}
}

