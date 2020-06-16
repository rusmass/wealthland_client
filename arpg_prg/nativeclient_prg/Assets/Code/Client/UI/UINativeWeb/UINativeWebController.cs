using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 显示内圈的web
    /// </summary>
    class UINativeWebController:UIController<UINativeWebWindow,UINativeWebController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/nativeweb.ab";
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


        public void HideWebView()
        {
            if(null !=_window && this.getVisible())
            {
                (_window as UINativeWebWindow).HideWebView();
            }
        }

        public void ShowWebView()
        {
            if (null != _window && this.getVisible())
            {
                (_window as UINativeWebWindow).ShowWebView();
            }
        }

        /// <summary>
        /// 设置内嵌网站的地址
        /// </summary>
        /// <param name="value"></param>
        public void SetTargetUrl(String value)
        {
            _targetUrl = value;
        }

        /// <summary>
        /// 获取内嵌网站的网址
        /// </summary>
        /// <returns></returns>
        public String GetTargetUrl()
        {
            return _targetUrl;
        }


        private String _targetUrl = "";

    }
}
