using System;

namespace Client
{
    /// <summary>
    ///  未引用
    /// </summary>
	public class CheckDetailVo
    {
        // 对账单详情 数据  记下外圈时每一笔帐， 涉及到非劳务收入的 股票增如 房产 公司 命运 支出  借贷

        //打印日期
        public string checkdate;
        // 信息说明
        public string infor;
        // 收入
        public string income;
        // 支出
        public string payment;
        // 差值
        public string checkout;
        // 流水帐号
        public string tmpcheckid;
    }
}

