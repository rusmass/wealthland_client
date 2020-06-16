using System;
using Metadata;

namespace Client.UI
{
    /// <summary>
    /// 股票资产信息展示
    /// </summary>
	public class UIBalanceShareInforWindowController:UIController<UIBalanceShareInforWindow,UIBalanceShareInforWindowController>
	{
		public UIBalanceShareInforWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uibalanceshareinfor.ab";
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

		public ChanceShares cardData
		{
			get
			{
				return _carddata;
			}
			set
			{
				_carddata = value;
			}
		}

		private ChanceShares _carddata;

	}
}

