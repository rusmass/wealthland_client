using System;

namespace Metadata
{
    /// <summary>
    /// 模型数据
    /// </summary>
    [Export(ExportFlags.ExportRaw)]
    public partial class ModelTemplate : Template
    {
        public string modelName;
        public string modelPath;
    }
}
