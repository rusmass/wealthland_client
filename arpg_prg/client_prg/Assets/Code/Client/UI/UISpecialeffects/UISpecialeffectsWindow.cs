using UnityEngine;
using System.Collections;

namespace Client.UI
{
	public partial class UISpecialeffectsWindow :  UIWindow<UISpecialeffectsWindow,UISpecialeffectsWindowController>  
	{
		public UISpecialeffectsWindow()
		{

		}

		protected override void _Init(GameObject go)
		{
			_OnInitEffects(go);
		}

		protected override void _OnShow()
		{
			
		}

		protected override void _OnHide()
		{

		}

		protected override void _Dispose()
		{

		}

		public void TickGame(float deltaTime)
		{

		}
	}

}
