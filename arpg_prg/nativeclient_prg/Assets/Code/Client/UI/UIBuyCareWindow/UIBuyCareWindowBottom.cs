using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBuyCareWindow
	{

		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
		}


		private void _OnShowBottom()
		{			
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
		}


		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
		}

        /// <summary>
        /// 点击购买保险
        /// </summary>
        /// <param name="go"></param>
		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			//if (_playerManager.IsHostPlayerTurn())
			//{
				//TODO HostPlayer Behaviour
				if (_controller.HandlerCardData () == true)
				{
					_HideBgImg ();
					TweenTools.MoveAndScaleTo("innerbuycare/Content", "uibattle/top/financementor", _CloseHandler);
				}
			//}
		}
        /// <summary>
        /// 关闭窗口
        /// </summary>
		private void _CloseHandler()
		{
			_controller.setVisible(false);
		}
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="go"></param>
		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.setVisible(false);
		}

		private Button _btnSure;
		private Button _btnCancle;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

