using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏提示窗口，进内圈和胜利时会提示有贷款未还
    /// </summary>
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

