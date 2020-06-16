using System;

namespace GameNet
{
    /// <summary>
    /// 接收注册返回的数据信息
    /// </summary>
	public class PlayerRegistBackVo
	{
		/// <summary>
		/// The status 返回值信息  0成功 ,1失败了 
		/// </summary>
		public int status;
		/// <summary>
		/// The message. 失败后的提示信息
		/// </summary>
		public string msg;
	}
}

