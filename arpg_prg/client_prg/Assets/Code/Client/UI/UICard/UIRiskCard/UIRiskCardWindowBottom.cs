using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public partial class UIRiskCardWindow
	{
		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
		}	

		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;


			if (!_playerManager.IsHostPlayerTurn())
            {
                var waitTime = MathUtility.Random(2, 5);
                _timer = new Counter(waitTime);

            }
        }

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
		}       
        
		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}
			_handleSuccess = true;
            if (_playerManager.IsHostPlayerTurn())
            {
                //TODO HostPlayer Behaviour
				_controller.HandlerCardData ();
            }
			_controller.NetBuyCard ();
			_HideBgImg ();
			TweenTools.MoveAndScaleTo("riskcard/Content", "uibattle/top/financementor", _CloseHandler);
		}

		private void _CloseHandler()
		{
			
			_controller.setVisible(false);
			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
		}

		private void _onCancleHandler(GameObject go)
		{
            if (_playerManager.IsHostPlayerTurn())
            {
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
                _controller.setVisible(false);
            }
		}

		/// <summary>
		/// Ons the bottom tick.  NPC进入窗口后 3--5秒自动选择
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		private void _OnBottomTick(float deltaTime)
		{
//			if (null != _timer && _timer.Increase(deltaTime))
//			{
//				_timer = null;
//				_controller.HandlerCardData ();
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//				_controller.setVisible(false);
//			}
		}

		private Button _btnSure;

        private Counter _timer;
        private PlayerManager _playerManager = PlayerManager.Instance;
    }
}

