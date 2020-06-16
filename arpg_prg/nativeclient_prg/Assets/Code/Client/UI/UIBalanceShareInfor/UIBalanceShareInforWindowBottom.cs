using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBalanceShareInforWindow
	{
		private void _InitBottom (GameObject go)
		{
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
		}


		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
		}

		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.setVisible(false);
		}


		private Button _btnCancle;

		private Counter _timer;
//		private PlayerManager _playerManager = PlayerManager.Instance;

	}
}

