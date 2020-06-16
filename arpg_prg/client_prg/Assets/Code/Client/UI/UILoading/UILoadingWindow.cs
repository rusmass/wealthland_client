using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UILoadingWindow :UIWindow <UILoadingWindow,UILoadingWindowController>
	{
		public UILoadingWindow()
		{
			
		}

		protected override void _Init (GameObject go)
		{
			progressBar = go.GetComponentEx<Slider>(Layout.progressBar);
		}

		protected override void _OnShow ()
		{
			
		}

		protected override void _OnHide()
		{
			
		}

		protected override void _Dispose()
		{
			
		}

		public void setProgressBarValue(float value)
		{
			progressBar.value = value;
		}

		private Slider progressBar;

	}
}

