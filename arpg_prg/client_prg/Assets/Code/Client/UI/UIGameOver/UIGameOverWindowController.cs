using System;

namespace Client.UI
{
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

        public override void Tick(float deltaTime)
        {
        }


        //public override void Tick (float deltaTime)
        //{

        //	if (null != _window && this.getVisible ())
        //	{
        //              var window = _window as UIGameOverWindow;
        //              window.Tick (deltaTime);
        //	}
        //}


        public void ShowOverScene()
		{
			
			if (null != _window && this.getVisible ())
			{
                var window = _window as UIGameOverWindow;
                window.ShowOverScene ();
			}
		}

		public void HideOverScene()
		{
			
			if (null != _window && this.getVisible ())
			{
                var window = _window as UIGameOverWindow;
                window.HideOverScene ();
			}
		}

	}
}

