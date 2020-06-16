using System;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 选择角色界面
    /// </summary>
	public class UISelectRoleWindowController:UIController<UISelectRoleWindow,UISelectRoleWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/selectrole.ab";
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

		public void StartBattleGame()
		{
			setVisible (false);
		}

        /// <summary>
        /// 根据索引值，选择角色信息
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
			//var tmpRandom = new Random ();
			//var tmpvalue = tmpRandom.Next(0,playerInitList.Count);
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
			var window = _window as UISelectRoleWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}

		public void setSelectedImage()
		{
			var window = _window as UISelectRoleWindow;
			window.setSelectedImageButton();
		}

		private bool _isIngame;
	}
}

