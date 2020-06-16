using System;

namespace Metadata
{
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class PlayerInitData:Template
	{
		/// <summary>
		/// The name of the play. 用户名
		/// </summary>
		public string playName;
		/// <summary>
		/// The head path. 头像图片路径 名
		/// </summary>
		public string headPath;

		/// <summary>
		/// The player image path.玩家人身展示图片路径
		/// </summary>
		public string playerImgPath;

		/// <summary>
		/// The one child prise. 生一个孩子需要的花费
		/// </summary>
		public float oneChildPrise;

	    /// <summary>
		/// The careers. 职业
	    /// </summary>
		public string careers;

		/// <summary>
		/// The init age.初始年龄
		/// </summary>
		public int initAge;

        /// <summary>
		/// The model res I.角色模型ID
        /// </summary>
        public int modelResID;

		/// <summary>
		/// The fix bank saving. 银行储蓄 固定做展示用
		/// </summary>
		public int fixBankSaving=0;
		/// <summary>
		/// The cash flow. 工资 
		/// </summary>
		public int cashFlow;
		/// <summary>
		/// The fix house mortgages. 总的住房抵押贷款 展示用
		/// </summary>
		public int fixHouseDebt=0;
		/// <summary>
		/// The fix education loan. 教育贷款 用于展示
		/// </summary>
		public int fixEducationDebt=0;
		/// <summary>
		/// The fix car loan. 购车贷款 用于展示
		/// </summary>
		public int fixCarDebt=0;
		/// <summary>
		/// The fix card debt. 信用卡贷款 用于展示
		/// </summary>
		public int fixCardDebt=0;
		/// <summary>
		/// The fix additional debt. 额外负债 用于展示 
		/// </summary>
		public int fixAdditionalDebt=0;

		/// <summary>
		/// The fix tax. 税金用于用于支出
		/// </summary>
		public int taxPay=0;
		/// <summary>
		/// The house mortgages. 每月要还的房子抵押贷款
		/// </summary>
		public int housePay=0;
		/// <summary>
		/// The education loan. 每月要还的教育贷款
		/// </summary>
		public int educationPay=0;
		/// <summary>
		/// The car loan. 每月要偿还的购车贷款
		/// </summary>
		public int carPay=0;
		/// <summary>
		/// The card debt. 每月的信用卡还款
		/// </summary>
		public int cardPay=0;
		/// <summary>
		/// The additional debt. 每月的额外支出
		/// </summary>
		public int additionalPay=0;
		/// <summary>
		/// The other spend. 必要支出 
		/// </summary>
		public int nessPay=0;
		/// <summary>
		/// The infor. 信息简介
		/// </summary>
		public string infor;
		/// <summary>
		/// The model path.加载模型路径
		/// </summary>
		public string modelPath;
		/// <summary>
		/// The player sex. 角色的性别
		/// </summary>
		public int playerSex;

        /// <summary>
        /// 角色的天赋介绍
        /// </summary>
        public string playerGift;
    }
}

