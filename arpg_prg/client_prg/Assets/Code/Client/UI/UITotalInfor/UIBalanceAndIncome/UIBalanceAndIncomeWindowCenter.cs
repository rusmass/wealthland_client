using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;


namespace Client.UI
{
	public partial class UIBalanceAndIncomeWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			btn_balance = go.GetComponentEx<Button> (Layout.btn_balance);
			btn_income = go.GetComponentEx<Button> (Layout.btn_income);
			img_btnbalancebg = go.GetComponentEx<Image> (Layout.img_btnBalancebg);
			img_btnincomebg = go.GetComponentEx<Image> (Layout.img_btnIncomebg);

//			lb_income = go.GetComponentEx<Text> (Layout.lb_cash);
			lb_nonIncome = go.GetComponentEx<Text> (Layout.lb_nonincome);
			lb_totalIncome = go.GetComponentEx<Text> (Layout.lb_totalincome);

//			lb_incomeindex = go.GetComponentEx<Text> (Layout.lb_cashIndex);
			lb_incomeName = go.GetComponentEx<Text> (Layout.lb_cashName);
			lb_incomeNum = go.GetComponentEx<Text> (Layout.lb_cashNum);

			img_nonIncomeItem = go.GetComponentEx<Image> (Layout.img_nonincometipitem);
			img_nonIncomeInforItem = go.GetComponentEx<Image> (Layout.img_nonincomeinforitem);

			transform_incomeScroll = go.DeepFindEx (Layout.transform_incomescroll);

			content_balance = go.DeepFindEx (Layout.content_balance);
			content_income = go.DeepFindEx (Layout.content_income);

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);
		}

		private void _OnShowCenter()
		{
			if (_controller.GetBalanceList ().Count > 0)
			{
				content_balance.SetActiveEx (true);
				content_income.SetActiveEx (false);
				img_btnbalancebg.SetActiveEx (true);
				img_btnincomebg.SetActiveEx (false);

				_CreateBalanceWrapGrid (img_nonIncomeInforItem.gameObject);
			}
			else
			{
				_noBalance = true;
				content_balance.SetActiveEx (false);
				content_income.SetActiveEx (true);
				img_btnbalancebg.SetActiveEx (false);
				img_btnincomebg.SetActiveEx (true);			
			}

			Console.WriteLine ("aaaaaaa"+_controller.GetIncomeList ().Count.ToString());
			if (_controller.GetIncomeList ().Count <= 0)
			{
				Console.WriteLine ("不显示非劳务收入啊啊啊啊啊啊啊啊啊");
//				_noIncome = true;
				transform_incomeScroll.SetActiveEx (false);
			}
			else
			{
				_CreateIncomeWrapGrid (img_nonIncomeItem.gameObject);
			}


			var playerInfor = _controller.playerInfor;

			var nonIncome = playerInfor.CurrentIncome;
			if (GameModel.GetInstance.isPlayNet == true)
			{
				nonIncome = playerInfor.netInforBalanceAndIncome.totalNonLaborIncome;
			}

			lb_nonIncome.text =string.Format(_greenText , nonIncome.ToString());

			var totalIncome = playerInfor.totalIncome + playerInfor.cashFlow;
			if (GameModel.GetInstance.isPlayNet == true)
			{
				totalIncome = playerInfor.netInforBalanceAndIncome.totalIncome;
			}
			lb_totalIncome.text =string.Format(_greenText , totalIncome.ToString ());



//			lb_incomeindex.text = "1";
			var tmpCashName = "工资";
			if (GameModel.GetInstance.isPlayNet == true)
			{
				tmpCashName = playerInfor.netInforBalanceAndIncome.laborTxt;
			}
			lb_incomeName.text = tmpCashName;

			var tmpCashNum = playerInfor.cashFlow;
			if (GameModel.GetInstance.isPlayNet == false)
			{
				tmpCashNum = playerInfor.netInforBalanceAndIncome.laoorMoney;
			}

			lb_incomeNum.text =string.Format(_greenText , tmpCashNum.ToString ());

			EventTriggerListener.Get (btn_balance.gameObject).onClick += _OnShowBalanceBoard;
			EventTriggerListener.Get (btn_income.gameObject).onClick += _OnShowIncomeBoard;

		}

		private void _OnHideCenter()
		{
			
			EventTriggerListener.Get (btn_balance.gameObject).onClick -= _OnShowBalanceBoard;
			EventTriggerListener.Get (btn_income.gameObject).onClick -= _OnShowIncomeBoard;

		}

		private void _OnDisposeCenter()
		{
//			if (null != _balanceWrapGrid)
//			{
//				_balanceWrapGrid.Dispose ();
//				_balanceWrapGrid.OnRefreshCell-=_OnRefreshBalanceCell;
//			}

			if (null != _incomeWrapGrid)
			{
				_incomeWrapGrid.Dispose ();
				_incomeWrapGrid.OnRefreshCell-=_OnRefreshIncomeCell;
			}
		}


		private void _CreateBalanceWrapGrid(GameObject go)
		{
			var items = _controller.GetBalanceList();

			if (isInitBalance == false)
			{
				isInitBalance = true;

				_balanceList.Clear ();

				for (var i = 0; i < items.Count; i++)
				{
					if (i == 0)
					{
						var cell = new UIBalanceInforItem (go); 
						cell.Refresh (items[i]);
						_balanceList.Add (cell);
					}
					else
					{
						var tmpObj = go.CloneEx ();
						tmpObj.transform.SetParent (go.transform.parent);
						tmpObj.transform.localPosition = go.transform.localPosition;
						tmpObj.transform.localScale = Vector3.one;
						var cell = new UIBalanceInforItem (tmpObj);
						cell.Refresh (items[i]);
						_balanceList.Add (cell);
					}				

				}

			}

			return; 

//			if (null == _balanceWrapGrid)
//			{
//				_balanceWrapGrid = new UIWrapGrid(go, items.Count);
//				for (int i = 0; i < _balanceWrapGrid.Cells.Length; ++i)
//				{
//					var cell = _balanceWrapGrid.Cells[i];
//					cell.DisplayObject = new UIBalanceInforItem(cell.GetTransform().gameObject);
//				}
//				_balanceWrapGrid.OnRefreshCell += _OnRefreshBalanceCell;
//			}
//			else
//			{
//				_balanceWrapGrid.GridSize = items.Count;
//			}		
//			_balanceWrapGrid.Refresh();
		}

		private void _OnRefreshBalanceCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetBalanceByIndex(index);
			var display = cell.DisplayObject as UIBalanceInforItem;
			display.Refresh(value);
		}


		private void _CreateIncomeWrapGrid(GameObject go)
		{
			var items = _controller.GetIncomeList();

			if (null == _incomeWrapGrid)
			{
				_incomeWrapGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _incomeWrapGrid.Cells.Length; ++i)
				{
					var cell = _incomeWrapGrid.Cells[i];
					cell.DisplayObject = new UIIncomeRecordInforItem(cell.GetTransform().gameObject);
				}

				_incomeWrapGrid.OnRefreshCell += _OnRefreshIncomeCell;
			}
			else
			{
				_incomeWrapGrid.GridSize = items.Count;
			}

		
			_incomeWrapGrid.Refresh();
		}

		private void _OnRefreshIncomeCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetIncomeByIndex(index);
			var display = cell.DisplayObject as UIIncomeRecordInforItem;
			display.Refresh(value);
		}


	


		private void _OnShowBalanceBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (_noBalance == true)
			{
				MessageHint.Show (string.Format("{0}暂无资产",_controller.playerInfor.playerName),null,true);
				return;
			}

			content_balance.SetActiveEx (true);
			content_income.SetActiveEx (false);
			img_btnbalancebg.SetActiveEx (true);
			img_btnincomebg.SetActiveEx (false);
		}

		private void _OnShowIncomeBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			content_balance.SetActiveEx (false);
			content_income.SetActiveEx (true);
			img_btnbalancebg.SetActiveEx (false);
			img_btnincomebg.SetActiveEx (true);
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

		private Vector3 _outerPosition;
		private Vector3 _initPosition;

		private Transform _selfTransform;

		/// <summary>
		/// 判断是不是已经移动出去，
		/// </summary>
		private bool _isOut=false;


		private Button btn_balance;
		private Button btn_income;

		private Image img_btnbalancebg;
		private Image img_btnincomebg;

//		private Text lb_income;
		private Text lb_nonIncome;
		private Text lb_totalIncome;

//		private Text lb_incomeindex;
		private Text lb_incomeNum;
		private Text lb_incomeName;

		private Image img_nonIncomeItem;
		private Image img_nonIncomeInforItem;

		private Transform transform_incomeScroll;

		private Transform content_income;
		private Transform content_balance;

		private bool _noBalance=false;
//		private bool _noIncome=false;

		private bool isInitBalance=false;
		private List<UIBalanceInforItem> _balanceList = new List<UIBalanceInforItem> ();


//		private UIWrapGrid _balanceWrapGrid;
		private UIWrapGrid _incomeWrapGrid;

		private string _redText="<color=#e53232>{0}</color>";
		private string _greenText="<color=#00b050>{0}</color>";

	}
}

