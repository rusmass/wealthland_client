using System;

namespace Metadata
{
    /// <summary>
    /// 外圈砖块数据
    /// </summary>
    [Export(ExportFlags.ExportRaw)]
    public partial class StageOuterPoint : Template
    {
        public string action;

        public bool IsCheckDay()
        {
            return action.Equals("CheckDayAction");
        }
    }
}
