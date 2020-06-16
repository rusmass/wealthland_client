using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBalanceFixedInforWindow
	{
		private void _InitTop(GameObject go)
		{
			img_title = go.GetComponentEx<Image> (Layout.img_title);
			img_display = new UIImageDisplay (img_title);
		}

		private void _OnShowTop()
		{
		}

		// set the txt context
		private void setTitle (string str)
		{
			if (null != img_display)
			{
				img_display.Load (str);
			}
		}

		private void _OnDisposeTop()
		{
			if (null != img_display) 
			{
				img_display.Dispose ();
			}
		}

		private Image img_title;
		private UIImageDisplay img_display;
	}
}

