using System;

namespace Client
{
	public class IncomePayVo
	{
		//非劳务收入发生变化或者每月支出发生变化的数据单元vo 

		// 对应的操作名称，或固定或者卡牌的title
		public string title;

		// 非劳务收入增加 
		public float income;

		// 花费 ，非劳务收入减少 ， 或者贷款
		public float paymeny;

	    // 还账界面是否选中还账选项
		public bool isSelect=false;
	}
}

