using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UISelectServerWindow:UIWindow<UISelectServerWindow,UISelectServerController>
	{
		public UISelectServerWindow()
		{
						
		}

		protected override void _Init (GameObject go)
		{
			this._initCenter (go);
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

