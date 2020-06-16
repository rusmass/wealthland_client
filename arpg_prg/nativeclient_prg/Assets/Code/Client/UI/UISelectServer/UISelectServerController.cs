using System;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 选择服务器界面
    /// </summary>
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

