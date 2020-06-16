using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏网络模式，loading字样UI
    /// </summary>
	public class UIGameNetLoadingWindowController:UIController<UIGameNetLoadingWindow,UIGameNetLoadingWindowController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uinetloading.ab";
			}
		}

		public UIGameNetLoadingWindowController ()
		{
		}
	}
}

