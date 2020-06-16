using System;

namespace Client.UI
{
    /// <summary>
    /// 双向选择窗口，可以选择确定或者肯定
    /// </summary>
	public class UIGameAutoTipController : UIController<UIGameAutoTipWindow, UIGameAutoTipController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gameautotip.ab";
			}
		}



        public override void Tick(float deltaTime)
        {
            if(getVisible())
            {
                _leftTime -= deltaTime;
                if(_leftTime<=0)
                {
                    this.setVisible(false);
                }
            }
        }

        public UIGameAutoTipController()
		{
		}
		// 初始后试着
		public string GameTip
		{	
            get
            {
                return this._txtStr;
            }

            set
            {                
                this._txtStr=value;
            }            
		}

        /// <summary>
        /// 剩余的时间
        /// </summary>
        public float LeftTime
        {
            get
            {
                return _leftTime;
            }

            set
            {
                _leftTime = value;
            }
        }
        		
		/// <summary>
		/// The text string. 文本框显示内容
		/// </summary>
		private string _txtStr="";

        private float _leftTime = 3;
	}
}

