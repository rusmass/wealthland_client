using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIDebtAndPayback
	{
		private void _OnInitCenter(GameObject go)
		{
			btn_debt = go.GetComponentEx<Button> (Layout.btn_debt);
			btn_pay = go.GetComponentEx<Button> (Layout.btn_pay);

			content_basedebt = go.DeepFindEx (Layout.content_basedebt);
			content_adddebt = go.DeepFindEx (Layout.content_adddebt);

			content_basepay = go.DeepFindEx (Layout.content_basepay);
			content_addpay = go.DeepFindEx (Layout.content_addpay);

			lb_debtitem = go.GetComponentEx<Text> (Layout.lb_baseItem);
			lb_payitem = go.GetComponentEx<Text> (Layout.lb_paybackitem);

//			img_basedebt = go.GetComponentEx<GameObject> (Layout.img_basedebtbg);
//			img_adddebt = go.GetComponentEx<GameObject> (Layout.img_newdebtbg);

			img_basebtnbg = go.GetComponentEx<Image> (Layout.img_basebtnimg);
			img_addbtnbg = go.GetComponentEx<Image> (Layout.img_addbtnimg);

			btn_payback = go.GetComponentEx<Button> (Layout.btn_payback);

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);

		}

		private void _OnShowCenter()
		{
//			MoveIn ();

			UIDebtAndPaybackController.isDebt = true;
			_OnSelecDebtHandler (null);

			if (_controller.playerInfor != PlayerManager.Instance.HostPlayerInfo)
			{
				btn_payback.SetActiveEx (false);				
			}


			EventTriggerListener.Get (btn_debt.gameObject).onClick += _OnSelecDebtHandler;
			EventTriggerListener.Get (btn_pay.gameObject).onClick += _OnSelectPayHandler;
			EventTriggerListener.Get(btn_payback.gameObject).onClick+=_ShowBorrowWindow;


			if (_controller.GetBasePayBackList ().Count <= 0)
			{
//				img_basedebt.SetActiveEx (false);
//				var tmpPos =  img_adddebt.transform.localPosition;
//				img_adddebt.transform.localPosition = new Vector3 (0,tmpPos.y,tmpPos.z);
			}

			if(_controller.GetPaybackList().Count<=0)
			{
//				img_adddebt.SetActiveEx (false);
//				Console.WriteLine ("aaaaaaaaaaaaaaaa,"+img_basedebt.ToString());
//				var tmpPos =  img_basedebt.transform.localPosition;
//				img_basedebt.transform.localPosition = new Vector3 (0,tmpPos.y,tmpPos.z);
			}
			img_addbtnbg.SetActiveEx (false);
		}


		private void _ShowBorrowWindow(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
			controller.playerInfor = _controller.playerInfor;		
			controller.isInitPayback = true;
			controller.setVisible (true);
			controller.SetBlackBg ();			
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_debt.gameObject).onClick -= _OnSelecDebtHandler;
			EventTriggerListener.Get (btn_pay.gameObject).onClick -= _OnSelectPayHandler;
			EventTriggerListener.Get(btn_payback.gameObject).onClick-=_ShowBorrowWindow;

			if (null != _controller)
			{
				if (null != _controller.playerInfor)
				{
					_controller.playerInfor.RemoveBasePayVoForShow ();
				}
			}

		}

		private void _OnSelecDebtHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			content_basedebt.SetActiveEx (true);
			content_adddebt.SetActiveEx (true);

			content_basepay.SetActiveEx (false);
			content_addpay.SetActiveEx (false);

			img_basebtnbg.SetActiveEx(true);
			img_addbtnbg.SetActiveEx (false);

			UIDebtAndPaybackController.isDebt = true;

			_CreateBasePayGrid (lb_debtitem.gameObject);
			_CreateAddPayGrid (lb_payitem.gameObject);
		}

		private void _OnSelectPayHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			content_basedebt.SetActiveEx (false);
			content_adddebt.SetActiveEx (false);

			content_basepay.SetActiveEx (true);
			content_addpay.SetActiveEx (true);

			img_basebtnbg.SetActiveEx(false);
			img_addbtnbg.SetActiveEx (true);

			UIDebtAndPaybackController.isDebt = false;
			_CreateBasePayGrid (lb_debtitem.gameObject);
			_CreateAddPayGrid (lb_payitem.gameObject);
		}

		private void _CreateBasePayGrid(GameObject go)
		{
			var items = _controller.GetBasePayBackList();

			if (items.Count <= 0)
			{
				go.SetActive (false);
				return;
			}
			else
			{
				go.SetActive (true);
			}


			if (null == _baseDebtGrid)
			{
				_baseDebtGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _baseDebtGrid.Cells.Length; ++i)
				{
					var cell = _baseDebtGrid.Cells[i];
					cell.DisplayObject = new UIInforBaseDebtItem(cell.GetTransform().gameObject);
				}

				_baseDebtGrid.OnRefreshCell += _OnRefreshBasePayCell;				
			}
			else
			{
				_baseDebtGrid.GridSize = items.Count;

			}
			_baseDebtGrid.Refresh();
		}

		private void _CreateAddPayGrid(GameObject go)
		{
			var items = _controller.GetPaybackList();

			if (items.Count <= 0)
			{
				go.SetActive (false);
				return;
			}
			else
			{
				go.SetActive (true);
			}

			if (null == _addDebtGrid)
			{
				_addDebtGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _addDebtGrid.Cells.Length; ++i)
				{
					var cell = _addDebtGrid.Cells[i];
					cell.DisplayObject = new UIInforAddDebtItem(cell.GetTransform().gameObject);
				}

				_addDebtGrid.OnRefreshCell += _OnRefreshCell;				
			}
			else
			{
				_addDebtGrid.GridSize = items.Count;

			}

			_addDebtGrid.Refresh();
		}


		private void _OnDisposeCenter()
		{
			if (null != _addDebtGrid)
			{
				_addDebtGrid.Dispose ();
				_addDebtGrid.OnRefreshCell-=_OnRefreshCell;
			}

			if (null != _baseDebtGrid)
			{
				_baseDebtGrid.Dispose ();
				_baseDebtGrid.OnRefreshCell-=_OnRefreshBasePayCell;
			}
		}

		private void _OnRefreshBasePayCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetBasePayBackByIndex(index);
			var display = cell.DisplayObject as UIInforBaseDebtItem;
			display.Refresh(value);
		}

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetPaybackByIndex(index);
			var display = cell.DisplayObject as UIInforAddDebtItem;
			display.Refresh(value);
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




		private Transform content_basedebt;
		private Transform content_adddebt;

		private Transform content_basepay;
		private Transform content_addpay;


		private Button btn_debt;
		private Button btn_pay;

		private Text lb_debtitem;
		private Text lb_payitem;

		private UIWrapGrid _baseDebtGrid;
		private UIWrapGrid _addDebtGrid;

//		private GameObject img_basedebt;
//		private GameObject img_adddebt;

		private Image img_basebtnbg;
		private Image img_addbtnbg;


		/// ytf20161012 还款按钮
		private Button btn_payback;




	}
}

