using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIConclusionDetailWindow
    {
		private void _OnInitButton(GameObject go)
		{
			_btnClose = go.GetComponentEx<Button>(Layout.btn_close);
            _scrollRect = go.GetComponentEx<ScrollRect>(Layout.scrolview);
            //_scrollRect = go.GetComponentEx<ScrollRect>("ScrollView");
           
        }
	
		private void _OnShowButton()
		{
			EventTriggerListener.Get(_btnClose.gameObject).onClick += _OnBtnSureClick;
            this._scrollRect.verticalScrollbar.value = 1;
        }


		private void _OnHideButton()
		{
			EventTriggerListener.Get(_btnClose.gameObject).onClick -= _OnBtnSureClick;			
        }
        /// <summary>
        /// 点击确定，关闭窗口
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnSureClick(GameObject go)
		{			
			_controller.setVisible (false);
		}   	

        private void _UpdateDateMove(float deltatime)
        {
            if(_autoMove==true)
            {
                this._stayTime -= deltatime;
                if (this._stayTime < 0)
                {
                    this._scrollRect.verticalScrollbar.value -= deltatime * this._moveSpeed;
                    //Console.WriteLine("zzzzzzzzz+========="+ this._scrollRect.verticalScrollbar.value);
                    if (this._scrollRect.verticalScrollbar.value <=0)
                    {
                        this._scrollRect.verticalScrollbar.value = 0;
                        //Console.WriteLine("zzzzzzzzz+=========" + this._scrollRect.verticalScrollbar.value);
                        _autoMove = false;
                    }
                }
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
		private Button _btnClose;

        /// <summary>
        /// 滑动版
        /// </summary>
        private ScrollRect _scrollRect;

        /// <summary>
        /// 滑板静止的时间
        /// </summary>
        private float _stayTime = 2f;
        /// <summary>
        /// 滑动的速度
        /// </summary>
        private float _moveSpeed = 0.02f;

        /// <summary>
        /// 当前面板是否是可以自动移动
        /// </summary>
        private bool _autoMove = true;
	}
}

