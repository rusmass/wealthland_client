using System;

namespace Client.UI
{
    /// <summary>
    /// 借款界面引导页
    /// </summary>
	public class UIGuidBorrowController : UIController<UIGuidBorrowWindow, UIGuidBorrowController>
	{
        /// <summary>
        /// 引用的预设路径
        /// </summary>
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/newplayerguid/uinewguidborrow.ab"; ;
			}
		}

		public UIGuidBorrowController()
		{
            
		}
	}
}

