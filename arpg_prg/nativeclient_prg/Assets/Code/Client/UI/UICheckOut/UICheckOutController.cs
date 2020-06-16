using System;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 结账日信息面板，游戏中未用到
    /// </summary>
	public class UICheckOutController:UIController<UICheckOutWindow,UICheckOutController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/checkout.ab";
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


		public override void Tick (float deltaTime)
		{
			var window=_window as UICheckOutWindow;
			if(null != window &&getVisible()==true)
			{
				window.TickLoad (deltaTime);
			}

		}

		public void initCardPath()
		{
			var template = MetadataManager.Instance.GetTemplateTable<Risk> ();
			var it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Risk;
				pathList.Add(value.cardPath);
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}

			template = MetadataManager.Instance.GetTemplateTable<ChanceFixed> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as ChanceFixed;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}


			template = MetadataManager.Instance.GetTemplateTable<ChanceShares> ();
			it = template.GetEnumerator ();

			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as ChanceShares;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}


			template = MetadataManager.Instance.GetTemplateTable<Opportunity> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Opportunity;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}


			template = MetadataManager.Instance.GetTemplateTable<OuterFate> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as OuterFate;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}


			template = MetadataManager.Instance.GetTemplateTable<Investment> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Investment;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}
		

			template = MetadataManager.Instance.GetTemplateTable<Relax> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as Relax;
				pathList.Add(value.cardPath);
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}
					
			template = MetadataManager.Instance.GetTemplateTable<QualityLife> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as QualityLife;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}
		

			template = MetadataManager.Instance.GetTemplateTable<InnerFate> ();
			it = template.GetEnumerator ();
			while (it.MoveNext ()) 
			{				
				var value = it.Current.Value as InnerFate;
				pathList.Add(value.cardPath);	
				cardNameList.Add (value.title);
				cardIdList.Add (value.id);
			}
		}

//		public void HandlerCardData()
//		{
//			if(null!=playerInfor)
//			{
//				var checkoutMoney=(playerInfor.cashFlow + playerInfor.totalIncome + playerInfor.innerFlowMoney-playerInfor.initCardLoan-playerInfor.initCarLoan-playerInfor.initEducationLoan-playerInfor.initHouseMortgages-playerInfor.initOtherSpend-playerInfor.initAdditionalDebt-playerInfor.initTax-(playerInfor.childNum*playerInfor.oneChildPrise)) ;
//
//				checkoutMoney -= playerInfor.childNum * playerInfor.oneChildPrise;
//
//				playerInfor.totalMoney += checkoutMoney;
//
//				MessageHint.Show (string.Format(SubTitleManager.Instance.subtitle.cardCheckOut,playerInfor.playerName,checkoutMoney.ToString()));
//				playerInfor.checkOutCount++;
//
//				Client.Unit.BattleController.Instance.Send_RoleSelected (1);
//
//				if (PlayerManager.Instance.IsHostPlayerTurn()==true) 
//				{
//					var battleController = Client.UIControllerManager.Instance.GetController<UIBattleController> ();
//					if (null != battleController) 
//					{
//						battleController.SetCashFlow ((int)playerInfor.totalMoney);
//					}
//				}
//			}						
//		}


		public List<string> pathList = new List<string> ();
		public List<string> cardNameList = new List<string> ();
		public List<int> cardIdList = new List<int> ();
		public PlayerInfo playerInfor;
	}
}

