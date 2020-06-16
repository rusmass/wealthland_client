using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏大厅设置
    /// </summary>
	public class UIGameHallSetController:UIController<UIGameHallSetWindow,UIGameHallSetController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/gamehallset.ab";
			}
		}
		
		public UIGameHallSetController ()
		{
			
		}


	}
}

