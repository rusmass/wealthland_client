using System;

namespace Client.UI
{
	/// <summary>
	/// User interface player infor controller.创建角色
	/// </summary>
	public class UIPlayerInforController:UIController<UIPlayerInforWindow,UIPlayerInforController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiplayerinfor.ab";
			}
		}
		
		public UIPlayerInforController ()
		{
			
		}

		/// <summary>
		/// The type of the window. 窗口的类型，如果是0，新建角色  如果是1人物信息
		/// </summary>
		public int windowType=0;
	}
}

