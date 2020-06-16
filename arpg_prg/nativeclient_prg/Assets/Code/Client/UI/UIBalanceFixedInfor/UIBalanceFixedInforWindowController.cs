using System;
using Metadata;

namespace Client.UI
{
    /// <summary>
    /// 固定资产展示信息
    /// </summary>
	public class UIBalanceFixedInforWindowController:UIController<UIBalanceFixedInforWindow,UIBalanceFixedInforWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uibalanceinfor.ab";
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

		public ChanceFixed cardData;
	}
}

