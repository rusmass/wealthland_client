using System;
using System.Collections.Generic;
namespace Client.UI
{
    /// <summary>
    /// 卖出界面的信息
    /// </summary>
	public class UISaleInforWindowController:UIController<UISaleInforWindow,UISaleInforWindowController>
	{
		public UISaleInforWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/sellboard.ab";
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

		public List<SaleRecordVo> GetSaleRecordList()
		{
			if (null != playerInfor)
			{
				return playerInfor.saleRecordList;
			}
			return null;
		}

		public SaleRecordVo GetSaleRecordByIndex(int index)
		{
			var items = playerInfor.saleRecordList;

			if (null != items && index < items.Count)
			{
				return items [index];
			}

			return  null;
		}


		public void MoveOut()
		{
			var window = _window as UISaleInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			var window = _window as UISaleInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}


		public PlayerInfo playerInfor;
	}
}

