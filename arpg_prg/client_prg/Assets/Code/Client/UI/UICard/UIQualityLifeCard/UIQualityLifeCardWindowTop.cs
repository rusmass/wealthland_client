using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIQualityLifeCardWindow
	{
		private void _InitTop(GameObject go)
		{
			img_bg = go.GetComponentEx<Image> (Layout.img_bg);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);

//			_cardAction = go.DeepFindEx (Layout.cardAction);
//			_cardAction1 = go.DeepFindEx(Layout.cardAction1);
		}

		private void _OnShowTop()
		{
			_timeStart ();
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
				//				lb_time.text ="0";
				_selfQuit=true;
				_controller.NetQuitCard ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
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

		//ytf20161018添加卡牌倒计时
		private float _limitTime=31;
		private float _leftTime=31f;

		private float _addTime=31f;
		private bool _isAddBorrow=false;
		private bool _initClock=false;

		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;



		private Image img_bg;

		/// <summary>
		/// 卡牌动画obj
		/// </summary>
//		private Transform _cardAction;
//		private Transform _cardAction1;
	}
}

