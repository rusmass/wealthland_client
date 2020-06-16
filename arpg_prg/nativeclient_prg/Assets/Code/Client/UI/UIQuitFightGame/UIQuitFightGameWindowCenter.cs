using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIQuitFightGameWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);
			btn_cancle = go.GetComponentEx<Button> (Layout.btn_cancle);

			lb_tip = go.GetComponentEx<Text> (Layout.lb_tip);
			lb_num = go.GetComponentEx<Text> (Layout.lb_number);

			lb_lefttime = go.GetComponentEx<Text> (Layout.lb_lefttime);

		}

		private void _ShowCenter()
		{
			EventTriggerListener.Get (btn_sure.gameObject).onClick += _OnClickSureHandler;
			EventTriggerListener.Get (btn_cancle.gameObject).onClick += _OnClickCancleHandler;

			lb_tip.text = string.Format ("{0}申请解散游戏，您是否同意", _controller.faqirenInfor);

			if (_controller._isHideBtn == true)
			{
				_HideButton ();
			}

			GameModel.GetInstance.isNetGameSetShow = true;

			ShowSelcetNum (_controller.agreeNum);
		}


		public void ShowSelcetNum(int value)
		{
			lb_num.text = string.Format ("已经有{0}({1})名玩家同意",value,_controller.totalNum);
		}

		private void _HideCenter()
		{
			GameModel.GetInstance.isNetGameSetShow = false;
			EventTriggerListener.Get (btn_sure.gameObject).onClick -= _OnClickSureHandler;
			EventTriggerListener.Get (btn_cancle.gameObject).onClick -= _OnClickCancleHandler;
			_controller.InitData ();
		}
		private void _OnClickSureHandler(GameObject go)
		{
			NetWorkScript.getInstance ().AgreeQuitGame ();
			_HideButton ();
		}

		private void _OnClickCancleHandler(GameObject go)
		{
			NetWorkScript.getInstance ().RefuseQuitGame();
			_HideButton ();
		}
			

		public void _HideButton()
		{
			_isSelect = true;
			btn_sure.SetActiveEx (false);
			btn_cancle.SetActiveEx (false);
		}

		/// <summary>
		/// Ticks the center.倒计时为0是自动处理
		/// </summary>
		/// <param name="deltatime">Deltatime.</param>
		public void TickCenter (float deltatime)
		{
			if (_isConunt == false)
			{
				return;
			}

			_leftTime -= deltatime;
			if (null != lb_lefttime)
			{
				lb_lefttime.text = GetTime (_leftTime);
			}

			if (_leftTime <=0)
			{
				_isConunt = false;
				if (null != lb_lefttime)
				{
					lb_lefttime.text = "00";

					if (_isSelect == false)
					{
						_isSelect = true;
						//NetWorkScript.getInstance ().AgreeQuitGame ();
					}
                    _controller.setVisible(false);
					_HideButton ();
//					_ExitGame ();
				}
			}
		}



		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		private Button btn_sure;
		private Button btn_cancle;

		private Text lb_tip;

		private Text lb_num;

		private Text lb_lefttime;

		private bool _isSelect=false;


		private bool _isConunt=true;
		//private float _countTime=0;
		private float _leftTime=30;
	}
}

