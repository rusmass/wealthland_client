using System;

namespace Client.UI
{
	/// <summary>
	/// User interface player infor controller.创建角色
	/// </summary>
	public class UIPlayerInforController:UIController<UIPlayerInforWindow,UIPlayerInforController>
	{
		protected override string _windowResource {
			get 
			{
				//uiplayerinfor
				return "prefabs/ui/scene/uiplayerinfor.ab";
			}
		}
		
		public UIPlayerInforController ()
		{
			
		}

		/// <summary>
		/// Sets the head path.上传头像路径
		/// </summary>
		/// <param name="value">Value.</param>
		//public void SetHeadPath(string value)
		//{
		//	if (null != _window && getVisible ())
		//	{
		//		(_window as UIPlayerInforWindow).SetHeadPath (value);
		//	}
		//}

		/// <summary>
		/// The type of the window. 窗口的类型，如果是0，新建角色  如果是1人物信息
		/// </summary>
		public int windowType=0;
	}
}

