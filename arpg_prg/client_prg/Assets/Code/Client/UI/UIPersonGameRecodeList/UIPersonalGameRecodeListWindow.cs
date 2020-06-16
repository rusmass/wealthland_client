using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{

    public partial class UIPersonalGameRecodeListWindow : UIWindow<UIPersonalGameRecodeListWindow, UIPersonGameRecodeListController>
    {
        ScrollRect scroll;
        GridLayoutGroup grid;
        GameObject item;
        Button back;
        List<ItemClass.PersonalGameRecodeList> listGroup;

        protected override void _Init(GameObject go)
        {
            scroll = go.GetComponentEx<ScrollRect>(Layout.sc);
            grid = go.GetComponentEx<GridLayoutGroup>(Layout.grid);
            item = grid.transform.Find(Layout.item).gameObject;
            back = go.GetComponentEx<Button>(Layout.back);
            listGroup = new List<ItemClass.PersonalGameRecodeList>();
            listGroup.Add(new  ItemClass.PersonalGameRecodeList(item.transform));
            EventTriggerListener.Get(listGroup[0].detail.gameObject).onClick = DetailClick;
        }

        protected override void _OnShow()
        {
            EventTriggerListener.Get(back.gameObject).onClick += BackClick;
            RushList();
        }

        protected override void _OnHide()
        {
            EventTriggerListener.Get(back.gameObject).onClick -= BackClick;
        }

        protected override void _Dispose()
        {

        }

        private void RushList()
        {
            List<llll> data = new List<llll>();
            for (int i = 0; i < 4; i++)
            {
                llll lp = new llll();
                lp.name = "123";
                lp.title = "123";
                lp.win = "323";
                lp.money = "334";
                lp.timeCount = "4343";
                lp.quality = "343";
                lp.time = "1243";
                data.Add(lp);
            }
            if(listGroup.Count<data.Count)
            {
                int co = data.Count - listGroup.Count;
                for(int i=0;i< co; i++)
                {
                    listGroup.Add(listGroup[0].Clone());
                    EventTriggerListener.Get(listGroup[listGroup.Count - 1].detail.gameObject).onClick = DetailClick;
                }
            }
            for(int i=0;i< data.Count; i++)
            {
                listGroup[i].o.gameObject.SetActive(true);
                listGroup[i].o.name = i.ToString();
                //listGroup[i].title. = data[i].name;
                listGroup[i].name.text = data[i].name;
                listGroup[i].money.text = data[i].money;
                listGroup[i].timeCount.text = data[i].timeCount;
                listGroup[i].quality.text = data[i].quality;
                listGroup[i].time.text = data[i].time;
            }
            for (int i = data.Count; i < listGroup.Count; i++)
            {
                listGroup[i].o.gameObject.SetActive(false);
            }
            scroll.content.localPosition = Vector3.zero;
        }
        private void BackClick(GameObject obj)
        {
            if (null != _controller)
            {
                _controller.setVisible(false);
            }
        }
        private void DetailClick(GameObject obj)
        {
            var hallset = UIControllerManager.Instance.GetController<UIPersonGameRecodeController>();
            hallset.setVisible(true);
        }



        class llll
        {
            public string title;
            public string name;
            public string win;
            public string money;
            public string timeCount;
            public string quality;
            public string time;
        }
    }
}