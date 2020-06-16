using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;


namespace Client.UI
{
	public class UIShareChangeItem:IDisposable
	{
		public static UIShareChangeItem selectedText;
		public static ChangeShareVo selectedTextVO;

		private Color _oldColor;
		private Color _oldShareNameColor;
		private Color _oldSharePayColor;
		private Color _oldCurrentpriseColor;
		private Color _oldShareNumColor;
		private Color _oldChangeMoneyColor;

		public UIShareChangeItem (GameObject go)
		{
			_lbcode = go.GetComponentEx<Text> (go.name);
			_lbsharename = go.GetComponentEx<Text> (Layout.lb_sharename);
			_lbsharepay = go.GetComponentEx<Text> (Layout.lb_sharepay);
			_lbcurrentprise = go.GetComponentEx<Text> (Layout.lb_currentPrice);
			_lbsharenum = go.GetComponentEx<Text> (Layout.lb_sharenum);
			_lbchangeMoney = go.GetComponentEx<Text> (Layout.lb_changeMoney);
			_btnselect = go.GetComponentEx<Button> (Layout.btn_select);
			EventTriggerListener.Get (_btnselect.gameObject).onClick += _SelectHandler;

			_oldColor = _lbcode.color;
			_oldShareNameColor = _lbsharename.color;
			_oldSharePayColor = _lbsharepay.color;
			_oldCurrentpriseColor = _lbcurrentprise.color;
			_oldShareNumColor = _lbsharenum.color;
			_oldChangeMoneyColor = _lbchangeMoney.color;
		}

		public void Refresh(ChangeShareVo changeValue)
		{			
			_lbcode.text=changeValue.shareData.ticketCode;
			_lbsharename.text=changeValue.shareData.ticketName;
			_lbsharepay.text=(Mathf.Abs(changeValue.shareData.payment)).ToString();
			_lbcurrentprise.text=Mathf.Abs(changeValue.saleMoney).ToString();

			var tmpStr = "";
			if(changeValue.changeNum>0)
			{
				tmpStr = string.Format ("(<color=#00b050>+{0}</color>)",changeValue.changeNum);			
				_lbsharenum.text=changeValue.shareData.shareNum.ToString()+tmpStr;
			}
			else if(changeValue.changeNum<0)
			{
				tmpStr = string.Format ("(<color=#e53232>{0}</color>)",changeValue.changeNum);
				_lbsharenum.text=changeValue.shareData.shareNum.ToString()+tmpStr;
			}
			else
			{
				_lbsharenum.text=changeValue.shareData.shareNum.ToString();
			}			

			if (changeValue.changeMoney > 0)
			{
				tmpStr = string.Format ("(<color=#00b050>+{0}</color>)",changeValue.changeMoney);
				_lbchangeMoney.text = tmpStr;
			}
			else if(changeValue.changeMoney<0)
			{
				tmpStr = string.Format ("(<color=#e53232>+{0}</color>)",changeValue.changeMoney);		
				_lbchangeMoney.text = tmpStr;
			}
			else
			{
				_lbchangeMoney.text = changeValue.changeMoney.ToString ();
			}

			_changeVo = changeValue;

			if (null != selectedTextVO)
			{
				if (_changeVo.shareData.id == selectedTextVO.shareData.id)
				{
					_SetSelectColor ();
				}
				else
				{
					SetSelfColor ();
				}
			}
		}

		//4000 点了   4000 

		private ChangeShareVo _changeVo;

		private void _SelectHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if(null != selectedText)
			{
				selectedText.SetSelfColor ();
				//selectedText.ChangeVo.changeNum = 0;
			    //selectedText.ChangeVo.changeMoney = 0;
				selectedText.Refresh (selectedText.ChangeVo);
			}

			_SetSelectColor ();

							
			if (selectedText != this)
			{
				selectedText = this;
			}else 
			{
				selectedText.SetSelfColor ();
				selectedText = null;
			}
		}

		// 初始是选择的状态
		public void InitSelected()
		{
			_SelectHandler (null);
		}

		public ChangeShareVo ChangeVo
		{
			get
			{
				return _changeVo;
			}
		}

		public void SetSelfColor()
		{
			_lbcode.color = _oldColor ;
			_lbsharename.color = _oldShareNameColor ;
			_lbsharepay.color = _oldSharePayColor ;
			_lbcurrentprise.color = _oldCurrentpriseColor ;
			_lbsharenum.color =  _oldShareNumColor ;
			_lbchangeMoney.color =  _oldChangeMoneyColor ;
		}

		private void _SetSelectColor()
		{
			_lbcode.color = Color.blue;
			_lbsharename.color = Color.blue;
			_lbsharepay.color =  Color.blue;
			_lbcurrentprise.color =  Color.blue;
			_lbsharenum.color =  Color.blue;
			_lbchangeMoney.color =  Color.blue;
		}

		public void Dispose()
		{
			EventTriggerListener.Get (_btnselect.gameObject).onClick -= _SelectHandler;
		}

		private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

		private Text _lbcode;
		private Text _lbsharename;
		private Text _lbsharepay;
		private Text _lbcurrentprise;
		private Text _lbsharenum;
		private Text _lbchangeMoney;
		private Button _btnselect;

		class Layout
		{
			public static string lb_sharename="sharename";
			public static string lb_sharepay="sharepay";
			public static string lb_currentPrice="sharecurrentmoney";
			public static string lb_sharenum="sharenum";
			public static string lb_changeMoney = "changemoney";
			public static string btn_select = "selectbtn";
		}
	}


}

