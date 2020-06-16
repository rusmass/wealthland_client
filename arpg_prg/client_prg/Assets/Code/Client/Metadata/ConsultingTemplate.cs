using System;

namespace Metadata
{
    /// <summary>
    /// 咨询列表查询数据
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial  class ConsultingTemplate :Template
	{
		public string questionGroupName;

		public string questionName;
		
		public string questionDescribe;
	}
}

