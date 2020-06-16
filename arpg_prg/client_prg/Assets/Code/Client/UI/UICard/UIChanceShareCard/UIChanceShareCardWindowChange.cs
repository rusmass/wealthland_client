using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
namespace Client.UI
{
	public partial class UIChanceShareCardWindow
	{
		private void _OnInitChange(GameObject go)
		{

			img_chagne=go.GetComponentEx<Image>(Layout.img_changeImg);
			_normalBoard = go.DeepFindEx (Layout.tran_normalboard);

			btn_closeChange = go.GetComponentEx<Button> (Layout.btn_closechange);
			btn_chagnesure = go.GetComponentEx<Button> (Layout.btn_surechange);
			btn_minus = go.GetComponentEx<Button> (Layout.btn_minuschange);
			btn_plus = go.GetComponentEx<Button> (Layout.btn_pluschange);
			lbchagne = go.GetComponentEx<Text> (Layout.lb_changecode);		

			btn_borrow = go.GetComponentEx<Button> (Layout.btn_borrow);

//			btn_chagnesure.SetActiveEx(false);
			btn_borrow.SetActiveEx(false);

			lb_changeTitle = go.GetComponentEx<Text> (Layout.lb_shareChange);

			_plusPosition=btn_plus.transform.localPosition;
			_minusPosition = btn_minus.transform.localPosition;

			_buyShareItem=new UIChanceShareCardWindowBuy(go.DeepFindEx(Layout.shareBuyContent).gameObject);
			_sharecode  =go.DeepFindEx(Layout.lb_shareCodeTxt);
			_scrollview =go.DeepFindEx(Layout.scrollerview) ;
			_HideChangeBoard ();


			var titleimg = go.GetComponentEx<Image> (Layout.img_sharetitle);
			img_changeTitle = new UIImageDisplay (titleimg);

			var tmpImg = btn_plus.gameObject.GetComponent<Image> ();
			img_plusWord = new UIImageDisplay (tmpImg);

			tmpImg = btn_minus.gameObject.GetComponent<Image> ();
			img_MinusWord = new UIImageDisplay (tmpImg);

//			tmpImg = btn_chagnesure.gameObject.GetComponentEx<Image> ("Image");
//			img_SureWord = new UIImageDisplay (tmpImg);

			lb_currentMoney = go.GetComponentEx<Text> (Layout.lb_currentmoney);
			lb_inputShare = go.GetComponentEx<InputField> (Layout.lb_intputChance);

		}

		private void _OnShowChange()
		{
			EventTriggerListener.Get (btn_closeChange.gameObject).onClick += _SureCloseHandler;
			EventTriggerListener.Get (btn_chagnesure.gameObject).onClick += _SureChangeHandler;
//			EventTriggerListener.Get (btn_minus.gameObject).onClick += _MinusChangeHandler;
//			EventTriggerListener.Get (btn_plus.gameObject).onClick += _PlusChangeHandle;
			EventTriggerListener.Get (btn_borrow.gameObject).onClick += _OnBorrowHandler;

			EventTriggerListener.Get(btn_plus.gameObject).onDown+=_OnDownPlus;
			EventTriggerListener.Get(btn_plus.gameObject).onUp+=_OnUpPlus;

			EventTriggerListener.Get(btn_minus.gameObject).onDown+=_OnDownMinus;
			EventTriggerListener.Get(btn_minus.gameObject).onUp+=_OnUpMinus;

			updatePlayerMoney ();

			lb_inputShare.onEndEdit.AddListener (_OnInputChanceShareEnded);
//			lb_inputShare.;

//			lb_inputShare.OnPointerExit
		}

		public void updatePlayerMoney()
		{
			lb_currentMoney.text = _controller.playerInfor.totalMoney.ToString();
		}

		private void _OnHideChange()
		{
			EventTriggerListener.Get (btn_closeChange.gameObject).onClick -= _SureCloseHandler;
			EventTriggerListener.Get (btn_chagnesure.gameObject).onClick -= _SureChangeHandler;
//			EventTriggerListener.Get (btn_minus.gameObject).onClick -= _MinusChangeHandler;
//			EventTriggerListener.Get (btn_plus.gameObject).onClick -= _PlusChangeHandle;
			EventTriggerListener.Get (btn_borrow.gameObject).onClick -= _OnBorrowHandler;

			EventTriggerListener.Get(btn_plus.gameObject).onDown-=_OnDownPlus;
			EventTriggerListener.Get(btn_plus.gameObject).onUp-=_OnUpPlus;

			EventTriggerListener.Get(btn_minus.gameObject).onDown-=_OnDownMinus;
			EventTriggerListener.Get(btn_minus.gameObject).onUp-=_OnUpMinus;
			lb_inputShare.onEndEdit.RemoveListener (_OnInputChanceShareEnded);

		}


		private void _OnShowBorrow()
		{
			btn_borrow.SetActiveEx (true);
		}

		private void _OnHideBorrow()
		{
			btn_borrow.SetActiveEx (false);
		}

		private void _OnBorrowHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			var _borrowController = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();

			if (GameModel.GetInstance.isPlayNet == false)
			{
				_borrowController.playerInfor = PlayerManager.Instance.HostPlayerInfo;
				_borrowController.setVisible (true);
			}
			else 
			{
				NetWorkScript.getInstance ().GetBorrowInfor ();
			}


			if(_isAddBorrow==false)
			{
				_isAddBorrow = true;
				_leftTime+=_addTime;
			}
			_borrowController.SetTime (_leftTime);

			GameModel.GetInstance.borrowBoardTime = _leftTime;

		}

		private void _OnDisposeChange()
		{
			
			if (null != _wrapGrid)
			{
				_wrapGrid.Dispose();
				_wrapGrid.OnRefreshCell -= _OnRefreshCell;
			}
			if (null != UIShareChangeItem.selectedText) 
			{
				UIShareChangeItem.selectedText = null;
			}

			if (null != img_changeTitle)
			{
				img_changeTitle.Dispose ();
			}

			if (null != img_plusWord) 
			{
				img_plusWord.Dispose ();
			}

			if(null!=img_MinusWord)
			{
				img_MinusWord.Dispose ();
			}

//			if (null != img_SureWord)
//			{
//				img_SureWord.Dispose ();
//			}

		}

		private void _OnUpPlus(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_updatePlus = false;
			if(_updateMinus ==false)
			{
				if (null != _changeTime)
				{				
					_changeTime = null;
				}
			}
		}

		private void _OnDownPlus(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_updateMinus == true)
			{
				_updateMinus = false;
			}

			if (null != _changeTime)
			{
				_changeTime = null;
			}

		 	var canContinue = _PlusChangeHandle (null);

			if (canContinue == true)
			{
				_updatePlus = true;
				waitTime = waitTimeMax;
				_changeTime = new Counter (waitTime);
			}
		}

		private void _OnUpMinus(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_updateMinus = false;
			if (_updatePlus == false)
			{
				if (null != _changeTime)
				{				
					_changeTime = null;
				}
			}			  
		}

		private void _OnDownMinus(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if(_updatePlus==true)
			{
				_updatePlus = false;
			}

			if (null != _changeTime)
			{
				_changeTime = null;
			}

			var canContinue = _MinusChangeHandler (null);

			if (canContinue == true)
			{
				_updateMinus = true;
				waitTime = waitTimeMax;
				_changeTime = new Counter (waitTime);
			}	

		}

		/// <summary>
		/// Hides the plus button.隐藏购买按钮
		/// </summary>
		public void _HidePlusButton()
		{			
//			img_plusWord.Load(_btnMinusPath);
			btn_plus.SetActiveEx (false);
		}

		/// <summary>
		/// Hides the minus button.隐藏卖出按钮
		/// </summary>
		public void _HideMinusButton()
		{
			btn_plus.transform.localPosition=_minusPosition;
			btn_minus.transform.localPosition = _plusPosition;

			img_MinusWord.Load(_btnMinusPath);
			img_plusWord.Load(_btnPlusPath);
			btn_minus.SetActiveEx (false);
		}

		// 确定交易按钮，显示离开
//		private void _SureToLeaveBtn()
//		{
//			img_SureWord.Load(_btnLeavePath);
//		}

		// 确定交易按钮 ，显示确定交易
//		private void _SureToChangeBtn ()
//		{
//			img_SureWord.Load(_btnSurePath);
//		}

		public void _ShowPlusAndMinusButton()
		{
			if (btn_plus.IsActive () == false)
			{
				btn_plus.SetActiveEx (true);
			}

			if (btn_minus.IsActive () == false)
			{
				btn_minus.SetActiveEx (true);
			}

		}

		private void _ShowChangeBoard(bool isBuy=false)
		{
			img_chagne.SetActiveEx (true);
			_normalBoard.SetActiveEx(false);
			_SetClockPositionForChange ();


			_isBuyShare = isBuy;

			_controller.IsBuyShare (isBuy);

			isSimpleLeave = true;

			if(_isBuyShare == false)
			{				
				_buyShareItem.SetActiveEx(false);
				_CreateWrapGrid(lbchagne.gameObject);
				lb_changeTitle.text = "出 售 证 券";
				_HidePlusButton ();
				img_changeTitle.Load (_saleTitlePath);
			}
			else
			{
				_sharecode.SetActiveEx (false);
				_scrollview.SetActiveEx (false);
				_buyShareItem.Refresh(_controller.GetValueByIndex(0));
				lb_changeTitle.text = "购 买 证 券";
				_HideMinusButton ();
				img_changeTitle.Load (_buyTitlePath);
			}

		}


		private void _HideChangeBoard()
		{
			img_chagne.SetActiveEx (false);
		}

		private void _CloseChangeHandler(GameObject go)
		{
			_HideChangeBoard ();
		}

		/// <summary>
		/// Sures the close handler. 确认离开
		/// </summary>
		/// <param name="go">Go.</param>
		private void _SureCloseHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true)
			{
				return;
			}

			_controller.NetQuitGame ();
			_controller.setVisible (false);
			Client.Unit.BattleController.Instance.Send_RoleSelected (0);

		}

		private void _SureChangeHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (_selfQuit == true)
			{
				return;
			}

			if (isSimpleLeave == true)
			{
				_controller.NetQuitGame ();
				_controller.setVisible (false);
				Client.Unit.BattleController.Instance.Send_RoleSelected (0);
				return;
			}


			if (_isBuyShare == false)
			{
				if (null != UIShareChangeItem.selectedText)
				{
					var valueVo = UIShareChangeItem.selectedText.ChangeVo;
//					if(_controller.IsCanBuyShare(valueVo.shareData,valueVo.changeNum))
//					{
						_handleSuccess = true;
						Console.WriteLine ("changeNumeber+++"+valueVo.changeNum);
						_controller.HandlerChangeCardData (valueVo);
						//valueVo.shareData.shareNum += valueVo.changeNum;
						_HideBgImg();

						if (_controller._netSaleList.Count <= 0)
						{
							_controller.NetQuitGame();
						}
						else
						{
							_controller.NetSaleCard (_controller._netSaleList);

						}

						TweenTools.MoveAndScaleTo("chanceshares/Content", "uibattle/top/financementor", _CloseHandler);									
//					}
				}
			} 
			else
			{

				var valueVo = _buyShareItem.ChangeVo;
				if(_controller.IsCanBuyShare(valueVo.shareData,valueVo.changeNum))
				{
					_handleSuccess = true;
					_controller.HandlerChangeCardData (valueVo);
					//valueVo.shareData.shareNum += valueVo.changeNum;
					_HideBgImg();
					if (valueVo.changeNum == 0)
					{
						_controller.NetQuitGame();
					}
					else
					{
						_controller.NetBuyCard (valueVo.changeNum);
//						NetWorkScript.getInstance ().BuyCard (Protocol.Game_BuyChanceShareCard , GameModel.GetInstance.curRoomId, _controller.cardData.id, (int)SpecialCardType.sharesChance, valueVo.changeNum);
					}
					TweenTools.MoveAndScaleTo("chanceshares/Content", "uibattle/top/financementor", _CloseHandler);									
				}
				else
				{
					_controller.NetQuitGame();
					_CloseHandler ();
				}
			}
		}

		private bool _MinusChangeHandler(GameObject go)
		{
			Console.WriteLine ("minuse  ddddd");
//			Audio.AudioManager.Instance.BtnMusic ();

			var canContinue = true;

			if (_isBuyShare == false)
			{
				if (null != UIShareChangeItem.selectedText)
				{
					var valueVo = UIShareChangeItem.selectedText.ChangeVo;
					if(null != _controller.cardData)
					{					
						if (_controller.cardData.income > 0)
						{
							valueVo.changeNum -= 1;
						} 
						else
						{
							valueVo.changeNum -= 100;
						}
						if (valueVo.changeNum <= -valueVo.shareData.shareNum) 
						{
							valueVo.changeNum = -valueVo.shareData.shareNum;
							canContinue = false;
						}
						_ShowPlusAndMinusButton ();
						valueVo.changeMoney =_controller.cardData.payment * valueVo.changeNum;
						UIShareChangeItem.selectedText.Refresh (valueVo);		

						isSimpleLeave = false;

						lb_inputShare.text = Math.Abs(valueVo.changeNum).ToString ();
					}	

				}
			}
			else
			{
				var valueVo =_buyShareItem.ChangeVo;

				if(null != _controller.cardData)
				{					
					if (_controller.cardData.income > 0)
					{
						valueVo.changeNum -= 1;
					} 
					else
					{
						valueVo.changeNum -= 100;
					}
					if (valueVo.changeNum <= -valueVo.shareData.shareNum) 
					{
						valueVo.changeNum = -valueVo.shareData.shareNum;
						_HideMinusButton ();
						isSimpleLeave = true;
						canContinue = false;
					}
					else
					{
						isSimpleLeave = false;
					}
					_OnHideBorrow ();
					valueVo.changeMoney =valueVo.shareData.payment * valueVo.changeNum;
					_buyShareItem.Refresh (valueVo);		

					lb_inputShare.text = Math.Abs(valueVo.changeNum).ToString ();
				}	
			}
			return canContinue;
		}

		/// <summary>
		/// Ons the input chance share ended.输入完成后的处理数据
		/// </summary>
		/// <param name="value">Value.</param>
		private void _OnInputChanceShareEnded(string value)
		{

			//Console.Error.WriteLine ("当前改变文本框啦啦啦啦啦啦");

			if (value == "")
			{
				//Console.Error.WriteLine ("灌灌灌灌");
				return;
			}

			var tarNum = int.Parse (value);

			if (tarNum <= 0)
			{
				tarNum = 0;
			}

			if (_isBuyShare == false)
			{
				if (null != UIShareChangeItem.selectedText)
				{
					var valueVo = UIShareChangeItem.selectedText.ChangeVo;
					valueVo.changeNum = 0;
					if(null != _controller.cardData)
					{					
						if (_controller.cardData.income > 0)
						{
							valueVo.changeNum -= tarNum;
							lb_inputShare.text = tarNum.ToString();

						} 
						else
						{
							var tmpmum = (tarNum / 100) * 100;
							valueVo.changeNum -= tmpmum;
							lb_inputShare.text = tmpmum.ToString();
						}
						if (valueVo.changeNum <= -valueVo.shareData.shareNum) 
						{
							valueVo.changeNum = -valueVo.shareData.shareNum;
							lb_inputShare.text = valueVo.shareData.shareNum.ToString();
						}

						_ShowPlusAndMinusButton ();
						valueVo.changeMoney =_controller.cardData.payment * valueVo.changeNum;
						UIShareChangeItem.selectedText.Refresh (valueVo);		
						isSimpleLeave = false;
					}	

				}
			}
			else
			{
				var valueVo =_buyShareItem.ChangeVo;

				if(null != _controller.cardData)
				{
					if (valueVo.shareData.id == _controller.cardData.id)
					{
						
						if (_controller.cardData.income > 0)
						{
							if (_controller.IsCanBuyShare (valueVo.shareData, valueVo.changeNum + 1))
							{
								valueVo.changeNum = 1;
								valueVo.changeMoney = valueVo.shareData.payment * valueVo.changeNum;
								_buyShareItem.Refresh (valueVo);

								lb_inputShare.text = valueVo.changeNum.ToString();

								_ShowPlusAndMinusButton ();
							}
							else
							{
								_OnShowBorrow ();
							}
						} 
						else
						{
							valueVo.changeNum = 0;

							var tmpshare = (tarNum/100)*100;

							if (_controller.IsCanBuyShare (valueVo.shareData, valueVo.changeNum + tmpshare)) 
							{
								valueVo.changeNum += tmpshare;
								valueVo.changeMoney = valueVo.shareData.payment * valueVo.changeNum;
								Console.WriteLine ("current share num ," + valueVo.changeNum.ToString () + "changeMoney , " + valueVo.changeNum.ToString ());
								_buyShareItem.Refresh (valueVo);
								_ShowPlusAndMinusButton ();
								lb_inputShare.text = valueVo.changeNum.ToString();
							}
							else
							{
								valueVo.changeMoney = valueVo.shareData.payment * valueVo.changeNum;
								Console.WriteLine ("current share num ," + valueVo.changeNum.ToString () + "changeMoney , " + valueVo.changeNum.ToString ());
								_buyShareItem.Refresh (valueVo);
								_ShowPlusAndMinusButton ();
								lb_inputShare.text = valueVo.changeNum.ToString();
								_OnShowBorrow ();
							}
						}
						isSimpleLeave = false;
					}
				}
			}
		}

		private bool _PlusChangeHandle(GameObject go)
		{
//			Audio.AudioManager.Instance.BtnMusic ();

			var canContinue = true;

			if (_isBuyShare == false)
			{
				if (null != UIShareChangeItem.selectedText)
				{
					var valueVo = UIShareChangeItem.selectedText.ChangeVo;
					if (valueVo.changeNum < 0)
					{
						if(null != _controller.cardData)
						{

							if (_controller.cardData.income > 0)
							{
								valueVo.changeNum += 1;
							} 
							else
							{
								valueVo.changeNum += 100;
							}
							if (valueVo.changeNum <= -valueVo.shareData.shareNum) 
							{
								valueVo.changeNum = -valueVo.shareData.shareNum;
								canContinue = false;
							}
						
							valueVo.changeMoney =_controller.cardData.payment * valueVo.changeNum;
							UIShareChangeItem.selectedText.Refresh (valueVo);		
							isSimpleLeave = false;
							lb_inputShare.text = Math.Abs(valueVo.changeNum).ToString ();


//							if (_controller.cardData.income > 0)
//							{							
//								valueVo.changeNum += 1;
//							} 
//							else
//							{								
//								valueVo.changeNum += 100;
//							}
					
						}


						if (valueVo.changeNum == 0)
						{
							_HidePlusButton ();
						}

						if (_controller.IsNonToSale() == true)
						{
							isSimpleLeave = true;
						}
						else
						{
							isSimpleLeave = false;
						}

					}

				}
			}
			else 
			{
				var valueVo =_buyShareItem.ChangeVo;
				if(null != _controller.cardData)
				{
					if (valueVo.shareData.id == _controller.cardData.id)
					{
						if (_controller.cardData.income > 0)
						{
							if (_controller.IsCanBuyShare (valueVo.shareData, valueVo.changeNum + 1))
							{
								valueVo.changeNum = 1;
								valueVo.changeMoney = valueVo.shareData.payment * valueVo.changeNum;
								_buyShareItem.Refresh (valueVo);

								_ShowPlusAndMinusButton ();
							}
							else
							{
								_OnShowBorrow ();
								canContinue = false;
							}
						} 
						else
						{
							if (_controller.IsCanBuyShare (valueVo.shareData, valueVo.changeNum + 100)) 
							{
								valueVo.changeNum += 100;
								valueVo.changeMoney = valueVo.shareData.payment * valueVo.changeNum;
								Console.WriteLine ("current share num ," + valueVo.changeNum.ToString () + "changeMoney , " + valueVo.changeNum.ToString ());
								_buyShareItem.Refresh (valueVo);
								_ShowPlusAndMinusButton ();
							}
							else
							{
								_OnShowBorrow ();
								canContinue = false;
							}
						}
						isSimpleLeave = false;
						lb_inputShare.text = Math.Abs(valueVo.changeNum).ToString ();
//						lb_inputShare.text = valueVo.changeNum.ToString ();
					}
				}

			}

			return canContinue;
		}



		private void _CreateWrapGrid(GameObject go)
		{
			var items = _controller.GetDataList();		

			_wrapGrid = new UIWrapGrid(go, items.Count);

			for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
			{
				var cell = _wrapGrid.Cells[i];

				var tmpChangeItem = new UIShareChangeItem (cell.GetTransform ().gameObject);

				cell.DisplayObject = tmpChangeItem;
				if(i==0)
				{
					tmpChangeItem.InitSelected();
				}
			}

			_wrapGrid.OnRefreshCell += _OnRefreshCell;
			_wrapGrid.Refresh();
		}

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetValueByIndex(index);

			var display = cell.DisplayObject as UIShareChangeItem;

			display.Refresh(value);
		}


		private void _OnChangeShareTick(float delaytime)
		{
			if (null != _changeTime && _changeTime.Increase (delaytime))
			{
				waitTime -= waitTimeMin;

				if (waitTime <= waitTimeMin)
				{
					waitTime = waitTimeMin;
				}

				_changeTime = null;

				_changeTime = new Counter (waitTime);
				
				if(_updatePlus==true)
				{					
					var canContinue =  _PlusChangeHandle (null);
					if (canContinue == false)
					{
						_updatePlus = false;
					}
				}

				if(_updateMinus==true)
				{
					var canContinue= _MinusChangeHandler (null);

					if (canContinue == false)
					{
						_updateMinus = false;
					}

				}
			}
		}

		private bool _isBuyShare = false;

		private Image img_chagne;
		private Transform _normalBoard;

		private UIWrapGrid _wrapGrid;
		private UIChanceShareCardWindowBuy _buyShareItem;
		private Text lb_changeTitle;

		private Text lbchagne;

		private Transform _sharecode;
		private Transform _scrollview;

		private Button btn_minus;
		private Button btn_plus;
		private Button btn_chagnesure;
		private Button btn_closeChange;
		private Button btn_borrow;

		private Vector3 _plusPosition;
		private Vector3 _minusPosition;

		private Counter _changeTime;

		// 长按按钮会连续改变
		private float waitTime=0.5f;
		private float waitTimeMin=0.06f;
		private float waitTimeMax=0.5f;

		// 更新按钮股票数据
		private bool _updatePlus=false;
		private bool _updateMinus=false;

		private UIImageDisplay img_changeTitle;
		private string _buyTitlePath="share/atlas/battle/newcard/gupiaogoumai.ab";
		private string _saleTitlePath="share/atlas/battle/newcard/gupiaochushou.ab";



//		private string _btnLeavePath="share/atlas/battle/newcard/likai.ab";
		private string _btnMinusPath = "share/atlas/battle/newcard/minusshares.ab";
		private string _btnPlusPath = "share/atlas/battle/newcard/plusshares.ab";

		private Text lb_currentMoney;
		private InputField lb_inputShare;

//		private string _btnSurePath = "share/atlas/battle/newcard/queren.ab";

		private UIImageDisplay img_plusWord;
		private UIImageDisplay img_MinusWord;
//		private UIImageDisplay img_SureWord;


		private bool _isSimpleLeave=false;
		// 是否是单纯离开 
		private bool isSimpleLeave
		{
			get
			{
				return _isSimpleLeave; 
			}
			set
			{
				_isSimpleLeave = value;

				if (_isSimpleLeave == false)
				{
//					_SureToChangeBtn ();
				}
				else
				{
//					_SureToLeaveBtn ();
				}

			}
		}
	}
}

