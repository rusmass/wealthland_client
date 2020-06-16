﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIChanceFixedCardWindow
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

			if (!_playerManager.IsHostPlayerTurn())
			{
				var waitTime = MathUtility.Random(2, 5);
				_timer = new Counter(waitTime);
			}
		}

		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
			EventTriggerListener.Get (_btnBorrow.gameObject).onClick -= _onBorrowHandler;
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


			if (_isAddedBorrow == false)
			{
				_leftTime += _addTime;
				_isAddedBorrow = true;
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
				if (_controller.HandlerCardData () == true)
				{
					_handleSuccess = true;
					_HideBgImg ();
					TweenTools.MoveAndScaleTo("opportunitycard/Content", "uibattle/top/financementor", _CloseHandler);
				}
				else 
				{
					_ShowBorrowBtn ();
				}
			}

		}

		private void _CloseHandler()
		{
			_controller.NetBuyCard ();
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
				_controller.QuitCard ();
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
			}
		}


		//npc玩家打开卡牌后自动选择
		private void _OnBottomTick(float deltaTime)
		{
			if (null != _timer && _timer.Increase(deltaTime))
			{
				_timer = null;
				//TODO NPC Behaviour
				_controller.HandlerCardData ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
				_controller.setVisible(false);
			}
		}

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnBorrow;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}
