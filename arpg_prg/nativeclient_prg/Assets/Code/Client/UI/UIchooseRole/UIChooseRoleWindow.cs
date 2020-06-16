using UnityEngine;
using System.Collections;


namespace Client.UI
{
	public partial class UIChooseRoleWindow : UIWindow<UIChooseRoleWindow,UIChooseRoleWindowController> 
	{
		public UIChooseRoleWindow()
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
			_OnHideButton ();
		}

		protected override void _Dispose()
		{
			
		}
	}
}
