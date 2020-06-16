using System;

namespace Client.UI
{
    /// <summary>
    /// 结账日面板
    /// </summary>
	public class UISettlementInforWindowController:UIController<UISettlementInforWindow,UISettlementInforWindowController>
	{
		public UISettlementInforWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/settlementboard.ab";
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

		public void MoveOut()
		{
			var window = _window as UISettlementInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			Console.WriteLine("结算controller");

			var window = _window as UISettlementInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}

		public PlayerInfo playerInfor;
	}
}

