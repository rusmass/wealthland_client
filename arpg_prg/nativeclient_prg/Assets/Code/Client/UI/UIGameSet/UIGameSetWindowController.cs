using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏设置
    /// </summary>
	public class UIGameSetWindowController:UIController<UIGameSetWindow,UIGameSetWindowController>
	{
		public UIGameSetWindowController ()
		{
		}

		protected override string _windowResource
		{
			get {
				return "prefabs/ui/scene/gameset.ab";
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
	}
}

