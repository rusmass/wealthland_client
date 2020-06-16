using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    class UIBorrowFriendController:UIController<UIBorrowFriendWindow,UIBorrowFriendController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/uiborrowfriend.ab";
            }
        }

        protected override void _OnShow()
        {
            base._OnShow();
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }

        protected override void _Dispose()
        {
            base._Dispose();
        }





    }
}
