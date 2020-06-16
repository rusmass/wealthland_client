using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public class UIShowRiskWindowController : UIController<UIShowRiskWindow,UIShowRiskWindowController> 
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/show/uishowrisk.ab";
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

		public void setRisk(Risk value)
		{
			risk = value;
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIShowBigWindow;
			if (null != window && getVisible ())
			{

			}
		}

		public Risk risk;
	}
}
