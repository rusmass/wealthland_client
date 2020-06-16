using System;

namespace GameNet
{
    /// <summary>
    /// 玩家登陆的数据单元
    /// </summary>
	public class LoginVo
	{
		/// <summary>
		/// The type of the player. 0表示手机用户，1表示微信用户
		/// </summary>
		public int playerType;
		/// <summary>
		/// The phone.手机用户需要的参数
		/// </summary>
		public string phone;
		/// <summary>
		/// The password.手机用户需要的参数
		/// </summary>
		public string password;

		/// <summary>
		/// The we chat identifier.微信用户需要的参数微信id
		/// </summary>
		public string weChatId; 

		/// <summary>
		/// The name of the server.服务器选区
		/// </summary>
		public string serverName;

	}
}

