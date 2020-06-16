using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIFightroomWindow:UIWindow<UIFightroomWindow,UIFightroomController>
	{
		public UIFightroomWindow ()
		{
		}

		protected override void _Init (UnityEngine.GameObject go)
		{
			_initCenter (go);
			_InitChat (go);
		}

		protected override void _OnShow ()
		{
			_showCenter ();
			_ShowChat ();
		}

		protected override void _OnHide ()
		{
			_hideCenter ();
			_HideChat ();
		}

		protected override void _Dispose ()
		{
			_disposeCenter ();
			_DisposeChat ();
		}
	}
}

