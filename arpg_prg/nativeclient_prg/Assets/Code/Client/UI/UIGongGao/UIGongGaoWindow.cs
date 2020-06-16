
using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIGongGaoWindow:UIWindow<UIGongGaoWindow,UIGongGaoController>
	{
		public UIGongGaoWindow ()
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
            this._DisposeCenter();
		}
	}
}

