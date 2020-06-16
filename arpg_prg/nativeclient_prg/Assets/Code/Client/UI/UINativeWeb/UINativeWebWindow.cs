using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI
{
    partial class UINativeWebWindow : UIWindow<UINativeWebWindow,UINativeWebController>
    {
        public UINativeWebWindow()
        {

        }

        protected override void _Init(GameObject go)
        {
            _InitCenter(go);
            _InitTop(go);
        }

        protected override void _OnShow()
        {
            _OnShowCenter();
            _OnShowTop();
        }

        protected override void _OnHide()
        {
            _OnHideCenter();
            _OnHideTop();
        }

        protected override void _Dispose()
        {

        }

    }
}
