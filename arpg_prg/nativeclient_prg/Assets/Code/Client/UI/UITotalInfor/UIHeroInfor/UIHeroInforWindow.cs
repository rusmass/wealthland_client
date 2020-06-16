using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIHeroInforWindow:UIWindow<UIHeroInforWindow,UIHeroInforWindowController>
	{
		public UIHeroInforWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			var img = go.GetComponentEx<Image> (Layout.img_hero);
			_heroImg = new UIImageDisplay (img);

			lb_name = go.GetComponentEx<Text> (Layout.lb_name+Layout.lb_txt);
			lb_age = go.GetComponentEx<Text> (Layout.lb_age+Layout.lb_txt);
			lb_career = go.GetComponentEx<Text> (Layout.lb_career+Layout.lb_txt);
			lb_moeny = go.GetComponentEx<Text> (Layout.lb_money+Layout.lb_txt);
			lb_balance = go.GetComponentEx<Text> (Layout.lb_balance+Layout.lb_txt);
			lb_debt = go.GetComponentEx<Text> (Layout.lb_debt+Layout.lb_txt);

			lb_nonIncome = go.GetComponentEx<Text> (Layout.lb_nonlabincome+Layout.lb_txt);
			lb_totalIncome = go.GetComponentEx<Text> (Layout.lb_totalincome+Layout.lb_txt);
			lb_timeScore = go.GetComponentEx<Text> (Layout.lb_timescore+Layout.lb_txt);
			lb_totalPaymeny = go.GetComponentEx<Text> (Layout.lb_payment+Layout.lb_txt);
			lb_qualityScore = go.GetComponentEx<Text> (Layout.lb_quality+Layout.lb_txt);
			lb_totalmoney = go.GetComponentEx<Text> (Layout.lb_totolmoney+Layout.lb_txt);

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);

		}

		protected override void _OnShow ()
		{
//			MoveIn ();

			if (null != _controller)
			{
				if(null !=_controller.playerInfor)
				{
					
					var player = _controller.playerInfor;
					_heroImg.Load (player.playerImgPath);

					lb_name.text = player.playerName;
					lb_age.text = player.totalAge.ToString();
					lb_career.text = player.career;
//					var tmpstr = HandleStringTool.HandleMoneyTostring (player.totalMoney);
					var tmpstr=player.totalMoney.ToString();
					lb_moeny.text =string.Format(_greenText, tmpstr);

					var tmpBalance = 0f;
					if (GameModel.GetInstance.isPlayNet == false)
					{
						if (player.capitalList.Count > 0)
						{
							tmpBalance = player.capitalList[player.capitalList.Count-1].captical;
						}
					}
					else
					{
						tmpBalance = player.netTotalBalanceMoney;
					}


					lb_balance.text =string.Format(_greenText, HandleStringTool.HandleMoneyTostring (tmpBalance));
					//HandleStringTool.HandleMoneyTostring (player.GetTotalDebt ());
					var totaldebt = 0;
					if (GameModel.GetInstance.isPlayNet == false)
					{
						totaldebt = player.GetTotalDebt ();
					}
					else
					{
						totaldebt = player.netTotalDebtMoney;
					}
					lb_debt.text = totaldebt.ToString();



					lb_nonIncome.text =string.Format(_greenText, player.CurrentIncome.ToString ());
					lb_totalIncome.text = string.Format(_greenText, (player.CurrentIncome+player.innerFlowMoney+player.cashFlow).ToString());
					lb_timeScore.text = player.timeScore.ToString();
					lb_qualityScore.text = player.qualityScore.ToString();

					if (player.isEnterInner == false)
					{
						lb_totalPaymeny.text = HandleStringTool.HandleMoneyTostring(player.MonthPayment);
					}
					else
					{
						lb_totalPaymeny.text = HandleStringTool.HandleMoneyTostring(player.MonthPayment);
					}

					lb_totalmoney.text =string.Format(_greenText,(player.CurrentIncome+player.innerFlowMoney+player.cashFlow-player.MonthPayment).ToString());


					if (player.isEnterInner == false)
					{
						lb_balance.SetActiveEx (true);
						lb_debt.SetActiveEx (true);

						lb_nonIncome.SetActiveEx (true);
						lb_totalIncome.SetActiveEx (true);
						lb_debt.SetActiveEx (true);
					}
					else
					{
						lb_balance.SetActiveEx (false);
						lb_debt.SetActiveEx (false);

						lb_nonIncome.SetActiveEx (false);
						lb_totalIncome.SetActiveEx (false);
						lb_debt.SetActiveEx (false);
					}


				}
			}
		}

		protected override void _OnHide ()
		{
			
		}

		protected override void _Dispose ()
		{
			if (null != _heroImg)
			{
				_heroImg.Dispose ();
			}
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
			_isOut = false;
			_selfTransform.localPosition = _outerPosition;
			var sequece = DOTween.Sequence ();
			sequece.Append (_selfTransform.DOLocalMove(_initPosition,1f));
			Console.WriteLine ("移动进来");
		}

		private UIImageDisplay _heroImg;

		private Text lb_name;
		private Text lb_age;
		private Text lb_career;
		private Text lb_moeny;
		private Text lb_balance;
		private Text lb_debt;


		private Text lb_nonIncome;
		private Text lb_totalIncome;
		private Text lb_timeScore;
		private Text lb_totalPaymeny;
		private Text lb_qualityScore;
		private Text lb_totalmoney;


		private Vector3 _outerPosition;
		private Vector3 _initPosition;

		private Transform _selfTransform;

		/// <summary>
		/// 判断是不是已经移动出去，
		/// </summary>
		private bool _isOut=false;




		//private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

	}
}

