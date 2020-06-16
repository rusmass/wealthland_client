using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIChanceShareCardWindowBuy
	{
		
		public UIChanceShareCardWindowBuy (GameObject go)
		{
			_gameObj = go;

			_lbcode = go.GetComponentEx<Text> (Layout.lb_shareCode);
			_lbsharename = go.GetComponentEx<Text> (Layout.lb_sharename);
			_lbsharepay = go.GetComponentEx<Text> (Layout.lb_sharepay);
			_lbsharenum = go.GetComponentEx<Text> (Layout.lb_sharenum);
			_lbchangeMoney = go.GetComponentEx<Text> (Layout.lb_changeMoney);

		}


		public void Refresh(ChangeShareVo changeValue)
		{			
			_lbcode.text=changeValue.shareData.ticketCode;
			_lbsharename.text=changeValue.shareData.ticketName;
			_lbsharepay.text=(Mathf.Abs(changeValue.shareData.payment)).ToString();

			var tmpStr = "";
			if(changeValue.changeNum>0)
			{
				tmpStr = string.Format ("(<color=#00ff00>+{0}</color>)",changeValue.changeNum);			
				_lbsharenum.text=changeValue.shareData.shareNum.ToString()+tmpStr;
			}
			else if(changeValue.changeNum<0)
			{
				tmpStr = string.Format ("(<color=#ff0000>{0}</color>)",changeValue.changeNum);
				_lbsharenum.text=changeValue.shareData.shareNum.ToString()+tmpStr;
			}
			else
			{
				_lbsharenum.text=changeValue.shareData.shareNum.ToString();
			}			

			// 负债文字显示不同颜色
			if (changeValue.changeMoney > 0)
			{
				tmpStr = string.Format ("(<color=#00ff00>+{0}</color>)",changeValue.changeMoney);		
				_lbchangeMoney.text = tmpStr;
			}
			else if(changeValue.changeMoney<0)
			{
				tmpStr = string.Format ("(<color=#ff0000>{0}</color>)",changeValue.changeMoney);
				_lbchangeMoney.text = tmpStr;
			}
			else
			{
				_lbchangeMoney.text = changeValue.changeMoney.ToString ();
			}


			_changeVo = changeValue;
		}

		public ChangeShareVo ChangeVo
		{
			get
			{
				return _changeVo;
			}
		}

		public void SetActiveEx(bool value)
		{
			_gameObj.SetActiveEx (value);
		}

		private ChangeShareVo _changeVo;

		private Text _lbcode;
		private Text _lbsharename;
		private Text _lbsharepay;
		private Text _lbsharenum;
		private Text _lbchangeMoney;
		private Button _btnselect;

		private GameObject _gameObj;

		class Layout
		{
			public static string lb_shareCode="changecodebuy";
			public static string lb_sharename="changecodebuy/sharename";
			public static string lb_sharepay="changecodebuy/sharepay";
			public static string lb_sharenum="changecodebuy/sharenum";
			public static string lb_changeMoney = "changecodebuy/changemoney";
		}
	}



}

