using System;

namespace Client.UI
{
    /// <summary>
    /// 网络模式下，选择觉的新手引导
    /// </summary>
	public class UIGuidNetSelectController : UIController<UIGuidNetSelectWindow, UIGuidNetSelectController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidnetselect.ab";
			}
		}

		public UIGuidNetSelectController()
		{
		}
	}
}

