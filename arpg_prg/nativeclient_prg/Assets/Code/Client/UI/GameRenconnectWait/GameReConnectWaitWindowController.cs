using System;

namespace Client.UI
{
    /// <summary>
    /// ui显示正在连接中
    /// </summary>
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

