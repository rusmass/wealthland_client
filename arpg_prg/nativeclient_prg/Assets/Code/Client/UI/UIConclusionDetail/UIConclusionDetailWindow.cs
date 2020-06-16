using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIConclusionDetailWindow : UIWindow<UIConclusionDetailWindow, UIConclusionDetailController> 
	{
		public UIConclusionDetailWindow()
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
			_OnShowWindowTxt ();
		}

		protected override void _OnHide()
		{
			_OnHideButton ();
		}

		protected override void _Dispose()
		{
			
		}

		public void TickGame(float deltaTime)
		{
            this._UpdateDateMove(deltaTime);
		}
	
	}
}
