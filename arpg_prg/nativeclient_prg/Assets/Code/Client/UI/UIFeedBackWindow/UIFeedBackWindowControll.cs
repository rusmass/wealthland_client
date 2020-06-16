using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏反馈界面
    /// </summary>
	public class UIFeedBackWindowControll:UIController<UIFeedBackWindow,UIFeedBackWindowControll>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uifeedback.ab";
			}
		}

		public UIFeedBackWindowControll ()
		{
		}

//		public void FeedBackSuccess()
//		{
//			if (null != _window && _window.Visible == true)
//			{
//				var tmpWindow = _window as UIFeedBackWindow;
//				tmpWindow.FeedbackSuccess ();
//			}			
//		}
	}
}

