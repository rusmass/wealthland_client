using System;
using UnityEngine;
namespace Client.UI
{
	public class UIGameEffectController:UIController<UIGameEffectWindow,UIGameEffectController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigold.ab";
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

		public void AddMoneyEffect(int playerIndex,Vector3 _initPosition)
		{
			Vector3 changePosition= _initPosition;

//			if(getVisible()==false)
//			{
//				this.setVisible (true);
//			}

			var window = _window as UIGameEffectWindow;
			if (null != window && this.getVisible ()==true)
			{
				window.AddMoneyEffect (playerIndex,changePosition);
			}
		}
	}
}

