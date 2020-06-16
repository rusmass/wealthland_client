using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGuidGameWindow
    {
		private void _InitCenter(GameObject go)
        {
            this.btn_next = go.GetComponentEx<Button>(Layout.btn_next);
            this.guid1 = go.GetComponentEx<Image>(Layout.img_guid1);
            this.guid2 = go.GetComponentEx<Image>(Layout.img_guid2);
        }

        private void _ShowCenter()
        {
            EventTriggerListener.Get(this.btn_next.gameObject).onClick += _NextHandler;
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
                GameGuidManager.GetInstance.DoneGameWindow = true;
                _controller.setVisible(false);
            }
        }

        /// <summary>
        /// 显示玩家的在引导的第几步
        /// </summary>
        /// <param name="index"></param>
        private void _ShowGuidStep(int index)
        {
            if(index==1)
            {
                this.guid1.SetActiveEx(true);
                this.guid2.SetActiveEx(false);
            }
            else if(index ==2)
            {
                this.guid1.SetActiveEx(false);
                this.guid2.SetActiveEx(true);
            }
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

        /// <summary>
        /// 初始步数
        /// </summary>
        private int step =1;
        /// <summary>
        /// 目标步数
        /// </summary>
        private int targetStep = 2;


        class Layout
        {
            public static string btn_next = "btn_next";

            public static string img_guid1 = "bg1";

            public static string img_guid2 = "bg2";
            
        }


	}
}

