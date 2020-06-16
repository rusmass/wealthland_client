using System;
using UnityEngine;
namespace Client.UI
{
	public partial class UIEnterInnerWindow:UIWindow<UIEnterInnerWindow,UIEnterInnerWindowController>
	{
		public UIEnterInnerWindow ()
		{
			
		}

		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_OnInitCenter (go);

		}

		protected override void _OnShow ()
		{
			_OnShowTop ();
			_OnShowCenter ();

		}

		protected override void _OnHide ()
		{
			
			_OnHideTop ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();

		}

		public void Tick(float deltaTime)
		{
			
		}
	}
}

