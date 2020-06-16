using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIInnerFateCardWindow
	{
		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			_btnBorrow = go.GetComponentEx<Button> (Layout.btn_borrow);

//			_btnSureInitPosition = _btnSure.transform.localPosition;
//			_btnCancleInitPosition = _btnCancle.transform.localPosition;
		}


		private void _OnShowBottom()
		{			
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
			EventTriggerListener.Get(_btnBorrow.gameObject).onClick+=_OnClickBorrowHandler;
			if (_controller.cardData.fateType != 3)
			{
				_fateShowBorrow = true;
				_ChangeSizeForBoarrow ();
				_btnBorrow.SetActiveEx (false);
			}
			else
			{
				_btnCancle.SetActiveEx (false);
				var tmpPosition = _btnSure.transform.localRotation;
				_btnSure.transform.localPosition = new Vector3 (0,tmpPosition.y,tmpPosition.z);
			}
		}


		private void _OnHideBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick -= _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick -= _onCancleHandler;
			EventTriggerListener.Get(_btnBorrow.gameObject).onClick-=_OnClickBorrowHandler;

			if (!_playerManager.IsHostPlayerTurn())
			{
				var waitTime = MathUtility.Random(2, 5);
				_timer = new Counter(waitTime);
			}
		}


		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (_playerManager.IsHostPlayerTurn())
			{
				if (_selfQuit == true) 
				{
					return;
				}

				if (_controller.cardData.fateType == 1)
				{
					if (_controller.HasMoneyBuyCraps () == true)
					{
						_handleSuccess = true;
						_ThrollCraps ();
					}
					else
					{
						if (_fateShowBorrow==true)
						{
							_ChangeBtnPositionForBorrow ();
						}
					}

					return;
				}

				//TODO HostPlayer Behaviour
				if (_controller.HandlerCardData () == true)
				{
					_handleSuccess = true;
					_HideBgImg ();
					TweenTools.MoveAndScaleTo("innerfate/Content", "uibattle/top/financementor", _CloseHandler);
				}
				else
				{
					if (_fateShowBorrow==true)
					{
						_ChangeBtnPositionForBorrow ();
					}
				}
			}

		}

		private void _CloseHandler()
		{
			_controller.setVisible(false);
			_controller.NetBuyCard (_controller.crapNum);
			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
		}


		private void _ChangeBtnPositionForBorrow()
		{
			_btnSure.transform.localPosition = _btnSureTargetPosition;
			_btnCancle.transform.localPosition = _btnCancleTargetPosition;
			_btnBorrow.SetActiveEx (true);
		}

		private void _onCancleHandler(GameObject go)
		{
			if (_selfQuit == true) 
			{
				return;
			}
			Audio.AudioManager.Instance.BtnMusic ();
			if (_playerManager.IsHostPlayerTurn())
			{
				_handleSuccess = true;
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
			}
		}

		private void _OnClickBorrowHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();

			if (null != controller)
			{

				if (GameModel.GetInstance.isPlayNet == false)
				{
					controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;
					controller.setVisible (true);
				}
				else
				{
					NetWorkScript.getInstance ().GetBorrowInfor ();
				}


				if (_isAddBorrow == false)
				{
					_isAddBorrow = true;
					_leftTime+=addtime;
				}
				controller.SetTime (_leftTime);

				GameModel.GetInstance.borrowBoardTime = _leftTime;
			}			
		}


		private void _OnBottomTick(float deltaTime)
		{
			if (null != _timer && _timer.Increase(deltaTime))
			{
				_timer = null;
				//TODO NPC Behaviour
//				_controller.HandlerCardData ();
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//				_controller.setVisible(false);
			}
		}


		/// <summary>
		/// Trolls the craps.掷色子
		/// </summary>
		private void _ThrollCraps()
		{
			_isShowAction = true;

			animator_crap.SetActiveEx (true);
			animator_crap.enabled = true;
			crapsNum = UnityEngine.Random.Range (1,6);
			_controller.crapNum = crapsNum;
		}

		private void _HideCrapAndHandler()
		{
			animator_crap.SetActiveEx (false);
			animator_crap.enabled = false;

			img_carp.SetActive (true);
			img_carp.Load (string.Format(_tempImagePath,crapsNum.ToString()));

		}

		private void _controllerHandler()
		{
			_controller.HandlerCardData ();
			_HideBgImg ();
			TweenTools.MoveAndScaleTo("innerfate/Content", "uibattle/top/financementor", _CloseHandler);
		}

		private void _SelfHandler()
		{
			if (_controller.cardData.fateType == 3)
			{
				_controllerHandler ();
			}
			else if(_controller.cardData.fateType==1 || _controller.cardData.fateType==2)
			{
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				_controller.setVisible(false);
			}
		}
		 

		private string _tempImagePath = "share/atlas/battle/battlemain/dian_{0}.ab";

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnBorrow;

//		private Vector3 _btnSureInitPosition;
		private Vector3 _btnSureTargetPosition = new Vector3 (-129, 0, 0);

//		private Vector3 _btnCancleInitPosition;
		private Vector3 _btnCancleTargetPosition = new Vector3 (135,0,0);

		private bool _fateShowBorrow=false;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

