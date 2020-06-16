using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UICheckOutWindow
	{
		private void _InitTop(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			btn_detail = go.GetComponentEx<Button> (Layout.btn_detail);

		//	Console.WriteLine (btn_close);
		}

		private void _OnShowTop()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick+=_ColseWindow;
			EventTriggerListener.Get (btn_detail.gameObject).onClick += _ShowDetailWindow;

			//btn_detail.SetActiveEx (false);

		}

		private void _OnHideTop()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick-=_ColseWindow;
			EventTriggerListener.Get (btn_detail.gameObject).onClick -= _ShowDetailWindow;
		}

		private void _ColseWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.setVisible (false);
		}

		private void _ShowDetailWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

//			var controller = UIControllerManager.Instance.GetController<UICheckOutDetailController> ();
//			controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;
//			controller.setVisible (true);
		}

		private Button btn_close;
		private Button btn_detail;


	}
}

