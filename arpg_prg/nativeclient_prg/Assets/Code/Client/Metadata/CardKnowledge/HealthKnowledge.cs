using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metadata
{
    /// <summary>
    /// 进修学习数据
    /// </summary>
    [ExportAttribute(ExportFlags.ExportRaw)]
    public partial class HealthKnowledge:Template
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 包涵的内容
        /// </summary>
        public string content;
    }
}
