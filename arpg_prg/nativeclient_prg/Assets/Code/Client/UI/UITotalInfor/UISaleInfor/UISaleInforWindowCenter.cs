using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UISaleInforWindow
	{
		private void _OnInItCenter(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			content_item = go.DeepFindEx (Layout.content_infor).gameObject;

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);
		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick += _OnCloseWindowHandler;

			_CreateWrapGridForSale (content_item);

//			MoveIn ();

//			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
//
//			if (null != controller)
//			{
//				controller.HideBoard ();
//			}

		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_close.gameObject).onClick -= _OnCloseWindowHandler;
		}

		private void _OnDisposeCenter()
		{
			if (null != _saleWrapGrid)
			{
				_saleWrapGrid.Dispose ();
				_saleWrapGrid.OnRefreshCell -=_OnRefreshCell;
			}
		}

		private void _OnCloseWindowHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.setVisible (false);

			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			if (null != controller)
			{
				controller.ShowBoardForSaleBoard ();
			}

		}


		private void _CreateWrapGridForSale(GameObject go)
		{
			var items = _controller.GetSaleRecordList ();

			if (items.Count <= 0)
			{
				go.SetActive (false);
				return;
			}
			else
			{
				go.SetActive (true);				
			}

			_saleWrapGrid = new UIWrapGrid (go, items.Count);
			for (var i = 0; i < _saleWrapGrid.Cells.Length; i++)
			{
				var cell = _saleWrapGrid.Cells [i];
				cell.DisplayObject=new UISaleRecordItem(cell.GetTransform().gameObject);
			}
			_saleWrapGrid.OnRefreshCell+=_OnRefreshCell;
			_saleWrapGrid.Refresh();
		}


		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetSaleRecordByIndex (index);
			var display = cell.DisplayObject as UISaleRecordItem;
			display.Refresh (value);
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



		private Button btn_close;
		private GameObject content_item;
		private UIWrapGrid _saleWrapGrid;

	}
}

