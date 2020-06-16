using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 总的游戏就得统计
    /// </summary>
    public class TotalGameRecordVo
    {
        public int totalNums = 0;
        public string winRate = "";
        public string avrageTime = "";


        public TotalGameRecordVo(int _totalNum,string _winRate,string _avrage)
        {
            this.totalNums = _totalNum;
            this.winRate = _winRate;
            this.avrageTime = _avrage;
        }


    }
}
