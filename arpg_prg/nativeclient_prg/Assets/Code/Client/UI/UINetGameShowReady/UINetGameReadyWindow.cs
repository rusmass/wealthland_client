using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UINetGameReadyWindow:UIWindow<UINetGameReadyWindow,UINetGameReadyWindowController>
	{
		public UINetGameReadyWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_InitCenter (go);
		}

		protected override void _OnShow ()
		{
			_ShowCenter ();
		}

		protected override void _OnHide ()
		{
			_HideCenter ();
		}

		protected override void _Dispose ()
		{
			_DisposeCenter ();
		}
	}
}

