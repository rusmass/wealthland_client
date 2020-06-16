using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIIncomeRecordInforItem
	{
		public UIIncomeRecordInforItem (GameObject go)
		{
						
			lb_index = go.GetComponentEx<Text> (Layout.lb_index);
			lb_name = go.GetComponentEx<Text> (Layout.lb_name);
			lb_num = go.GetComponentEx<Text> (Layout.lb_numtxt);
		}


		public void Refresh(InforRecordVo value)
		{
			lb_index.text = value.index.ToString();
			lb_num.text = string.Format (_greenText, value.num.ToString ());
			lb_name.text = value.title;
		}

		private Text lb_index;
		private Text lb_name;
		private Text lb_num;

		//private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

		class Layout
		{
			public static string lb_index="index";
			public static string lb_name="nametxt";
			public static string lb_numtxt="numtxt";
		}
	}
}

