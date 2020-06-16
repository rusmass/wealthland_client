using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIBorrowBasePayItem
	{
		public UIBorrowBasePayItem (GameObject go)
		{
			_lbTitleTxt = go.GetComponent<Text>();
			_lbBorrow = go.GetComponentEx<Text> (Layout.lb_borrow);
			_lbDebt = go.GetComponentEx<Text> (Layout.lb_debt);
			_select = go.GetComponentEx<Toggle> (Layout.toggle_select);

			_select.onValueChanged.AddListener (_OnChnageValue);

		}


		public void Refresh(PaybackVo value)
		{			
			_lbTitleTxt.text = value.title;
			_lbBorrow.text = value.borrow.ToString ();
			_lbDebt.text = value.debt.ToString ();
			_paybackVo = value;

		}

		private void _OnChnageValue(bool value)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_paybackVo.isSeleted = value;

			if (null == _controller)
			{
				_controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
			}
			_controller.UpdatePayBackMoney ();
		}

		private UIBorrowWindowController _controller;

		private PaybackVo _paybackVo;

		public void Dispose()
		{
			_controller = null;
			_select.onValueChanged.RemoveListener (_OnChnageValue);
		}

		private Text _lbTitleTxt;


		private Text _lbBorrow;
		private Text _lbDebt;
		private Toggle _select;

		class Layout
		{			
			public static string lb_borrow="borrowmoney";
			public static string lb_debt="debt";
			public static string toggle_select="Toggle";

		}
	}
}

