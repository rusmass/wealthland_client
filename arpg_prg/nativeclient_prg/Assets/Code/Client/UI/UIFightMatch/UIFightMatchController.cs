using System;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 好友对战的房间界面
    /// </summary>
	public class UIFightCatchController:UIController<UIFightCatchWindow, UIFightCatchController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uifightmatch.ab";
			}
		}

		public UIFightCatchController()
		{
		}

        public override void Tick(float deltaTime)
        {
            if(null!=this._window &&this.getVisible())
            {
                (this._window as UIFightCatchWindow).OnTick(deltaTime);
            }
        }

        /// <summary>
        /// Sets the player infors. 刷新房间人物信息
        /// </summary>
        /// <param name="tmpList">Tmp list.</param>
        public void SetPlayerInfors(List<PlayerHeadInfor> tmpList)
		{
			if (null != _window && _window.Visible == true)
			{
				(_window as UIFightCatchWindow).SetPlayerHaed (tmpList);
			}
		}

		public List<PlayerHeadInfor> headInforList;

		/// <summary>
		/// Sets the sure button disabled. 设置确认按钮不可点击
		/// </summary>
		public void SetSureBtnDisabled()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIFightCatchWindow).SetSureBtnDisabled ();
			}

		}       
	}
}

