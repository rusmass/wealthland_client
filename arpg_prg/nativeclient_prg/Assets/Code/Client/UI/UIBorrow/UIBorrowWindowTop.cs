using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBorrowWindow
	{
        /// <summary>
        /// 设置背景颜色为黑色
        /// </summary>
		public void SetWindowBlack()
		{
			if (null != img_blackbg)
			{
				img_blackbg.color = blackcolor;
			}
		}


		private void _OnInitTop(GameObject go)
		{
            //var realHight =(float)Screen.height;
            //var realWidth = Screen.width;
            //if (realHight / realWidth>GameModel.GetInstance.screenWidhtHightRate)
            //{
            //    Console.Error.WriteLine("太宽了");
            //    var canvasScaler = go.GetComponent<CanvasScaler>();
            //    canvasScaler.matchWidthOrHeight = 0;
            //}

            GameModel.GetInstance.MathWidthOrHeightByCondition(go,0);

			_lbTopTitle = go.GetComponentEx<Text>(Layout.lb_title);
			_btnClose = go.GetComponentEx<Button> (Layout.btn_closeWindow);
			var tmpImg = go.GetComponentEx<Image> (Layout.img_titleImg);
			_titleImg = new UIImageDisplay (tmpImg);

			img_blackbg = go.GetComponentEx<Image> (Layout.img_blackbg);

			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			img_clock = go.GetComponentEx<Image> (Layout.img_clock);

		}

		private void _OnShowTop()
		{
			EventTriggerListener.Get(_btnClose.gameObject).onClick+=_ClickCloseWindow;

			if(null != _controller)
			{
				if(_controller.playerInfor.isEnterInner==true)
				{
					_titleImg.Load (_loadImgTitlePath);
				}
			}
			img_clock.SetActiveEx(false);
			_timeStart ();

		}

		private void _OnHideTop()
		{
			EventTriggerListener.Get(_btnClose.gameObject).onClick+=_ClickCloseWindow;
		}

		private void _OnDisposeTop()
		{
			if (null != _titleImg)
			{
				_titleImg.Dispose ();
			}
		}
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="go"></param>
		private void _ClickCloseWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_boardstate == 0)
			{
				GameModel.GetInstance.borrowBoardTime = -10;
				_controller.setVisible (false);
			}
			else
			{
				_OnShowBorrowItem (null);
			}

            this._HidePaybackGuid();
		}

        /// <summary>
        /// 未引用 ， 设置标题
        /// </summary>
        /// <param name="str"></param>
		private void _SetTopTitle(string str)
		{
			_lbTopTitle.text = str;
		}


		private void _TickTop(float deltaTime)
		{
			_TimeUpdateHandler (deltaTime);
		}

		#region 借贷界面倒计时


		public void SetTime(float lefttime)
		{
			_limitTime = lefttime;
			_isClockStart = true;
			_timeStart ();
			img_clock.SetActiveEx (true);
		}

		private void _timeStart()
		{
			_leftTime = _limitTime;
			lb_time.text = _leftTime.ToString();
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
				GameModel.GetInstance.borrowBoardTime = -10;
				_controller.setVisible(false);
			}
		}



		private string GetTime(float time)
		{
			return HandleNumToTimeTool.ChangeNumberToTime (time);
		}

		private string GetSecond(float time)
		{
			int timer = (int)((time % 3600));
			string timerStr;
			if (timer < 10)
				timerStr = "0" + timer.ToString();
			else
				timerStr = timer.ToString();

			return timerStr;
		}

		//ytf20161018添加卡牌倒计时
		private float _limitTime=61f;
		private float _leftTime=0f;
		private Text lb_time;
		private Image img_clock;
		private bool _isClockStart=false;

		#endregion

		private Text _lbTopTitle;
		private Button _btnClose;
		private UIImageDisplay _titleImg;

		private Image img_blackbg ;
		private Color blackcolor = new Color (0f,0f,0f,1f);

		private string _loadImgTitlePath="share/atlas/battle/newborrow/biaoti2.ab";
		                           
	}
}

