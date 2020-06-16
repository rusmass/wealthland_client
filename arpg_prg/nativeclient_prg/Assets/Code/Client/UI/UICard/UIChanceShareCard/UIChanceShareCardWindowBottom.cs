using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIChanceShareCardWindow
	{

		private void _InitBottom (GameObject go)
		{
			_btnSure = go.GetComponentEx<Button> (Layout.btn_sure);
			_btnCancle = go.GetComponentEx<Button> (Layout.btn_cancle);
			_btnSale = go.GetComponentEx<Button> (Layout.btn_sale);
            _lbSaleTip = go.GetComponentEx<Text>(Layout.bottom_tip);

        }


		private void _OnShowBottom()
		{
			EventTriggerListener.Get (_btnSure.gameObject).onClick += _onSureHandler;
			EventTriggerListener.Get (_btnCancle.gameObject).onClick += _onCancleHandler;
			EventTriggerListener.Get (_btnSale.gameObject).onClick += _OnSaleHandler;

			if (_playerManager.IsHostPlayerTurn()==false)
			{
				_btnSure.SetActiveEx (false);
                _lbSaleTip.SetActiveEx(true);
			}
            else
            {
                _lbSaleTip.SetActiveEx(false);
            }

			if (null != _controller)
			{
				if(_controller.HasSameTypeShare()==false)
				{
					_btnSale.SetActiveEx (false);
				}
			}

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
			EventTriggerListener.Get (_btnSale.gameObject).onClick -= _OnSaleHandler;
		}


		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_ShowChangeBoard (true);
		}

		private void _OnSaleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_ShowChangeBoard (false);
		}

		private void _CloseHandler()
		{
			_controller.setVisible(false);

			if (GameModel.GetInstance.isPlayNet == true)
			{
				MessageHint.Show ("其他玩家正在操作中");
			}

			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
		}



		private void _onCancleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true)
			{
				return;
			}

			_controller.QuitCard ();
			_controller.NetQuitGame ();
			_handleSuccess = true;
			Client.Unit.BattleController.Instance.Send_RoleSelected (0);
			if (GameModel.GetInstance.isPlayNet == true)
			{
				MessageHint.Show ("其他玩家正在操作中");
			}
			_controller.setVisible(false);

            //			if (_playerManager.IsHostPlayerTurn())
            //			{
            //				
            //			}
            //Console.Error.WriteLine("sss");
		}


		//卡牌界面不会在界面执行自动选择
		private void _OnBottomTick(float deltaTime)
		{
            if(GameModel.GetInstance.isPlayNet==true)
            {
                return;
            }

            if (null != _timer && _timer.Increase(deltaTime))
            {
                _timer = null;
                //W_controller.QuitCard();
               // _controller.NetQuitGame();
                Client.Unit.BattleController.Instance.Send_RoleSelected(0);
                _controller.setVisible(false);
            }
        }

		private Button _btnSure;
		private Button _btnCancle;
		private Button _btnSale;
        private Text _lbSaleTip;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

