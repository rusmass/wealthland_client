using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using System;
using System.Text;

namespace Client.UI
{
	public partial class UIShowSmallWindow
	{
		private void _OnInitContent(GameObject go)
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

			_cardPic = go.GetComponentEx<Image>(Layout.loadImg);
			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);

			card2_lb_desc = go.GetComponentEx<Text> (Layout.card2_lb_desc);
			card2_lb_ticketName = go.GetComponentEx<Text> (Layout.card2_lb_ticketName);
			card2_lb_ticketTxt = go.GetComponentEx<Text> (Layout.card2_lb_ticketTxt);
			card2_lb_paymentTxt = go.GetComponentEx<Text> (Layout.card2_lb_paymentTxt);
			card2_lb_profitTxt = go.GetComponentEx<Text> (Layout.card2_lb_profitTxt);
			card2_lb_priceRangeTxt = go.GetComponentEx<Text> (Layout.card2_lb_pricerangeTxt);
			card2_lb_priceRangeName = go.GetComponentEx<Text> (Layout.card2_lb_pricerangeName);
			card2_lb_incomeName = go.GetComponentEx<Text> (Layout.card2_lb_incomeName);
			card2_lb_incomeTxt = go.GetComponentEx<Text> (Layout.card2_lb_incomeTxt);

			card2_lb_profitNameTxt = go.GetComponentEx<Text> (Layout.card2_lb_profitName);

			card2_lb_qualityName = go.GetComponentEx<Text> (Layout.card2_lb_qualityName);
			card2_lb_qualityTxt = go.GetComponentEx<Text> (Layout.card2_lb_qualityTxt);
			card2_lb_qualityDescTxt = go.GetComponentEx<Text> (Layout.card2_lb_qualitydescTxt);

			card2_lb_cardName = go.GetComponentEx<Text> (Layout.card2_lb_cardName);
			card2_cardPic = go.GetComponentEx<Image>(Layout.card2_loadImg);
			card2_sureBtn = go.GetComponentEx<Button>(Layout.card2_btnSure);

			card1 = go.DeepFindEx(Layout.card1).gameObject;
			card2 = go.DeepFindEx(Layout.card2).gameObject;
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			EventTriggerListener.Get(card2_sureBtn.gameObject).onClick += _OnBtnSureClick2;
			SetContent(_controller.chance);

		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
			EventTriggerListener.Get(card2_sureBtn.gameObject).onClick -= _OnBtnSureClick2;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void _OnBtnSureClick2(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(Chance go)
		{
			if(go.cash_belongsTo == 2)
			{
				card1.SetActiveEx(false);
				card2.SetActiveEx(true);
				card2_lb_cardName.text = go.cash_title;

				var str = go.cash_desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
		
				card2_lb_desc.text = str2;

				card2_lb_ticketTxt.text = go.cash_ticketCode;
				card2_lb_ticketName.text = String.Concat(go.cash_ticketName," :");

				var stringbuilder = new StringBuilder ();
				stringbuilder.Append ("￥");
				stringbuilder.Append (Mathf.Abs(go.cash_payment));

				card2_lb_paymentTxt.text = stringbuilder.ToString();

				if (null == go.cash_returnRate || go.cash_returnRate == "") 
				{
					card2_lb_profitNameTxt.SetActiveEx (false);
				}
				else
				{					
					var tmpProfit = ""; //float.Parse(go.cash_returnRate);

					if (GameModel.GetInstance.isPlayNet == false)
					{
						var tmpValue = 	float.Parse(go.cash_returnRate);
						tmpProfit = string.Format ("{0}%", (tmpValue * 100).ToString ());
					}
					else
					{
						tmpProfit = go.cash_returnRate;
					}

					card2_lb_profitTxt.text=tmpProfit;
				}


				if (null == go.cash_priceRagne || go.cash_priceRagne == "")
				{
					card2_lb_priceRangeTxt.SetActiveEx (false);		
					card2_lb_priceRangeName.SetActiveEx (false);
				} else
				{
					card2_lb_priceRangeTxt.text = go.cash_priceRagne;
				}


				if (go.income == 0) {
					card2_lb_incomeTxt.SetActiveEx (false);
					card2_lb_incomeName.SetActiveEx (false);
				} else
				{
					lb_incomeTxt.text = "￥ "+ Math.Abs (go.income);
				}

				if (go.cash_qualityScore == 0)
				{
					card2_lb_qualityName.SetActiveEx (false);
					card2_lb_qualityTxt.SetActiveEx (false);
					card2_lb_qualityDescTxt.SetActiveEx (false);
				} else
				{
					card2_lb_qualityTxt.text = "" + go.cash_qualityScore;
					card2_lb_qualityDescTxt.text = "品质积分用于进入内圈后乘以10倍";
				}

				WebManager.Instance.LoadWebItem(go.cash_cardPath,item =>{
					using(item)
					{
						card2_cardPic.sprite = item.sprite;
					}
				});

			}
			else
			{
				card1.SetActiveEx(true);
				card2.SetActiveEx(false);

				lb_cardName.text = go.title;

				var str = go.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				lb_desc.text = str2;

				lb_coastTxt.text = go.coast;
				lb_saleTxt.text = go.sale;
			
				lb_paymentTxt.text="￥ "+Math.Abs(go.payment);

				if (null == go.profit || go.profit == "")
				{
					lb_profitNameTxt.SetActiveEx (false);
				}
				else
				{
					var tmpProfit = "";

					if (GameModel.GetInstance.isPlayNet == false)
					{
						var tmpValue = 	float.Parse(go.profit);
						tmpProfit = string.Format ("{0}%", (tmpValue * 100).ToString ());
					}
					else
					{
						tmpProfit = go.profit;
					}

					lb_profitTxt.text=tmpProfit;
				}


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


				WebManager.Instance.LoadWebItem(go.cardPath,item =>{
					using(item)
					{
						_cardPic.sprite = item.sprite;
					}
				});
			}
		}


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

		private Image _cardPic;
		private Button _btnSure;

		private GameObject card1;
		private GameObject card2;

		private Image card2_cardPic;
		private Button card2_sureBtn;
		private Text card2_lb_cardName;

		private Text card2_lb_desc;
		private Text card2_lb_ticketName;
		private Text card2_lb_ticketTxt;
		private Text card2_lb_paymentTxt;
		private Text card2_lb_profitTxt;
		private Text card2_lb_priceRangeTxt;
		private Text card2_lb_priceRangeName;
		private Text card2_lb_incomeName;
		private Text card2_lb_incomeTxt;
		private Text card2_lb_qualityName;
		private Text card2_lb_qualityTxt;
		private Text card2_lb_qualityDescTxt;

		private Text card2_lb_profitNameTxt;



	}
}
