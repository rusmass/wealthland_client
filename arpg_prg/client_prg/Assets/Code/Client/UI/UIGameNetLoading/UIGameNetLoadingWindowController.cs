using System;

namespace Client.UI
{
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

