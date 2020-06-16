using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class ClockItem
	{
		public ClockItem (GameObject go)
		{
			
		}

		public bool GetRunning(float deltaTime)
		{
			var _isContinue = false;
			if (_leftTime > 0)
			{
				_leftTime -= deltaTime;
				lb_time.text = GetTime(_leftTime);
				_isContinue = true;
			}
			else
			{
				lb_time.text ="0";
				_isContinue = false;
			}

			return _isContinue;
		}

		private string GetTime(float time)
		{
			return GetSecond(time);
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


		//private float _limitTime=16;
		private float _leftTime=0f;
		private Text lb_time;
		private Image img_bg;
	}
}

