using System;
using UnityEngine;
using System.Collections.Generic;

namespace Client.UI
{
	public class UIBorrowWindowController:UIController<UIBorrowWindow,UIBorrowWindowController>
	{		

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiborrow.ab";
			}
		}
		
		protected override void _OnLoad ()
		{
			
		}

		protected override void _OnShow ()
		{
			
		}

		protected override void _OnHide ()
		{
			
		}

		protected override void _Dispose ()
		{
			
		}	

		/// <summary>
		/// Updates the borrow infor.刷新结款面板数据
		/// </summary>
		public void UpdateBorrowInfor()
		{
			if (null != _window && getVisible () == true)
			{
				(_window as UIBorrowWindow).UpdateBoardInfor ();
			}
		}

		public void BorrowMoney()
		{
			if (null != playerInfor) 
			{		
				_netBorrowList.Clear ();

				if(curborrowBank>0)
				{
					if (GameModel.GetInstance.isPlayNet == false)
					{
						playerInfor.totalMoney += curborrowBank;
						playerInfor.bankIncome +=curborrowBank;
						playerInfor.bankLoans += curbankDebt;
					}

					_loanType = 0;
				}
									
				if (curborrowCard > 0)
				{
					if (GameModel.GetInstance.isPlayNet == false)
					{
						playerInfor.totalMoney += curborrowCard;
						playerInfor.creditIncome += curborrowCard;
						playerInfor.creditDebt += curcardDebt;
					}

					_loanType = 1;
				}			   

				if (curborrowBank > 0 && curborrowCard > 0)
				{
					_loanType = 2;
				}
					

				var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
				controller.SetCashFlow ((int)playerInfor.totalMoney,-1,false);

				if (curborrowBank != 0 || curborrowCard != 0)
				{
					var borrowvo = new BorrowVo ();

					borrowvo.bankborrow = curborrowBank;
					borrowvo.bankdebt = curbankDebt;
					borrowvo.cardborrow = curborrowCard;
					borrowvo.carddebt = curcardDebt;
					borrowvo.times = playerInfor.borrowList.Count + 1;

					if (GameModel.GetInstance.isPlayNet == false)
					{
						playerInfor.borrowList.Add (borrowvo);
					}


					var banktip = "10%";
					if (playerInfor.isEnterInner == true)
					{
						banktip = "1%";
					}
					borrowvo.bankRate = banktip;
					borrowvo.cardRate = "3%";

					_netBorrowList.Add (borrowvo);

					if (curborrowBank != 0)
					{
						var payback =new PaybackVo();

						playerInfor.borrowbankTimes++;
						payback.title = string.Format ("银行贷款{0}:",playerInfor.borrowbankTimes);
						payback.borrow = curborrowBank;
						payback.isBank = 1;
						payback.debt = curbankDebt;

						if (GameModel.GetInstance.isPlayNet == false)
						{
							playerInfor.paybackList.Add (payback);
						}

					}

					if (curborrowCard != 0)
					{
						var payback = new PaybackVo ();
						playerInfor.borrowcardTimes++;
						payback.title=string.Format("信用卡透支{0}:",playerInfor.borrowcardTimes);
						payback.borrow = curborrowCard;
						payback.debt = curcardDebt;
						payback.isBank = 0;

						if (GameModel.GetInstance.isPlayNet == false)
						{
							playerInfor.paybackList.Add (payback);
						}

					}


					curborrowBank = 0;
					curbankDebt = 0;
					curborrowCard = 0;
					curcardDebt = 0;
				}

				if(playerInfor.isEnterInner==true)
				{
					controller.SetNonLaberIncome ((int)playerInfor.CurrentIncome,playerInfor.TargetIncome);
				}
				else
				{
					controller.SetNonLaberIncome ((int)playerInfor.totalIncome,(int)playerInfor.MonthPayment);
				}				
			}			
		}

		public bool CanPayBack()
		{
			var canpayBack = true;
			var tmpMoney = 0;

			var tmpList = playerInfor.paybackList;
			for (var i = tmpList.Count - 1; i >= 0; i--)
			{
				var tmpVo = tmpList [i];
				if (tmpVo.isSeleted == true)
				{
					if (tmpVo.isBank == 1)
					{
						tmpMoney += tmpVo.borrow;
					}
					else if(tmpVo.isBank==0)
					{
						tmpMoney += tmpVo.borrow;
					}
				}
			}

			var baseList = playerInfor.basePayList;

			for (var i = baseList.Count - 1; i >= 0; i--)
			{
				var tmpVo = baseList[i];
				if (tmpVo.isSeleted == true)
				{
					tmpMoney += tmpVo.borrow;
				}
			}

			if (playerInfor.totalMoney - tmpMoney >= 0)
			{
				playerInfor.totalMoney -= tmpMoney;
			}
			else
			{
				canpayBack = false;
				MessageHint.Show ("金额不足，还款失败");
			}
			return canpayBack;
		}


		public void payBackHandler()
		{
			var tmpList = playerInfor.paybackList;
			_netPayBackList.Clear ();

			for (var i = tmpList.Count - 1; i >= 0; i--)
			{
				var tmpVo = tmpList [i];
				if (tmpVo.isSeleted == true)
				{
					if (tmpVo.isBank == 1)
					{
						playerInfor.bankIncome -= tmpVo.borrow;
						playerInfor.bankLoans -= tmpVo.debt;
					}
					else if(tmpVo.isBank==0)
					{
						playerInfor.creditIncome -= tmpVo.borrow;
						playerInfor.creditDebt -= tmpVo.debt;
					}
					_netPayBackList.Add(tmpVo);
					tmpList.Remove (tmpVo);
				}
			}


			var baseList = playerInfor.basePayList;

			for (var i = baseList.Count - 1; i >= 0; i--)
			{
				var tmpVo = baseList[i];
				if (tmpVo.isSeleted == true)
				{
					var tmpType = tmpVo.basetype;
					switch (tmpType)
					{
					case (int)BaseDebtType.HouseDebt:
						playerInfor.initHouseMortgages = 0;
						playerInfor.fixedHouseMortgages = 0;
						break;
					case (int)BaseDebtType.EducationDebt:
						playerInfor.initEducationLoan = 0;
						playerInfor.fixedEducation = 0;
						break;
					case (int)BaseDebtType.CarDebt:
						playerInfor.initCarLoan = 0;
						playerInfor.fixedCarLoan = 0;
						break;

					case (int)BaseDebtType.CardDebt:
						playerInfor.initCardLoan = 0;
						playerInfor.fixedCardLoan = 0;
						break;
					case (int)BaseDebtType.AdditionDebt:
						playerInfor.initAdditionalDebt = 0;
						playerInfor.fixedAdditionalDebt = 0;
						break;
					default:
						break;

					}
					_netPayBackList.Add(tmpVo);
					baseList.Remove (tmpVo);
					playerInfor.UptatePaymentData();
				}

			}


			var battleController = UIControllerManager.Instance.GetController<UIBattleController>();

			if (null != battleController)
			{
//				battleController.SetQualityScore ((int)playerInfor.qualityScore);
//				battleController.SetTimeScore ((int)playerInfor.timeScore);
				battleController.SetNonLaberIncome ((int)playerInfor.CurrentIncome,(int)playerInfor.TargetIncome);
				battleController.SetCashFlow ((int)playerInfor.totalMoney,-1,false);										
			}

		}


		public void UpdatePayBackMoney()
		{
			var tmpMoney = 0f;

			var tmpList = playerInfor.paybackList;
			for (var i = tmpList.Count - 1; i >= 0; i--)
			{
				var tmpVo = tmpList [i];
				if (tmpVo.isSeleted == true)
				{
					if (tmpVo.isBank == 1)
					{
						tmpMoney += tmpVo.borrow;
					}
					else if(tmpVo.isBank==0)
					{
						tmpMoney += tmpVo.borrow;
					}
				}
			}

			var baseList = playerInfor.basePayList;

			for (var i = baseList.Count - 1; i >= 0; i--)
			{
				var tmpVo = baseList[i];
				if (tmpVo.isSeleted == true)
				{
					tmpMoney += tmpVo.borrow;
				}
			}

			var window = _window as UIBorrowWindow;

			if (null != window && getVisible ())
			{
				window.UpdatePayBackMoney (tmpMoney);
			}
		}

		// 基本负债的列表
		public List<PaybackVo> GetBasePayBackList()
		{
			if (null != playerInfor)
			{
//				Console.Error.WriteLine ("当前基本负债的数组长度"+playerInfor.basePayList.Count.ToString());
				return playerInfor.basePayList;
			}

			return null;
		}

		public PaybackVo GetBasePayBackByIndex(int index)
		{
			var values = playerInfor.basePayList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		// 获取还款信息的列表
		public List<PaybackVo> GetPaybackList()
		{
			if (null != playerInfor)
			{
				return playerInfor.paybackList;
			}

			return null;
		}


		public PaybackVo GetPaybackByIndex(int index)
		{
			var values = playerInfor.paybackList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}

			return null;
		}


		public List<BorrowVo> GetBorrowRectdList()
		{
			if (null != playerInfor)
			{
				return playerInfor.borrowList;
			}

			return null;
		}

		public BorrowVo GetBorrowDataByIndex(int index)
		{
			var values = playerInfor.borrowList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public void SetBlackBg()
		{
			var window = _window as UIBorrowWindow;

			if (null != window)
			{
				window.SetWindowBlack ();
			}
		}


		public void SetTime(float leftTime)
		{
			var window = _window as UIBorrowWindow;
			if (null != window && this.getVisible() == true)
			{
				window.SetTime (leftTime);
			}
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UIBorrowWindow;

			if (null != window && this.getVisible () == true)
			{
				window.Tick(deltaTime);
			}
		}

		/// <summary>
		/// Updates the borrow board money. 刷新结款面板的人物现金
		/// </summary>
		public void UpdateBorrowBoardMoney()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIBorrowWindow).UpdateBorrowBoardMoney();
			}
		}

		public List<BorrowVo> _netBorrowList = new List<BorrowVo> ();
		public int _loanType = 0;

		public List<PaybackVo> _netPayBackList = new List<PaybackVo> ();

		// 用户的信息
		public PlayerInfo playerInfor;

		public int curborrowBank=0;
		public int curbankDebt=0;

		public int curborrowCard=0;
		public int curcardDebt=0;

		public bool isInitPayback = false;

		public void OnChangeType()
		{
			
		}

	}
}

