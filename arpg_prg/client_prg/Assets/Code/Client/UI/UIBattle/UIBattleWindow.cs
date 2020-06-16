using System;
using UnityEngine.UI;
using Core;
using UnityEngine;
using Metadata;

namespace Client.UI
{
	public partial class UIBattleWindow : UIWindow<UIBattleWindow, UIBattleController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_InitCenter (go);
			_OnInitCountdown (go);
			_InitBottom (go);
			_OnInitRuntip (go);
		}

		protected override void _OnShow ()
		{
			_OnTopShow ();
			_OnShowCountdown ();
			_OnCenterShow ();
			_OnBottomShow ();
			_OnShowRuntip ();
		}

		protected override void _OnHide ()
		{
			_OnTopHide ();
			_OnCenterHide ();
			_OnBottomHide ();
			_OnHideRuntip ();
		} 

		protected override void _Dispose ()
		{
			_OnBottomDispose ();
		}

        public void Tick(float deltaTime)
        {
            _OnBottomTick(deltaTime);
			_OnTickRunning (deltaTime);
            updateControllerBoardTime(deltaTime);
        }
	}
}

