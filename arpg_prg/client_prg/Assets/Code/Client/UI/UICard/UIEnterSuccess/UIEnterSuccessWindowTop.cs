using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public partial class UIEnterSuccessWindow
    {
        private void _InitTop(GameObject go)
        {
            _titleTxt = go.GetComponentEx<Text>(Layout.lb_priseInfor);
            lb_countTime = go.GetComponentEx<Text>(Layout.lb_countTime);
            //_btnClose = go.GetComponentEx<Button>(Layout.btn_sure);
        }

        private void _OnShowTop()
        {
            var tipString = "恭喜你获得胜利";
            if (PlayerManager.Instance.IsHostPlayerTurn() == false)
            {
                tipString = string.Format("恭喜<color=#e94444ff>{0}</color>获得胜利", _controller.playerInfor.playerName);
            }
            _titleTxt.text = tipString;
            //EventTriggerListener.Get(_btnClose.gameObject).onClick += _onSureHandler;
            _timeStart();
           // GameEventManager.Instance.SubscribeEvent(GameEvents.GameOver, _ShowGameOver);
        }

        private void _OnHideTop()
        {
            //EventTriggerListener.Get(_btnClose.gameObject).onClick -= _onSureHandler;
           // GameEventManager.Instance.UnsubscribeEvent(GameEvents.GameOver, _ShowGameOver);
        }

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="go"></param>
        private void _onSureHandler(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();
            if (_playerManager.IsHostPlayerTurn())
            {
                _controller.setVisible(false);
            }
        }

        private void _timeStart()
        {
            _leftTime = _limitTime;
            lb_countTime.text = _leftTime.ToString();

            UISynergy.Instance.ChanceToGameOver();

        }

        private void _OnTopTick(float deltaTime)
        {
            if (GameModel.GetInstance.AlowGameCount() == false)
            {
                return;
            }

            if (isCount == false)
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
                _controller.setVisible(false);

            }
        }

        /// <summary>
        /// 显示游戏结束
        /// </summary>
        private void _ShowGameOver(GameEventArgs args)
        {
            var overHandler = Client.UIControllerManager.Instance.GetController<UIGameOverWindowController>();
            overHandler.setVisible(true);
        }

        private string GetTime(float time)
        {
            return HandleNumToTimeTool.ChangeNumberToTime(time);
        }

        private float _limitTime = 9;
        private float _leftTime = 9f;
        private bool isCount = true;
       
        private Text _titleTxt;
        private Text lb_countTime;
        //private Button _btnClose;

        private Counter _timer;
        private PlayerManager _playerManager = PlayerManager.Instance;
    }
}
