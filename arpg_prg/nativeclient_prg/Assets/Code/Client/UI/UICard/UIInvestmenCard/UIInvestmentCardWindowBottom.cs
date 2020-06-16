using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIInvestmentCardWindow
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
            if (_controller.IsOnlyShow==true)
            {
                var waitTime = MathUtility.Random(GameModel.minRangeTime, GameModel.maxRangeTime);
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
            //TODO HostPlayer Behaviour
            if (_controller.cardData.isDice == 1)
            {
                if (_controller.HasMoneyBuyCraps() == true)
                {
                    _handleSuccess = true;
                    _ThrollCraps();
                }
                else
                {
                    _ShowBorrowBtn();
                }
                return;
            }

            if (_controller.HandlerCardData() == true)
            {
                _handleSuccess = true;
                _OnHideBottom();
                _HideBgImg();
                TweenTools.MoveAndScaleTo("investment/Content", "uibattle/top/financementor", _CloseHandler);
            }
            else
            {
                _ShowBorrowBtn();
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
			TweenTools.MoveAndScaleTo("investment/Content", "uibattle/top/financementor", _CloseHandler);
		}


		private void _CloseHandler()
		{
			_controller.setVisible(false);
			_controller.NetBuyCard(_controller.crapNum);
			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
		}

		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true) 
			{
				return;
			}


            _handleSuccess = true;
            _controller.NetQuitCard();
            Client.Unit.BattleController.Instance.Send_RoleSelected(0);
            _controller.setVisible(false);

        }


		private void _OnBottomTick(float deltaTime)
		{
            if(GameModel.GetInstance.isPlayNet==true)
            {
                return;
            }

            if (null != _timer && _timer.Increase(deltaTime))
            {
                _timer = null;
                //TODO NPC Behaviour
                //_controller.HandlerCardData();
                Client.Unit.BattleController.Instance.Send_RoleSelected(1);
                _controller.setVisible(false);
            }
        }

		private string _tempImagePath = "share/atlas/battle/battlemain/dian_{0}.ab";

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnBorrow;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

