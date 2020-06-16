using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIChooseRoleNetWindow:UIWindow<UIChooseRoleNetWindow,UIChooseRoleNetWindowController>
	{
		public UIChooseRoleNetWindow ()
		{
		}

		protected override void _Init(GameObject go)
		{
			_OnInitButton(go);
			_OnInitText(go);
		}

		protected override void _OnShow()
		{
			_OnShowButton();
		}

		protected override void _OnHide()
		{
			//_OnHideButton ();
		}

		protected override void _Dispose()
		{

		}
	}
}

