using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class GameTipBoardWindow
	{
		private void _InitCenter(GameObject go)
		{
			lb_txt = go.GetComponentEx<Text> (Layout.lb_txt);
			btn_tip = go.GetComponentEx<Button> (Layout.btn_tip);
			btn_know = go.GetComponentEx<Button> (Layout.btn_know);
			img_bgimg = go.GetComponentEx<Image> (Layout.img_bgimg);
		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get(btn_tip.gameObject).onClick+=_HideGameWindow;
			EventTriggerListener.Get(btn_know.gameObject).onClick +=_KnowHandler;
			if (null != _controller)
			{
				lb_txt.text = _controller.gameTip;
			}

		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_tip.gameObject).onClick -= _HideGameWindow;
			EventTriggerListener.Get (btn_know.gameObject).onClick += _KnowHandler;
		}


		private void _HideGameWindow(GameObject go)
		{
			if (null != _controller) 
			{
				_controller.setVisible (false);
			}

			if (GameModel.GetInstance.isPlayNet == false)
			{
				var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
				controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;
				controller.isInitPayback = true;
				controller.setVisible (true);
			}
			else
			{
				NetWorkScript.getInstance ().GetBorrowInfor ();
			}



			_ShowGamePayBtn ();
		}


		private void _KnowHandler(GameObject go)
		{
			if (null != img_bgimg)
			{
				img_bgimg.SetActiveEx (false);
			}

			TweenTools.MoveAndScaleTo("gametipboard/Content", "uibattle/top/financementor",_MoveHideWindow);
		}


		private void _MoveHideWindow()
		{
			_controller.setVisible (false);
			_ShowGamePayBtn ();
		}

		private void _ShowGamePayBtn()
		{
			var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
			controller.ShowPaybackBtn ();
		}
	

		private Text lb_txt;
		private Button btn_tip;
		private Button btn_know;

		private Image img_bgimg;
	}
}

