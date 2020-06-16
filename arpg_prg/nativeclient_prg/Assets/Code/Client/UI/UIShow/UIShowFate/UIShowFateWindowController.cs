using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public class UIShowFateWindowController : UIController<UIShowFateWindow,UIShowFateWindowController> 
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/show/uishowfate.ab";
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

		public void setFate(Fate value)
		{
			fate = value;
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIShowBigWindow;
			if (null != window && getVisible ())
			{

			}
		}

		public Fate fate;


	}
}
