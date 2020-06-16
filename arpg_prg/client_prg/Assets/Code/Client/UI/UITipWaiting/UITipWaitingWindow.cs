﻿using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UITipWaitingWindow:UIWindow<UITipWaitingWindow,UITipWaitingWindowController>
	{
		public UITipWaitingWindow ()
		{
			
		}

		protected override void _Init (GameObject go)
		{
			this._InitCenter (go);
		}

		protected override void _OnShow ()
		{
			this._ShowCenter ();
		}

		protected override void _OnHide ()
		{
			this._HideCenter ();
		}

		protected override void _Dispose ()
		{
			
		}
	}
}
