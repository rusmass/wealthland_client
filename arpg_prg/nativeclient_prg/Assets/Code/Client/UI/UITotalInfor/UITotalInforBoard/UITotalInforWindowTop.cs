using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UITotalInforWindow
	{
		private void _OnInitTop(GameObject  go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			var img = go.GetComponentEx<Image> (Layout.img_title);
			_titleImg = new UIImageDisplay (img);

			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			img_clock = go.GetComponentEx<Image> (Layout.img_clock);
		}

		private void _OnShowTop()
		{
			EventTriggerListener.Get(btn_close.gameObject).onClick+=_OnCloseWindow;
			if (null != _controller)
			{
				if (null != _controller.playerInfor)
				{
					if (_controller.playerInfor.isEnterInner == true)
					{
						_titleImg.Load (_loadImgTitlePath);
					}
				}
			}
			_timeStart ();
			img_clock.SetActiveEx (false);
		}

		private void _OnHideTop()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnCloseWindow;
		}

		private void _OnDisposeTop()
		{
			if (null != _titleImg)
			{
				_titleImg.Dispose ();
			}
		}

		private void _OnCloseWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			CloseHandler ();
		}


		public void CloseHandler()
		{
			_controller.setVisible (false);


			GameModel.GetInstance.hasLoadBalanceAndIncome = false;
			GameModel.GetInstance.hasLoadCheck = false;
			GameModel.GetInstance.hasLoadDebtAndPay = false;
			GameModel.GetInstance.hasLoadSaleInfor = false;
			GameModel.GetInstance.hasLoadTarget = false;

			_OnCloseAllWindow ();
		}

		private void _TickTop(float deltaTime)
		{
			_TimeUpdateHandler (deltaTime);
		}

		#region 倒计时部分

		private void _timeStart()
		{
			_leftTime = _limitTime;
			lb_time.text = _leftTime.ToString();
		}

		public void SetTime(float lefttime)
		{
			_limitTime = lefttime;
			_timeStart ();
			img_clock.SetActiveEx (true);
			_isClockStart = true;

		}


		private void _TimeUpdateHandler(float deltaTime)
		{

			if (_isClockStart == false)
			{
				return;
			}

			if (_leftTime > 0)
			{
				_leftTime -= deltaTime;
				lb_time.text = GetTime(_leftTime);
			}
			else
			{
				lb_time.text ="0";
				_OnCloseWindow (null);
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

		//ytf20161018添加卡牌倒计时
		private float _limitTime=31;
		private float _leftTime=0f;

		private bool _isClockStart = false;

		//private float _addTime = 31f;
		//private bool _isAddBorrow=false;

		private Text lb_time;
		private Image img_clock;


		#endregion


		private Button btn_close;
		private UIImageDisplay _titleImg;
		private string _loadImgTitlePath="share/atlas/battle/newborrow/biaoti2.ab";
	}
}

