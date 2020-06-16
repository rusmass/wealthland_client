using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public partial class UIShareBoardWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_wechatmoment = go.GetComponentEx<Button> (Layout.btn_sharewechatmoment);
			btn_weichat = go.GetComponentEx<Button> (Layout.btn_sharewechat);

			btn_bg = go.GetComponentEx<Button> (Layout.btn_imgbg);
		}

		private void _ShowCenter()
		{
			EventTriggerListener.Get (btn_wechatmoment.gameObject).onClick += _OnClickWeiChatMentHandler;
			EventTriggerListener.Get (btn_weichat.gameObject).onClick += _OnClickWeiChatHandler;
			EventTriggerListener.Get (btn_bg.gameObject).onClick += _OnCloseHandler;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (btn_wechatmoment.gameObject).onClick -= _OnClickWeiChatMentHandler;
			EventTriggerListener.Get (btn_weichat.gameObject).onClick -= _OnClickWeiChatHandler;
			EventTriggerListener.Get (btn_bg.gameObject).onClick -= _OnCloseHandler;
		}

		private void _OnCloseHandler(GameObject go)
		{
			if (null != _controller)
			{
				_controller.setVisible (false);
			}
            _HandlerCallBack();
		}

        /// <summary>
        /// 分享微信朋友圈
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickWeiChatMentHandler(GameObject go)
		{
			Console.WriteLine ("分享微信朋友圈");
            if(_controller.ShareWindow==0)
            {
			MBGame.Instance.ShareWeiChatMonment (ShareContentInfor.Instance.normalTitleContent);
            }
            else
            {
                MBGame.Instance.ShareWeiChatMonment(ShareContentInfor.Instance.dreamShareContent);
            }
            _HandlerCallBack();
            _controller.setVisible (false);
		}

        private void _HandlerCallBack()
        {
            if (null != _controller.CallBack)
            {
                _controller.CallBack();
                _controller.CallBack = null;
            }
        }

        /// <summary>
        /// 分享给朋友
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickWeiChatHandler(GameObject go)
		{
			Console.WriteLine ("分享给朋友");

            if(_controller.ShareWindow==0)
            {
                MBGame.Instance.ShareWeiChat(ShareContentInfor.Instance.normalTitleContent);
            }
            else
            {
                MBGame.Instance.ShareWeiChat(ShareContentInfor.Instance.dreamShareContent);

            }

            _HandlerCallBack();
            _controller.setVisible (false);
		}

		private Button btn_wechatmoment;

		private Button btn_weichat;

		private Button btn_bg;
	}
}

