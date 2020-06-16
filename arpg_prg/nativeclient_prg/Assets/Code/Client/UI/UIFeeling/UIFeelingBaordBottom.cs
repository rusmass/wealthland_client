using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    partial class UIFeelingBaordWindow
    {
        private void _InitBottom(GameObject go)
        {
            this.btn_previous = go.GetComponentEx<Button>(Layout.btn_previous);
            this.btn_next = go.GetComponentEx<Button>(Layout.btn_next);
            this.lb_page = go.GetComponentEx<Text>(Layout.lb_page);
            this._bottom = go.DeepFindEx(Layout.content);
        }

        private void _ShowBottom()
        {
            EventTriggerListener.Get(this.btn_previous.gameObject).onClick +=_ShowPriviousHandler;
            EventTriggerListener.Get(this.btn_next.gameObject).onClick += _ShowNextHandler;

        }

        private void _HideBottom()
        {
            EventTriggerListener.Get(this.btn_previous.gameObject).onClick -= _ShowPriviousHandler;
            EventTriggerListener.Get(this.btn_next.gameObject).onClick -= _ShowNextHandler;
        }

        /// <summary>
        /// 显示上一页
        /// </summary>
        /// <param name="go"></param>
        private void _ShowPriviousHandler(GameObject go)
        {
            this._pageIndex--;
            if(_pageIndex<1)
            {
                _pageIndex = 1;
            }
            if(_feelTypes==0)
            {
                _ShowGameFeelByIndex(_pageIndex);
            }
            else if(_feelTypes==1)
            {
                _ShowSelfShareByIndex(_pageIndex);
            }

        }

        /// <summary>
        /// 显示下一页
        /// </summary>
        /// <param name="go"></param>
        private void _ShowNextHandler(GameObject go)
        {
            this._pageIndex++;
            if(_feelTypes==0)
            {
                if(_pageIndex>_controller.GameFeelPages)
                {
                    _pageIndex = _controller.GameFeelPages;
                }
                if(_pageIndex<=0)
                {
                    _pageIndex = 1;
                }
                _ShowGameFeelByIndex(_pageIndex);
            }
            else if(_feelTypes==1)
            {
                if(_pageIndex>_controller.SelfFeelPages)
                {
                    _pageIndex = _controller.SelfFeelPages;
                }
                if(_pageIndex<=0)
                {
                    _pageIndex = 1;
                }
                _ShowSelfShareByIndex(_pageIndex);
            }
            
        }

        /// <summary>
        /// 刷洗当前的按钮状态
        /// </summary>
        /// <param name="index"></param>
        private void _UpdatePage()
        {
            var str = string.Format("{0}/{1}", this._pageIndex, this._totalPage);
            this.lb_page.text = str;
            if(this._pageIndex<=0)
            {
                this.btn_previous.SetActiveEx(false);
            }
            else
            {
                this.btn_previous.SetActiveEx(true);
            }

            if(_pageIndex>=this._totalPage)
            {
                this.btn_next.SetActiveEx(false);
            }
            else
            {
                this.btn_next.SetActiveEx(true);
            }

        }

        /// <summary>
        ///当前的索引
        /// </summary>
        private int _pageIndex = 1;

        /// <summary>
        /// 返回的总页数
        /// </summary>
        private int _totalPage = 1;

        /// <summary>
        /// 页数长度
        /// </summary>
        private readonly int _pageLength = 10;

        /// <summary>
        /// 设置底部是否可见
        /// </summary>
        /// <param name="value"></param>
        private void _BottomState(bool value)
        {
            this._bottom.SetActiveEx(value);
        }

        /// <summary>
        /// 上一页按钮
        /// </summary>
        private Button btn_previous;
        /// <summary>
        /// 下一页按钮
        /// </summary>
        private Button btn_next;
        /// <summary>
        /// 显示页数
        /// </summary>
        private Text lb_page;

        /// <summary>
        /// 底部组件容器
        /// </summary>
        private Transform _bottom;

        /// <summary>
        /// 玩家列表的显示类型  0游戏感悟，1自己的感悟列表
        /// </summary>
        private int _feelTypes = 0;
       
    }   
}
