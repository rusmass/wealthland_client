using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIRiskCardWindow
	{
		
		private void _InitTop(GameObject go)
		{
			img_bg = go.GetComponentEx<Image> (Layout.img_bg);

			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
            _cardAction = go.DeepFindEx (Layout.transformCard);
            //			_cardAction1 = go.DeepFindEx(Layout.cardAction1);

            btn_closeShow = go.GetComponentEx<Button>(Layout.btn_closeshow);
            _bottom = go.DeepFindEx(Layout.bottom);
        }

		private void _OnShowTop()
		{
			_timeStart ();
            _cardAction.localPosition = _initPosition;

            Tweener tween =  _cardAction.DOLocalMoveY(0, duringTime);
            tween.SetDelay(delayTime);
            tween.SetEase(Ease.OutBounce);// 10);

            EventTriggerListener.Get(btn_closeShow.gameObject).onClick = _CloseShowHandler;

            isOnlyShow = _controller.IsOnlyShow;

            if(isOnlyShow==false)
            {
                btn_closeShow.SetActiveEx(false);
            }
            else
            {
                _bottom.SetActiveEx(false);
                _selectToggle.SetActiveEx(false);
            }

        }

        /// <summary>
        /// 关闭展示界面
        /// </summary>
        private void _CloseShowHandler(GameObject go)
        {
            _controller.setVisible(false);
            if(GameModel.GetInstance.isPlayNet==false)
            {
                _controller.HandlerCardData();
                Client.Unit.BattleController.Instance.Send_RoleSelected(0);
            }
        }

        private void _HideBgImg()
		{
			img_bg.SetActiveEx (false);
//			if (null != _cardAction)
//			{
//				_cardAction.SetActiveEx(false);
//			}
//
//			if (null != _cardAction1)
//			{
//				_cardAction1.SetActiveEx (false);
//			}
		}


		private void _timeStart()
		{
			_leftTime = _limitTime;
			lb_time.text = _leftTime.ToString();
			_initClock = true;
		}

		private void _TimeUpdateHandler(float deltaTime)
		{

			if (GameModel.GetInstance.AlowGameCount () == false)
			{
				return;
			}

			if (_initClock==false || _handleSuccess == true ||_selfQuit==true)
			{
				return;
			}

			if (_leftTime > 0)
			{
				_leftTime -= deltaTime;
				if (null != lb_time)
				{
					lb_time.text = GetTime(_leftTime);
				}
			}
			else
			{
				//lb_time.text ="0";
				_selfQuit=true;
				if (PlayerManager.Instance.IsHostPlayerTurn ())
				{
					_controller.HandlerCardData ();
				}
                if(isOnlyShow==false)
                {
                    _controller.NetBuyCard ();
				    Client.Unit.BattleController.Instance.Send_RoleSelected (1);

                }
		
				_controller.setVisible(false);
			}
		}

		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		private string GetSecond(float time)
		{
			int timer = (int)((time % 3600) % 60);
			string timerStr;
			if (timer < 10)
				timerStr = "0" + timer.ToString();
			else
				timerStr = timer.ToString();

			return timerStr;
		}

        /// <summary>
        /// 20180619 展示模式的关闭按钮
        /// </summary>
        private Button btn_closeShow;
        private bool isOnlyShow;
        private Transform _bottom;

        //ytf20161018添加卡牌倒计时
        private float _limitTime=31;
		private float _leftTime=31f;
		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;
		private bool _initClock=false;


		private Image img_bg;

        /// <summary>
        /// 卡牌动画obj
        /// </summary>
        private Transform _cardAction;
        /// <summary>
        /// 玩家初始的位置
        /// </summary>
        private Vector3 _initPosition=new Vector3(0,528,0);
        /// <summary>
        /// 缓动动画的时间
        /// </summary>
        private float duringTime = 0.8f;

        /// <summary>
        /// 动画延迟时间
        /// </summary>
        private float delayTime = 0.4f;

        //private Transform _cardAction1;
    }
}

