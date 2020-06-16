using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIChanceShareCardWindow
	{
		private void _InitTop(GameObject go)
		{
			img_bg = go.GetComponentEx<Image> (Layout.img_bg);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			img_clock = go.GetComponentEx<Image> (Layout.img_clock);

			_initPostion = img_clock.transform.localPosition;

//			_cardAction = go.DeepFindEx (Layout.cardAction);
//			_cardAction1 = go.DeepFindEx(Layout.cardAction1);

            _cardTransform = go.DeepFindEx(Layout.transform_card);
            btn_closeShow = go.GetComponentEx<Button>(Layout.btn_closeshow);
            _bottom = go.DeepFindEx(Layout.bottom);
            

        }

		private void _OnShowTop()
		{
			_timeStart ();

            _cardTransform.localScale = Vector3.zero;

            _cardTransform.DOScale(1, _duringTime).SetDelay(_dalayTime);//.SetEase(Ease.OutBounce) ;
            EventTriggerListener.Get(btn_closeShow.gameObject).onClick = _CloseShowHandler;
            isOnlyShow = _controller.IsOnlyShow;

            if(isOnlyShow==false)
            {
                btn_closeShow.SetActiveEx(false);
            }
            else
            {
                _bottom.SetActiveEx(false);
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
                //_controller.HandlerCardData();
                Client.Unit.BattleController.Instance.Send_RoleSelected(0);
            }
        }
        private void _HideBgImg()
		{
			if (null != img_bg)
			{
				Console.WriteLine ("kakkakkakka");
				img_bg.enabled = false;
			}

			if (null != img_chagne)
			{
				img_chagne.SetActiveEx(false);
				_SetClockPositionForInit ();
				_normalBoard.SetActiveEx (true);
			}
		}

		private void _SetClockPositionForChange()
		{
			img_clock.rectTransform.localPosition = _clockPositionChange;

			if (null != img_chagne)
			{
				img_clock.transform.SetParent (img_chagne.transform);
			}
		}



		private void _SetClockPositionForInit()
		{
			img_clock.rectTransform.localPosition = _initPostion;

			if (null != _normalBoard)
			{
				img_clock.transform.SetParent (_normalBoard);
			}
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

			if (_initClock==false||_handleSuccess == true ||_selfQuit==true)
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
				//				lb_time.text ="0";
				_selfQuit=true;
                if(isOnlyShow==false)
                {
                    _controller.NetQuitGame();
                    if (GameModel.GetInstance.isPlayNet == true)
                    {
                        MessageHint.Show("其他玩家正在操作中");
                    }
                    Client.Unit.BattleController.Instance.Send_RoleSelected(0);
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

        private Transform _bottom;

        private bool isOnlyShow=false;

        //ytf20161018添加卡牌倒计时
        private float _limitTime=61;
		private float _leftTime=61f;

		private bool _initClock=false;

		private float _addTime=31;
		private bool _isAddBorrow=false;

		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;
		private Image img_clock;

		/// <summary>
		/// 卡牌动画obj
		/// </summary>
//		private Transform _cardAction;
//		private Transform _cardAction1;

		private Vector3 _clockPositionChange = new Vector3 (-381, 204, 0);
		private Vector3 _initPostion;

		private Image img_bg;

        /// <summary>
        /// 缓动效果的transfor
        /// </summary>
        private Transform _cardTransform;
        /// <summary>
        /// 缓动的时间
        /// </summary>
        private float _duringTime = 0.4f;
        /// <summary>
        /// 延迟的时间
        /// </summary>
        private float _dalayTime = 0.4f;
	}
}

