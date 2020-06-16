using System;

namespace Client
{
    /// <summary>
    /// 网络版游戏，个人信息中结算面板数据
    /// </summary>
	public class NetInforBoardCheckVo
    {
        public NetInforBoardCheckVo()
        {
        }

        /// <summary>
        /// The total income. 收入
        /// </summary>
        public float totalIncome = 0;

        /// <summary>
        /// The total pay. 支出
        /// </summary>
        public float totalPay = 0;

        /// <summary>
        /// The check money. 结算
        /// </summary>
        public float checkMoney = 0;
    }
}

