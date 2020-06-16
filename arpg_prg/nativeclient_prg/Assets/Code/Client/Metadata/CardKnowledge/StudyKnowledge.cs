using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metadata
{
    [ExportAttribute(ExportFlags.ExportRaw)]
    public partial class StudyKnowledge:Template
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
