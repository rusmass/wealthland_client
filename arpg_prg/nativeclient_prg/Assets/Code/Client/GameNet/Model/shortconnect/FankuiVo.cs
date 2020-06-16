﻿using System;

namespace GameNet
{
    /// <summary>
    /// 游戏反馈，发送的数据
    /// </summary>
    public class FankuiVo
	{
		/// <summary>
		/// 反馈建议,用户输入的文本
		/// </summary>
		public string input;
	}

    /// <summary>
    /// 游戏反馈，返回的数据
    /// </summary>
    public class FankuiBackVo
	{
        /// <summary>
        /// 0成功，1失败 
        /// </summary>
        public int status;
        /// <summary>
        /// 反馈提示玩家
        /// </summary>
        public string msg;


	}


}
