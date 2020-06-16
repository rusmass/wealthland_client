using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIFightCatchWindow:UIWindow<UIFightCatchWindow, UIFightCatchController>
	{
		public UIFightCatchWindow()
		{
		}

		protected override void _Init (UnityEngine.GameObject go)
		{
			_initCenter (go);

		}

		protected override void _OnShow ()
		{
			_showCenter ();
			
		}

		protected override void _OnHide ()
		{
			_hideCenter ();
			
		}

		protected override void _Dispose ()
		{
			_disposeCenter ();
			
		}

        /// <summary>
        /// 时间倒计时
        /// </summary>
        /// <param name="deltime"></param>
        public void OnTick(float deltime)
        {
            this._OnTickCenter(deltime);
        }
	}
}

