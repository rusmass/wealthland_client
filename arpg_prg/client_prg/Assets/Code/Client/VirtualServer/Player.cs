using System;

namespace Server
{
    /// <summary>
    ///  玩家的级别分类
    /// </summary>
    public enum PlayerLevel
    {
        None,
        Outer,
        Inner,
        Count
    }

    /// <summary>
    /// 记录玩家数据，是筛子点数,玩家id,当前点数,玩家级别
    /// </summary>
	public class Player
	{
		public int RollPoints;
		public string PlayerID;
		public int CurrentPos=0;
        public PlayerLevel Level = PlayerLevel.Outer; //玩家所在层数0外圈 1内圈
	}
}