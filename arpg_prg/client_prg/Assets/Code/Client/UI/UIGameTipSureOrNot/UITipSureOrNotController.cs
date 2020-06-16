using System;

namespace Client.UI
{
	public class UITipSureOrNotController:UIController<UITipSureOrNotWindow,UITipSureOrNotController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gametipsureornot.ab";
			}
		}
		public UITipSureOrNotController ()
		{
		}
		// 初始后试着
		public void SetTip(string value , Action _callsure,Action _callNo=null)
		{
//			if (null != _window)
//			{
//				(_window as UITipSureOrNotWindow).ShowTip (value,callsure,callNo);
//			}		
			callSure=null;
			callNo = null;
			txtStr = "";

			txtStr = value;
			callSure = _callsure;
			callNo = _callNo;
		}

		/// <summary>
		/// The call sure. 确定的回调函数
		/// </summary>
		public Action callSure=null;

		/// <summary>
		/// The call no. 取消的回调函数
		/// </summary>
		public Action callNo=null;

		/// <summary>
		/// The text string. 文本框显示内容
		/// </summary>
		public string txtStr="";
	}
}

