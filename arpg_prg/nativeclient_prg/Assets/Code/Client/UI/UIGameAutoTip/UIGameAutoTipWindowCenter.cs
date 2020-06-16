using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGameAutoTipWindow
    {
		private void _InitCenter(GameObject go)
		{
			lb_txt = go.GetComponentEx<Text> (Layout.lb_txt);
            lb_title = go.GetComponentEx<Text>(Layout.lb_title);
		}

		private void _OnShowCenter()
		{			
			lb_txt.text = _controller.GameTip;
		}

		private void _OnHideCenter()
		{
			
		}
        
        /// <summary>
        /// 帧频刷新提示倒计时
        /// </summary>
        /// <param name="delatime"></param>
        private void _TickCenter(float delatime)
        {

        }

        /// <summary>
        /// 提示信息文本框
        /// </summary>
		private Text lb_txt;

        /// <summary>
        /// 文本框标题
        /// </summary>
        private Text lb_title;	
		
	}
}

