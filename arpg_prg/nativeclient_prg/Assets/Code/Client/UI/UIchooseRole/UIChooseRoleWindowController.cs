using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 单机模式选择角色界面
    /// </summary>
	public class UIChooseRoleWindowController : UIController<UIChooseRoleWindow,UIChooseRoleWindowController> 
	{
		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/chooserole.ab";
			}
		}

		protected override void _OnLoad()
		{
			
		}

		protected override void _OnShow()
		{
			
		}

		protected override void _OnHide()
		{
			
		}

		protected override void _Dispose ()
		{

		}

        /// <summary>
        /// 根据索引值选择角色
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public PlayerInitData SelectRole(int index)
		{
			var playerInitList = new List<PlayerInitData> ();

			var template = MetadataManager.Instance.GetTemplateTable<PlayerInitData> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as PlayerInitData;
				playerInitList.Add(value);			
			}

			var tmpvalue=index;
			return playerInitList[tmpvalue];
		}
			
		public bool IsSelectedIngame
		{
			get
			{
				return _isIngame;			
			}

			set 
			{
				_isIngame = value;
			}
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIChooseRoleWindow;
			if (null != window && getVisible ())
			{

			}
		}

		public void setSelectedImage()
		{
			var window = _window as UIChooseRoleWindow;

//			if (null != window && )
//			{
//				window.setSelectedImageButton();
//			}

			if (null != window && getVisible () == true)
			{
				window.setSelectedImageButton();
			}
		
		}

		private bool _isIngame;
	}
}
