﻿using System;
using UnityEngine;
namespace Client.UI
{
	public partial class UITipSureOrNotWindow:UIWindow<UITipSureOrNotWindow,UITipSureOrNotController>
	{
		public UITipSureOrNotWindow ()
		{
		}

		protected override void _Init (GameObject go)
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

		}
	}
}
