using System;

namespace Client
{
	/// <summary>
	/// Player head infor.玩家头像信息单元
	/// </summary>
	public class PlayerHeadInfor
	{
		public PlayerHeadInfor ()
		{
		}

		/// <summary>
		/// The name of the nick. 游戏昵称
		/// </summary>
		public string nickName="";

		/// <summary>
		/// The UUID. 唯一id
		/// </summary>
		public string uuid = "";

		/// <summary>
		/// The sex. 0 女  1， 男
		/// </summary>
		public int sex = 0 ;

		/// <summary>
		/// The head image. 玩家头像信息
		/// </summary>
		public string headImg="";

		public bool isReady= false;

		public bool isInitlize;

		/// <summary>
		/// The is robot. 是机器、掉线  ， 还是真人玩家
		/// </summary>
		public bool isRobot;
	}
}

