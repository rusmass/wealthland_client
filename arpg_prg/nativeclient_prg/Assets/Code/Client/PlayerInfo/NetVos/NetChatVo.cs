using System;

namespace Client
{
    /// <summary>
    /// 聊天数据vo
    /// </summary>
	public class NetChatVo
    {
        public NetChatVo()
        {
        }
        /// <summary>
        /// 玩家id
        /// </summary>
		public string playerId;

        /// <summary>
        /// 昵称
        /// </summary>
		public string playerName;

        /// <summary>
        ///  头像
        /// </summary>
		public string playerHead;

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string chat;

        /// <summary>
        /// The sex. 性别 0 nv  1 nan
        /// </summary>
        public int sex = 0;

        /// <summary>
        /// The send time.发言时间
        /// </summary>
        public string sendTime;
    }
}

