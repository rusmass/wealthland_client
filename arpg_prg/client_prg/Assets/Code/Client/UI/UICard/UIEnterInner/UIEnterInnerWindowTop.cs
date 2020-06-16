using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIEnterInnerWindow
	{
		private void _InitTop(GameObject go)
		{
			_titleTxt = go.GetComponentEx<Text> (Layout.lb_tipInner);
			_btnClose = go.GetComponentEx<Button> (Layout.btn_sure);
		}

		private void _OnShowTop()
		{
            var tipString = "恭喜你成功进入富人圈";
            if(PlayerManager.Instance.IsHostPlayerTurn()==false)
            {
                tipString = string.Format("恭喜<color=#e94444ff>{0}</color>成功进入富人圈", _controller.playerInfor.playerName);
            }

            _titleTxt.text = tipString;

            EventTriggerListener.Get (_btnClose.gameObject).onClick += _onSureHandler;
            _timeStart();
        }

		private void _OnHideTop()
		{
			EventTriggerListener.Get (_btnClose.gameObject).onClick -= _onSureHandler;
		}

		private void _onSureHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (_playerManager.IsHostPlayerTurn())
			{
				//TODO HostPlayer Behaviour
				_controller.HandlerCardData ();
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//				Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
				_controller.setVisible(false);

//				var tmpController = UIControllerManager.Instance.GetController<UIConditionWindowController> ();
//				tmpController.showConditionType = 1;
//				tmpController.setVisible (true);
			}
		}

        private void _timeStart()
        {
            _leftTime = _limitTime;
            lb_countTime.text = _leftTime.ToString();
        }

        private void _OnTopTick(float deltaTime)
        {
            if (GameModel.GetInstance.AlowGameCount() == false)
            {
                return;
            }

            if(isCount==false)
            {
                return;
            }

            if (_leftTime > 0)
            {
                _leftTime -= deltaTime;
                if (null != lb_countTime)
                {
                    lb_countTime.text = GetTime(_leftTime);
                }

            }
            else
            {
                isCount = false;

                if (null != lb_countTime)
                {
                    lb_countTime.text = "00";
                }
                _controller.HandlerCardData();             
                _controller.setVisible(false);
            }
        }

        private string GetTime(float time)
        {
            return HandleNumToTimeTool.ChangeNumberToTime(time);
        }


        private float _limitTime = 9;
        private float _leftTime = 9f;
        private bool isCount = true;

        // set the txt context
        public void setTitle (string str)
		{
//			_titleTxt.text = str;
		}

        private Text _titleTxt;
        private Button _btnClose;

		private Counter _timer;
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

