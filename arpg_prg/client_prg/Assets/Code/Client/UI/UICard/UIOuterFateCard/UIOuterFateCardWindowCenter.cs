using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIOuterFateCardWindow
	{

		private void _OnInitCenter(GameObject go)
		{
			desc1 = go.GetComponentEx<Text> (Layout.lb_desc);
//			desc2 = go.GetComponentEx<Text> (Layout.lb_desc2);
//			desc3 = go.GetComponentEx<Text> (Layout.lb_desc3);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);

			lb_cardName = go.GetComponentEx<Text> (Layout.lb_cardname);

			_cardPic = new UIImageDisplay (rawImg);

			lb_noCondition = go.GetComponentEx<Text> (Layout.lb_noConditiionTip);
			lb_noCondition.SetActiveEx (false);

			//zll 2016.10.21 add card action
			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);
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

			if (null != _controller.cardData) 
			{
				setOuterFateCardData (_controller.cardData,_controller.cardData.cardPath);
			}

			//zll 2016.10.21 add card action
			cardAction.SetActiveEx(true);
			cardAction2.SetActiveEx(false);
		}

		private void _ShowNoCondition()
		{
			lb_noCondition.SetActiveEx (true);
			lb_noCondition.text = "你没有对应资产,下次再说吧";
		}

		public void setOuterFateCardData(OuterFate carddata, string imgPath)
		{			
			//desc1.text = carddata.desc;

			var str =carddata.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			desc1.text =str2;



//			desc2.SetActiveEx (false);
//			desc3.SetActiveEx (false);

			lb_cardName.text = carddata.title;

			if(""!=imgPath)
			{
				if(null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}

		}

		private bool _isShowAction=false;
		//zll 2016.10.21 add card action
		private void actionTime(float deltaTime)
		{
			if (_isShowAction == true)
			{
				return;
			}

			addtime += deltaTime;

			if(addtime >= 0.40f)
			{
				cardAction.SetActiveEx(true);
				cardAction2.SetActiveEx(true);
				_isShowAction = true;
				addtime = 0;
			}
		}

		private Text lb_cardName;

		private Text desc1;
//		private Text desc2;
//		private Text desc3;
		private UIImageDisplay _cardPic;

		private Text lb_noCondition;

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;
	}
}

