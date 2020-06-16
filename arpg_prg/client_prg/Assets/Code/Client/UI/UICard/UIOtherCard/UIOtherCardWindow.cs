using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIOtherCardWindow:UIWindow<UIOtherCardWindow,UIOtherCardWindowController>
	{
		public UIOtherCardWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_OnInitCenter (go);
			_OnitBottom (go);
		}

		protected override void _OnShow ()
		{
			_OnShowTop ();
			_OnShowCenter ();
			_OnShowBottom ();
		}

		protected override void _OnHide ()
		{
			_OnHideBottom ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeTop ();
			_OnDisposeCenter ();
			_OnDisposeBottom ();

		}

		public void Tick(float deltaTime)
		{
			_OnBottomTick(deltaTime);
			actionTime(deltaTime);
			_TickTop (deltaTime);
		}
	}
}

