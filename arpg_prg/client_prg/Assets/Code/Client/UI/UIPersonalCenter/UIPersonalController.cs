using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


namespace Client.UI
{
    public class UIPersonalController : UIController<UIPersonalWindow, UIPersonalController>
    {

        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/personalcenter.ab";
            }
        }

        protected override void _OnLoad()
        {
            base._OnLoad();
        }

        protected override void _OnShow()
        {
            base._OnShow();
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }

        protected override void _Dispose()
        {

            base._Dispose();
        }

        public void HeadImage(string str)
        {
            UIPersonalWindow win = _window as UIPersonalWindow;     
            AsyncImageDownload.Instance.HeadImage(win.ReturnIm());
        }
      

    }
}

