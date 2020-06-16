using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏页面中新手引导
    /// </summary>
	public class UIGuidGameController : UIController<UIGuidGameWindow, UIGuidGameController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidgame.ab";

            }
		}

		public UIGuidGameController()
		{
		}
	}
}

