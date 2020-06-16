using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace Client.UI
{
    public class UIMessageTipsController : UIController<UIMessageTipsWindow, UIMessageTipsController>
    {
        protected override string _windowResource { get { return "prefabs/ui/scene/uimessagetips.ab"; } }

        public UIMessageTipsController()
        {

        }

        protected override void _OnLoad()
        {
            
        }

        protected override void _OnShow()
        {
            
        }

        protected override void _OnHide()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            var window = _window as UIMessageTipsWindow;
            if (null != window)
            {
                window.Tick(deltaTime);
            }

            if (null != _cacheList && _cacheList.Count > 0)
            {
                var item = _cacheList[0];
                var result = window.ShowFlyText(item.Text, item.CallBack);
                if (result)
                {
                    _cacheList.RemoveAt(0);
                }
            }


        }

        public void ShowFlyText(string text, TweenCallback callBack = null)
        {
			if (_cacheList.FindAll ( item => item.Text == text).Count == 0) 
			{
				_cacheList.Add(new ShowTextParam() {
					Text = text,
					CallBack = callBack
				});
			}
        }


		public void Dispose()
		{
			var window = _window as UIMessageTipsWindow;

			if (null != window && this.getVisible ())
			{
				window.Dispose ();
				this.setVisible (false);
			}
		}



        private List<ShowTextParam> _cacheList = new List<ShowTextParam>();

        private struct ShowTextParam
        {
            public string Text;
            public TweenCallback CallBack;
        }
    }
}

