using System;
using UnityEngine;
using UnityEngine.UI;

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


		}

		private void _OnShowTop()
		{
			_timeStart ();
		}

		private void _HideBgImg()
		{
			if (null != img_bg)
			{
				Console.WriteLine ("kakkakkakka");
				img_bg.enabled = false;
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
				if (null != _leftTime)
				{
					lb_time.text = GetTime(_leftTime);
				}

			}
			else
			{
				//				lb_time.text ="0";
				_selfQuit=true;
				_controller.NetQuitGame ();
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				if (GameModel.GetInstance.isPlayNet == true)
				{
					MessageHint.Show ("其他玩家正在操作中");
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

	}
}

