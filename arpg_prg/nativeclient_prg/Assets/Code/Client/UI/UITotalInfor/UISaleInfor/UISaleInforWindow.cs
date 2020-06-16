using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UISaleInforWindow:UIWindow<UISaleInforWindow,UISaleInforWindowController>
	{
		public UISaleInforWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_OnInItCenter (go);
		}

		protected override void _OnShow ()
		{
			_OnShowCenter ();
		}

		protected override void _OnHide ()
		{
			_OnHideCenter ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}
	}
}

