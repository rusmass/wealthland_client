using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGuidGameHallWindow : UIWindow<UIGuidGameHallWindow, UIGuidGameHallController>
	{
		public UIGuidGameHallWindow()
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

