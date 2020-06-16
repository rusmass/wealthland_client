using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    partial class UINativeWebWindow
    {
        private void _InitTop(GameObject go)
        {
            _btnClose = go.GetComponentEx<Button>(Layout.closeBtn);
        }

        private void _OnShowTop()
        {
            EventTriggerListener.Get(_btnClose.gameObject).onClick += _CloseHandler;
        }

        private void _CloseHandler(GameObject go)
        {
            _controller.setVisible(false);
        }


        private void _OnHideTop()
        {
            EventTriggerListener.Get(_btnClose.gameObject).onClick -= _CloseHandler;
        }

        private Button _btnClose;

    }    
}
