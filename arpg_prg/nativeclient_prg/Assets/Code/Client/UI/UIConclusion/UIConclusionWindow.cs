using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIConclusionWindow : UIWindow<UIConclusionWindow,UIConclusionController> 
	{
		public UIConclusionWindow()
		{
			
		}

		protected override void _Init(GameObject go)
		{
			_OnInitText(go);
			_OnInitButton(go);
		}

		protected override void _OnShow()
		{
			_OnShowButton();
			_OnShowWindowTxt ();
		}

		protected override void _OnHide()
		{
			_OnHideButton ();
		}

		protected override void _Dispose()
		{
			_DisposeText ();
		}

		public void TickGame(float deltaTime)
		{

		}
	
	}
}
