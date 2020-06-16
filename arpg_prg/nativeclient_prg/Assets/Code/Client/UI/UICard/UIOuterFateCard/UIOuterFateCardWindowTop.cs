using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIOuterFateCardWindow
	{
		private void _InitTop(GameObject go)
		{

            GameModel.GetInstance.MathWidthOrHeightByCondition(go, 0);

			img_bg = go.GetComponentEx<Image> (Layout.img_bg);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			img_clock = go.GetComponentEx<Image> (Layout.img_clock);

			_clockPositionInit = img_clock.rectTransform.localPosition;
			_cardAction = go.DeepFindEx (Layout.gameobjectCard).gameObject;
            btn_closeShow = go.GetComponentEx<Button>(Layout.btn_closeshow);
            //			_cardAction1 = go.DeepFindEx(Layout.cardAction1);

            _bottom = go.DeepFindEx(Layout.bottom);

		}

		private void _OnShowTop()
		{
			_timeStart ();

            _colorGroup = _cardAction.AddComponent<CanvasGroup>();
            _colorGroup.alpha = 0;
            var tweener = _colorGroup.DOFade(1, duringTime).SetDelay(delayTime);//.SetEase(Ease.InBounce);  

          
            EventTriggerListener.Get(btn_closeShow.gameObject).onClick = _CloseShowHandler;
            isOnlyShow = _controller.IsOnlyShow;

            if(isOnlyShow == false )
            {
                btn_closeShow.SetActiveEx(false);
            }else
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

			if(null !=_saleImg)
			{
				_saleImg.SetActiveEx (false);
				_SetColorPositionInit ();
				_normalBoard.SetActiveEx (true);
			}
		}

		private void _SetClockPositionForSale()
		{
			img_clock.rectTransform.localPosition = _clockPositionForSale;
			img_clock.transform.SetParent(_saleImg.transform);
		}

		private void _SetColorPositionInit()
		{			
			img_clock.rectTransform.localPosition=_clockPositionInit;
			img_clock.transform.SetParent (_normalBoard.transform);
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
				//				lb_time.text ="0";
				_selfQuit=true;
                if(isOnlyShow==false)
                {
                    _SelfHandler ();
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
        private bool isOnlyShow;

        //ytf20161018添加卡牌倒计时
        private float _limitTime=61;
		private float _leftTime=61f;
		private Text lb_time;

		private bool _initClock=false;

		private bool _handleSuccess=false;
		private bool _selfQuit=false;

		private Image img_clock;

		private Vector3 _clockPositionForSale=new Vector3(-408,207,0);
		private Vector3 _clockPositionInit;



		private Image img_bg;

        /// <summary>
        /// 卡牌动画obj
        /// </summary>
        private GameObject _cardAction;
        /// <summary>
        /// 控制整体颜色渐变的组件
        /// </summary>
        private CanvasGroup _colorGroup;
        /// <summary>
        /// 渐变的过度时间
        /// </summary>
        private float duringTime = 0.6f;
        /// <summary>
        /// 动画延时的时间
        /// </summary>
        private float delayTime = 0.4f;

        //		private Transform _cardAction1;

    }
}

