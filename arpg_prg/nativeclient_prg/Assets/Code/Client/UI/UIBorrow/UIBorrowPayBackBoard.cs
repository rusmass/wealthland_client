using System;
using UnityEngine;
using UnityEngine.UI;


namespace Client.UI
{
    /// <summary>
    /// 还款面板
    /// </summary>
	public class UIBorrowPayBackBoard
	{
		public UIBorrowPayBackBoard (GameObject go,UIBorrowWindowController controller)
		{
//			_selfObj = go;
			_controller = controller;

			lb_baseItem = go.GetComponentEx<Text> (Layout.lb_baseitem);
			lb_paybackItem = go.GetComponentEx<Text> (Layout.lb_paybackItem);
			btn_sureback = go.GetComponentEx<Button> (Layout.btn_sure);

			_baseDebtScroll = go.GetComponentEx<RectTransform>(Layout.baseDebtScroll);
			_newDebtScroll = go.GetComponentEx<RectTransform> (Layout.newDebtScroll);

//			_baseDebtScrollSize = _baseDebtScroll.sizeDelta;
//			_newDebtScrollSize = _newDebtScroll.sizeDelta;

			_baseDebtPosition = _baseDebtScroll.localPosition;
			_newDebtPosition = _newDebtScroll.localPosition;

			lb_shownumTxt = go.GetComponentEx<Text> (Layout.lb_shownumTxt);

			lb_currentnumTxt = go.GetComponentEx<Text> (Layout.lb_currentnumTxt);


			EventTriggerListener.Get(btn_sureback.gameObject).onClick+=_SurePayBackHandler;
		}

        /// <summary>
        /// 点击确定还款
        /// </summary>
        /// <param name="go"></param>
		private void _SurePayBackHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_controller.CanPayBack () == true)
			{
				_controller.payBackHandler ();
				OnShowNumTxt (0);	
				_UpdatePayBackData();

				if (_controller._netPayBackList.Count > 0)
				{
					CardManager.Instance.NetPayBackMoney (_controller._netPayBackList);
				}

				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (PlayerManager.Instance.HostPlayerInfo.bankIncome == 0 && PlayerManager.Instance.HostPlayerInfo.creditIncome == 0)
					{
						var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
						controller.HidePaybackBtn ();
					}
				}
				else
				{
					if (PlayerManager.Instance.HostPlayerInfo.paybackList.Count<=0) 
					{
						var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
						controller.HidePaybackBtn ();
					}
				}
			}

			//_controller.setVisible(false);
		}

        /// <summary>
        /// 游戏是否初始显示过
        /// </summary>
        /// <returns></returns>
        public bool IsShowNewPlayerGuid()
        {
            return _isShowed;
        }

        private bool _isShowed = false;

        /// <summary>
        /// 更新还款界面的数据
        /// </summary>
        private void _UpdatePayBackData()
		{
			InitPaybackBoard ();
		}

        /// <summary>
        /// 隐藏还款界面时移除事件
        /// </summary>
		public void OnHidePayBackBoard()
		{
			EventTriggerListener.Get (btn_sureback.gameObject).onClick -= _SurePayBackHandler;
		}

        /// <summary>
        /// 初始化还款信息
        /// </summary>
		public void InitPaybackBoard()
		{
            if(GameGuidManager.GetInstance.DoneGamePayback==false)
            {
                GameGuidManager.GetInstance.ShowPayBackGuid();
            }

            _isShowed = true;

            //var isShowPayback=true;
            //var isShowBasePayBack = true;				

            _CreateWrapGrid (lb_paybackItem.gameObject);

//			if (_controller.GetPaybackList().Count > 0)
//			{				
//				
//			}
//			else
//			{
//				isShowPayback = false;
//			}

			_CreateBasePayGrid (lb_baseItem.gameObject);

//			if (_controller.playerInfor.basePayList.Count > 0)
//			{
//				_CreateBasePayGrid (lb_baseItem.gameObject);
//			}
//			else
//			{
//				isShowBasePayBack = false;
//			}

//			if (isShowPayback == false)
//			{
//				var tmpPosition =  _baseDebtScroll.localPosition;
//				_baseDebtScroll.localPosition = new Vector3 (0,tmpPosition.y,tmpPosition.z);
//				_newDebtScroll.SetActiveEx (false);
//			}
//			else
//			{
//				_newDebtScroll.SetActiveEx (true);
//				_baseDebtScroll.localPosition = _baseDebtPosition;
//			}
//
//			if (isShowBasePayBack == false)
//			{
//				var tmpPosition = _newDebtScroll.localPosition;
//				_newDebtScroll.localPosition = new Vector3 (0,tmpPosition.y,tmpPosition.z);
//				_baseDebtScroll.SetActiveEx (false);
//			}
//			else
//			{
//				_baseDebtScroll.SetActiveEx (true);
//				_newDebtScroll.localPosition = _newDebtPosition;
//			}

			lb_currentnumTxt.text = _controller.playerInfor.totalMoney.ToString ("F0");

			_controller.UpdateBorrowBoardMoney ();

		}

        /// <summary>
        /// 显示金币数
        /// </summary>
        /// <param name="value"></param>
		public void OnShowNumTxt(float value)
		{
			lb_shownumTxt.text =HandleStringTool.HandleMoneyTostring(value);
		}

        /// <summary>
        /// 初始化基本负债的数据
        /// </summary>
        /// <param name="go"></param>
		private void _CreateBasePayGrid(GameObject go)
		{
			var items = _controller.GetBasePayBackList();

			if (null == _basePayGrid)
			{
				if (items.Count<= 0)
				{
					go.SetActive (false);
					return;
				}

				_basePayGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _basePayGrid.Cells.Length; ++i)
				{
					var cell = _basePayGrid.Cells[i];
					cell.DisplayObject = new UIBorrowBasePayItem(cell.GetTransform().gameObject);
				}

				_basePayGrid.OnRefreshCell += _OnRefreshBasePayCell;				
			}
			else
			{
				_basePayGrid.GridSize = items.Count;

			}
			_basePayGrid.Refresh();
		}
        /// <summary>
        /// 初始化新增负债的数据
        /// </summary>
        /// <param name="go"></param>
		private void _CreateWrapGrid(GameObject go)
		{
			var items = _controller.GetPaybackList();

			Console.WriteLine ("当前的还款个数"+items.Count.ToString());

			if (null == _wrapGrid)
			{
				if (items.Count <= 0)
				{
					go.SetActiveEx (false);
					return;
				}
				
				_wrapGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
				{
					var cell = _wrapGrid.Cells[i];
					cell.DisplayObject = new UIBorrowPayBackItem(cell.GetTransform().gameObject);
				}

				_wrapGrid.OnRefreshCell += _OnRefreshCell;				
			}
			else
			{
				_wrapGrid.GridSize = items.Count;

			}
			_wrapGrid.Refresh();
		}

        /// <summary>
        /// 刷新基本负债信息
        /// </summary>
        /// <param name="cell"></param>
		private void _OnRefreshBasePayCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetBasePayBackByIndex(index);
			var display = cell.DisplayObject as UIBorrowBasePayItem;
			display.Refresh(value);
		}
        /// <summary>
        /// 刷新新增负债信息
        /// </summary>
        /// <param name="cell"></param>
		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetPaybackByIndex(index);
			var display = cell.DisplayObject as UIBorrowPayBackItem;
			display.Refresh(value);
		}

        /// <summary>
        /// 释放还款界面
        /// </summary>
		public void DisposePayBackBaord()
		{
			if (null != _wrapGrid)
			{
				_wrapGrid.Dispose ();
				_wrapGrid.OnRefreshCell-=_OnRefreshCell;
			}

			if (null != _basePayGrid)
			{
				_basePayGrid.Dispose ();
				_basePayGrid.OnRefreshCell-=_OnRefreshBasePayCell;
			}
		}

//		private GameObject _selfObj;
		private UIBorrowWindowController _controller;
		private PlayerInfo _playerInfor;

		private UIWrapGrid _wrapGrid;
		private UIWrapGrid _basePayGrid;


		private Text lb_baseItem;
		private Text lb_paybackItem;
		private Button btn_sureback;

		/// 显示借款数目的文本
		private Text lb_shownumTxt;
		/// <summary>
		/// ytf20161012 新增当前人物金币
		/// </summary>
		private Text lb_currentnumTxt;

		private RectTransform _baseDebtScroll;
		private RectTransform _newDebtScroll;

//		private Vector2 _baseDebtScrollSize;
//		private Vector2 _newDebtScrollSize;

		/// 基础负债组件坐标
		private Vector3 _baseDebtPosition;
		/// 新增负债组件坐标
		private Vector3 _newDebtPosition;

        
		class Layout
		{
			
			public static string lb_baseitem="baseItem";
			public static string lb_paybackItem="paybackitem";
			public static string btn_sure="surebackbtn";

			public static string baseDebtScroll="basedebtbg";
			public static string newDebtScroll="newdebtbg";

			// 显示贷款数的
			public static string lb_shownumTxt="shownumTxt";

			/// <summary>
			///ytf20161012 The lb currentnum text. 当前现有金币
			/// </summary>
			public static string lb_currentnumTxt="currentnumTxt";
		}
	}
}

