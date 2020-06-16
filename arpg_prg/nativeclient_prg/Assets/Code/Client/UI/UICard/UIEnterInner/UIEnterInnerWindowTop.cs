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
            var outertime = GameTimerManager.Instance.getOuterTime(_controller.playerInfor.playerID);
            var tipTime = "您在外圈的游戏时间是:"+outertime;
            if(PlayerManager.Instance.IsHostPlayerTurn()==false)
            {
                tipString = string.Format("恭喜<color=#e94444ff>{0}</color>成功进入富人圈", _controller.playerInfor.playerName);
                tipTime = string.Format(String.Format("玩家{0}在外圈的游戏时间:{1}",_controller.playerInfor.playerName, outertime));
            }

            _titleTxt.text = tipString;
            lb_outerTimer.text = tipTime;

            EventTriggerListener.Get (_btnClose.gameObject).onClick += _onSureHandler;
          
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


        private bool isCount = true;

        // set the txt context
        public void setTitle (string str)
		{
//			_titleTxt.text = str;
		}

        private Text _titleTxt;
        private Button _btnClose;
        
		private PlayerManager _playerManager = PlayerManager.Instance;
	}
}

