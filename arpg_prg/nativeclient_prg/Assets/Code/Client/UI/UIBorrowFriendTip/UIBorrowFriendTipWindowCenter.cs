using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    partial class UIBorrowFriendTipWindow
    {
        private void _InitCenter(GameObject go)
        {          
            btn_quit = go.GetComponentEx<Button>(Layout.btn_quit);
            btn_borrow = go.GetComponentEx<Button>(Layout.btn_borrow);
            var tmpImg = go.GetComponentEx<RawImage>(Layout.img_head);
            this.headimg = new UIRawImageDisplay(tmpImg);
            txt_infor = go.GetComponentEx<Text>(Layout.txt_tip);
            lb_letfTime = go.GetComponentEx<Text>(Layout.lb_lefttime);
        }

        private void _ShowCenter()
        {
            EventTriggerListener.Get(btn_quit.gameObject).onClick += this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick += this._BorrowHandler;

            var tmpStr = string.Format("{0}向您借款{1}",_controller.PlayerName,_controller.TargetMoney);
            this.txt_infor.text = tmpStr;
            this.headimg.Load(_controller.HeadPath);

            this._leftTime = 30;
        }

        private void _HideCenter()
        {
            EventTriggerListener.Get(btn_quit.gameObject).onClick -= this._BackHandler;
            EventTriggerListener.Get(btn_borrow.gameObject).onClick -= this._BorrowHandler;
        }

        private void _DisposeCenter()
        {
            if(null!=headimg)
            {
                headimg.DisposeEx();
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
            float rate = 1;

            NetWorkScript.getInstance().AgreeBorrowedMoney(_controller.TargetPlayerID, _controller.TargetMoney,_controller.Rate);
            _controller.setVisible(false);
        }

        /// <summary>
        /// 刷新剩余
        /// </summary>
        /// <param name="deltime"></param>
        private void _TickCenter(float deltime)
        {
            _leftTime -= deltime;
            
            if(_leftTime>0)
            {
                if(null!=this.lb_letfTime)
                {
                    lb_letfTime.text = GameTimerManager.GetTime(_leftTime);
                }
            }
            else
            {
                _controller.setVisible(false);
            }


        }

        /// <summary>
        /// 倒计时文本
        /// </summary>
        private Text lb_letfTime;

        /// <summary>
        /// 倒计时剩余的时间
        /// </summary>
        private float _leftTime = 30;
             
        
        /// <summary>
        /// 退出返回
        /// </summary>
        private Button btn_quit;
        /// <summary>
        /// 借款按钮
        /// </summary>
        private Button btn_borrow;

        /// <summary>
        /// 文本信息的提示
        /// </summary>
        private Text txt_infor;

        /// <summary>
        /// 加载的玩家的头像
        /// </summary>
        private UIRawImageDisplay headimg;
       
    }
}
