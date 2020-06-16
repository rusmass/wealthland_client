
using System;

namespace Client.UI
{
    /// <summary>
    /// 选择机会UI界面
    /// </summary>
	public class UIChanceSelectController:UIController<UIChanceSelectWindow,UIChanceSelectController>
	{
		public UIChanceSelectController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uichanceselect.ab";
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
			var window = _window as UIChanceSelectWindow;

			if (null != window && this.getVisible () == true)
			{
				window.OnTick(deltaTime);
			}
		}

	}
}

