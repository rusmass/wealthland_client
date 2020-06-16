using System;

namespace Client
{
    /// <summary>
    /// 卖出记录的数据单元
    /// </summary>
	public class SaleRecordVo
    {
        public string title;
        public float price;
        public int number;
        public float mortage = -1;
        public float saleMoney;
        public float income;
        public float quality;
        public float getMoney;
    }
}

