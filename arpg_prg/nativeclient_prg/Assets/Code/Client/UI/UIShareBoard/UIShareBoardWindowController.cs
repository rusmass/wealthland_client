using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏大厅分享窗口
    /// </summary>
	public class UIShareBoardWindowController:UIController<UIShareBoardWindow,UIShareBoardWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gameshareboard.ab";
			}
		}

        private int _shareType = 0;

        /// <summary>
        /// 分享的应用窗口，0正常的游戏分享，1内嵌网页的梦想版分享
        /// </summary>
        public int ShareWindow
        {
            get
            {
                return _shareType;
            }

            set
            {
                _shareType = value;
            }
        }
        /// <summary>
        /// 分享后的回调函数
        /// </summary>
        public Action CallBack = null;

        /// <summary>
        /// 未引用
        /// </summary>
		public UIShareBoardWindowController ()
		{
		}
	}
}

