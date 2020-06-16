using System;

namespace Client.UI
{
	public class GameReConnectWaitWindowController:UIController<GameReconnectWaitWindow,GameReConnectWaitWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uireconnectwait.ab";;
			}
		}

		public GameReConnectWaitWindowController ()
		{
		}
	}
}

