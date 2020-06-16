using System;
using UnityEngine;
namespace Client.UI
{
    /// <summary>
    /// 游戏中金币特效
    /// </summary>
	public class UIGameEffectController:UIController<UIGameEffectWindow,UIGameEffectController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigold.ab";
			}
		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		protected override void _Dispose ()
		{

		}

        /// <summary>
        /// 添加金币的特效
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="_initPosition"></param>
		public void AddMoneyEffect(int playerIndex,Vector3 _initPosition)
		{
			Vector3 changePosition= _initPosition;

//			if(getVisible()==false)
//			{
//				this.setVisible (true);
//			}

			var window = _window as UIGameEffectWindow;
			if (null != window && this.getVisible ()==true)
			{
				window.AddMoneyEffect (playerIndex,changePosition);
			}
		}
	}
}

