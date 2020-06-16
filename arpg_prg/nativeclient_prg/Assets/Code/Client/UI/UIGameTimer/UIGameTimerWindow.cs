using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI
{
    partial class UIGameTimerWindow:UIWindow<UIGameTimerWindow,UIGameTimerWindowController>
    {
        public UIGameTimerWindow()
        {

        }

        protected override void _Init(GameObject go)
        {
            _InitCenter(go);
        }

        protected override void _OnShow()
        {
            _ShowCenter();
        }

        protected override void _OnHide()
        {
            _HideCenter();
        }

        protected override void _Dispose()
        {
            _DisposeCenter();
        }
    }
}
