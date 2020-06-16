using System;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 游戏开始时的引导图界面
    /// </summary>
	public class UIStartGuildWindowController : UIController<UIStartGuildWindow, UIStartGuildWindowController>
	{
        protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uistartguild.ab";
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
       
		public override void Tick (float deltaTime)
		{
			var window = _window as UIRedPacketWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}

		public int menuType=0;
        
	}
}

