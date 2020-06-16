using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGameHallWindow:UIWindow<UIGameHallWindow,UIGameHallWindowController>
	{
		public UIGameHallWindow ()
		{

		}
		protected override void _Init (GameObject go)
		{
			this._InitTop (go);
			this._InitCenter (go);
			this._InitBottom (go);
			this._InitEnterRoom (go);
		}

		protected override void _OnShow ()
		{
			_ShowCenter ();
			_ShowTop ();
			_ShowBottom ();
			_ShowEnterRoom ();
		}

		protected override void _OnHide ()
		{
			_HideTop ();
			_HideCenter ();
			_HideBottom ();
			_HideEnterRoom ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeTop ();
		}
	}
}

