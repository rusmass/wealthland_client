using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIOpportunityCardWindow:UIWindow<UIOpportunityCardWindow,UIOpportunityCardController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_InitCenter (go);
			_InitBottom (go);

		}


		protected override void _OnShow()
		{
			_OnShowBottom ();
			_OnShowTop ();
			_OnShowCenter ();
		}


		protected override void _OnHide()
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

