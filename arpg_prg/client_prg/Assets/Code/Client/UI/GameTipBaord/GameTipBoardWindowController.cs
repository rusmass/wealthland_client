using System;

namespace Client.UI
{
	public class GameTipBoardWindowController:UIController<GameTipBoardWindow,GameTipBoardWindowController>
	{
		public GameTipBoardWindowController ()
		{
			
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gametipboard.ab";
			}
		}



		protected override void _Dispose ()
		{

		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		public string gameTip="";

	}
}

