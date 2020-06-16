using System;

namespace Client.UI
{
    /// <summary>
    ///  联网模式，在房间中新手引导页面
    /// </summary>
	public class UIGuidRoomController : UIController<UIGuidRoomWindow, UIGuidRoomController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidroom.ab";
			}
		}

		public UIGuidRoomController()
		{
		}
	}
}

