using System;

namespace Client
{
    /// <summary>
    /// 还款的数据单元
    /// </summary>
	public class PaybackVo
    {
        public string title = "";
        public int borrow = 0;
        public int debt = 0;
        public bool isSeleted = false;

        // 0 是信用卡 1是银行贷款
        public int isBank = 0;

        public int basetype;

        /// <summary>
        /// The type of the net.还款的类型 0 基本负债还款 ， 1 ，银行还款 2 ，信用卡还款
        /// </summary>
        public int netType = 0;

    }
}
