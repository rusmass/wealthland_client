using System;

namespace Client
{
	/// <summary>
	/// Player head infor.
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

        /// <summary>
        /// 玩家生日，默认为新建角色时的信息
        /// </summary>
        public string birthday = "";

        /// <summary>
        /// 玩家星座信息
        /// </summary>
        public string constellation = "";


        public bool isReady= false;

		public bool isInitlize;

		/// <summary>
		/// The is robot. 是机器、掉线  ， 还是真人玩家
		/// </summary>
		public bool isRobot;
	}
}

