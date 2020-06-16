using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UICheckOutWindow
	{
		private void _InitBottom(GameObject go)
		{
		    lb_income = go.GetComponentEx<Text> (Layout.lb_income);
			lb_checkOut = go.GetComponentEx<Text> (Layout.lb_checkout);
			lb_payment = go.GetComponentEx<Text> (Layout.lb_payment);
		}


		private void _OnShowBottom()
		{
			if (null != _controller.playerInfor) 
			{
				showBottomData (_controller.playerInfor);

			}
		}

		private void _OnHideBottom()
		{
			
		}


		private void showBottomData(PlayerInfo player)
		{
			var totolcome=player.cashFlow + player.totalIncome + player.innerFlowMoney;
			var totalpay = player.MonthPayment;

			Console.WriteLine ("totalPay,"+player.MonthPayment);

			lb_income.text = totolcome.ToString();
			lb_payment.text = totalpay.ToString();

			var checkoutNum =totolcome-totalpay ;
			lb_checkOut.text =checkoutNum.ToString ();

		}


		private Text lb_income;
		private Text lb_checkOut;
		private Text lb_payment;
	}
}

