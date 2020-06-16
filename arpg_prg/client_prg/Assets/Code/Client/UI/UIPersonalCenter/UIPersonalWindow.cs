using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace Client.UI
{
    public partial class UIPersonalWindow : UIWindow<UIPersonalWindow, UIPersonalController>
    {
    
        protected override void _Init(GameObject go)
        {
            Init(go);
            RushPersonal();
            RushList();
        }

        protected override void _OnShow()
        {
            InitButton();
        }

        protected override void _OnHide()
        {
            RemoveButton();
        }

        protected override void _Dispose()
        {

        }


        private void RushPersonal()
        {
            UpdateInforData("123", "水瓶座", "1990.11.12");
            UpdateBattleData("200场", "200场", "30分钟", "200");
        }

        /// <summary>
        ///  更新对战信息
        /// </summary>
        public void UpdateBattleData(string totalNum, string winNum, string totalTime, string scores)
        {
            allCount.text = totalNum;
            allWin.text = winNum;
            allTime.text = totalTime;
            allIntegration.text = scores;
        }

        /// <summary>
        /// 更新人物数据
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="star"></param>
        /// <param name="birthday"></param>
        public void UpdateInforData(string _name, string star, string birthday, int sex = 0)
        {
            name.text = _name;
            //sex.sprite = 

            if (sex == 0)
            {
                img_sexdisplay.Load(_herosexwomanPath);
            }
            else
            {
                img_sexdisplay.Load(_herosexmanPath);
            }

            lb_star.text = star;
            birsday.text = birthday;
        }


        private void RushList()
        {
            List<ListData> dataList = new List<ListData>();
            for(int i = 0; i < 6; i++)
            {
                ListData mm = new ListData();
                mm.title = Random.Range(1, 3).ToString();
                mm.win = Random.Range(2, 5).ToString();
                mm.money = Random.Range(3, 5).ToString();
                mm.timeCount = Random.Range(4,8).ToString();
                mm.quality = Random.Range(5, 8).ToString();
                mm.time = Random.Range(6, 9).ToString();
                dataList.Add(mm);
            }
            if(ltemList.Count< dataList.Count)
            {
                int con = dataList.Count - ltemList.Count;
                for(int i=0;i< con;i++)
                {
                    ltemList.Add(ltemList[0].Clone());
                    EventTriggerListener.Get(ltemList[ltemList.Count - 1].detail.gameObject).onClick = DetailClick;
                }
            }
            
            for(int i=0;i< dataList.Count; i++)
            {
                ltemList[i].o.SetActiveEx(true);
                ltemList[i].o.name = i.ToString();
                //ltemList[i].title = dataList[i].title;
                //ltemList[i].win =  dataList[i].win;
                ltemList[i].money.text = dataList[i].money;
                ltemList[i].timeCount.text = dataList[i].timeCount;
                ltemList[i].quality.text = dataList[i].quality;
                ltemList[i].time.text = dataList[i].time;
            }
            for(int i= dataList.Count;i< ltemList.Count; i++)
            {
                ltemList[i].o.SetActiveEx(false);
            }
            scrollView.verticalNormalizedPosition = 1;
        }
        private void RushSet()
        {
            set.gameObject.SetActiveEx(true);
            setName.text = "123";
            nianInt = 1993;
            yueInt = 12;
            riInt = 31;
            InitNianList();            
            InitYueList();
            InitRiList();
            tips.gameObject.SetActive(false);
            ChangeSex(true);
        }
    }



    class ListData
    {
        public string title;
        public string win;
        public string money;
        public string timeCount;
        public string quality;
        public string time;
    }
}