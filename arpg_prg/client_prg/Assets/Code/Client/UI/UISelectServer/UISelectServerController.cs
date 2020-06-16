using System;
using System.Collections.Generic;

namespace Client.UI
{
	public class UISelectServerController:UIController<UISelectServerWindow,UISelectServerController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiselectserver.ab";
			}
		}

		public UISelectServerController ()
		{
		}

		protected override void _OnShow ()
		{
			
		}

		protected override void _OnHide ()
		{
			
		}


		public List<string> serverList;

		public string curServer;
	}
}

