using System;

namespace Client.UI
{
    /// <summary>
    /// 借贷还款的新手引导界面
    /// </summary>
	public class UIGuidPaybackController : UIController<UIGuidPaybackWindow, UIGuidPaybackController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidpayback.ab"; 
			}
		}

		public UIGuidPaybackController()
		{

		}
	}
}

