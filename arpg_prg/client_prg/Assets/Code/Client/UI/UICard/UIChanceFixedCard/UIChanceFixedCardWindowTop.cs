using System;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;


namespace Client.UI
{
	public partial class UIChanceFixedCardWindow
	{                    
		private void _InitTop(GameObject go)
		{
			img_title = go.GetComponentEx<Image> (Layout.img_title);

			img_titleDisplay = new UIImageDisplay (img_title);

			img_bg = go.GetComponentEx<Image> (Layout.img_bg);

			lb_time = go.GetComponentEx<Text> (Layout.lb_timer);

//			_cardActionone = go.DeepFindEx (Layout.cardAction);
//			_cardActiontwo = go.DeepFindEx(Layout.cardAction1);
		}

		private void _OnShowTop()
		{			
			_timeStart ();
			if (null != img_titleDisplay)
			{
				img_titleDisplay.Load (CardTitlePath.Chance_Fixed_Card);
				img_title.SetNativeSize ();
			}

		}

		private void _OndisposeTop()
		{
			if (null != img_titleDisplay)
			{
				img_titleDisplay.Dispose ();
			}
		}

		private void _HideBgImg()
		{
			if (null != img_bg)
			{
				img_bg.SetActiveEx (false);
			}

//			if (null != _cardActionone)
//			{
//				_cardActionone.SetActiveEx(false);
//			}
//
//			if (null != _cardActiontwo)
//			{
//				_cardActiontwo.SetActiveEx (false);
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
				_controller.QuitCard ();
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

		private bool _initClock=false;

		private float _addTime=31;
		private bool _isAddedBorrow=false;

		private Text lb_time;
		private bool _handleSuccess=false;
		private bool _selfQuit=false;


		private Image img_title;
		private UIImageDisplay img_titleDisplay;
		private Image img_bg;

		/// <summary>
		/// 卡牌动画obj
		/// </summary>
//		private Transform _cardActionone;
//		private Transform _cardActiontwo;
	
	}
}




//public class AddCamera : MonoBehaviour {
//
//	public float time_All = 10;//计时的总时间（单位秒）  
//	public float time_Left;//剩余时间  
//	public bool isPauseTime = false;
//	public Text time;
//
//	public static int jishu;
//	// Use this for initialization  
//	void Start()
//	{
//		time_Left = time_All;
//		jishu = 1;
//	}
//
//	// Update is called once per frame  
//	void Update()
//	{
//		if (!isPauseTime)
//		{
//			if (time_Left > 0)
//				StartTimer();
//			isPauseTime = false;
//			time_Left = 10;
//		}
//
//	}
//	/// <summary>  
//	/// 开始计时   
//	/// </summary>  
//	void StartTimer()
//	{
//		time_Left -= Time.deltaTime;
//		time.text = GetTime(time_Left);
//
//	}
//	/// <summary>  
//	///继续游戏，这个暂时加在这里，后期代码重构时加在UIControl中   
//	/// </summary>  
//	public void ContinueGame()
//	{
//
//		isPauseTime = false;
//		Time.timeScale = 1;
//	}
//
//	/// <summary>  
//	/// 暂停计时  
//	/// </summary>  
//	public void PauseTimer()
//	{
//		isPauseTime = true;
//		Time.timeScale = 0;
//	}
//	/// <summary>  
//	/// 获取总的时间字符串  
//	/// </summary>  
//	string GetTime(float time)
//	{
//		return GetMinute(time) + GetSecond(time);
//	}
//
//	/// <summary>  
//	/// 获取小时  
//	/// </summary>  
//	string GetHour(float time)
//	{
//		int timer = (int)(time / 3600);
//		string timerStr;
//		if (timer < 10)
//			timerStr = "0" + timer.ToString() + ":";
//		else
//			timerStr = timer.ToString() + ":";
//		return timerStr;
//	}
//	/// <summary>  
//	///获取分钟   
//	/// </summary>  
//	string GetMinute(float time)
//	{
//		int timer = (int)((time % 3600) / 60);
//		string timerStr;
//		if (timer < 10)
//			timerStr = "0" + timer.ToString() + ":";
//		else
//			timerStr = timer.ToString() + ":";
//		return timerStr;
//	}
//	/// <summary>  
//	/// 获取秒  
//	/// </summary>  
//	string GetSecond(float time)
//	{
//		int timer = (int)((time % 3600) % 60);
//		string timerStr;
//		if (timer < 10)
//			timerStr = "0" + timer.ToString();
//		else
//			timerStr = timer.ToString();
//
//		return timerStr;
//	}
//
//}


