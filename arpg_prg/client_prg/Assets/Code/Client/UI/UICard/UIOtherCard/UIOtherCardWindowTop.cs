using System;
using UnityEngine;
using UnityEngine.UI;

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

//			_cardAction = go.DeepFindEx (Layout.cardAction);
//			_cardAction1 = go.DeepFindEx(Layout.cardAction1);
		}

		private void _OnShowTop()
		{
//			img_clock.SetActiveEx (false);
			_timeStart();
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
				_SelfHandler ();
			}
		}

		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		//ytf20161018添加卡牌倒计时
		private float _limitTime=31;
		private float _leftTime=31f;

		private float _addTime=31;
		private bool _isAddedBorrow=false;

		private bool _initClock=false;

	
		private bool _handleSuccess=false;
		private bool _selfQuit=false;
		private Image img_clock;
		private Text lb_time;

		#endregion

		private Image img_bg;

		private Image img_title;
		private UIImageDisplay img_display;

		/// <summary>
		/// 卡牌动画obj
		/// </summary>
//		private Transform _cardAction;
//		private Transform _cardAction1;

	}
}

