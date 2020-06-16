using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIBorrowWindow
	{
		private void _OnInitCenter(GameObject go)
		{			
			_borrowBank = new UIBorrowWindowBank (go.GetComponentEx<Image> (Layout.img_borrowBank).gameObject, _controller);
			_borrowCard = new UIBorrowWindowCard (go.GetComponentEx<Image> (Layout.img_borrowCard).gameObject, _controller);
			btn_sure = go.GetComponentEx<Button> (Layout.btn_sure);
			btn_borrowRecord = go.GetComponentEx<Button> (Layout.btn_record);
			btn_payback = go.GetComponentEx<Button> (Layout.btn_payback);
			btn_borrow = go.GetComponentEx<Button> (Layout.btn_borrowmoney);

			content_borrowitem = go.DeepFindEx (Layout.content_itemborrow);
			content_record = go.DeepFindEx (Layout.content_itemrecord);
			content_borrowbaord = go.DeepFindEx (Layout.content_borrowBoard);
			content_paybackboard = go.DeepFindEx (Layout.content_itempayback);

			lb_noCanBorrowTip = go.GetComponentEx<Text> (Layout.lb_notCanBorrowTip);

			img_borrowbtn = btn_borrow.GetComponent<Image> ();
			img_paybanbtn = btn_payback.GetComponent<Image> ();

			lb_tatalMoeny = go.GetComponentEx<Text> (Layout.lb_playerTotalMoney);

			_borrowColor = img_borrowbtn.color;
			_payColor = img_paybanbtn.color;

			btn_buycare = go.GetComponentEx<Button> (Layout.btn_buycard);

		}

		private void _OnShowCenter()
		{

			EventTriggerListener.Get (btn_sure.gameObject).onClick+=_SureBorrowMoney;
			EventTriggerListener.Get(btn_borrowRecord.gameObject).onClick+=_OnShowRecordItem;
			EventTriggerListener.Get(btn_payback.gameObject).onClick+=_OnShowPayBackItem;
			EventTriggerListener.Get(btn_borrow.gameObject).onClick+=_OnShowBorrowItem;
			EventTriggerListener.Get(btn_buycare.gameObject).onClick+=OnBuyCareHandler;


			if (_controller.playerInfor.isEnterInner == true)
			{
				_borrowCard.SetActiveEx(false);
				_borrowBank.MoveDownY (70);

				//备注隐藏银行贷款，后面删除掉
//				_borrowBank.SetActiveEx (false);

				btn_buycare.SetActiveEx (true);
			}
			else
			{
				_borrowCard.OnShowBorrowCard ();
				btn_buycare.SetActiveEx (false);
			}		

			if(_controller.isInitPayback==true)
			{
				content_paybackboard.SetActiveEx (true);
				content_borrowbaord.SetActiveEx (false);
				_ShowPaybackBtnAlpah ();
				if (null == _borrowPayBack)
				{
					_borrowPayBack = new UIBorrowPayBackBoard (content_paybackboard.gameObject,_controller);
				}
				_borrowPayBack.InitPaybackBoard ();
			}
			else
			{
				content_paybackboard.SetActiveEx (false);
				_ShowBorrowBtnAlpah ();
				_borrowBank.OnShowBorrowBank ();
			}

			content_record.SetActiveEx (false);
	

			if (_controller.playerInfor.borrowList.Count < 1)
			{
				btn_borrowRecord.SetActiveEx (false);
			}

			if (_controller.playerInfor.paybackList.Count <= 0 && _controller.GetBasePayBackList().Count<=0)
			{
				btn_borrowRecord.SetActiveEx (false);
//				btn_payback.SetActiveEx (false);
			}

			_isBorrowBank = false;
			_isBorrowCard = false;

			_UpdateSureBtnState ();

			_borrowBank.IsVisible += SetBorrowBankVisible;
			_borrowCard.IsSetVisible += SetBorrowCardVisible;

			lb_tatalMoeny.text = _controller.playerInfor.totalMoney.ToString ();

		}

		private void _OnShowBorrowItem(GameObject go)
		{

			Audio.AudioManager.Instance.BtnMusic ();

			content_borrowitem.SetActiveEx (true);
			content_borrowbaord.SetActiveEx (true);
			content_paybackboard.SetActiveEx (false);
			content_record.SetActiveEx (false);
			_boardstate = 0;

			_borrowBank.OnShowBorrowBank ();
			_borrowCard.OnShowBorrowCard ();

			_ShowBorrowBtnAlpah ();

			_UpdateSureBtnState ();
		}

		private void _OnShowPayBackItem(GameObject go)
		{

			Audio.AudioManager.Instance.BtnMusic ();

			content_borrowitem.SetActiveEx (true);
			content_borrowbaord.SetActiveEx (false);
			content_paybackboard.SetActiveEx (true);
			content_record.SetActiveEx (false);

			if (null == _borrowPayBack)
			{
				_borrowPayBack = new UIBorrowPayBackBoard (content_paybackboard.gameObject,_controller);
			}
			_borrowPayBack.InitPaybackBoard ();

			_ShowPaybackBtnAlpah ();

		}

		private void _ShowBorrowBtnAlpah()
		{
			img_borrowbtn.color = _borrowColor;
			img_paybanbtn.color =new Color(_payColor.r,_payColor.g,_payColor.b,0.01f);
		}

		private void _ShowPaybackBtnAlpah()
		{
			img_borrowbtn.color =new Color(_borrowColor.r,_borrowColor.g,_borrowColor.b,0.01f);
			img_paybanbtn.color = _payColor;
		}

		private void _OnShowRecordItem(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			content_borrowitem.SetActiveEx (false);
			content_borrowbaord.SetActiveEx (false);
			content_paybackboard.SetActiveEx (false);
			content_record.SetActiveEx (true);

			if (null == _borrowRecord)
			{
				_borrowRecord = new UIBorrowRecord (content_record.gameObject,_controller);
			}

			_borrowRecord.InitBorrowRecord ();
			_boardstate = 1;		
		}



		private void _OnHideCenter()
		{			
			_borrowBank.OnHideBorrowBank ();
			if (_controller.playerInfor.isEnterInner == false)
			{				
				_borrowCard.OnHideBorrowCard ();
			}
			EventTriggerListener.Get (btn_sure.gameObject).onClick-=_SureBorrowMoney;
			EventTriggerListener.Get(btn_borrowRecord.gameObject).onClick-=_OnShowRecordItem;
			EventTriggerListener.Get(btn_payback.gameObject).onClick-=_OnShowPayBackItem;
			EventTriggerListener.Get(btn_borrow.gameObject).onClick-=_OnShowBorrowItem;
			EventTriggerListener.Get(btn_buycare.gameObject).onClick-=OnBuyCareHandler;

			if (null != _borrowPayBack)
			{
				_borrowPayBack.OnHidePayBackBoard ();
			}

			_borrowBank.IsVisible -= SetBorrowBankVisible;
			_borrowCard.IsSetVisible -= SetBorrowCardVisible;
		}

		private void OnBuyCareHandler(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UIBuyCareWindowController> ();
			controller.setVisible (true);
//			if (GameModel.GetInstance.isPlayNet == false)
//			{
//				var controller = UIControllerManager.Instance.GetController<UIBuyCareWindowController> ();
//				controller.setVisible (true);
//			}
//			else
//			{
//				if (PlayerManager.Instance.HostPlayerInfo.InsuranceList.Count>0)
//				{
//					MessageHint.Show ("您已经购买保险");
//					return;
//				}
//
//				var controller = UIControllerManager.Instance.GetController<UIBuyCareWindowController> ();
//				controller.setVisible (true);
//			}
		}

		private void _OnDisposeCenter()
		{			
			if(null !=_borrowPayBack)
			{
				_borrowPayBack.DisposePayBackBaord ();
			}

			if (null != _borrowRecord)
			{
				_borrowRecord.DisposeBorrowRecord ();
			}
		}

		private void _SureBorrowMoney(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if(null !=_controller)
			{
				_controller.BorrowMoney ();

				if (GameModel.GetInstance.isPlayNet == false)
				{
					_UpdateBoarrowedData ();
				}

				if (_controller._netBorrowList.Count > 0)
				{
					CardManager.Instance.NetBorrowMoney (_controller._netBorrowList,_controller._loanType);
				}

				if (_controller.playerInfor.borrowList.Count < 1)
				{
					btn_borrowRecord.SetActiveEx (false);
				}
				else
				{
					btn_borrowRecord.SetActiveEx (true);
				}

			}
		}

		public void UpdateBoardInfor()
		{
			_UpdateBoarrowedData ();
		}

		private void _UpdateBoarrowedData()
		{
			_borrowBank.OnShowBorrowBank ();

			if (_controller.playerInfor.isEnterInner == false)
			{
				_borrowCard.OnShowBorrowCard ();
			}

			UpdateBorrowBoardMoney ();

			var chanceShareController = UIControllerManager.Instance.GetController<UIChanceShareCardController> ();
			if (chanceShareController.getVisible ())
			{
				chanceShareController.UpdatePlayerMoney ();
			}
		}

		public void UpdateBorrowBoardMoney()
		{
			if (null != lb_tatalMoeny)
			{
				lb_tatalMoeny.text = _controller.playerInfor.totalMoney.ToString ("F0");
			}
		}

		private void SetBorrowBankVisible(bool value)
		{
			_isBorrowBank = value;
			_UpdateSureBtnState ();
		}

		private void SetBorrowCardVisible(bool value)
		{
			_isBorrowCard = value;
			_UpdateSureBtnState ();
		}


		private void _UpdateSureBtnState()
		{
			if(_isBorrowBank ==false && _isBorrowCard==false)
			{
				if (btn_sure.IsActive ())
				{
					btn_sure.SetActiveEx (false);
				}

			}
			else
			{
				if (btn_sure.IsActive ()==false)
				{
					btn_sure.SetActiveEx (true);
				}
			}

			var player = _controller.playerInfor;
			var canborrow = player.GetTotalBorrowBank () + player.GetTotalBorrowCard () - player.bankIncome - player.creditIncome;

			if (GameModel.GetInstance.isPlayNet == true)
			{
//				canborrow = player;
				canborrow = player.netBorrowBoardBankCanBorrow+player.netBorrowBoardCardCanBorrow;
			}

			if (canborrow <= 0)
			{
				lb_noCanBorrowTip.SetActiveEx (true);
			}
			else
			{
				lb_noCanBorrowTip.SetActiveEx (false);
			}
		}

		public void UpdatePayBackMoney(float value)
		{
			if(null != _borrowPayBack)
			{
				_borrowPayBack.OnShowNumTxt (value);
			}
		}
			

		private UIBorrowWindowBank _borrowBank;
		private UIBorrowWindowCard _borrowCard;
		private Button btn_sure;
		private Button btn_borrowRecord;
		private Button btn_payback;
		private Button btn_borrow;
		private Button btn_buycare;
		private Text lb_tatalMoeny;


		private bool _isBorrowBank=false;
		private bool _isBorrowCard=false;


		private Image img_borrowbtn;
		private Image img_paybanbtn;

		private Color _borrowColor;
		private Color _payColor;

		/// <summary>
		/// 20161019 不能在贷款的提示
		/// </summary>
		private Text lb_noCanBorrowTip;

		// ytf0927 确定按钮开始的颜色
//		private Color _sureInitColor;
//		private Color _sureGrayColor = new Color ();



		private Transform content_borrowitem;
		private Transform content_borrowbaord;
		private Transform content_paybackboard;
		private Transform content_record;

		private UIBorrowRecord _borrowRecord;
		private UIBorrowPayBackBoard _borrowPayBack;



		private int _boardstate=0;



	}
}

