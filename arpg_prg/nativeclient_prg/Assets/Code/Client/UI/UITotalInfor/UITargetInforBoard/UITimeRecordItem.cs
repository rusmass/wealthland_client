﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UITimeRecordItem
	{
		public UITimeRecordItem (GameObject go)
		{
			lb_index = go.GetComponentEx<Text> (Layout.lb_index);
			lb_name = go.GetComponentEx<Text> (Layout.lb_name);
			lb_num = go.GetComponentEx<Text>(Layout.lb_num);

			_selfGo = go;
		}



		public void Refresh(InforRecordVo value)
		{
			lb_index.text = value.index.ToString ();
			lb_name.text = value.title;

			var tmpStr = value.num.ToString();
			//var tmpColor = Color.red;
			if (value.num >= 0)
			{
				tmpStr = string.Format (_greenText,value.num.ToString());
//				tmpColor = Color.green;
			}

			lb_num.text =tmpStr;
		}	

		public void SetActive(bool value)
		{
			if (null != _selfGo)
			{
				_selfGo.SetActiveEx (value);
			}
		}

		//private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>+{0}</color>";

		private Text lb_index;
		private Text lb_name;
		private Text lb_num;

		private GameObject _selfGo;

		class Layout
		{
			public static string lb_index="index";
			public static string lb_name="nametxt";
			public static string lb_num="numtxt";
		}
	}
}
