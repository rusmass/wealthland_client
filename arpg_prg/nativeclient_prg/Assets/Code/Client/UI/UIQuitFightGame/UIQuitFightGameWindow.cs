using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIQuitFightGameWindow:UIWindow<UIQuitFightGameWindow,UIQuitFightGameWindowController>
	{
		public UIQuitFightGameWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_InitCenter (go);

		}

		protected override void _OnShow ()
		{
			_ShowCenter ();
		}

		protected override void _OnHide ()
		{
			_HideCenter ();
		}

		protected override void _Dispose ()
		{
			
		}
	}
}

