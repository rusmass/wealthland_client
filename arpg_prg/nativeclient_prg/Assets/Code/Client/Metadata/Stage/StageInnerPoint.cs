using System;

namespace Metadata
{
    /// <summary>
    ///  内圈砖块的点的类型
    /// </summary>
    [Export(ExportFlags.ExportRaw)]
    public partial class StageInnerPoint : Template
    {
        public string action;

        public bool IsCheckDay()
        {
            return action.Equals("InnerCheckDayAction");
        }

    }
}
