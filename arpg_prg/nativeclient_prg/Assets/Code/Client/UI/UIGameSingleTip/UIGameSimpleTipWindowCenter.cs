using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGameSimpleTipWindow
	{
		private void _InitCenter(GameObject go)
		{
			lb_txt = go.GetComponentEx<Text> (Layout.lb_txt);
			btn_cancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);
			img_bgimg = go.GetComponentEx<Image> (Layout.img_bgimg);
		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get(btn_cancle.gameObject).onClick+=_HideGameWindow;
			EventTriggerListener.Get(btn_sure.gameObject).onClick +=_KnowHandler;
			if (null != _controller)
			{

			}

			if (_controller.TipType == 0)
			{
				btn_sure.SetActiveEx (false);
				var tmpPosition =  btn_cancle.transform.localPosition;
				btn_cancle.transform.localPosition = new Vector3 (0,tmpPosition.y,tmpPosition.z);
			}
			else
			{
				btn_cancle.SetActiveEx (false);
				var tmpPosition =  btn_sure.transform.localPosition;
				btn_sure.transform.localPosition = new Vector3 (0,tmpPosition.y,tmpPosition.z);
			}

			lb_txt.text = _controller.txtStr;

		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_cancle.gameObject).onClick -= _HideGameWindow;
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _KnowHandler;
		}


		private void _HideGameWindow(GameObject go)
		{
			if (null != _controller) 
			{
				_controller.setVisible (false);
			}

			if (null != _controller.callCancle)
			{
				_controller.callCancle ();
			}

			//var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
			//controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;
			//controller.isInitPayback = true;
			//controller.setVisible (true);


		}

		private void _KnowHandler(GameObject go)
		{
			//if (null != img_bgimg)
			//{
			//img_bgimg.SetActiveEx (false);
			//}

			//TweenTools.MoveAndScaleTo("gametipboard/Content", "uibattle/top/financementor",_MoveHideWindow);			

			if (null != _controller.callSure)
			{
				_controller.callSure ();
			}

            if (null != _controller)
            {
                _controller.setVisible(false);
            }

            if (GameModel.GetInstance.isNeedNewVersion)
            {
#if UNITY_ANDROID               
               Application.OpenURL(GameModel.GetInstance.androidDownPath);

#elif UNITY_IPHONE
                Console.Error.WriteLine("ddd:---"+GameModel.GetInstance.iosDownPath);
              Application.OpenURL(GameModel.GetInstance.iosDownPath);
#endif



            }

        }

	

		private Text lb_txt;

		private Button btn_cancle;
		private Button btn_sure;

		private Image img_bgimg;
	}
}

