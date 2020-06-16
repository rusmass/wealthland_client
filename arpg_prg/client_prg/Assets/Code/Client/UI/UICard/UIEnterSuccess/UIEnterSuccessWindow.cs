using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI
{
    public partial class UIEnterSuccessWindow:UIWindow<UIEnterSuccessWindow,UIEnterSuccessWindowController>
    {
        public UIEnterSuccessWindow()
        {
        }

        protected override void _Init(GameObject go)
        {
            _InitTop(go);
            _OnInitCenter(go);

        }

        protected override void _OnShow()
        {
            _OnShowTop();
            _OnShowCenter();

        }

        protected override void _OnHide()
        {

            _OnHideTop();
        }

        protected override void _Dispose()
        {
            _OnDisposeCenter();

        }

        public void Tick(float deltaTime)
        {
            _OnTopTick(deltaTime);
        }
    }    
}
