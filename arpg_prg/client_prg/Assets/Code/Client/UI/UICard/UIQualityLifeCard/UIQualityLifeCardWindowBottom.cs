using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public partial class UIQualityLifeCardWindow
	{
		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			_btnBorrow = go.GetComponentEx<Button> (Layout.btn_borrow);
		}


		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
			EventTriggerListener.Get (_btnBorrow.gameObject).onClick += _onBorrowHandler;
			_btnBorrow.SetActiveEx (false);
		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
			EventTriggerListener.Get (_btnBorrow.gameObject).onClick -= _onBorrowHandler;

//			if (!_playerManager.IsHostPlayerTurn())
//			{
//				var waitTime = MathUtility.Random(2, 5);
//				_timer = new Counter(waitTime);
//			}
		}

		private void _onBorrowHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			
			var _contro = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
			if (GameModel.GetInstance.isPlayNet == false)
			{
				_contro.playerInfor = PlayerManager.Instance.HostPlayerInfo;
				_contro.setVisible (true);
			}
			else
			{
				NetWorkScript.getInstance ().GetBorrowInfor ();
			}

		

			if (_isAddBorrow == false)
			{
				_isAddBorrow = true;
				_leftTime += _addTime;
			}

			_contro.SetTime (_leftTime);

			GameModel.GetInstance.borrowBoardTime = _leftTime;
		}

		private void _ShowBorrowBtn()
		{
			_btnBorrow.SetActiveEx (true);
		}


		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}

			if (_playerManager.IsHostPlayerTurn())
			{
				//TODO HostPlayer Behaviour
				var tmpState=_controller.HandlerCardData ();
				if (tmpState == 1)
				{
					_handleSuccess = true;
					_HideBgImg ();
					_controller.NetBuyCard ();
					TweenTools.MoveAndScaleTo("qualitylifecard/Content", "uibattle/top/financementor", _CloseHandler);
				}
				else if(tmpState==0)
				{
					_ShowBorrowBtn ();
				}
			}

		}

		private void _CloseHandler()
		{

			_controller.setVisible(false);
			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
		}

		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}

			if (_playerManager.IsHostPlayerTurn())
			{
				_handleSuccess = true;
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
				_controller.QuitCard ();
			}
		}

		/// <summary>
		/// Ons the bottom tick. NPC  3--5秒自动选择
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		private void _OnBottomTick(float deltaTime)
		{
//			if (null != _timer && _timer.Increase(deltaTime))
//			{
//				_timer = null;
				//TODO NPC Behaviour
//				_controller.HandlerCardData ();
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//				_controller.setVisible(false);
//			}
		}

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnBorrow;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

