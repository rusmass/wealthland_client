using System;

namespace Client.UI
{
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

