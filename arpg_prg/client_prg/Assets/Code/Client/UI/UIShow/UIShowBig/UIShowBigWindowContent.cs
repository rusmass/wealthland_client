using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core.Web;
using Metadata;
using System;

namespace Client.UI
{
	public partial class UIShowBigWindow
	{
		private void _OnInitContent(GameObject go)
		{
			_txtLbcardname = go.GetComponentEx<Text>(Layout.txt_lbcardname);

			_txtCoast = go.GetComponentEx<Text>(Layout.txt_coast);
			_txtSale = go.GetComponentEx<Text>(Layout.txt_sale);
			_txtPayment = go.GetComponentEx<Text>(Layout.txt_payment);
			_txtProfit = go.GetComponentEx<Text>(Layout.txt_profit);

			_txtMortgage = go.GetComponentEx<Text>(Layout.txt_mortgage);
			_txtIncome = go.GetComponentEx<Text>(Layout.txt_income);

			_txtDesc = go.GetComponentEx<Text>(Layout.txt_desc);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);

		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.opportunity);

		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(Opportunity value)
		{

			_txtLbcardname.text = value.title;

			_txtCoast.text = value.cost;
			_txtSale.text = value.sale;
			_txtPayment.text = "￥ "+ Math.Abs (value.payment);

			var tmpProfit = "";

			if (GameModel.GetInstance.isPlayNet == false)
			{
				var tmpValue = 	float.Parse(value.profit);
				tmpProfit = string.Format ("{0}%", (tmpValue * 100).ToString ());
			}
			else
			{
				tmpProfit = value.profit;
			}

		
			_txtProfit.text = tmpProfit;

			_txtMortgage.text = "￥ "+ Math.Abs(value.mortgage);
			_txtIncome.text = "￥ "+ Math.Abs(value.income);

			var str = value.desc;
			var str1 = str.Replace ("\\u3000", "\u3000");
			var str2 = str1.Replace ("\\n","\n");

			_txtDesc.text = str2;

			WebManager.Instance.LoadWebItem(value.cardPath,item =>{
				using(item)
				{
					_imgShowImage.sprite = item.sprite;
				}
			});
		}


		private Text _txtLbcardname;

		private Text _txtCoast;
		private Text _txtSale;
		private Text _txtPayment;
		private Text _txtProfit;

		private Text _txtMortgage;
		private Text _txtIncome;
		private Text _txtQuality;
		private Text _txtQualitydesc;
		private Text _txtDesc;

		private Button	_btnSure;
		private Image   _imgShowImage;
	}
}
