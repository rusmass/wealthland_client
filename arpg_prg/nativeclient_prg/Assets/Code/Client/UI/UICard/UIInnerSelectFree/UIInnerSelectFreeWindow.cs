using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIInnerSelectFreeWindow:UIWindow<UIInnerSelectFreeWindow,UIInnerSelectFreeWindowController>
	{
		public UIInnerSelectFreeWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_OnInitTop (go);
			_OnInitCenter (go);
		}


		protected override void _OnShow()
		{
			_OnShowTop ();
			_OnShowCenter ();
		}


		protected override void _OnHide()
		{
			_OnHideTop ();
			_OnHideCenter ();
		}

		protected override void _Dispose ()
		{
		}

		public void OnTick(float deltaTime)
		{
			//			_OnTickTop (deltaTime);
			_TimeUpdateHandler(deltaTime);
		}
	}
}

