using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIEventLogWindow :UIWindow<UIEventLogWindow,UIEventLogController>
	{

		public UIEventLogWindow()
		{

		}

		protected override void _Init(GameObject go)
		{
			_OnInitEventLog(go);
		}

		protected override void _OnShow()
		{
			_OnBtnShow();
		}

		protected override void _OnHide()
		{
			_OnHideButton();
		}

		protected override void _Dispose()
		{

		}

		public void TickGame(float deltaTime)
		{

		}
	
	}
}
