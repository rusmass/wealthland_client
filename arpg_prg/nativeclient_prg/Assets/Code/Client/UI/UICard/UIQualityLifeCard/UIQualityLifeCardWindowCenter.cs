using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIQualityLifeCardWindow
	{
		private void _InitCenter(GameObject go)
		{
//			lb_cardTitle = go.GetComponentEx<Text> (Layout.lb_cardTitle);
			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);

			lb_paymenyTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);

			lb_timeTxt = go.GetComponentEx<Text> (Layout.lb_timeTxt);

			lb_qualityTxt = go.GetComponentEx<Text> (Layout.lb_qualityTxt);

			lb_cardName = go.GetComponentEx<Text> (Layout.lb_cardname);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay(rawImg);

			//zll 2016.10.21 add card action
			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);
		}

		private void _OnShowCenter ()
		{
			if(null!=_controller.cardData)
			{
				_ShowQualityLifeCardData (_controller.cardData,_controller.cardData.cardPath);
			}

			//zll 2016.10.21 add card action
			cardAction.SetActiveEx(true);
			cardAction2.SetActiveEx(false);
		}

		private void _ShowQualityLifeCardData (QualityLife go, string imgPath)
		{

			lb_cardName.text = go.title;
//			lb_cardTitle.text = go.title;
			if (go.desc == null || go.desc == "") 
			{
				lb_desc.SetActiveEx (false);
			}else
			{

				var str = go.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc.text =str2;
			}

			var tmpPay=HandleStringTool.HandleMoneyTostring(Mathf.Abs(go.payment*_controller.castRate));
			var tmpStr ="￥ {0}";
//			if (go.payment < 0)
//			{
//				tmpStr="￥ -{0}";
//			}


			lb_paymenyTxt.text =string.Format(tmpStr,tmpPay) ;

			lb_timeTxt.text = go.timeScore.ToString ();

			lb_qualityTxt.text = string.Concat (go.qualityScore);

			if ("" != imgPath) 
			{
				if(null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}

		}


		private void _OnDisposeCenter()
		{
			if(null != _cardPic)
			{
				_cardPic.Dispose ();
			}
		}

		private bool _isShowAction;

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

				addtime = 0;
				_isShowAction = true;
			}
		}

		private Text lb_cardName;

//		private Text lb_cardTitle;
		private Text lb_desc;

		//private Text lb_paymentName;
		private Text lb_paymenyTxt;

		//private Text lb_timeName;
		private Text lb_timeTxt;

		//private Text lb_qualityName;
		private Text lb_qualityTxt;

		private UIImageDisplay _cardPic;

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;
	}
}

