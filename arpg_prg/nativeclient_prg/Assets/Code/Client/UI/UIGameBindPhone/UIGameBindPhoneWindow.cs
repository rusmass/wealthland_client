using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGameBindPhoneWindow:UIWindow<UIGameBindPhoneWindow,UIGameBindPhoneWindowController>
	{
		public UIGameBindPhoneWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			this._initCenter (go);
		}

		protected override void _OnShow()
		{
			this._onShowCenter ();
		}

		protected override void _OnHide ()
		{
			this._onHideCenter();
		}

		protected override void _Dispose ()
		{
			this._onDisposeCenter ();
		}

	}
}

