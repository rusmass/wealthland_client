using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏主页面新手引导
    /// </summary>
	public class UIGuidGameHallController : UIController<UIGuidGameHallWindow, UIGuidGameHallController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidgamehall.ab";
			}
		}

		public UIGuidGameHallController()
		{
		}
	}
}

