using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 向好友借款的面板
    /// </summary>
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


        public List<string> PlayerIDArrs
        {
            get
            {
                return _playerIdArr;
            }

            set
            {
                _playerIdArr = value;
            }

        }

         List<string> _playerIdArr;
    }
}
