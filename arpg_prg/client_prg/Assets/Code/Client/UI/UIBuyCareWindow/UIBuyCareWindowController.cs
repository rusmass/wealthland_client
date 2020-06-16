using System;
using Metadata;

namespace Client.UI
{
	public class UIBuyCareWindowController:UIController<UIBuyCareWindow,UIBuyCareWindowController>
	{
		public UIBuyCareWindowController ()
		{
		}

		/// <summary>
		/// 窗口预设  在  重复使用
		/// </summary>
		/// <value>The window resource.</value>
		protected override string _windowResource {
			get 
			{
				return "prefabs/ui/scene/innerbuycare.ab";				
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

		public void QuitCard()
		{

		}

		public bool HandlerCardData()
		{
			var canHandle = false;

			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
			var heroInfor=PlayerManager.Instance.Players[turnIndex];
					//保险
			if (heroInfor.totalMoney + paymeny<0)
			{
				Console.WriteLine ("余额不足了");
				MessageHint.Show (SubTitleManager.Instance.subtitle.lackOfGold);

				canHandle = false;
				return canHandle;
			}
			else
			{
				if (GameModel.GetInstance.isPlayNet == false)
				{
					if (heroInfor.InsuranceList.Count > 0)
					{
						MessageHint.Show ("您已经购买保险,不能重复购买");
						return false;
					}
					MessageHint.Show ("购买保险成功");
					heroInfor.totalMoney+=paymeny;
					heroInfor.totalPayment -= paymeny;
					canHandle = true;
					//				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.innerFateSafe2,heroInfor.playerName,title),null,true);
					heroInfor._buyCareNum+=1;
					heroInfor.InsuranceList.Add (90001);

					var borrowController = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
					borrowController.UpdateBorrowBoardMoney ();

				}
				else if (GameModel.GetInstance.isPlayNet == true)
				{
					NetWorkScript.getInstance ().Game_BuyEnsurance (GameModel.GetInstance.curRoomId,Math.Abs((int)paymeny));
				}
			}

			if (GameModel.GetInstance.isPlayNet == false)
			{
				var battleController = UIControllerManager.Instance.GetController<UIBattleController>();
				if(null!=battleController)
				{
					if (PlayerManager.Instance.IsHostPlayerTurn ()) 
					{						
						battleController.SetQualityScore ((int)heroInfor.qualityScore,heroInfor.targetQualityScore);
						battleController.SetTimeScore ((int)heroInfor.timeScore,heroInfor.targetTimeScore);
						battleController.SetNonLaberIncome ((int)heroInfor.CurrentIncome,(int)heroInfor.TargetIncome);
						battleController.SetCashFlow ((int)heroInfor.totalMoney,heroInfor.TargetIncome);
					}
					else 
					{
						battleController.SetPersonInfor (heroInfor,turnIndex);
					}
				}
			}


			return canHandle;
		}

		public string title="30W购买一份综合保险";
		public string desc="30W购买一份综合保险";
		public float paymeny=-300000f;
		public string cardPath="share/atlas/battle/card/innercard1/card_e_3.ab";

		public int crapNum=1;
	}
}

