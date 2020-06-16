using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIFeelingBaordWindow : UIWindow<UIFeelingBaordWindow, UIFeelingBaordController>
	{
		public UIFeelingBaordWindow()
		{
			
		}

		protected override void _Init (GameObject go)
		{
			_OnInitCenter (go);
            _InitBottom(go);
		}

		protected override void _OnShow ()
		{
			_OnShowCenter ();
            _ShowBottom();
		}

		protected override void _OnHide ()
		{
			_OnHideCenter ();
            _HideBottom();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}

		public void TickGame(float deltaTime)
		{
			
		}
	}
}

