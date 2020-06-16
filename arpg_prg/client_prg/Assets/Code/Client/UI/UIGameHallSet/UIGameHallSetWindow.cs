using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGameHallSetWindow:UIWindow<UIGameHallSetWindow,UIGameHallSetController>
	{
		public UIGameHallSetWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_OnInitCenter (go);
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

