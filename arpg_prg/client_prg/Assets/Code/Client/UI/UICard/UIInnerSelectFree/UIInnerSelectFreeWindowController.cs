using System;

namespace Client.UI
{
	public class UIInnerSelectFreeWindowController:UIController<UIInnerSelectFreeWindow,UIInnerSelectFreeWindowController>
	{
		public UIInnerSelectFreeWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiselectforfree.ab";
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

		public override void Tick (float deltaTime)
		{
			var window = _window as UIInnerSelectFreeWindow;

			if (null != window && this.getVisible () == true)
			{
				window.OnTick(deltaTime);
			}
		}

	}
}

