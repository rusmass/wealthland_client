using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIOuterFateCardWindow:UIWindow<UIOuterFateCardWindow,UIOuterFateCardController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_OnInitCenter (go);
			_OnitBottom (go);
			_OnInitSale (go);
		}

		protected override void _OnShow ()
		{
			_OnShowTop ();
			_OnShowCenter ();
			_OnShowBottom ();
			_OnShowSale ();
		}

		protected override void _OnHide ()
		{
			_OnHideBottom ();
			_OnHideSale ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
			_OnDisposeSale ();
			_OnDisposeBottom ();
		}

		public void Tick(float deltaTime)
		{
			_OnBottomTick(deltaTime);
			_TimeUpdateHandler (deltaTime);
			actionTime(deltaTime);
		}

	}
}

