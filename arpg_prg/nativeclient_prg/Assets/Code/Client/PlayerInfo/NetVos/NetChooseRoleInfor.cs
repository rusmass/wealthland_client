using System;

namespace Client
{
    /// <summary>
    /// 网络游戏中选择角色数据
    /// </summary>
	public class NetChooseRoleInfor
    {
        public NetChooseRoleInfor()
        {
        }

        /// <summary>
        /// 名字
        /// </summary>
		public string nickName = "";
        /// <summary>
        /// 玩家id
        /// </summary>
		public string playerId = "";
        /// <summary>
        /// 职业id
        /// </summary>
		public int careerId = 0;

    }
}

