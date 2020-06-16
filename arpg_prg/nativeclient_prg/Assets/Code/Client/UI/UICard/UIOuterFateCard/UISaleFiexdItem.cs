using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UISaleFiexdItem:IDisposable
	{
		public UISaleFiexdItem (GameObject go)
		{
			lb_salename=go.GetComponentEx<Text>(go.name);
			lb_salepayment=go.GetComponentEx<Text>(Layout.lb_salepayment);
			lb_salemortgage = go.GetComponentEx<Text> (Layout.lb_salemortgage);
			lb_salemoeny=go.GetComponentEx<Text>(Layout.lb_salemoeny);
			lb_salechangeMoney=go.GetComponentEx<Text>(Layout.lb_salechangeMoney);
			lb_saleincome=go.GetComponentEx<Text>(Layout.lb_saleincome);
			lb_salequality=go.GetComponentEx<Text>(Layout.lb_salequality);
			toggle_saleselect=go.GetComponentEx<Toggle>(Layout.toggle_saleselect);

			toggle_saleselect.onValueChanged.AddListener(_SelectToSale);
		}


		private void _SelectToSale(bool value)
		{

			Audio.AudioManager.Instance.BtnMusic ();

			_saleFixedVo.isSlected = value;

			var controller = UIControllerManager.Instance.GetController<UIOuterFateCardController> ();
			controller.UpdateTextData ();
		}

		public void Refresh(FateSaleFiexdVo saleValue)
		{
			lb_salename.text=saleValue.title;
			lb_salepayment.text=Mathf.Abs(saleValue.payment).ToString();
			lb_salemortgage.text =Mathf.Abs(saleValue.mortgage).ToString() ;
			lb_salemoeny.text=saleValue.saleMoney.ToString();

			var tmpStr="";
			if (saleValue.changeMoney > 0)
			{
				tmpStr = string.Format (_greenText,saleValue.changeMoney);
				lb_salechangeMoney.text=tmpStr;
			}
			else if(saleValue.changeMoney<0)
			{
				tmpStr = string.Format (_redText,saleValue.changeMoney);
				lb_salechangeMoney.text=tmpStr;
			}
			else
			{
				lb_salechangeMoney.text=saleValue.changeMoney.ToString();
			}

			if (saleValue.income > 0)
			{
				tmpStr = string.Format (_greenText,saleValue.income);
				lb_saleincome.text=tmpStr;
			}
			else if(saleValue.income<0)
			{
				tmpStr = string.Format (_redText,saleValue.income);
				lb_saleincome.text=tmpStr;
			}
			else
			{
				lb_saleincome.text=saleValue.income.ToString();
			}

			if(saleValue.quality>0)
			{
				tmpStr = string.Format (_greenText,saleValue.quality);
				lb_salequality.text=tmpStr;
			}
			else if(saleValue.quality<0)
			{
				tmpStr = string.Format (_redText,saleValue.quality);
				lb_salequality.text=tmpStr;
			}
			else 
			{
				lb_salequality.text=saleValue.quality.ToString();
			}

			_saleFixedVo= saleValue;
		}

		public void Dispose()
		{			
			toggle_saleselect.onValueChanged.RemoveListener(_SelectToSale);
		}

		private FateSaleFiexdVo _saleFixedVo;

		public FateSaleFiexdVo SaleFixedVo
		{
			get
			{
				return _saleFixedVo;
			}
		}

		private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

		private Text lb_salename;
		private Text lb_salepayment;
		private Text lb_salemortgage;
		private Text lb_salemoeny;
		private Text lb_salechangeMoney;
		private Text lb_saleincome;
		private Text lb_salequality;
		private Toggle toggle_saleselect;

		class Layout
		{
			public static string lb_salename="salenameitem";
			public static string lb_salepayment="salepayment";
			public static string lb_salemortgage="salemortgae";
			public static string lb_salemoeny="salemoney";
			public static string lb_salechangeMoney="salechangemoney";
			public static string lb_saleincome="saleincome";
			public static string lb_salequality="salequality";
			public static string toggle_saleselect="saletoggle";
		}
	}
}

