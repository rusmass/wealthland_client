using System;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
	public class UIConsultingWindowController:UIController<UIConsultingWindow,UIConsultingWindowController>
	{

        protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiconsulting.ab";
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

		public bool isShowBlackBg=false;
       
		public override void Tick (float deltaTime)
		{
			var window = _window as UIConsultingWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}
        
	}
}

