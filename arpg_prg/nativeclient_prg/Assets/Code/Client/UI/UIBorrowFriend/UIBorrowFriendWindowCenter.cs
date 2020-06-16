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

            for(var i=1;i<4;i++)
            {
                var tmpStr = Layout.imgplayer + i;
                var borrowItem = new BorrowFriendItem(go.DeepFindEx(tmpStr).gameObject);
                _itemArr[i - 1] = borrowItem;
            }


        }

        private void _ShowCenter()
        {
            EventTriggerListener.Get(btn_back.gameObject).onClick += this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick += this._BorrowHandler;

            var idList = _controller.PlayerIDArrs;
            var dataLenth = idList.Count;
            Console.Error.WriteLine("当前的玩家id数组长度:"+dataLenth);
            for(var i=0;i<3;i++)
            {
                if(i < dataLenth)
                {
                    var tmpPlayer = PlayerManager.Instance.GetPlayerInfo(idList[i]);
                    _itemArr[i].InitItemData(tmpPlayer, i);
                }
                else
                {
                    _itemArr[i].SetVisible(false);
                }
                
            }

            for(var k=0;k<_itemArr.Length;k++)
            {
                EventTriggerListener.Get(_itemArr[k].ItemBtn.gameObject).onClick += onSelectHandler;
            }

        }

        private void onSelectHandler(GameObject go)
        {
            Console.WriteLine("ddddd---点击了-----"+go.transform.parent.name);
            var targetName = go.transform.parent.name;
            var headIndex =int.Parse(targetName.Substring(6));
            Console.WriteLine("当前的玩家的索引++++="+headIndex);
            if(null!=tmpItem)
            {
                tmpItem.IsSelect = false;
            }
            tmpItem = _itemArr[headIndex];
            tmpItem.IsSelect =true;

        }

        private void _HideCenter()
        {
            EventTriggerListener.Get(btn_back.gameObject).onClick -= this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick -= this._BorrowHandler;

            for (var k = 0; k < _itemArr.Length; k++)
            {
                EventTriggerListener.Get(_itemArr[k].ItemBtn.gameObject).onClick -= onSelectHandler;
            }
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
                MessageHint.Show("请选择要借款的好友");
                return;
            }

            if(_inputMoney.text=="")
            {
                MessageHint.Show("请选择要借款的数目");
                return;
            }

            //var borrowMoney = int.Parse(_inputMoney.text);
            var tmpId = tmpItem.PlayerID;
            float rate = 1;
            var tmpMoney =int.Parse(_inputMoney.text);

            if(tmpMoney>tmpItem.MaxMoney)
            {
                MessageHint.Show("请输入的金额大于好友拥有的金额，请重新输入");
                _inputMoney.text = "";
                return;
            }

            NetWorkScript.getInstance().BorrowFriendMoney(tmpId, tmpMoney);
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
