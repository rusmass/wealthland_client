using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Client.UI
{
    /// <summary>
    /// 游戏结算信息详情
    /// </summary>
	public class UIConclusionDetailController : UIController<UIConclusionDetailWindow, UIConclusionDetailController>
	{
        public UIConclusionDetailController()
        {

        }

        protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uiconclusiondetail.ab";
			}
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

		protected override void _Dispose ()
		{

		}

        /// <summary>
        /// 设置要展示的玩家信息
        /// </summary>
        /// <param name="playerInfor"></param>
		public void setUpBaseText(PlayerInfo playerInfor)
		{
			player = playerInfor;
		}     		

        /// <summary>
        /// 玩家信息
        /// </summary>
		public PlayerInfo player;

        /// <summary>
        /// 综合评分话术
        /// </summary>
        public string totalScoreTip="";

		public override void Tick (float deltaTime)
		{
			var window = _window as UIConclusionDetailWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}
	}
}
