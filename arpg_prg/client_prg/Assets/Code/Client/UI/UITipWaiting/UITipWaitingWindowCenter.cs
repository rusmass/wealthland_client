using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UITipWaitingWindow
	{
		private void _InitCenter(GameObject go)
		{
			lb_leftTime = go.GetComponentEx<Text> (Layout.lb_lefetime);
			btn_cancle = go.GetComponentEx<Button> (Layout.btn_cancle);
		}

		private void _ShowCenter()
		{
			NetWorkScript.getInstance ().startToNetGame();
			_isaddrobot = false;
			EventTriggerListener.Get(btn_cancle.gameObject).onClick+=_OnClickCancleHandler;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get(btn_cancle.gameObject).onClick-=_OnClickCancleHandler;
		}

		private void _OnClickCancleHandler(GameObject go)
		{
			_controller.setVisible (false);

			if (null != callBack)
			{
				callBack ();
			}
		}

		public void SetCallBack(Action _callBack)
		{
			callBack = _callBack;
		}


		public void DoTick(float delayTime)
		{
			if (_leftTime > 0)
			{
				_leftTime -= delayTime;
				if (null != lb_leftTime)
				{
					lb_leftTime.text = GetTime(_leftTime);
				}

				if (_leftTime < 1 && _isaddrobot==false)
				{
					_isaddrobot = true;
					NetWorkScript.getInstance ().startWidthRobot ();
				}

			}
			else if(_leftTime<=0)
			{
				//				lb_time.text ="0";

				btn_cancle.SetActiveEx (false);
				_leftTime -= delayTime;

				if (_leftTime < -1)
				{
					
					Console.WriteLine ("kakkakakakak");
					_controller.setVisible (false);

					NetWorkScript.getInstance ().exitToNetGame ();

					if (null != callBack)
					{
						callBack ();
					}
				}


			}
		}


		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		//ytf20161018添加卡牌倒计时
		private float _limitTime=16;
		private float _leftTime=16f;

		private bool _isaddrobot = false;

		private Text lb_leftTime;
		private Button btn_cancle;
		private Action callBack;
	}
}

