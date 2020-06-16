using System;

namespace Client.UI
{
    /// <summary>
    /// 资产界面显示资产信息的界面
    /// </summary>
	public class FixedInforVo
    {
        public int id;

        public int belongsTo;

        public string title;

        public string desc;

        public string cardPath;

        public string slaeMoney;


        // coefficient 
        public int baseNumber;

        // show coast
        public string coast;

        // price range
        public string sale;

        // pay 
        public float payment;

        // profit show
        public string profit;

        //need pay when sale
        public float mortgage;

        // 1 timeScore 2 qualityScore
        public int scoreType;

        // socres
        public int scoreNumber;

        //extend income with card
        public float income;

    }
}

