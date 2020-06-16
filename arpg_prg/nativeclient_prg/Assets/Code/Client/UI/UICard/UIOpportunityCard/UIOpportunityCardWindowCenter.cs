using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
namespace Client.UI
{
	public partial class UIOpportunityCardWindow
	{

		private void _InitCenter(GameObject go)
		{
			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);
			lb_coastTxt = go.GetComponentEx<Text> (Layout.lb_coastTxt);
			lb_saleTxt = go.GetComponentEx<Text> (Layout.lb_saleTxt);
			lb_paymentTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);
			lb_profitTxt = go.GetComponentEx<Text> (Layout.lb_profitTxt);
			lb_mortgageTxt = go.GetComponentEx<Text> (Layout.lb_mortgageTxt);
			lb_incomeTxt = go.GetComponentEx<Text> (Layout.lb_incomeTxt);
			lb_qualityName = go.GetComponentEx<Text> (Layout.lb_qualityName);
			lb_qualityTxt = go.GetComponentEx<Text> (Layout.lb_qualityTxt);
			lb_qualityDescTxt = go.GetComponentEx<Text> (Layout.lb_qualitydescTxt);

			lb_profitNameTxt = go.GetComponentEx<Text> (Layout.lb_profitName);
			lb_mortgageNameTxt = go.GetComponentEx<Text> (Layout.lb_mortgageName);
			lb_incomeNameTxt = go.GetComponentEx<Text> (Layout.lb_incomeName);

			lb_cardname = go.GetComponentEx<Text> (Layout.lb_cardName);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);

			_cardPic = new UIImageDisplay (rawImg);

			//zll 2016.10.21 add card action
			cardAction = go.GetComponentEx<Image>(Layout.cardAction);
			cardAction2 = go.GetComponentEx<Image>(Layout.cardAction1);
		}

		private void _OnDisposeCenter()
		{
			if(null != _cardPic)
			{
				_cardPic.Dispose();
			}

		}


		private void _OnShowCenter()
		{

			if (null != _controller.cardData)
			{				
				_SetOpportunityData (_controller.cardData,_controller.cardData.cardPath);
				Console.WriteLine (_controller.cardData.id);
			}

			//zll 2016.10.21 add card action
			cardAction.SetActiveEx(true);
			cardAction2.SetActiveEx(false);
		}

	

		private void _SetOpportunityData(Opportunity go,string imgPath)
		{	

			lb_cardname.text = go.title ;

			var str = go.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			lb_desc.text =str2;
//			lb_desc.text = go.desc;
			lb_coastTxt.text = HandleStringTool.HandleMoneyTostring(go.cost);
			lb_saleTxt.text = go.sale;
			//lb_paymentTxt.text = String.Format ("{0:0} ",Math.Abs (go.payment));
			lb_paymentTxt.text="￥ "+HandleStringTool.HandleMoneyTostring(Mathf.Abs(go.payment));

			if (GameModel.GetInstance.isPlayNet == false)
			{
				if(null == go.profit || go.profit =="")
				{
					lb_profitNameTxt.SetActiveEx (false);
				}
				else
				{
					var tmpProfit = float.Parse (go.profit);
					lb_profitTxt.text = string.Format ("{0}%",(tmpProfit *100).ToString());
				}
			}
			else
			{
				if(null == go.profit || go.profit =="")
				{
					lb_profitNameTxt.SetActiveEx (false);
				}
				else
				{
					lb_profitTxt.text =  go.profit;
				}
			}

			lb_mortgageTxt.text="￥ "+HandleStringTool.HandleMoneyTostring(Math.Abs(go.mortgage));
			lb_incomeTxt.text = "￥ "+ Math.Abs (go.income);

			if (go.mortgage == 0)
			{
				lb_mortgageNameTxt.SetActiveEx (false);
			}

			if (go.income == 0)
			{
				lb_incomeNameTxt.SetActiveEx (false);
			}
				

			lb_qualityName.SetActiveEx (false);
			lb_qualityTxt.SetActiveEx (false);
			lb_qualityDescTxt.SetActiveEx (false);

			if ("" != imgPath)
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
			if(_isShowAction==true)
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

		private UIImageDisplay _cardPic;

		private Text lb_cardname;

		private Text lb_desc;
		private Text lb_coastTxt;
		private Text lb_saleTxt;
		private Text lb_paymentTxt;
		private Text lb_profitTxt;
		private Text lb_mortgageTxt;
		private Text lb_incomeTxt;
		private Text lb_qualityName;
		private Text lb_qualityTxt;
		private Text lb_qualityDescTxt;

		//隐藏收益率文本
		private Text lb_profitNameTxt;
		// 隐藏抵押贷款文本
		private Text lb_mortgageNameTxt;
		// 隐藏非劳务收入文本
		private Text lb_incomeNameTxt;

		//zll 2016.10.21 add card action
		private Image cardAction;
		private Image cardAction2;
		private float addtime = 0;
	}
}

