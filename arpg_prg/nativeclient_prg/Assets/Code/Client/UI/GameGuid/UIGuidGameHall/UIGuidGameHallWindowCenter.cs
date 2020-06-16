using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGuidGameHallWindow
    {
		private void _InitCenter(GameObject go)
        {
            this.btn_next = go.GetComponentEx<Button>(Layout.btn_next);
            this.guid1 = go.GetComponentEx<Image>(Layout.img_guid1);
            this.guid2 = go.GetComponentEx<Image>(Layout.img_guid2);
            this.guid3 = go.GetComponentEx<Image>(Layout.img_guid3);
            this.guid4 = go.GetComponentEx<Image>(Layout.img_guid4);
            this.guid5 = go.GetComponentEx<Image>(Layout.img_guid5);


        }

        private void _ShowCenter()
        {
            EventTriggerListener.Get(this.btn_next.gameObject).onClick += _NextHandler;

            guids =new Image[]{ this.guid1,this.guid2,this.guid3,this.guid4,this.guid5};

            this._ShowGuidStep(this.step);
          
        }

        private void _HideCenter()
        {
            EventTriggerListener.Get(this.btn_next.gameObject).onClick -= _NextHandler;

        }

        private void _NextHandler(GameObject go)
        {
            this.step++;
            if(this.step<=this.targetStep)
            {
                this._ShowGuidStep(this.step);
            }
            else
            {
                GameGuidManager.GetInstance.DoneGameHall = true;

                if (GameModel.GetInstance.gonggaoList.Count > 0)
                {
                    var gonggaocontroller = UIControllerManager.Instance.GetController<Client.UI.UIGongGaoController>();
                    gonggaocontroller.inforList = GameModel.GetInstance.gonggaoList;
                    gonggaocontroller.setVisible(true);
                }

                _controller.setVisible(false);
            }
        }

        /// <summary>
        /// 显示玩家的在引导的第几步
        /// </summary>
        /// <param name="index"></param>
        private void _ShowGuidStep(int index)
        {
            if(null!=this.tmpImg)
            {
                this.tmpImg.SetActiveEx(false);
            }
            this.tmpImg = this.guids[index - 1];
            tmpImg.SetActiveEx(true);
        }

        /// <summary>
        /// 下一步
        /// </summary>
        private Button btn_next;

        /// <summary>
        /// 第一步展示
        /// </summary>
        private Image guid1;

        /// <summary>
        /// 第二步展示
        /// </summary>
        private Image guid2;

        private Image guid3;

        private Image guid4;

        private Image guid5;

        private Image[] guids;

        private Image tmpImg=null;

        /// <summary>
        /// 初始步数
        /// </summary>
        private int step =1;
        /// <summary>
        /// 目标步数
        /// </summary>
        private int targetStep = 4;


        class Layout
        {
            public static string btn_next = "btn_next";

            public static string img_guid1 = "guid1";

            public static string img_guid2 = "guid2";

            public static string img_guid3 = "guid3";

            public static string img_guid4 = "guid4";

            public static string img_guid5 = "guid5";
        }


	}
}

