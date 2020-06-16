using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    class BorrowFriendItem
    {
        public BorrowFriendItem(GameObject go)
        {
            this._InitItem(go);
        }

        private void _InitItem(GameObject go)
        {
            var tmpimg=go.GetComponentEx<RawImage>(Layout.img_head);
            this.img_head =new UIRawImageDisplay(tmpimg);// go.GetComponentEx<Image>
            this.img_select = go.GetComponentEx<Image>(Layout.img_ready);
            this.txt_currentMoney = go.GetComponentEx<Text>(Layout.txt_currentmoney);
            this.txt_name = go.GetComponentEx<Text>(Layout.txt_name);

            this.btn_head = go.GetComponentEx<Button>(Layout.img_head);

            this._ItemObj = go;

        }

        /// <summary>
        /// 玩家playerid
        /// </summary>
        public string PlayerID
        {
            get
            {
                return _playerId;
            }
        }

        public float MaxMoney
        {
            get
            {
                return _totalMoney;
            }
        }

        /// <summary>
        /// 返回按钮组件
        /// </summary>
        public Button ItemBtn
        {
            get
            {
                return btn_head;
            }
        }


       

        /// <summary>
        /// 初始化组件数据
        /// </summary>
        /// <param name="value"></param>
        public void InitItemData(PlayerInfo value,int friendIndex)
        {
            img_head.Load(value.headName);
            img_select.SetActiveEx(false);
            this._totalMoney = value.totalMoney;
            txt_currentMoney.text = _totalMoney.ToString();
            txt_name.text = value.playerName;
            _playerId = value.playerID;

            _ItemObj.name = "friend" + friendIndex;
                       
        }

        /// <summary>
        /// 设置组件的可见或者不可见
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value)
        {
            this._ItemObj.SetActiveEx(value);
        }

        

        /// <summary>
        /// 释放掉头像的资源
        /// </summary>
        public void disposeItem()
        {
            if(null!=img_head)
            {
                img_head.Dispose();
            }
        }


        /// <summary>
        /// 是否是选择了当前的头像
        /// </summary>
        public bool IsSelect
        {
            get
            {
                return _isSelect;
            }

            set
            {
                _isSelect = value;
                if(_isSelect==false)
                {
                    img_select.SetActiveEx(false);
                }
                else
                {
                    img_select.SetActiveEx(true);
                }
            }
        }

        private bool _isSelect=false;

        private string _playerId = "";

        private float _totalMoney=0;

        /// <summary>
        /// 角色头像的image
        /// </summary>
        private UIRawImageDisplay img_head;
        /// <summary>
        /// 是否是选择的image
        /// </summary>
        private Image img_select;

        /// <summary>
        /// 玩家姓名的text
        /// </summary>
        private Text txt_name;
        /// <summary>
        /// 当前玩家名称的txt
        /// </summary>
        private Text txt_currentMoney;
        /// <summary>
        /// 点击按钮组件
        /// </summary>
        private Button btn_head;

        /// <summary>
        /// 自身的gameobj
        /// </summary>
        private GameObject _ItemObj;


        class Layout
        {
            /// <summary>
            /// 头像图片
            /// </summary>
            public static string img_head = "headimg";
            /// <summary>
            /// 是否选择头像
            /// </summary>
            public static string img_ready = "img_ready";
            /// <summary>
            /// 显示名字的文本框
            /// </summary>
            public static string txt_name = "nametxt";

            /// <summary>
            /// 当前玩家有的金币
            /// </summary>
            public static string txt_currentmoney = "currrentmoney";

        }
    }
}
