using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 游戏中倒计时UI
    /// </summary>
    class UIGameTimerWindowController:UIController<UIGameTimerWindow,UIGameTimerWindowController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/uitimercount.ab";
            }
        }



        
        public override void Tick(float deltaTime)
        {
            SetTime(GameTimerManager.Instance.LeftTime);            
        }

        /// <summary>
        /// 设置剩余的时间
        /// </summary>
        /// <param name="value"></param>
        private void SetTime(float value)
        {
            if(null!=_window && this.getVisible())
            {
                (_window as UIGameTimerWindow).SetTime(value);
            }
        }


    }    
}
