using System;

namespace Client.UI
{
	public class UILoadingNetGameWindowController:UIController<UILoadingNetGameWindow,UILoadingNetGameWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uinetgameloading.ab";
			}
		}

		public UILoadingNetGameWindowController ()
		{
		}

		public override void Tick(float deltaTime)
		{
			if (null != _window && getVisible ())
			{
				index+= 0.8f;
				var window = _window as UILoadingNetGameWindow;
				window.setProgressBarValue(index);
				if(index >= 100)
				{
					index = 0;
					//				var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
					setVisible (false);
				}
			}
		}

		public void LoadBattleNetUI()
		{
			Console.WriteLine(" LoadBattleNetUI ");
			UISynergy.Instance.loadNetGameScene();
		}

		private float index = 0;
	}
}

