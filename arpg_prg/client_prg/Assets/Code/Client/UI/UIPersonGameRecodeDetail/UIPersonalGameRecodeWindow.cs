using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{

    public partial class UIPersonalGameRecodeWindow : UIWindow<UIPersonalGameRecodeWindow, UIPersonGameRecodeController>
    {
        ScrollRect scroll;
        GridLayoutGroup grid;
        GameObject item;
        Button back;
        List<ItemClass.PersonalGameRecodeItem> listGroup;

        protected override void _Init(GameObject go)
        {
            scroll = go.GetComponentEx<ScrollRect>(Layout.sc);
            grid = go.GetComponentEx<GridLayoutGroup>(Layout.grid);
            item = grid.transform.Find(Layout.item).gameObject;
            back = go.GetComponentEx<Button>(Layout.back);
            listGroup = new List<ItemClass.PersonalGameRecodeItem>();
            listGroup.Add(new  ItemClass.PersonalGameRecodeItem(item.transform));
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
                lp.pro = "123";
                lp.creat = "123";
                lp.use = "323";
                lp.mgr = "334";
                lp.beyond = "4343";
                lp.all = "343";
                data.Add(lp);
            }
            if(listGroup.Count<data.Count)
            {
                int co = data.Count - listGroup.Count;
                for(int i=0;i< co; i++)
                {
                    listGroup.Add(listGroup[0].Clone());
                }
            }
            for(int i=0;i< data.Count; i++)
            {
                listGroup[i].o.gameObject.SetActive(true);
                listGroup[i].o.name = i.ToString();
                listGroup[i].name.text = data[i].name;
                listGroup[i].pro.text = data[i].pro;
                listGroup[i].creat.text = data[i].creat;
                listGroup[i].use.text = data[i].use;
                listGroup[i].mgr.text = data[i].mgr;
                listGroup[i].beyond.text = data[i].beyond;
                listGroup[i].all.text = data[i].all;
            }
            for(int i = data.Count;i< listGroup.Count; i++)
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




        class llll
        {
            public string name;
            public string pro;
            public string creat;
            public string use;
            public string mgr;
            public string beyond;
            public string all;
        }
    }
}