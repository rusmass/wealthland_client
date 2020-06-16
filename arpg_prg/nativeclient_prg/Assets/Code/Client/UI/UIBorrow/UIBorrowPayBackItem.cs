using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    /// <summary>
    /// 借贷界面，新增还款的数据单元
    /// </summary>
	public class UIBorrowPayBackItem:IDisposable
	{
		public UIBorrowPayBackItem (GameObject go)
		{
			_lbTitleTxt = go.GetComponent<Text>();

			_lbBorrow = go.GetComponentEx<Text> (Layout.lb_borrow);
			_lbDebt = go.GetComponentEx<Text> (Layout.lb_debt);
			_select = go.GetComponentEx<Toggle> (Layout.toggle_select);

			_select.onValueChanged.AddListener (_OnChnageValue);
		}

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="value"></param>
		public void Refresh(PaybackVo value)
		{			
			_lbTitleTxt.text = value.title;
			_lbBorrow.text = value.borrow.ToString ();
			_lbDebt.text = value.debt.ToString ();
			_paybackVo = value;

			if (value.isSeleted == false)
			{
				_select.isOn = false;
			}
			else
			{
				_select.isOn = true;
			}

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

