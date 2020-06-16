using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
    public partial class UIInvestmentCardWindow
    {
        private void _InitTop(GameObject go)
        {
            img_bg = go.GetComponentEx<Image>(Layout.img_bg);
            lb_time = go.GetComponentEx<Text>(Layout.lb_time);
            //			_cardAction = go.DeepFindEx (Layout.cardAction);
            //			_cardAction1 = go.DeepFindEx(Layout.cardAction1);

            _cardTransform = go.DeepFindEx(Layout.cardTransform);
            btn_closeShow = go.GetComponentEx<Button>(Layout.btn_closeshow);
            _bottom = go.DeepFindEx(Layout.bottom);
        }

        private void _OnShowTop()
        {
            _timeStart();
            if (null != img_bg)
            {
                img_bg.SetActiveEx(true);
            }

            _cardTransform.Rotate(new Vector3(20, 20, 0));
            _cardTransform.localPosition = new Vector3(900, 0, 0);

            var sequence = DOTween.Sequence();
            sequence.Append(_cardTransform.DOMoveX(0, 1));
            sequence.Append(_cardTransform.DOShakeRotation(0.3f, 10, 30, 0));
            sequence.Append(_cardTransform.DORotate(Vector3.zero, 0.3f));//.SetEase(Ease.InBounce));
            //sequence.OnComplete(OnSequenceEnd);

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
               // _controller.HandlerCardData();
                Client.Unit.BattleController.Instance.Send_RoleSelected(0);
            }
        }

        private void OnSequenceEnd()
        {
            _cardTransform.Rotate(Vector3.one);
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
                if(isOnlyShow==false)
                {
                    _controller.NetQuitCard();
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
        private float _limitTime=31f;
		private float _leftTime=31f;

		private float _addTime=31;
		private bool _isAddBorrow=false;
		private bool _initClock=false;

		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;

		private Image img_bg;


        /// <summary>
        /// 卡牌动画obj
        /// </summary>
        private Transform _cardTransform;

//		private Transform _cardAction;
//		private Transform _cardAction1;

	}
}

