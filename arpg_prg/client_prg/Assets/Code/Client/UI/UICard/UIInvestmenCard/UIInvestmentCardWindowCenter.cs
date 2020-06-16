﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIInvestmentCardWindow
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

			lb_cardname = go.GetComponentEx<Text> (Layout.lb_cardname);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);

			_cardPic = new UIImageDisplay (rawImg);

			//zll 2016.10.21 add card action
//			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
//			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);

			animator_crap = go.GetComponentEx<Animator> (Layout.action_craps);
			var tmpcarp = go.GetComponentEx<Image> (Layout.img_craps);
			img_carp =new UIImageDisplay(tmpcarp);

		}


		private void _OnShowCenter ()
		{
			if(null!=_controller.cardData)
			{
				_ShowQualityLifeCardData (_controller.cardData,_controller.cardData.cardPath);
			}

			img_carp.SetActive (false);
			animator_crap.enabled=false;
			animator_crap.SetActiveEx (false);


			//zll 2016.10.21 add card action
//			cardAction.SetActiveEx(true);
//			cardAction2.SetActiveEx(false);
		}

		private void _ShowQualityLifeCardData (Investment go, string imgPath)
		{

			lb_cardname.text=go.title;

			lb_cardTitle.text = go.title;
			lb_cardTitle.SetActiveEx(false);
			if (go.desc == null || go.desc == "") 
			{
				lb_desc.SetActiveEx (false);
			}else
			{
				var str = go.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc.text =str2;
//				lb_desc.text = go.desc;
			}

			var tmpPay=HandleStringTool.HandleMoneyTostring(Mathf.Abs (go.payment));

			lb_paymenyTxt.text = string.Format ("￥ {0}",tmpPay);

			lb_timeName.SetActiveEx (false);
			lb_timeTxt.SetActiveEx (false);

			//if (go.timeScore == 0) 
			//{
			//	lb_timeName.SetActiveEx (false);
			//	lb_timeTxt.SetActiveEx (false);
			//} else
			//{
			//	lb_timeTxt.text = string.Concat (go.timeScore);
			//}

			if (null == go.profit || go.profit == "")
			{
				lb_profitName.SetActiveEx (false);
				lb_profitTxt.SetActiveEx (false);
			} else 
			{	
				var tmpProfit = "";
				if (GameModel.GetInstance.isPlayNet == false)
				{
					var tmpValue = float.Parse (go.profit);
					tmpProfit=string.Format ("{0}%",(tmpValue *100).ToString());
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
				lb_incometxt.text =string.Concat(go.income);
			}

			if(""!=imgPath)
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

			if (null != img_carp)
			{
				img_carp.Dispose ();
			}
		}
		private bool _isShowAction=false;
		//zll 2016.10.21 add card action
		private void actionTime(float deltaTime)
		{
			if(_isShowAction==false)
			{
				return;
			}

			addtime += deltaTime;
			if (addtime >= 1f && _isRolledCrap==false)
			{
				//cardAction.SetActiveEx(false);
				//				cardAction2.SetActiveEx(true);
				_isRolledCrap=true;
				_HideCrapAndHandler();
			}
			else if(addtime>2f)
			{
				_isShowAction = false;
				addtime = 0;
				_controllerHandler ();
			}
		}

		private Text lb_cardname;

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

		//zll 2016.10.21 add card action
//		private Image cardAction;
//		private Image cardAction2;

		private float addtime = 0;

		private Animator animator_crap;
		private UIImageDisplay img_carp;
		private bool _isRolledCrap=false;
		private int crapsNum=0;
	}
}
