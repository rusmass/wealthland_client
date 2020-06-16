using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI
{
    partial class UIBorrowFriendWindow:UIWindow<UIBorrowFriendWindow,UIBorrowFriendController>
    {
        public UIBorrowFriendWindow()
        {

        }

        protected override void _Init(GameObject go)
        {
            this._InitCenter(go);
        }

        protected override void _OnShow()
        {
            this._ShowCenter();
        }

        protected override void _OnHide()
        {
            this._HideCenter();
        }

        protected override void _Dispose()
        {
            this._DisposeCenter();
        }


    }
}
