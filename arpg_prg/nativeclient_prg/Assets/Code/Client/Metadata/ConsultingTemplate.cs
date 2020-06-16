using System;

namespace Metadata
{
    /// <summary>
    /// 咨询列表查询数据
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial  class ConsultingTemplate :Template
	{
        /// <summary>
        /// 分类群组
        /// </summary>
		public string questionGroupName;
        /// <summary>
        /// 卡牌类型的
        /// </summary>
        public int typeID;
        /// <summary>
        /// 问题的标题名称
        /// </summary>
		public string questionName;
		/// <summary>
        /// 词条内容描述
        /// </summary>
		public string questionDescribe;
	}
}

