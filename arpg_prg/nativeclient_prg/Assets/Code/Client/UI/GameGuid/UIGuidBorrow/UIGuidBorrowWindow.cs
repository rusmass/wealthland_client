using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGuidBorrowWindow : UIWindow<UIGuidBorrowWindow, UIGuidBorrowController>
	{
		public UIGuidBorrowWindow()
		{
		}

		protected override void _Init (GameObject go)
		{
            _InitCenter(go);
		}

		protected override void _OnShow ()
		{
            _ShowCenter();
		}

		protected override void _OnHide ()
		{
            _HideCenter();
		}

		protected override void _Dispose ()
		{
			
		}
	}
}

