using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIBorrowRecordItem
	{
		public UIBorrowRecordItem (GameObject go)
		{
			_lbTitleNum = go.GetComponentEx<Text> (Layout.lb_titleNum);
			_lbTitleTxt = go.GetComponentEx<Text> (Layout.lb_titleTxt);

			_lbBankBorrow = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowbank,Layout.lb_borrow));
			_lbBankDebt = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowbank,Layout.lb_debt));
			_lbBankRate = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowbank,Layout.lb_rate));
			_lbBankTitle = go.GetComponentEx<Text> (Layout.lb_borrowbank);
			_initBankVec3 = _lbBankTitle.transform.localPosition;


			_lbCardBorrow = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowcard,Layout.lb_borrow));
			_lbCardDebt = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowcard,Layout.lb_debt));
			_lbCardRate = go.GetComponentEx<Text> (string.Format("{0}/{1}",Layout.lb_borrowcard,Layout.lb_rate));
			_lbCardTitle = go.GetComponentEx<Text> (Layout.lb_borrowcard);
			_initCardVec3 = _lbCardTitle.transform.localPosition;
		
		}

		public void Refresh(BorrowVo value)
		{			
			_lbTitleTxt.text = string.Format ("第{0}次贷款:",value.times);
			_lbTitleNum.text =(value.bankborrow+value.cardborrow).ToString();

			var showBank = true;
			var showCard = true;
			if (value.bankborrow > 0)
			{
				_lbBankTitle.SetActiveEx (true);
				_lbBankBorrow.text = value.bankborrow.ToString ();
				_lbBankDebt.text = value.bankdebt.ToString ();
				_lbBankRate.text = value.bankRate;

			}
			else
			{
				_lbBankTitle.SetActiveEx (false);
				showBank = false;
			}
		
			if (value.cardborrow > 0)
			{
				_lbCardTitle.SetActiveEx (true);
				_lbCardBorrow.text = value.cardborrow.ToString ();
				_lbCardDebt.text = value.carddebt.ToString ();
				_lbCardRate.text = value.cardRate;

			}
			else
			{
				_lbCardTitle.SetActiveEx (false);
				showCard = false;
			}

			if (showBank == false)
			{
				_lbCardTitle.transform.localPosition = new Vector3 (_initCardVec3.x,0,_initCardVec3.z);
			} 
			else
			{					
				_lbCardTitle.transform.localPosition = _initCardVec3;
			}

			if (showCard == false)
			{
				_lbBankTitle.transform.localPosition = new Vector3 (_initBankVec3.x,0,_initBankVec3.z);
			}
			else
			{
				_lbBankTitle.transform.localPosition = _initBankVec3;	
			}
		}

		private Text _lbTitleTxt;
		private Text _lbTitleNum;

		private Text _lbBankBorrow;
		private Text _lbBankDebt;
		private Text _lbBankRate;
		private Text _lbBankTitle;

		private Text _lbCardBorrow;
		private Text _lbCardDebt;
		private Text _lbCardRate;
		private Text _lbCardTitle;

		private Vector3 _initBankVec3;
		private Vector3 _initCardVec3;




		class Layout
		{
			public static string lb_titleTxt="titletext";
			public static string lb_titleNum="titlenum";
			public static string lb_borrowbank="borrowbank";
			public static string lb_borrowcard="borrowcard";

			public static string lb_borrow="borrownum";
			public static string lb_debt="debtnum";
			public static string lb_rate="ratenum";

		}
	}
}

