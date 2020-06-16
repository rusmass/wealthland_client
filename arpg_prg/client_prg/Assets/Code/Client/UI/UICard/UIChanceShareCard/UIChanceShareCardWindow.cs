using System;
using UnityEngine;

namespace Client.UI
{
	public partial class UIChanceShareCardWindow:UIWindow<UIChanceShareCardWindow,UIChanceShareCardController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_InitCenter (go);
			_InitBottom (go);
			_OnInitChange (go);

		}


		protected override void _OnShow()
		{
			_OnShowBottom ();
			_OnShowTop ();
			_OnShowCenter ();
			_OnShowChange ();
		}


		protected override void _OnHide()
		{
			_OnHideBottom ();
			_OnHideChange ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
			_OnDisposeChange ();
		}

		public void Tick(float deltaTime)
		{
//			_OnBottomTick(deltaTime);
			_OnChangeShareTick (deltaTime);
			_TimeUpdateHandler (deltaTime);
			actionTime(deltaTime);
		}


	}
}

