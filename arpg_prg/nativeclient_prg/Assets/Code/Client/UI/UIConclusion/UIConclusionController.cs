using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Client.UI
{
    /// <summary>
    /// 游戏结算信息
    /// </summary>
	public class UIConclusionController : UIController<UIConclusionWindow,UIConclusionController>
	{
		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uiconclusionnew.ab";
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
        /// 设置是游戏进内圈还是游戏胜利
        /// </summary>
        /// <param name="_mstate"></param>
		public void setTiletText(bool _mstate)
		{
			mstate = _mstate;
		}


		public bool mstate = false;

		public PlayerInfo player;

		public override void Tick (float deltaTime)
		{
			var window = _window as UIConclusionWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}
	}
}
