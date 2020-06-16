using System;

namespace Client.UI
{
    /// <summary>
    /// 玩家信息的界面
    /// </summary>
	public class UIHeroInforWindowController:UIController<UIHeroInforWindow,UIHeroInforWindowController>
	{
		public UIHeroInforWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/heroinforboard.ab";
			}
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

		protected override void _Dispose ()
		{

		}

		public void MoveIn()
		{
			var window = _window as UIHeroInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}

		public void MoveOut()
		{
			var window = _window as UIHeroInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut();
			}
		}


		public PlayerInfo playerInfor;
	}
}

