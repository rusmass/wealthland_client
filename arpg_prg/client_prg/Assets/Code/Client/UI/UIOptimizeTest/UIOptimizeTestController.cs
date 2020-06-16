using System;
using UnityEngine;

namespace Client.UI
{
	public class UIOptimizeTestController : UIController<UIOptimizeTestWindow, UIOptimizeTestController>
	{
		protected override string _windowResource { get { return "prefabs/ui/scene/uioptimizetest.ab";} }
//		protected override string _windowResource { get { return "prefabs/ui/scene/uibattle.ab";} }

		public UIOptimizeTestController()
		{
			
		}

		protected override void _OnLoad ()
		{
			base._OnLoad ();
		}

		protected override void _OnShow ()
		{
			base._OnShow ();
		}

		protected override void _OnHide ()
		{
			base._OnHide ();
		}

		public override void Tick (float deltaTime)
		{
			base.Tick (deltaTime);
		}

		protected override void _Dispose ()
		{
			
			base._Dispose ();
		}
	}
}

