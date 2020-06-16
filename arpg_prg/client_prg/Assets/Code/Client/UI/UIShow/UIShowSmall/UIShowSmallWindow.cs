using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIShowSmallWindow : UIWindow<UIShowSmallWindow,UIShowSmallWindowController> 
	{
		public UIShowSmallWindow()
		{

		}

		protected override void _Init(GameObject go)
		{
			_OnInitContent(go);
		}

		protected override void _OnShow()
		{
			_OnShowContent();
		}

		protected override void _OnHide()
		{

		}

		protected override void _Dispose()
		{

		}

		public void TickGame(float deltaTime)
		{

		}

	}
}

