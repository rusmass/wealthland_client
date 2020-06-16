using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIBalanceShareInforWindow:UIWindow<UIBalanceShareInforWindow,UIBalanceShareInforWindowController>
	{
		public UIBalanceShareInforWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_InitCenter (go);
			_InitBottom (go);

		}


		protected override void _OnShow()
		{
			_OnShowBottom ();
			_OnShowTop ();
			_OnShowCenter ();
		}


		protected override void _OnHide()
		{
			_OnHideBottom ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}

	}
}

