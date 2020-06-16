using System;

namespace Client.UI
{
    /// <summary>
    /// 游戏结束界面
    /// </summary>
	public class UIGameOverWindowController:UIController<UIGameOverWindow,UIGameOverWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiovergamenew.ab";
			}
		}

		protected override void _OnLoad ()
		{
			
		}

		protected override void _OnHide ()
		{
			
		}

		protected override void _OnShow ()
		{
			
		}

		protected override void _Dispose ()
		{
			
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIGameOverWindow;
			if (null != window && this.getVisible ())
			{
				window.Tick (deltaTime);
			}
		}

        /// <summary>
        /// 显示游戏结束的面板
        /// </summary>
		public void ShowOverScene()
		{
			var window = _window as UIGameOverWindow;
			if (null != window && this.getVisible ())
			{
				window.ShowOverScene ();
			}
		}

        /// <summary>
        /// 隐藏游戏结束的面板，未引用
        /// </summary>
		public void HideOverScene()
		{
			var window = _window as UIGameOverWindow;
			if (null != window && this.getVisible ())
			{
				window.HideOverScene ();
			}
		}

	}
}

