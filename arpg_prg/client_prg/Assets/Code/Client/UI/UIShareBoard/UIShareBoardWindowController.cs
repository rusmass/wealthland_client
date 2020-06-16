using System;

namespace Client.UI
{
	public class UIShareBoardWindowController:UIController<UIShareBoardWindow,UIShareBoardWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gameshareboard.ab";
			}
		}

		public UIShareBoardWindowController ()
		{
		}
	}
}

