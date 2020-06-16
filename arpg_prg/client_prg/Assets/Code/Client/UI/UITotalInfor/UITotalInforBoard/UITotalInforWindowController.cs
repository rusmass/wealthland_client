using System;

namespace Client.UI
{
	public class UITotalInforWindowController:UIController<UITotalInforWindow,UITotalInforWindowController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/totalinforboard.ab";
			}
		}

		public UITotalInforWindowController ()
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

		protected override void _Dispose ()
		{

		}

		/// <summary>
		/// Nets the show target board. 网络对战显示目标界面
		/// </summary>
		public void NetShowTargetBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.NetShowTargetInforBaord(); 
			}
		}

		/// <summary>
		/// Nets the show balance and income baord. 网络对战显示资产和收入界面
		/// </summary>
		public void NetShowBalanceAndIncomeBaord()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.NetShowBalanceAndIncome(); 
			}
		}

		/// <summary>
		/// Nets the show debt and pay board. 网络对战显示负债和支出
		/// </summary>
		public void NetShowDebtAndPayBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.NetShowDebtAndPayback(); 
			}
		}

		/// <summary>
		/// Nets the show sale infor board.网络对战显示出售信息界面
		/// </summary>
		public void NetShowSaleInforBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.NetShowSaleBoard(); 
			}
		}

		/// <summary>
		/// Nets the show check infor board. 网络对战显示结算信息
		/// </summary>
		public void NetShowCheckInforBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.NetShowCheckBoard(); 
			}
		}

		public void ShowBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.ShowBoard (); 
			}
		}

		public void ShowBoardForSaleBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.ShowBoardForSaleBoard(); 
			}
		}

		public void HideBoard()
		{
			var window = _window as UITotalInforWindow;
			if (null != _window && getVisible())
			{
				window.HideBoard(); 
			}
		}
			

		public void CloseHandler()
		{
			if (null != _window && getVisible ())
			{
				(_window as UITotalInforWindow).CloseHandler ();
			}
		}


		public override void Tick (float deltaTime)
		{
			var window = _window as UITotalInforWindow;
			if (null != window && getVisible())
			{
				window.Tick(deltaTime);
			}
		}
			
		public PlayerInfo playerInfor;
	}
}

