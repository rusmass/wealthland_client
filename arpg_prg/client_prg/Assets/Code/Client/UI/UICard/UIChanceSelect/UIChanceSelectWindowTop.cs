using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIChanceSelectWindow
	{
		private void _OnInitTop(GameObject go)
		{
			lb_time = go.GetComponentEx<Text> (Layout.lb_clock);
		}


		private void _OnShowTop()
		{
			_timeStart ();
		}

		private void _OnHideTop()
		{
			
		}

		private void _OnDisposeTop()
		{
			
		}

		private void _OnTickTop(float deltatime)
		{
			
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

			if (_initClock==false || _handleSuccess == true || _selfQuit==true)
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
				if (null != lb_time)
				{
					lb_time.text ="0";
				}
				_selfQuit=true;
				_SelfHandler ();

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
		private float _leftTime=31f;

		private bool _initClock=false;

		private float _addTime=31;
		private bool _isAddedBorrow=false;

		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;

	}
}

