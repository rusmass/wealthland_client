using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using System;
namespace Client.UI
{
	public partial class UIShowInvestmentWindow
	{
		private void _OnInitContent(GameObject go)
		{

			_txtLbcardname = go.GetComponentEx<Text>(Layout.txt_lbcardname);

			_txtCoast = go.GetComponentEx<Text>(Layout.txt_lbcardname);
			_txtSale = go.GetComponentEx<Text>(Layout.txt_sale);
			_txtPayment = go.GetComponentEx<Text>(Layout.txt_payment);
			_txtProfitName = go.GetComponentEx<Text>(Layout.lb_profitName);
			_txtProfit = go.GetComponentEx<Text>(Layout.txt_profit);
			_txtIncomeName = go.GetComponentEx<Text>(Layout.txt_lbcardname);
			_txtIncome = go.GetComponentEx<Text>(Layout.txt_income);
			_txtTimeName = go.GetComponentEx<Text>(Layout.lb_timeName);
			_txtTimescore = go.GetComponentEx<Text>(Layout.txt_timescore);
			_txtDesc = go.GetComponentEx<Text>(Layout.txt_desc);
			_txtTitle = go.GetComponentEx<Text>(Layout.txt_title);

			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.investment);

		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(Investment value)
		{
			_txtLbcardname.text = value.title;

			_txtTitle.text = value.title;
			_txtTitle.SetActiveEx(false);

			if (value.desc == null || value.desc == "") 
			{
				_txtDesc.SetActiveEx (false);
			}
			else
			{
				var str = value.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				_txtDesc.text = str2;
			}

			var tmpPay=HandleStringTool.HandleMoneyTostring(Mathf.Abs (value.payment));

			_txtPayment.text = string.Format ("￥ {0}",tmpPay);

			_txtTimescore.SetActiveEx (false);
			_txtTimeName.SetActiveEx(false);
			if (null == value.profit || value.profit == "")
			{
				_txtProfitName.SetActiveEx (false);
				_txtProfit.SetActiveEx (false);
			} 
			else 
			{
				var tmpProfit ="";

				if (GameModel.GetInstance.isPlayNet == false)
				{
					var tmpValue = 	float.Parse(value.profit);
					tmpProfit = string.Format ("{0}%", (tmpValue * 100).ToString ());
				}
				else
				{
					tmpProfit = value.profit;
				}

				_txtProfit.text =tmpProfit;
			}

			if (value.income == 0)
			{
				_txtIncomeName.SetActiveEx (false);
				_txtIncome.SetActiveEx (false);
			}
			else
			{
				_txtIncome.text =string.Concat(value.income);
			}

			WebManager.Instance.LoadWebItem(value.cardPath,item =>{
				using(item)
				{
					_imgShowImage.sprite = item.sprite;
				}
			});

		}





		private Text  _txtLbcardname;

		private Text  _txtCoast;
		private Text  _txtSale;

		private Text  _txtPayment;
		private Text  _txtProfitName;
		private Text  _txtProfit ;
		private Text  _txtIncomeName;
		private Text  _txtIncome ;
		private Text  _txtTimeName;
		private Text  _txtTimescore;


		private Text  _txtDesc ;
		private Text  _txtTitle;

		private Image _imgShowImage ;

		private Button _btnSure;


	}
}
