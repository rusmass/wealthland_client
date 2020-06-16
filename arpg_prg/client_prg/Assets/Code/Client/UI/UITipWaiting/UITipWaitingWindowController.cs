using System;

namespace Client.UI
{
	public class UITipWaitingWindowController:UIController<UITipWaitingWindow,UITipWaitingWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gametipconnect.ab";
			}
		}


		public UITipWaitingWindowController ()
		{
		}

		public void SetCallBack(Action _callBack)
		{
			if (null != _window)
			{
				(_window as UITipWaitingWindow).SetCallBack (_callBack);
			}
		}

		public override void Tick (float deltaTime)
		{
			if (null != _window)
			{
				(_window as UITipWaitingWindow).DoTick(deltaTime);
			}
		}
	}
}

