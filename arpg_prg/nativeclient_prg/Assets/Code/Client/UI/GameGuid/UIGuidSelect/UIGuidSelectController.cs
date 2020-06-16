using System;

namespace Client.UI
{
    /// <summary>
    /// 单机模式选择角色界面新手引导
    /// </summary>
	public class UIGuidSelectController : UIController<UIGuidSelectWindow, UIGuidSelectController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidselect.ab";
			}
		}

		public UIGuidSelectController()
		{

		}
	}
}

