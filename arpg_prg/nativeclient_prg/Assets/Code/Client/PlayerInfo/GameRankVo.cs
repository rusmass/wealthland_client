using System;

namespace Client
{
    /// <summary>
    /// 排行榜数据单元
    /// </summary>
	public class GameRankVo
    {
        public GameRankVo()
        {
        }
        /// <summary>
        ///  排名
        /// </summary>
		public int rankIndex = 0;
        /// <summary>
        /// 昵称
        /// </summary>
		public string playerName = "";
        /// <summary>
        /// 头像路径
        /// </summary>
		public string headPath = "";
        /// <summary>
        /// 排行提示
        /// </summary>
        public string rankTip = "";

    }
}