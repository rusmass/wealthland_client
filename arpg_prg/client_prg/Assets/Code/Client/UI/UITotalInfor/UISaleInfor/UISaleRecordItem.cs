using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UISaleRecordItem
	{
		public UISaleRecordItem (GameObject go)
		{
			lb_title = go.GetComponentEx<Text> (Layout.lb_title);
			lb_price = go.GetComponentEx<Text> (Layout.lb_prise);
			lb_number = go.GetComponentEx<Text> (Layout.lb_number);
			lb_morget = go.GetComponentEx<Text> (Layout.lb_morget);
			lb_sale = go.GetComponentEx<Text> (Layout.lb_sale);
			lb_income = go.GetComponentEx<Text> (Layout.lb_income);
			lb_quality = go.GetComponentEx<Text> (Layout.lb_quality);
			lb_zhuan = go.GetComponentEx<Text> (Layout.lb_zhuan);
		}

		public void Refresh(SaleRecordVo value)
		{
			lb_title.text = value.title;
			lb_price.text = value.price.ToString ();
			lb_number.text = Mathf.Abs(value.number).ToString ();

			var tmpMorget = value.mortage.ToString();
			if ( value.mortage < 0)
			{
				tmpMorget = "-";
			}
			lb_morget.text = tmpMorget;
			lb_sale.text = value.saleMoney.ToString();
			lb_income.text = value.income.ToString();
			lb_quality.text = value.quality.ToString();

			lb_zhuan.text = value.getMoney.ToString();

			// 净赚大于0，是绿色 。 否则是红色
			if (value.getMoney < 0)
			{
				lb_quality.color = Color.red;

				lb_zhuan.text =string.Format(_redText , value.getMoney.ToString());
			}
			else
			{
//				lb_zhuan.color = Color.green;
				lb_zhuan.text = string.Format(_greenText,value.getMoney.ToString());
			}

		}


		private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

		private Text lb_title;
		private Text lb_price;
		private Text lb_number;
		private Text lb_morget;
		private Text lb_sale;
		private Text lb_income;
		private Text lb_quality;
		private Text lb_zhuan;


		class Layout
		{
			public static string lb_title="txtAsset";
			public static string lb_prise="txtPrice";
			public static string lb_number="txtNum";
			public static string lb_morget="txtDiYa";
			public static string lb_sale="txtSell";
			public static string lb_income="txtFeiLaoWu";
			public static string lb_quality="txtQuality";
			public static string lb_zhuan="txtZhuan";
		}
	}
}

