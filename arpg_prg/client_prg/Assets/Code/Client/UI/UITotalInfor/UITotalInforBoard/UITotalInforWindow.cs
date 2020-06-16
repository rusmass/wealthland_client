using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UITotalInforWindow:UIWindow<UITotalInforWindow,UITotalInforWindowController>
	{
		public UITotalInforWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_OnInitTop (go);
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
			_OnHideCenter ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
			_OnDisposeTop ();
		}

		public void Tick(float deltatime)
		{
			_TickTop (deltatime);
		}
	}
}

