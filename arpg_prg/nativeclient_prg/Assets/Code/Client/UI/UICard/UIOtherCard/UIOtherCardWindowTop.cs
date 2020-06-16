using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIOtherCardWindow
	{
		private void _InitTop(GameObject go)
		{
			img_title = go.GetComponentEx<Image> (Layout.img_title);
			img_display = new UIImageDisplay (img_title);

			img_bg = go.GetComponentEx<Image> (Layout.img_bg);

			img_clock = go.GetComponentEx<Image> (Layout.img_clock);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);

            _cardTransform = go.DeepFindEx(Layout.transform_card);

            btn_closeShow = go.GetComponentEx<Button>(Layout.btn_closeshow);

            //			_cardAction = go.DeepFindEx (Layout.cardAction);
            //			_cardAction1 = go.DeepFindEx(Layout.cardAction1);
            _bottom = go.DeepFindEx(Layout.bottom);
        }

		private void _OnShowTop()
		{
			_timeStart();
            img_clock.SetActiveEx(false);
            _cardTransform.localScale = Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(_cardTransform.DOScale(1,duringTime));//new Vector3(1, 6000, 1)
            sequence.Join(_cardTransform.DORotate(new Vector3(0, 360, 0), duringTime,RotateMode.FastBeyond360));
            sequence.SetDelay(delayTime);
            //sequence.OnStart(OnTweenStart);
            sequence.OnComplete(OnTweenComplete);
            EventTriggerListener.Get(btn_closeShow.gameObject).onClick = _CloseShowHandler;

            isOnlyShow = _controller.IsOnlyShow;

            btn_closeShow.SetActiveEx(false);
            if (isOnlyShow == false)
            {
              
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
                _controller.HandlerCardData();
                Client.Unit.BattleController.Instance.Send_RoleSelected(0);
            }
        }

        private void OnTweenStart()
        {
            _cardTransform.SetActiveEx(true);
        }
        private void OnTweenComplete()
        {
            img_clock.SetActiveEx(true);
            _cardTransform.localScale = Vector3.one;
            _cardTransform.rotation = Quaternion.identity;// new Quaternion(1, 1, 1, 1);

            if (isOnlyShow == false)
            {
                
            }
            else
            {
                btn_closeShow.SetActiveEx(true);
               
            }
        }

		/// 加载卡牌标题
		private void setTitle (string str)
		{
			if (null != img_display)
			{
				img_display.Load (str);
				img_title.SetNativeSize ();
			}
		}
		private void _OnDisposeTop()
		{
			if (null != img_display)
			{
				img_display.Dispose();
			}
		}
		private void _HideBgImg()
		{
			if (null != img_bg)
			{
				img_bg.SetActiveEx (false);
			}
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

		private void _TickTop(float deltatime)
		{
			_TimeUpdateHandler (deltatime);
		}

		#region 倒计时功能

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

			if (_initClock==false || _handleSuccess == true||_selfQuit==true)
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
                    _SelfHandler();

                }
                
                //_controller.setVisible(false);
                
			}
		}

		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		//ytf20161018添加卡牌倒计时
		private float _limitTime=31;
		private float _leftTime=31f;

		//private float _addTime=31;
		//private bool _isAddedBorrow=false;

		private bool _initClock=false;

	
		private bool _handleSuccess=false;
		private bool _selfQuit=false;
		private Image img_clock;
		private Text lb_time;

        #endregion

        /// <summary>
        /// 20180619 展示模式的关闭按钮
        /// </summary>
        private Button btn_closeShow;
        private bool isOnlyShow = false;
        private Transform _bottom;

        private Image img_bg;

		private Image img_title;
		private UIImageDisplay img_display;


        private Transform _cardTransform;
        /// <summary>
        /// 缓动执行的是时间
        /// </summary>
        private float duringTime=0.8f;
        /// <summary>
        /// 缓动延迟的时间
        /// </summary>
        private float delayTime=0.4f;

		/// <summary>
		/// 卡牌动画obj
		/// </summary>
//		private Transform _cardAction;
//		private Transform _cardAction1;

	}
}

