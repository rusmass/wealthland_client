using System;

namespace Client.UI
{
    /// <summary>
    /// 未引用
    /// </summary>
	public class UIRegistController:UIController<UIRegistWindow,UIRegistController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiregistgo.ab";
			}
		}

		public UIRegistController ()
		{
		}

		protected override void _Dispose ()
		{

		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		private System.Timers.Timer _timerCount;//=new System.Timers.Timer();

		/// <summary>
		/// Starts the count time. 开启倒计时
		/// </summary>
		public void StartCountTime()
		{
			if (null == _timerCount) 
			{
				_timerCount = new System.Timers.Timer (1000);
				_timerCount.Elapsed += new System.Timers.ElapsedEventHandler(_HandlerTimerAcount); //到达时间的时候执行事件；   
				_timerCount.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
				_timerCount.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
			}

			leftTime = _initTime;
			_ShowTimeUI ();
			_UpdateTimeUI (leftTime.ToString ());
			_timerCount.Start ();

		}

		/// <summary>
		/// Stops the count time.停止倒计时
		/// </summary>
		public void StopCountTime()
		{
			if (null != _timerCount)
			{
				_timerCount.Stop ();
				_timerCount.Dispose ();
			}
		}

		/// <summary>
		/// 
		/// </summary> 倒计时函数
		/// <param name="obj">Object.</param>
		/// <param name="e">E.</param>
		public void _HandlerTimerAcount(object obj, System.Timers.ElapsedEventArgs e)
		{
			leftTime--;
//			_UpdateTimeUI (leftTime.ToString ());
			if (leftTime <= 0)
			{
				StopCountTime ();
			}
		}

		/// <summary>
		/// Hides the time U.隐藏倒计时的ui
		/// </summary>
		private void _HideTimeUI()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIRegistWindow).HideTimeCount ();
			}
		}

		private void _ShowTimeUI()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIRegistWindow).ShowTimeCount ();
			}
		}

		/// <summary>
		/// Updates the time U. 更新倒计时数据
		/// </summary>
		/// <param name="value">Value.</param>
		private void _UpdateTimeUI(string value)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIRegistWindow).UpdateTimeNum(value);
			}
		}

		public override void Tick (float deltaTime)
		{
			if (currentTime != leftTime)
			{
				currentTime = leftTime;

				if (currentTime <= 0)
				{
					_HideTimeUI ();
				}

				_UpdateTimeUI (currentTime.ToString());
			}
		}


		public bool isShowTime()
		{
			var shouTime = false;

			if (null != _timerCount)
			{
				if (_timerCount.Enabled == true)
				{
					shouTime = true;
				}
			}

			return shouTime;
		}

		/// <summary>
		/// The init time.初始的时间
		/// </summary>
		private int _initTime=60;

		/// <summary>
		/// The left time. 倒计时剩余的时间
		/// </summary>
		private int leftTime=60;

		/// <summary>
		/// The current time.当前的时间倒计时
		/// </summary>
		private int currentTime=0;

	}
}

