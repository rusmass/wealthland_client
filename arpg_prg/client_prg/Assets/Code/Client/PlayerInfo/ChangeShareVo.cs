using System;
using Metadata;
namespace Client
{
    /// <summary>
    /// 买卖股票的  股票的数据  买卖的数量  买卖的金币
    /// </summary>
	public class ChangeShareVo
	{
		public ChanceShares shareData;

		public int changeNum=0;

		public float changeMoney=0f;

		public float saleMoney;
	}
}

