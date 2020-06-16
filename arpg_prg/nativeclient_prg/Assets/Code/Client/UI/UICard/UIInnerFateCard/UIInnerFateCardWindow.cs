using System;
using UnityEngine;
namespace Client.UI
{
	public partial class UIInnerFateCardWindow:UIWindow<UIInnerFateCardWindow,UIInnerFateCardController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_OnInitCenter (go);
			_InitBottom (go);
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
		}

		public void Tick(float deltaTime)
		{
			_OnBottomTick(deltaTime);
			_TimeUpdateHandler (deltaTime);
			actionTime(deltaTime);
		}
	}
}

