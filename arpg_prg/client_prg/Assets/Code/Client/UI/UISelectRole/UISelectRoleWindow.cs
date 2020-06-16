using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UISelectRoleWindow:UIWindow<UISelectRoleWindow,UISelectRoleWindowController>
	{
		public UISelectRoleWindow ()
		{
			
		}

		protected override void _Init (GameObject go)
		{
			_OnInitCenter (go);
			_OnInitBottom (go);
		}

		protected override void _OnShow ()
		{
			_OnShowCenter ();
			_OnShowBottom ();
		}

		protected override void _OnHide ()
		{
			_OnHideCenter ();
			_OnHideBottom ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}

		public void TickGame(float deltaTime)
		{
			_BottomTick (deltaTime);
		}
	}
}

