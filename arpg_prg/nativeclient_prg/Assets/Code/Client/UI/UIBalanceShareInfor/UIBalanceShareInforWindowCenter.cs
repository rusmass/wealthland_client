using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using System.Text;

namespace Client.UI
{
	public partial class UIBalanceShareInforWindow
	{

		private void _InitCenter(GameObject go)
		{
			lb_desc = go.GetComponentEx<Text> (Layout.lb_desc);
			lb_ticketName = go.GetComponentEx<Text> (Layout.lb_ticketName);
			lb_ticketTxt = go.GetComponentEx<Text> (Layout.lb_ticketTxt);
			lb_paymentTxt = go.GetComponentEx<Text> (Layout.lb_paymentTxt);
			lb_profitTxt = go.GetComponentEx<Text> (Layout.lb_profitTxt);
			lb_priceRangeTxt = go.GetComponentEx<Text> (Layout.lb_pricerangeTxt);
			lb_priceRangeName = go.GetComponentEx<Text> (Layout.lb_pricerangeName);
			lb_incomeName = go.GetComponentEx<Text> (Layout.lb_incomeName);
			lb_incomeTxt = go.GetComponentEx<Text> (Layout.lb_incomeTxt);

			lb_profitNameTxt = go.GetComponentEx<Text> (Layout.lb_profitName);

			lb_qualityName = go.GetComponentEx<Text> (Layout.lb_qualityName);
			lb_qualityTxt = go.GetComponentEx<Text> (Layout.lb_qualityTxt);
			lb_qualityDescTxt = go.GetComponentEx<Text> (Layout.lb_qualitydescTxt);

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

//			setTitle (SubTitleManager.Instance.subtitle.cardChance);

			if (null != _controller.cardData)
			{
				SetChanceShaersData (_controller.cardData,_controller.cardData.cardPath);
				Console.WriteLine (_controller.cardData.id);
			}
		}



		private void SetChanceShaersData(ChanceShares go,string imgPath)
		{	
			lb_cardName.text = go.title;

//			lb_desc.text = go.desc;

			var str = go.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");
			lb_desc.text =str2;

			lb_ticketTxt.text = go.ticketCode;
			lb_ticketName.text =String.Concat(go.ticketName," :");

			//lb_paymentTxt.text = String.Concat ("￥",Math.Abs (go.payment));

			//lb_paymentTxt.text="￥ "+Math.Abs(go.payment);

			var stringbuilder = new StringBuilder ();
			stringbuilder.Append ("￥");
			stringbuilder.Append (Mathf.Abs(go.payment));

			lb_paymentTxt.text = stringbuilder.ToString();

			if (null == go.returnRate || go.returnRate == "") 
			{
				lb_profitNameTxt.SetActiveEx (false);
			}
			else
			{
//				var tmpProfit = float.Parse(go.returnRate);

				var tmpProfit = go.returnRate;
				if (tmpProfit == "")
				{
					lb_profitNameTxt.SetActiveEx (false);
				} 
				else 
				{
					//					lb_profitTxt.text=go.returnRate;
					var tmpRate = "";
					if (GameModel.GetInstance.isPlayNet == false)
					{
						tmpRate=string.Format("{0}%",(float.Parse(tmpProfit) *100).ToString());
					}
					else
					{
						tmpRate = tmpProfit;
					}

					lb_profitTxt.text = tmpRate;

				}

			}


			if (null == go.priceRagne || go.priceRagne == "")
			{
				lb_priceRangeTxt.SetActiveEx (false);		
				lb_priceRangeName.SetActiveEx (false);
			} else
			{
				lb_priceRangeTxt.text = go.priceRagne;
			}


			//lb_incomeTxt.text = String.Format ("￥{0} ", (Math.Abs (go.income)).ToString);

			if (go.income == 0) {
				lb_incomeTxt.SetActiveEx (false);
				lb_incomeName.SetActiveEx (false);
			} else
			{
				lb_incomeTxt.text = "￥ "+ Math.Abs (go.income);
			}

			if (go.qualityScore == 0)
			{
				lb_qualityName.SetActiveEx (false);
				lb_qualityTxt.SetActiveEx (false);
				lb_qualityDescTxt.SetActiveEx (false);
			} else
			{
				lb_qualityTxt.text = "" + go.qualityScore;
				lb_qualityDescTxt.text = "品质积分用于进入内圈后乘以10倍";
			}

			if(""!=imgPath)
			{
				if(null != _cardPic)
				{
					_cardPic.Load (imgPath);
				}
			}




		}

		private UIImageDisplay _cardPic;

		private Text lb_cardName;

		private Text lb_desc;
		private Text lb_ticketName;
		private Text lb_ticketTxt;
		private Text lb_paymentTxt;
		private Text lb_profitTxt;
		private Text lb_priceRangeTxt;
		private Text lb_priceRangeName;
		private Text lb_incomeName;
		private Text lb_incomeTxt;
		private Text lb_qualityName;
		private Text lb_qualityTxt;
		private Text lb_qualityDescTxt;

		private Text lb_profitNameTxt;

	}
}

