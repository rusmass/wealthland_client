using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏反馈界面
    /// </summary>
	public class UIFellingWindowControll:UIController<UIFeelingBackWindow,UIFellingWindowControll>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uifeeling.ab";
			}
		}

		public UIFellingWindowControll ()
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

