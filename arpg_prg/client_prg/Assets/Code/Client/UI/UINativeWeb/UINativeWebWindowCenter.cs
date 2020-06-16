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
        private void _InitCenter(GameObject go)
        {
            //_selfObj = go;
            var img = go.GetComponentEx<Image>(Layout.blackbg);
            img.color = new Color(1f, 1f, 1f, 1f);
            _selfObj = img.gameObject;
        }



        private void _OnShowCenter()
        {
            if(Audio.AudioManager.Instance.IsOpenMusic)
            {
                this.gameQuiet = false;
                Audio.AudioManager.Instance.CloseMusic();
            }
            else
            {
                this.gameQuiet = true;
            }

            if(null==_webView)
            {
                _webView = _selfObj.AddComponent<UniWebView>();
                //this._webView.url = "https://www.baidu.com/";  
                _webView.OnWebViewShouldClose += _OnCloseWebHandler;
                _webView.InsetsForScreenOreitation += InsetsForScreenOreitation;
                _webView.SetBackgroundColor(new Color(0.5f,0.5f,0.5f,0));
            }

            _webView.insets.top = 120;
            //_webView.insets.left = 50;
            //_webView.insets.bottom = 50;
            //_webView.insets.right = 50;           
            _webView.url = _controller.GetTargetUrl();
            _webView.Load();    //加载网页  
            ShowOrHide(true);
        }

        /// <summary>
        /// 这是内嵌网页的可见或者不可见
        /// </summary>
        /// <param name="flag"></param>
        public void ShowOrHide(bool flag)
        {
            if (flag)
            {
                _webView.Show();
            }
            else
            {
                _webView.Hide();
            }
        }

        private bool  _OnCloseWebHandler(UniWebView webview)
        {
            _controller.setVisible(false);
            //return true;            
            return true;
        }

        /// <summary>
        /// 隐藏嵌入的网页
        /// </summary>
        public void HideWebView()
        {
            if(null!=_webView)
            {
                _webView.Hide();
            }
        }

        /// <summary>
        /// 显示嵌入的网页
        /// </summary>
        public void ShowWebView()
        {
            if(null!=_webView)
            {
                _webView.Show();
            }
        }

        UniWebViewEdgeInsets InsetsForScreenOreitation(UniWebView webView, UniWebViewOrientation orientation)
        {
#if UNITY_IOS
            var iphoneGen=UnityEngine.iOS.Device.generation;  
            if(iphoneGen==UnityEngine.iOS.DeviceGeneration.iPhone5S)  
            {  
                topInset=54;  
                bottomInset=34;  
            }  
            else if(iphoneGen==UnityEngine.iOS.DeviceGeneration.iPhone6)  
            {  
                topInset=64;  
                bottomInset=40;  
            }  
            else if(iphoneGen==UnityEngine.iOS.DeviceGeneration.iPhone6Plus)  
            {  
                topInset=70;  
                bottomInset=44;  
            } 
            
            if (orientation == UniWebViewOrientation.Portrait)
            {
                return new UniWebViewEdgeInsets(topInset, 0, bottomInset, 0);
            }
            else
            {
                return new UniWebViewEdgeInsets(topInset, 0, bottomInset, 0);
            }
#elif UNITY_ANDROID
            //if (orientation == UniWebViewOrientation.Portrait)
            //{
            //    return new UniWebViewEdgeInsets(0, 0, 0, 120);
            //}
            //else
            //{
            //    return new UniWebViewEdgeInsets(0, 0, 0, 120);
            //}        
#endif
            if (orientation == UniWebViewOrientation.Portrait)
            {
                return new UniWebViewEdgeInsets(0, 0, 0, 0);
            }
            else
            {
                return new UniWebViewEdgeInsets(0, 0, 0, 0);
            }

        }

        private void _OnHideCenter()
        {
            if(this.gameQuiet == false)
            {
                Audio.AudioManager.Instance.OpenMusic();
            }

            if (null!=_webView)
            {
                _webView.OnWebViewShouldClose -= _OnCloseWebHandler;
                _webView.InsetsForScreenOreitation -= InsetsForScreenOreitation;
                GameObject.Destroy(_webView);
                _webView = null;
            }
        }

        private void _OnDisposeCenter()
        {

        }

        private void TestNeiqian()
        {
            if (null == this._webView)
            {
                this._webView = _selfObj.AddComponent<UniWebView>();
                _webView.insets.top = 100;
                _webView.insets.left = 0;
                //_webView.insets.bottom = 50;
                _webView.insets.right = 0;
                //webView.url = strurl;

            }
            // this._webView.InsetsForScreenOreitation += InsetsForScreenOreitation;
           
        }

        private GameObject _selfObj;
        private UniWebView _webView;

        private bool gameQuiet = false;

    }
}
