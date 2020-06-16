using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIBuyCareWindow
	{

		private void _OnInitCenter(GameObject go)
		{
			desc1 = go.GetComponentEx<Text> (Layout.lb_desc);
			desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);
			desc3 = go.GetComponentEx<Text> (Layout.lb_desc3);

			lb_cardname = go.GetComponentEx<Text> (Layout.lb_cardName);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay (rawImg);

			img_bgBoard = go.GetComponentEx<Image> (Layout.img_bgBoard);
		}

		private void _OnDisposeCenter()
		{
			if (null != _cardPic) 
			{
				_cardPic.Dispose ();
			}
		}


		private void _OnShowCenter()
		{
			SetInnerFateCardData (_controller.title,_controller.desc,_controller.cardPath);
		}

		private void SetInnerFateCardData(string title,string desc,string imgPath)
		{
			lb_cardname.text = title ;

			desc1.text = desc;
			desc2.SetActiveEx(false);
			desc3.SetActiveEx (false);

			if ("" != imgPath)
			{
				if(null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}

		}

		private Text lb_cardname;

		private Text desc1;
		private Text desc2;
		private Text desc3;

		private Image img_bgBoard;

		private UIImageDisplay _cardPic;

	}
}

