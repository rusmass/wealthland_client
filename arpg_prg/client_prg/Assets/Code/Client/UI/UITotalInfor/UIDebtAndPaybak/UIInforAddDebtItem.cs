using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIInforAddDebtItem
	{
		public UIInforAddDebtItem (GameObject go)
		{
			_lbTitleTxt = go.GetComponent<Text>();
			_lbBorrow = go.GetComponentEx<Text> (Layout.lb_borrow);
		}

		public void Refresh(PaybackVo value)
		{			
			_lbTitleTxt.text = value.title;

			if(UIDebtAndPaybackController.isDebt==true)
			{
				_lbBorrow.text = value.borrow.ToString ();
			}
			else
			{
				_lbBorrow.text =string.Format("-{0}", Mathf.Abs(value.debt).ToString ()) ;
			}
//			_paybackVo = value;

		}

//		private PaybackVo _paybackVo;
		private Text _lbTitleTxt;
		private Text _lbBorrow;

		class Layout
		{			
			public static string lb_borrow="borrowmoney";
		}
	}
}

