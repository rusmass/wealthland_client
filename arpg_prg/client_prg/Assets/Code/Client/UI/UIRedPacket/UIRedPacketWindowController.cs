using System;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
	public class UIRedPacketWindowController:UIController<UIRedPacketWindow,UIRedPacketWindowController>
	{
        public PlayerInfo player;

        protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigetredpacket.ab";
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
        public void OpenGetRedPacket()
        {
            SetMenuType(0);
        }
        public void OpenPackRedPacket()
        {
            SetMenuType(1);
        }
        private void SetMenuType(int type)
        {
//            var window = _window as UIRedPacketWindow;
//            window.SetMenuType(type);
			menuType=type;
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

