using System;

namespace GameNet
{
    /// <summary>
    /// 未引用
    /// </summary>
	public class LoginBackVo
	{
		///0成功，1失败 
		public int status;

		/// <summary>
		/// The message.成功失败返回信息
		/// </summary>
		public string msg;

		/// <summary>
		/// The data. 
		/// </summary>
		public LoginBackVoData data;
	}

	/// <summary>
	/// Login back vo data.返回成功后的数据信息
	/// </summary>
	public class LoginBackVoData
	{
		/// <summary>
		/// The player image.玩家头像存储地址 
		/// </summary>
		public string playerImg;  
		/// <summary>
		/// The name.玩家昵称，微信用户从微信服务器获取
		/// </summary>
		public string name; 

		///登录成功系统分配
		public int gameId;  
	}
}

