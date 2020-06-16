using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGameHallChatWindow:UIWindow<UIGameHallChatWindow,UIGameHallChatController>
	{
		public UIGameHallChatWindow ()
		{
		}

		protected override void _Init (UnityEngine.GameObject go)
		{
			_InitCenter (go);
		}

		protected override void _OnShow ()
		{
			_OnShowCenter ();
		}

		protected override void _OnHide ()
		{
			_OnHideCenter ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}
	}
}

