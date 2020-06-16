using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
namespace Client.UI
{
	public partial class UIChanceFixedCardWindow
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
			lb_mortgageName = go.GetComponentEx<Text> (Layout.lb_mortgageName);
			lb_incomeNameTxt = go.GetComponentEx<Text> (Layout.lb_incomeName);

			lb_cardName = go.GetComponentEx<Text> (Layout.lb_cardName);

			var rawImg = go.GetComponentEx<Image> (Layout.loadImg);
			_cardPic = new UIImageDisplay (rawImg);


		}

		private void _OnDisposeCenter()
		{
			if(null != _cardPic)
			{
				_cardPic.Dispose ();
			}

		}


		private void _OnShowCenter()
		{

			if (null != _controller.cardData)
			{

				SetFixedData (_controller.cardData,_controller.cardData.cardPath);
				Console.WriteLine (_controller.cardData.id);
			}
		}

		private void SetFixedData(ChanceFixed go,string imgPath)
		{	
			lb_cardName.text = go.title;

			var str = go.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			lb_desc.text =str2;

//			lb_desc.text = go.desc;
			lb_coastTxt.text = go.coast;
			lb_saleTxt.text = go.sale;
			//lb_paymentTxt.text = String.Format ("{0:0} ",Math.Abs (go.payment));
			lb_paymentTxt.text="￥ "+Math.Abs(go.payment);

			if (GameModel.GetInstance.isPlayNet == false)
			{
				var tmpProfit = float.Parse(go.profit);

				if (tmpProfit == 0)
				{
					lb_profitNameTxt.SetActiveEx (false);
				} 
				else 
				{
					lb_profitTxt.text=string.Format("{0}%",(tmpProfit * 100).ToString());
				}
			}
			else
			{
				var tmpProfit = go.profit;

				if (tmpProfit == "")
				{
					lb_profitNameTxt.SetActiveEx (false);
				} 
				else 
				{
					lb_profitTxt.text=tmpProfit;
				}
			}



			//lb_mortgageTxt.text=String.Format("￥{0} ",Math.Abs(go.mortgage));
			//lb_incomeTxt.text = String.Format ("￥ ", Math.Abs (go.income));
			lb_mortgageTxt.text="￥ "+Math.Abs(go.mortgage);
			lb_incomeTxt.text = "￥ "+ Math.Abs (go.income);

			if (go.mortgage == 0)
			{
				lb_mortgageName.SetActiveEx (false);
			}

			if (go.income == 0)
			{
				lb_incomeNameTxt.SetActiveEx (false);
			}


			if (go.scoreNumber == 0) {
				lb_qualityName.SetActiveEx (false);
				lb_qualityTxt.SetActiveEx (false);
				lb_qualityDescTxt.SetActiveEx (false);
			} else
			{
				if (go.scoreType == 2) {
					lb_qualityName.text = "品质积分: ";
					lb_qualityDescTxt.text = "品质积分用于进入内圈后乘以10倍";
				} else 
				{
					lb_qualityName.text = "时间积分: ";
					lb_qualityDescTxt.text = "时间积分用于进入内圈后乘以100倍";
				}					

				lb_qualityTxt.text =""+go.scoreNumber;
			}

			if ("" != imgPath)
			{
				if (null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}
		}

		private UIImageDisplay _cardPic;

		private Text lb_cardName;

		private Text lb_desc;
		private Text lb_coastTxt;
		private Text lb_saleTxt;
		private Text lb_paymentTxt;
		private Text lb_profitTxt;
		//投资收益率 隐藏
		private Text lb_profitNameTxt;
		//隐藏抵押贷款 文本框
		private Text lb_mortgageName;
		private Text lb_mortgageTxt;
		// 隐藏 非劳务收入的
		private Text lb_incomeNameTxt;
		private Text lb_incomeTxt;
		private Text lb_qualityName;
		private Text lb_qualityTxt;
		private Text lb_qualityDescTxt;
	}
}

