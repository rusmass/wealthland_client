using System;
using UnityEngine;
using UnityEngine.UI;


namespace Client.UI
{
	public class UIBorrowRecord
	{
		public UIBorrowRecord (GameObject go,UIBorrowWindowController controller)
		{
			
//			_selfObj = go;
			_controller = controller;
			_playerInfor = _controller.playerInfor;

			lb_limit = go.GetComponentEx<Text> (Layout.lb_limit);
			lb_canborrow = go.GetComponentEx<Text> (Layout.lb_conborrow);
			lb_already = go.GetComponentEx<Text> (Layout.lb_alredy);

			img_recordImg = go.GetComponentEx<Image> (Layout.img_item);
		}

		public void InitBorrowRecord()
		{		
			var canBorrowStr=HandleStringTool.HandleMoneyTostring(_playerInfor.GetTotalBorrowBank () + _playerInfor.GetTotalBorrowCard ()-_playerInfor.bankIncome - _playerInfor.creditIncome);
			if (GameModel.GetInstance.isPlayNet == true)
			{
				canBorrowStr = _playerInfor.netRecordCanBorrow.ToString ();
			}
			lb_canborrow.text = canBorrowStr;

			var limitTxt=HandleStringTool.HandleMoneyTostring(_playerInfor.GetTotalBorrowBank () + _playerInfor.GetTotalBorrowCard ());
			if (GameModel.GetInstance.isPlayNet == true)
			{
				limitTxt = _playerInfor.netRecordLimitBorrow.ToString();
			}
			lb_limit.text = limitTxt;

			var alreayTxt = HandleStringTool.HandleMoneyTostring(_playerInfor.bankIncome + _playerInfor.creditIncome);
			if (GameModel.GetInstance.isPlayNet == true)
			{
				alreayTxt = _playerInfor.netRecordAlreadyBorrow.ToString ();
			}
			lb_already.text = alreayTxt;

			_CreateWrapGrid (img_recordImg.gameObject);
		}


		private void _CreateWrapGrid(GameObject go)
		{
			var items = _controller.GetBorrowRectdList();

			if (null == _wrapGrid)
			{
				_wrapGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
				{
					var cell = _wrapGrid.Cells[i];
					cell.DisplayObject = new UIBorrowRecordItem(cell.GetTransform().gameObject);
				}
				_wrapGrid.OnRefreshCell += _OnRefreshCell;
			}
			else
			{
				_wrapGrid.GridSize = items.Count;
			}

			_wrapGrid.Refresh();

		}

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetBorrowDataByIndex(index);

			var display = cell.DisplayObject as UIBorrowRecordItem;

			display.Refresh(value);
		}

		public void DisposeBorrowRecord()
		{
			if (null != _wrapGrid)
			{
				_wrapGrid.Dispose ();
				_wrapGrid.OnRefreshCell-=_OnRefreshCell;
			}
		}

//		private GameObject _selfObj;
		private UIBorrowWindowController _controller;
		private PlayerInfo _playerInfor;

		private Text lb_already;
		private Text lb_canborrow;
		private Text lb_limit;

		private Image img_recordImg;

		private UIWrapGrid _wrapGrid;

		class Layout{
			public static string lb_alredy="alreadytxt";
			public static string lb_conborrow="canborrowtxt";
			public static string lb_limit="limittxt";
			public static string img_item="recorditem";

		}
	}
}

