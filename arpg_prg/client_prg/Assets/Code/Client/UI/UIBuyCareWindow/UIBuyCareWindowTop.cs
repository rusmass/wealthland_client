using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBuyCareWindow
	{
		private void _InitTop(GameObject go)
		{
			img_title = go.GetComponentEx<Image> (Layout.img_title);
			img_titleDisplay = new UIImageDisplay (img_title);
			img_bg = go.GetComponentEx<Image> (Layout.img_bg);
		}

		private void _OnShowTop()
		{
			if (null != img_titleDisplay)
			{
				img_titleDisplay.Load (CardTitlePath.InnerFate_Card);
				img_title.SetNativeSize ();
			}
		}

		private void _OnDisposeTop()
		{
			if (null != img_titleDisplay)
			{
				img_titleDisplay.Dispose ();
			}
		}


		private void _HideBgImg()
		{
			if (null != img_bg)
			{
				img_bg.SetActiveEx (false);
			}
		}

		private Image img_bg;
		private Image img_title;
		private UIImageDisplay img_titleDisplay;

	}
}

