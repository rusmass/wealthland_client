using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public class UIShowQualityWindowController : UIController<UIShowQualityWindow,UIShowQualityWindowController> 
	{
		public UIShowQualityWindowController()
		{

		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/show/uishowquality.ab";
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

		public void setQualityLife(QualityLife value)
		{
			qualityLife = value;
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIShowBigWindow;
			if (null != window && getVisible ())
			{

			}
		}

		public QualityLife qualityLife;

	}
}

