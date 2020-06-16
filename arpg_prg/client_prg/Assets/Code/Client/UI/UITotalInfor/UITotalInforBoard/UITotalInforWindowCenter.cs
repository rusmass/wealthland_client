using System;
using UnityEngine;
using UnityEngine.UI;


namespace Client.UI
{
	public partial class UITotalInforWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			var raw = go.GetComponentEx<RawImage> (Layout.raw_head);
			_headImg = new UIRawImageDisplay (raw);

			img_board = go.GetComponentEx<Image> (Layout.img_board);

			btn_heroInfor = go.GetComponentEx<Button> (Layout.btn_heroInfor);
			btn_target = go.GetComponentEx<Button> (Layout.btn_target);
			btn_balaceIncome = go.GetComponentEx<Button> (Layout.btn_balance);
			btn_debt = go.GetComponentEx<Button> (Layout.btn_debt);
			btn_sale = go.GetComponentEx<Button> (Layout.btn_sale);
			btn_checkout = go.GetComponentEx<Button> (Layout.btn_checkout);


		}

		private void _OnShowCenter()
		{
			var player = _controller.playerInfor;

			if (null != player)
			{
				_headImg.Load (player.headName);

				if (_controller.playerInfor.isEnterInner == false)
				{
					_inforController = UIControllerManager.Instance.GetController<UIHeroInforWindowController> ();
					_inforController.playerInfor = _controller.playerInfor;
					_inforController.setVisible (true);

					_infortype = HeroInforType.HeroInfor;
					btn_tmp = btn_heroInfor.gameObject;

					_GreyBtnColor (btn_target.gameObject);
					_GreyBtnColor (btn_balaceIncome.gameObject);
					_GreyBtnColor (btn_debt.gameObject);
					_GreyBtnColor (btn_sale.gameObject);
					_GreyBtnColor (btn_checkout.gameObject);

				} 
				else
				{				
//					_targetInnerController = UIControllerManager.Instance.GetController<UITargetInforInnerWindowController> ();
//					_targetInnerController.playerInfor = _controller.playerInfor;
//					_targetInnerController.setVisible (true);


					if (GameModel.GetInstance.isPlayNet == false ||GameModel.GetInstance.hasLoadTarget==true)
					{
						NetShowTargetInforBaord ();
					}
					else
					{				
						NetWorkScript.getInstance ().GetPlayerTargetInfor (_controller.playerInfor.playerID);
					}		

					_infortype = HeroInforType.TargetInfor;

					btn_tmp = btn_target.gameObject;

					_GreyBtnColor (btn_balaceIncome.gameObject);
					_GreyBtnColor (btn_debt.gameObject);
					_GreyBtnColor (btn_sale.gameObject);
					_GreyBtnColor (btn_checkout.gameObject);

					btn_heroInfor.SetActiveEx (false);
					btn_balaceIncome.SetActiveEx (false);
					btn_debt.SetActiveEx (false);
					btn_sale.SetActiveEx (false);

				}
			}

			EventTriggerListener.Get(btn_heroInfor.gameObject).onClick+=_ShowInforBoard;
			EventTriggerListener.Get(btn_target.gameObject).onClick+=_ShowTargetBoard;
			EventTriggerListener.Get(btn_balaceIncome.gameObject).onClick+=_ShowBalanceInforBoard;
			EventTriggerListener.Get(btn_debt.gameObject).onClick+=_ShowDebtBoard;
			EventTriggerListener.Get(btn_sale.gameObject).onClick+=_ShowSaleBoard;
			EventTriggerListener.Get(btn_checkout.gameObject).onClick+=_ShowCheckoutBoard;

		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get(btn_heroInfor.gameObject).onClick-=_ShowInforBoard;
			EventTriggerListener.Get(btn_target.gameObject).onClick-=_ShowTargetBoard;
			EventTriggerListener.Get(btn_balaceIncome.gameObject).onClick-=_ShowBalanceInforBoard;
			EventTriggerListener.Get(btn_debt.gameObject).onClick-=_ShowDebtBoard;
			EventTriggerListener.Get(btn_sale.gameObject).onClick-=_ShowSaleBoard;
			EventTriggerListener.Get(btn_checkout.gameObject).onClick-=_ShowCheckoutBoard;
		}

		private void _OnDisposeCenter()
		{
			if (null != _headImg)
			{
				_headImg.Dispose ();
			}
		}

		private void _ShowInforBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("基本信息");
			if (_infortype == HeroInforType.HeroInfor)
			{
				return;
			}

			_OnUpdateChange ();
			_OnChangeBtnColor (go);

			if (null == _inforController)
			{
				_inforController = UIControllerManager.Instance.GetController<UIHeroInforWindowController> ();
				_inforController.playerInfor = _controller.playerInfor;
				_inforController.setVisible (true);
			}
			else
			{
				_inforController.setVisible (true);
//				_inforController.MoveIn ();
			}
		

			_infortype = HeroInforType.HeroInfor;
		}

		private void _ShowTargetBoard(GameObject go)
		{
			Console.WriteLine ("目标信息");
			Audio.AudioManager.Instance.BtnMusic ();
			if (_infortype == HeroInforType.TargetInfor)
			{
				return;
			}
			_infortype = HeroInforType.TargetInfor;
			_OnUpdateChange ();
			_OnChangeBtnColor (go);

			if (GameModel.GetInstance.isPlayNet == false ||GameModel.GetInstance.hasLoadTarget==true)
			{
				NetShowTargetInforBaord ();
			}
			else
			{				
				NetWorkScript.getInstance ().GetPlayerTargetInfor (_controller.playerInfor.playerID);
			}		
		}

		/// <summary>
		/// Shows the target infor baord. 显示目标界面
		/// </summary>
		public void NetShowTargetInforBaord()
		{
			if (_controller.playerInfor.isEnterInner == false)
			{
				if (null == _targetController)
				{
					_targetController = UIControllerManager.Instance.GetController<UITargetInforWindowController> ();
					_targetController.playerInfor = _controller.playerInfor;
					_targetController.setVisible (true);
				}
				else
				{
					_targetController.setVisible (true);
				}

			}
			else
			{
				if (null == _targetInnerController)
				{
					_targetInnerController = UIControllerManager.Instance.GetController<UITargetInforInnerWindowController> ();
					_targetInnerController.playerInfor = _controller.playerInfor;
					_targetInnerController.setVisible (true);
				}
				else
				{
					_targetInnerController.setVisible (true);
					//					_targetInnerController.MoveIn ();
				}			
			}
		}

		private void _ShowBalanceInforBoard(GameObject go)
		{
			Console.WriteLine ("房产收入信息");
			Audio.AudioManager.Instance.BtnMusic ();
			if(_infortype == HeroInforType.BalanceIncomeInfor)
			{
				return;
			}

			_infortype = HeroInforType.BalanceIncomeInfor;
			_OnUpdateChange ();
			_OnChangeBtnColor (go);

			if (GameModel.GetInstance.isPlayNet == false ||GameModel.GetInstance.hasLoadBalanceAndIncome==true)
			{
				NetShowBalanceAndIncome ();
			}
			else
			{
				NetWorkScript.getInstance ().GetPlayerBalanceAndIncome (_controller.playerInfor.playerID);
			}

		}

		public void NetShowBalanceAndIncome()
		{
			if (null == _balanceAndIncomeController)
			{
				_balanceAndIncomeController = UIControllerManager.Instance.GetController<UIBalanceAndIncomeWindowController> ();
				_balanceAndIncomeController.playerInfor = _controller.playerInfor;
				_balanceAndIncomeController.setVisible (true);
			}
			else
			{
				_balanceAndIncomeController.setVisible (true);
			}
		}

		private void _ShowDebtBoard(GameObject go)
		{
			Console.WriteLine ("负债信息");
			Audio.AudioManager.Instance.BtnMusic ();
			if (_infortype == HeroInforType.DebtInfor)
			{
				return;
			}		
			_OnUpdateChange ();
			_OnChangeBtnColor (go);
			_infortype = HeroInforType.DebtInfor;

			if (GameModel.GetInstance.isPlayNet == false || GameModel.GetInstance.hasLoadDebtAndPay==true)
			{
				NetShowDebtAndPayback ();
			}
			else
			{
				NetWorkScript.getInstance ().GetPlayerDebtAndPayInfor (_controller.playerInfor.playerID);
			}
		}

		public void NetShowDebtAndPayback()
		{
			if(null ==_debtpaybackController)
			{
				_debtpaybackController = UIControllerManager.Instance.GetController<UIDebtAndPaybackController> ();
				_debtpaybackController.playerInfor = _controller.playerInfor;
				_debtpaybackController.setVisible (true);
			}
			else
			{
				_debtpaybackController.setVisible (true);
			}
		}

		private void _ShowSaleBoard(GameObject go)
		{
			Console.WriteLine ("卖出信息");
			Audio.AudioManager.Instance.BtnMusic ();
			if(	_infortype == HeroInforType.SaleInfor)
			{
				return; 
			}
			_OnUpdateChange ();
			_OnChangeBtnColor (go);
			_infortype = HeroInforType.SaleInfor;

			if (GameModel.GetInstance.isPlayNet == false ||GameModel.GetInstance.hasLoadSaleInfor==true)
			{
				NetShowSaleBoard ();
			}
			else
			{
				NetWorkScript.getInstance ().GetPlayerInforSaleRecord (_controller.playerInfor.playerID);
			}

//			if (null != _saleController.GetSaleRecordList())
//			{
//				if (_saleController.GetSaleRecordList ().Count <= 0)
//				{
//					MessageHint.Show (string.Format("{0}暂无出售信息",_saleController.playerInfor.playerName));
//					return;
//				}
//			}
		}

		public void NetShowSaleBoard()
		{
			if (null == _saleController)
			{
				_saleController = UIControllerManager.Instance.GetController<UISaleInforWindowController> ();
				_saleController.playerInfor= _controller.playerInfor;
				_saleController.setVisible(true);
			}
			else
			{
				_saleController.setVisible(true);
			}

		}

		private void _ShowCheckoutBoard(GameObject go)
		{
			Console.WriteLine ("结算信息");
			Audio.AudioManager.Instance.BtnMusic ();
			if(_infortype == HeroInforType.CheckOutInfor)
			{
				return;	
			}

			_OnUpdateChange ();
			_OnChangeBtnColor (go);
			_infortype = HeroInforType.CheckOutInfor;

			if (GameModel.GetInstance.isPlayNet == false ||GameModel.GetInstance.hasLoadCheck==true)
			{
				NetShowCheckBoard ();
			}
			else
			{
				NetWorkScript.getInstance ().GetPlayerInforCheckInfor (_controller.playerInfor.playerID);
			}

		}

		public void NetShowCheckBoard()
		{
			if(null == _settlementController)
			{
				_settlementController = UIControllerManager.Instance.GetController<UISettlementInforWindowController>();
				_settlementController.playerInfor=_controller.playerInfor;
				_settlementController.setVisible(true);
			}
			else
			{
				Console.WriteLine ("结算界面点击滑入");
				_settlementController.setVisible(true);
				//				_settlementController.MoveIn ();
			}
		}

		public void _OnUpdateChange()
		{
			if (null != _inforController)
			{
				if(_inforController.getVisible()==true)
				{
					_inforController.setVisible(false);
//					_inforController.MoveOut();
				}
			}

			if (null != _targetController)
			{
				if (_targetController.getVisible () == true) 
				{
					_targetController.setVisible (false);
//					_targetController.MoveOut();
				}
			}

			if (null != _debtpaybackController)
			{
				if (_debtpaybackController.getVisible () == true) 
				{
					_debtpaybackController.setVisible (false);
//					_debtpaybackController.MoveOut();
				}
			}

			if (null != _balanceAndIncomeController)
			{
				if (_balanceAndIncomeController.getVisible () == true) 
				{
					_balanceAndIncomeController.setVisible (false);
//					_balanceAndIncomeController.MoveOut ();
				}
			}

			if (null != _saleController)
			{
				if (_saleController.getVisible () == true) 
				{
					_saleController.setVisible (false);
//					_saleController.MoveOut();
				}
			}

			if (null != _settlementController)
			{
				if (_settlementController.getVisible () == true) 
				{
					_settlementController.setVisible (false);
//					_settlementController.MoveOut();
				}
			}

			if (null != _targetInnerController)
			{
				if (_targetInnerController.getVisible () == true) 
				{
					_targetInnerController.setVisible (false);
//					_targetInnerController.MoveOut();
				}
			}

		}


		private void _OnCloseAllWindow()
		{
			if (null != _inforController)
			{
				if(_inforController.getVisible()==true)
				{
					_inforController.setVisible(false);
				}
			}

			if (null != _targetController)
			{
				if (_targetController.getVisible () == true) 
				{
				    _targetController.setVisible (false);
				}
			}

			if (null != _debtpaybackController)
			{
				if (_debtpaybackController.getVisible () == true) 
				{
					_debtpaybackController.setVisible (false);
				}
			}

			if (null != _balanceAndIncomeController)
			{
				if (_balanceAndIncomeController.getVisible () == true) 
				{
					_balanceAndIncomeController.setVisible (false);
				}
			}

			if (null != _saleController)
			{
				if (_saleController.getVisible () == true) 
				{
					_saleController.setVisible (false);
				}
			}

			if (null != _settlementController)
			{
				if (_settlementController.getVisible () == true) 
				{
					_settlementController.setVisible (false);
				}
			}

			if (null != _targetInnerController)
			{
				if (_targetInnerController.getVisible () == true) 
				{
					_targetInnerController.setVisible (false);
				}
			}

		}


		private void _OnChangeBtnColor(GameObject go)
		{
			if (null != btn_tmp)
			{
				_GreyBtnColor (btn_tmp);
			}

			_InitBtnColor (go);
			btn_tmp = go;
		}

		private void _InitBtnColor(GameObject go)
		{
			go.GetComponent<Image> ().color = initColor;
			go.GetComponentEx<Image> ("Image").color=initColor;
		}

		private void _GreyBtnColor(GameObject go)
		{
			Console.WriteLine (string.Format("{0}变成灰色了",go.name));
			go.GetComponent<Image> ().color = btnbgColor;
			go.GetComponentEx<Image> ("Image").color=imgbgColor;
		}

		public void ShowBoardForSaleBoard()
		{
			img_board.SetActiveEx(true);
			_ShowInforBoard (this.btn_heroInfor.gameObject);
		}

		public void ShowBoard()
		{
			img_board.SetActiveEx(true);
		}

		public void HideBoard()
		{
			img_board.SetActiveEx(false);
		}





		private Image img_board;

		private UIRawImageDisplay _headImg;

		private Button btn_heroInfor;
		private Button btn_target;
		private Button btn_balaceIncome;
		private Button btn_debt;
		private Button btn_sale;
		private Button btn_checkout;
		private GameObject btn_tmp;


		private UIHeroInforWindowController _inforController;
		private UITargetInforWindowController _targetController;
		private UIDebtAndPaybackController _debtpaybackController;

		private UIBalanceAndIncomeWindowController _balanceAndIncomeController;
		private UISaleInforWindowController _saleController;
		private UISettlementInforWindowController _settlementController;

		private UITargetInforInnerWindowController _targetInnerController;

		private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);
		private Color imgbgColor = new Color (180f/255,180f/255,180f/255,1f);
		private Color initColor = new Color (255f/255,255f/255,255f/255,1f);

		private HeroInforType _infortype;
	}
}

