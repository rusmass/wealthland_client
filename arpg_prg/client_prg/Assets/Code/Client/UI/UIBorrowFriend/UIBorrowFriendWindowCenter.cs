using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    partial class UIBorrowFriendWindow
    {
        private void _InitCenter(GameObject go)
        {
            _inputMoney = go.GetComponentEx<InputField>(Layout.intput_txt);
            btn_back = go.GetComponentEx<Button>(Layout.btn_back);
            btn_borrow = go.GetComponentEx<Button>(Layout.btn_borrow);
        }

        private void _ShowCenter()
        {
            EventTriggerListener.Get(btn_back.gameObject).onClick += this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick += this._BorrowHandler;

        }

        private void _HideCenter()
        {
            EventTriggerListener.Get(btn_back.gameObject).onClick -= this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick -= this._BorrowHandler;
        }

        private void _DisposeCenter()
        {
            for(var i =0; i<this._itemArr.Length;i++)
            {
                var tmpItem = this._itemArr[i];
                if(null!=tmpItem)
                {
                    tmpItem.disposeItem();
                }
            }
        }

        private void _BackHandler(GameObject go)
        {
            _controller.setVisible(false);
        }

        private void _BorrowHandler(GameObject go)
        {
            ///借款接口
            ///
            if(null==tmpItem)
            {
                return;
            }

            if(_inputMoney.text=="")
            {
                return;
            }

            var borrowMoney = float.Parse(_inputMoney.text);
            var tmpId = tmpItem.PlayerID;
            float rate = 1;

        }

        private BorrowFriendItem tmpItem;

        /// <summary>
        /// 存放头像组件的数组
        /// </summary>
        private BorrowFriendItem[] _itemArr = new BorrowFriendItem[3];
        /// <summary>
        /// 输入金币文本框
        /// </summary>
        private InputField _inputMoney;
        /// <summary>
        /// 退出返回
        /// </summary>
        private Button btn_back;
        /// <summary>
        /// 借款按钮
        /// </summary>
        private Button btn_borrow;

       
    }
}
