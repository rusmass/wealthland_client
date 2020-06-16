using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 游戏胜利后的胜利提示，然后跳到结束界面
    /// </summary>
    public class UIEnterSuccessWindowController:UIController<UIEnterSuccessWindow,UIEnterSuccessWindowController>
    {
        /// <summary>
        /// ui引用的资源
        /// </summary>
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/uientersuccess.ab";
            }
        }

       
        public override void Tick(float deltaTime)
        {
            if (null != _window && getVisible())
            {
                var window = _window as UIEnterSuccessWindow;

                window.Tick(deltaTime);
            }
        }

        public PlayerInfo playerInfor;
    }
}
