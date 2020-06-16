using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using System;
namespace Client.UI
{
	public partial class UIShowRelaxWindow
	{
		private void _OnInitContent(GameObject go)
		{
			_txtLbcardname = go.GetComponentEx<Text>(Layout.txt_lbcardname);
			_txtPayment = go.GetComponentEx<Text>(Layout.txt_payment);

			_txtProfitName = go.GetComponentEx<Text>(Layout.txt_profitName);
			_txtProfit = go.GetComponentEx<Text>(Layout.txt_profit);
			_txtIncomeName = go.GetComponentEx<Text>(Layout.txt_incomeName);
			_txtIncome= go.GetComponentEx<Text>(Layout.txt_income);
			_txtTimeName = go.GetComponentEx<Text>(Layout.txt_timeName);
			_txtTimescore = go.GetComponentEx<Text>(Layout.txt_timescore);
			_txtTitle = go.GetComponentEx<Text>(Layout.txt_title);
			_txtDesc = go.GetComponentEx<Text>(Layout.txt_desc);

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);

			_imgShowImage = go.GetComponentEx<Image>(Layout.img_showImage);
		}

		protected void _OnShowContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			SetContent(_controller.relax);

		}

		protected void _OnHideContent()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			_controller.setVisible(false);
		}

		private void SetContent(Relax value)
		{
			_txtLbcardname.text = value.title;

			_txtTitle.text = value.title;

			if (value.desc == null || value.desc == "") 
			{
				_txtDesc.SetActiveEx (false);
			}else
			{
				var str = value.desc;
				var str1 = str.Replace ("\\u3000", "\u3000");
				var str2 = str1.Replace ("\\n","\n");
				_txtDesc.text = str2;
			}

			var tmpPay=HandleStringTool.HandleMoneyTostring(Mathf.Abs(value.payment));

			_txtPayment.text =string.Format("￥ {0}",tmpPay);

			if (value.timeScore == 0) 
			{
				_txtTimeName.SetActiveEx (false);
				_txtTimescore.SetActiveEx (false);
			} else
			{
				_txtTimescore.text = string.Concat (value.timeScore);
			}

			if (null == value.profit || value.profit == "")
			{
				_txtProfitName.SetActiveEx (false);
				_txtProfit.SetActiveEx (false);
			} else 
			{
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
			}

			if (value.income == 0)
			{
				_txtIncomeName.SetActiveEx (false);
				_txtIncome.SetActiveEx (false);
			} else
			{
				_txtIncome.text =string.Format("￥ {0}",value.income.ToString());
			}

			WebManager.Instance.LoadWebItem(value.cardPath,item =>{
				using(item)
				{
					_imgShowImage.sprite = item.sprite;
				}
			});
		}

		private Text _txtLbcardname;

		private Text _txtPayment;
		private Text _txtProfitName;
		private Text _txtProfit;
		private Text _txtIncomeName;
		private Text _txtIncome;
		private Text _txtTimeName;
		private Text _txtTimescore;

		private Text _txtTitle ;
		private Text _txtDesc;

		private Button _btnSure;

		private Image _imgShowImage;
	}
}

