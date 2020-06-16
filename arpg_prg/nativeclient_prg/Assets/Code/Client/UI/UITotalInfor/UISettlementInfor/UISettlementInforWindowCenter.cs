using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UISettlementInforWindow
	{
		
		private void _OnInitCenter(GameObject go)
		{
			lb_income = go.GetComponentEx<Text> (Layout.lb_income);
			lb_payment = go.GetComponentEx<Text> (Layout.lb_payment);
			lb_checkout = go.GetComponentEx<Text> (Layout.lb_settlement);

			var imgIncome = go.GetComponentEx<Image> (Layout.img_income);
			img_income = new UIImageDisplay (imgIncome);

			var imgPay = go.GetComponentEx<Image> (Layout.img_payment);
			img_payment = new UIImageDisplay (imgPay);

			var imgcheck = go.GetComponentEx<Image> (Layout.img_checkout);
			img_checkout = new UIImageDisplay (imgcheck);

			img_tmpAdd = go.GetComponentEx<Image> (Layout.img_add);
			img_add = new UIImageDisplay (img_tmpAdd);


			btn_checkRecord = go.GetComponentEx<Button> (Layout.btn_record);

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);

		}

		private void _OnShowCenter()
		{
//			MoveIn ();

			EventTriggerListener.Get (btn_checkRecord.gameObject).onClick += _OnShowStateRecord;

			//btn_checkRecord.SetActiveEx(false);

			var player = _controller.playerInfor;

			if (null != player)
			{
				if (player.isEnterInner == true)
				{
//					img_tmpAdd.rectTransform.sizeDelta = new Vector2 (addWithSize, addWithSize);
//					img_income.Load (incomeInnerPath);
//					img_payment.Load (payInnerPath);
//					img_checkout.Load (checkoutInnerPath);
//					img_add.Load (addInnerPath);

					var totalIncome = player.innerFlowMoney;
					if (GameModel.GetInstance.isPlayNet == true)
					{
						totalIncome = player.netInforCheckVo.totalIncome;
					}
					lb_income.text =string.Format(_greenText, HandleStringTool.HandleMoneyTostring(totalIncome));

					var totalDebt = player.MonthPayment;
					if (GameModel.GetInstance.isPlayNet == true)
					{
						totalDebt = player.netInforCheckVo.totalPay;
					}
					lb_payment.text =string.Format(_redText, HandleStringTool.HandleMoneyTostring(totalDebt));

					//player.CurrentIncome + player.innerFlowMoney
					var tmpIncome=totalIncome-totalDebt;
					if (GameModel.GetInstance.isPlayNet == true)
					{
						tmpIncome = player.netCheckDayNum;
					}		

					lb_checkout.text =string.Format(_greenText, HandleStringTool.HandleMoneyTostring(tmpIncome));
				}
				else
				{
					var totalIncome = (player.CurrentIncome + player.cashFlow);
					if (GameModel.GetInstance.isPlayNet == true)
					{
						totalIncome = player.netInforCheckVo.totalIncome;
					}
					lb_income.text =string.Format(_greenText, totalIncome.ToString());


					var totalDebt =  player.MonthPayment;
					if (GameModel.GetInstance.isPlayNet == true)
					{
						totalDebt = player.netInforCheckVo.totalPay;
					}
					lb_payment.text =string.Format(_redText ,totalDebt.ToString());

					//player.CurrentIncome + player.cashFlow - player.MonthPayment
					lb_checkout.text =string.Format(_greenText,(totalIncome-totalDebt).ToString());

				}
			}
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_checkRecord.gameObject).onClick -= _OnShowStateRecord;
		}

		private void _OnDisoseCenter()
		{
			if(null !=img_income)
			{
				img_income.Dispose ();
			}

			if(null !=img_payment)
			{
				img_payment.Dispose ();
			
			}

			if(null !=img_checkout)
			{
				img_checkout.Dispose ();
			}

			if (null != img_add)
			{
				
				img_add.Dispose ();
			}

		}

		private void _OnShowStateRecord(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("显示结算记录界面");
		}


		public void MoveOut()
		{
			if (_isOut == true)
			{
				return;
			}

			_isOut = true;
			_selfTransform.localPosition = _initPosition;
			var sequence = DOTween.Sequence();
			sequence.Append (_selfTransform.DOLocalMove(_outerPosition,1f));
			Console.WriteLine ("移动出去");


		}

		public void MoveIn()
		{
			Console.WriteLine ("结算界面进入");

			_isOut = false;
			_selfTransform.localPosition = _outerPosition;
			var sequece = DOTween.Sequence ();
			sequece.Append (_selfTransform.DOLocalMove(_initPosition,1f));
			Console.WriteLine ("移动进来");
		}

		private Vector3 _outerPosition;
		private Vector3 _initPosition;

		private Transform _selfTransform;

		/// <summary>
		/// 判断是不是已经移动出去，
		/// </summary>
		private bool _isOut=false;




		private Text lb_income;
		private Text lb_payment;
		private Text lb_checkout;

		private UIImageDisplay img_income;
		private UIImageDisplay img_payment;
		private UIImageDisplay img_checkout;
		private UIImageDisplay img_add;

		private Image img_tmpAdd;
		//private float addWithSize=47;

		private Button btn_checkRecord;

		private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

		private const string incomeInnerPath="share/atlas/battle/totalinfor/settleinfor/settlement_wealth_circle_initial_cash_flow.ab";
		private const string payInnerPath="share/atlas/battle/totalinfor/settleinfor/settlement_wealth_circle_increase_liquidity.ab";
		private const string checkoutInnerPath="share/atlas/battle/totalinfor/settleinfor/settlement_wealth_circle_settlement_date.ab";
		private const string addInnerPath="share/atlas/battle/totalinfor/settleinfor/settlement_wealth_circle_plus.ab";

	}
}

