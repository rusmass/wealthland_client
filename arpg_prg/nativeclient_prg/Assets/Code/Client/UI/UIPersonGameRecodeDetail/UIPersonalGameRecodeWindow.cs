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
            GameModel.GetInstance.MathWidthOrHeightByCondition(go, 0);

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
            for(var i=0;i < listGroup.Count;i++)
            {
                listGroup[i].Dispose();
            }
        }

        private void RushList()
        {
            List<DetailVo> data = _controller.detailList;
            //for (int i = 0; i < 4; i++)
            //{
            //    DetailVo lp = new DetailVo();
            //    lp.headPath = "share/texture/head/lvshi.ab";
            //                 //"share/atlas/battle/battlemain/chatbglight.ab"
            //    lp.name = "123";
            //    lp.pro = "123";
            //    lp.creat = "123";
            //    lp.use = "323";
            //    lp.mgr = "334";
            //    lp.beyond = "4343";
            //    lp.all = "343";
            //    data.Add(lp);
            //}
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
                listGroup[i].loadHead(data[i].headPath);
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




       
    }


    public class DetailVo
    {
        /// <summary>
        /// 头像路径
        /// </summary>
        public string headPath;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string name;
        /// <summary>
        /// 职业
        /// </summary>
        public string pro;
        /// <summary>
        /// 创造财富
        /// </summary>
        public string creat;
        /// <summary>
        /// 运用财富
        /// </summary>
        public string use;
        /// <summary>
        /// 管理财富
        /// </summary>
        public string mgr;
        /// <summary>
        /// 超越财富
        /// </summary>
        public string beyond;
        /// <summary>
        /// 综合得分
        /// </summary>
        public string all;
        /// <summary>
        /// 游戏进程级别，外圈、内圈、核心圈
        /// </summary>
        public int level;


    }
}