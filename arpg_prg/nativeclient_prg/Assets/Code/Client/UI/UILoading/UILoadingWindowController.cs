using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏loading界面
    /// </summary>
	public class UILoadingWindowController : UIController<UILoadingWindow,UILoadingWindowController>
	{
		public UILoadingWindowController ()
		{
			
		}

		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uiloading.ab";
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

		protected override void _Dispose()
		{
			
		}
			
		public override void Tick(float deltaTime)
		{
			index+= 0.8f;
			//Console.WriteLine("tick");
			var window = _window as UILoadingWindow;
			window.setProgressBarValue(index);

			if(index >= 100)
			{
				index = 0;
				var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
				controller.setVisible (false);

				if(m_bool_play == true)
				{
					var controller2 = Client.UIControllerManager.Instance.GetController<UIBattleController>();
					controller2.setBoolTrue();
				}
			}
		}

        /// <summary>
        /// 加载选择角色图片
        /// </summary>
		public void LoadSeletRoleUI()
		{
			Console.WriteLine(" LoadSeletRoleUI ");
			UISynergy.Instance.loadSelectScene();
		}

        /// <summary>
        /// 加载游戏大厅界面
        /// </summary>
		public void LoadGameHallUI()
		{
			UISynergy.Instance.loadGameHallScene ();
		}

        /// <summary>
        /// 加载游戏界面
        /// </summary>
		public void LoadBattleUI()
		{
			Console.WriteLine(" LoadBattleUI ");
			UISynergy.Instance.loadBattleScene();
			m_bool_play = true;
		}

//		public void LoadBattleNetUI()
//		{
//			Console.WriteLine(" LoadBattleNetUI ");
//
//			UISynergy.Instance.loadNetGameScene();
//		}

		private float index = 0;
		//2016-09-22 zll fix paly music
		private bool m_bool_play = false;
	}
}

