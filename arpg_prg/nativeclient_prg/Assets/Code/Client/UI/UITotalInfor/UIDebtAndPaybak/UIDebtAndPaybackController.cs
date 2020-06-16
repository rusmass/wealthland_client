using System;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 负债与支出的界面
    /// </summary>
	public class UIDebtAndPaybackController:UIController<UIDebtAndPayback,UIDebtAndPaybackController>
	{
		public static bool isDebt=true;

		public UIDebtAndPaybackController ()
		{
			
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/infordebtandpay.ab";
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

		public bool HasDebt()
		{
			var hasDebt = false;

			if (playerInfor.paybackList.Count > 0 ||playerInfor.basePayList.Count > 0)
			{
				hasDebt = true;
				return hasDebt;
			}

			return hasDebt;
		}

		// 基本负债的列表
		public List<PaybackVo> GetBasePayBackList()
		{
			if (null != playerInfor)
			{
				

				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (isDebt == false)
					{
						playerInfor.AddBasePayVoForShow ();
					}
					else
					{
						playerInfor.RemoveBasePayVoForShow ();
					}
					tmpBasicList=playerInfor.basePayList;
				}
				else
				{
					if (isDebt == false)
					{
//						playerInfor.AddBasePayVoForShow ();
						tmpBasicList = playerInfor.netInforDebtAndPay.basicPayList;
					}
					else
					{
//						playerInfor.RemoveBasePayVoForShow ();
						tmpBasicList = playerInfor.netInforDebtAndPay.basicDebtList;
					}
				}


				return tmpBasicList;
			}
			return null;
		}



		public PaybackVo GetBasePayBackByIndex(int index)
		{
//			var values = playerInfor.basePayList;
			var values = tmpBasicList;
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
				if (GameModel.GetInstance.isPlayNet == false)
				{
					tmpAddNewList=playerInfor.paybackList;
				}
				else
				{
					if (isDebt == false)
					{
						//						playerInfor.AddBasePayVoForShow ();
						tmpAddNewList = playerInfor.netInforDebtAndPay.newAddPayList;
					}
					else
					{
						//						playerInfor.RemoveBasePayVoForShow ();
						tmpAddNewList = playerInfor.netInforDebtAndPay.newAddDebtList;
					}
				}


				return tmpAddNewList;
			}
			return null;
		}


		public PaybackVo GetPaybackByIndex(int index)
		{
//			var values = playerInfor.paybackList;
			var values = tmpAddNewList;

			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public void MoveOut()
		{
			var window = _window as UIDebtAndPayback;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			var window = _window as UIDebtAndPayback;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}

		List<PaybackVo> tmpBasicList = null;
		List<PaybackVo> tmpAddNewList=null;

		public PlayerInfo playerInfor;

	}
}

