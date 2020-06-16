using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIRelaxCardWindow
	{
		private void _InitCenter(GameObject go)
		{
			lb_cardTitle = go.GetComponentEx<Text> (Layout.lb_cardTitle);
			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);

			lb_paymenyTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);

			lb_timeTxt = go.GetComponentEx<Text> (Layout.lb_timeTxt);
			lb_timeName = go.GetComponentEx<Text> (Layout.lb_timeName);

			lb_profitName = go.GetComponentEx<Text> (Layout.lb_profitName);
			lb_profitTxt = go.GetComponentEx<Text> (Layout.lb_profitTxt);

			lb_incomeName = go.GetComponentEx<Text> (Layout.lb_incomeName);
			lb_incometxt = go.GetComponentEx<Text> (Layout.lb_incomeTxt);
			lb_cardName = go.GetComponentEx<Text> (Layout.lb_cardname);


			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);

			_cardPic = new UIImageDisplay (rawImg);

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

		private void _ShowQualityLifeCardData (Relax go, string imgPath)
		{
			lb_cardName.text = go.title;

			lb_cardTitle.text = go.title;
			if (go.desc == null || go.desc == "") 
			{
				lb_desc.SetActiveEx (false);
			}else
			{
//				lb_desc.text = go.desc;
				var str = go.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc.text =str2;
			}

			var tmpPay=HandleStringTool.HandleMoneyTostring(Mathf.Abs(go.payment));

			lb_paymenyTxt.text =string.Format("￥ {0}",tmpPay);

			if (go.timeScore == 0) 
			{
				lb_timeName.SetActiveEx (false);
				lb_timeTxt.SetActiveEx (false);
			} else
			{
				lb_timeTxt.text = string.Concat (go.timeScore);
			}

			if (null == go.profit || go.profit == "")
			{
				lb_profitName.SetActiveEx (false);
				lb_profitTxt.SetActiveEx (false);
			} else 
			{
				var tmpProfit = "";
				if (GameModel.GetInstance.isPlayNet == false)
				{
					var tmpvalue =  float.Parse (go.profit);
					tmpProfit=string.Format ("{0}%", (tmpvalue * 100).ToString());
				}
				else
				{
					tmpProfit = go.profit;
				}

				lb_profitTxt.text = tmpProfit;
			}

			if (go.income == 0)
			{
				lb_incomeName.SetActiveEx (false);
				lb_incometxt.SetActiveEx (false);
			} else
			{
				lb_incometxt.text =string.Format("￥ {0}",go.income.ToString());
			}

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

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;

		private Text lb_cardName;
		private Text lb_cardTitle;
		private Text lb_desc;

		//private Text lb_paymentName;
		private Text lb_paymenyTxt;

		private Text lb_timeName;
		private Text lb_timeTxt;

		private Text lb_profitName;
		private Text lb_profitTxt;

		private Text lb_incomeName;

		private Text lb_incometxt;

		private UIImageDisplay _cardPic;
	}
}

