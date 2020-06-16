using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIOuterFateCardWindow
	{
		private void _OnInitSale(GameObject go)
		{
			_btnSaleSure = go.GetComponentEx<Button> (Layout.btn_salesure);
			lb_saletotal = go.GetComponentEx<Text> (Layout.lb_saletotal);
			lb_saleimcome = go.GetComponentEx<Text> (Layout.lb_saleimcone);
			lb_salequality = go.GetComponentEx<Text> (Layout.lb_quality);

			_btnCloseSale = go.GetComponentEx<Button> (Layout.btn_close);

			_saleImg = go.GetComponentEx<Image> (Layout.img_salebg);
			_saleImg.SetActiveEx (false);

			_normalBoard = go.DeepFindEx (Layout.transform_normal);

			var tmpSaletxt = go.GetComponentEx<Text> (Layout.lb_salenametxt);
			_CreateWrapGrid (tmpSaletxt.gameObject);
		}

		private void _OnShowSale()
		{
			EventTriggerListener.Get (_btnSaleSure.gameObject).onClick += _OnSureSaleHandler;
			EventTriggerListener.Get (_btnCloseSale.gameObject).onClick += _OnCloseSaleHandler;
			lb_saletotal.text ="";
			lb_saleimcome.text = "";
			lb_salequality.text = "";
			if (null != _controller)
			{
				_controller.UpdateTextData ();
			}

		}

		private void _OnHideSale()
		{
			EventTriggerListener.Get (_btnSaleSure.gameObject).onClick -= _OnSureSaleHandler;
			EventTriggerListener.Get (_btnCloseSale.gameObject).onClick -= _OnCloseSaleHandler;
		}

		private void _OnSureSaleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (_selfQuit == true) 
			{
				return;
			}

			if(null != _controller)
			{
				_handleSuccess = true;
				_controller.SaleFiexedData ();
				_HideBgImg ();
				TweenTools.MoveAndScaleTo("outerfatecard/Content", "uibattle/top/financementor", _CloseSaleHandler);
			}

		}

		private void _CloseSaleHandler()
		{
			if (_controller._netSaleList.Count <= 0)
			{
				_controller.NetQuitCard ();
			}
			else
			{
				_controller.NetSaleCard (_controller._netSaleList);
			}
			_controller.setVisible(false);
			if (GameModel.GetInstance.isPlayNet == true)
			{
				MessageHint.Show ("其他玩家正在操作");
			}
			Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			Particle.Instance.DestroyCardParticle();
		}

		private void _OnCloseSaleHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_SetColorPositionInit ();
			_saleImg.SetActiveEx (false);
		}

		private void _OnShowSaleBoard()
		{
			_SetClockPositionForSale ();
			_saleImg.SetActiveEx (true);
		}

		private void _OnDisposeSale()
		{
			if (null != _wrapGrid)
			{
				_wrapGrid.Dispose();
				_wrapGrid.OnRefreshCell -= _OnRefreshCell;
			}
		}

		private void _CreateWrapGrid(GameObject go)
		{
			var items = _controller.GetDataList();

			_wrapGrid = new UIWrapGrid(go, items.Count);

			for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
			{
				var cell = _wrapGrid.Cells[i];
				cell.DisplayObject = new UISaleFiexdItem(cell.GetTransform().gameObject);
			}

			_wrapGrid.OnRefreshCell += _OnRefreshCell;
			_wrapGrid.Refresh();

		}

		public void UpdateTxtData(string totaltxt ,string income,string qualitytxt)
		{
			lb_saletotal.text = totaltxt;
			lb_saleimcome.text = income;
			lb_salequality.text = qualitytxt;
		}

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetValueByIndex(index);

			var display = cell.DisplayObject as UISaleFiexdItem;

			display.Refresh(value);
		}

		private Button _btnSaleSure;
		private UIWrapGrid _wrapGrid;
		private Button _btnCloseSale;

		private Text lb_saletotal;
		private Text lb_saleimcome;
		private Text lb_salequality;

		private Image _saleImg;
		private Transform _normalBoard;

	}
}

