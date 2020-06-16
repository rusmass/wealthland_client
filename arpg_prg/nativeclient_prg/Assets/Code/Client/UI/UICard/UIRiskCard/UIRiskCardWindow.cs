using System;
using UnityEngine;


namespace Client.UI
{
    public partial class UIRiskCardWindow : UIWindow<UIRiskCardWindow, UIRiskCardController>
    {
        protected override void _Init(GameObject go)
        {
            _InitTop(go);
            _InitBottom(go);
            _InitCenter(go);
        }

        protected override void _OnShow()
        {
            _OnShowTop();
            _OnShowBottom();
            _OnShowCenter();
        }

        public void Tick(float deltaTime)
        {
           _OnBottomTick(deltaTime);
			_TimeUpdateHandler (deltaTime);
			actionTime(deltaTime);
        }

		protected override void _OnHide ()
		{
			_OnHideBottom ();
			_OnHideCenter ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		
		}
	}
}

