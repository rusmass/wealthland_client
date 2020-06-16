using System;
using UnityEngine;
using UnityEngine.UI;
using Audio;


namespace Client.UI
{
	public partial  class UIChanceSelectWindow
	{

		private void _OnInitCenter(GameObject go)
		{
			btn_opportunity = go.GetComponentEx<Button> (Layout.btn_opportunity);
			btn_chance = go.GetComponentEx<Button> (Layout.btn_chance);
		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get (btn_opportunity.gameObject).onClick += _OnClickOpportunityHandler;
			EventTriggerListener.Get (btn_chance.gameObject).onClick += _OnClickChanceHandler;
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_opportunity.gameObject).onClick -= _OnClickOpportunityHandler;
			EventTriggerListener.Get (btn_chance.gameObject).onClick -= _OnClickChanceHandler;
		}

		private void _OnDisposeCenter()
		{
			
		}


		private void _OnClickOpportunityHandler(GameObject go)
		{
			_handleSuccess = true;
			AudioManager.Instance.BtnMusic ();
			_ShowOpprotunityCard ();
			_controller.setVisible(false);
		}

		private void _OnClickChanceHandler(GameObject go)
		{
			_handleSuccess = true;
			AudioManager.Instance.BtnMusic ();
			_ShowChanceCard ();
			_controller.setVisible(false);
		}

		private void _ShowOpprotunityCard()
		{
//			int[] array;
//			var list1 = CardManager.Instance.outerOpportunityList;
//			var list2 = CardManager.Instance.outerChanceList;
//			array = new int[list1.Count + list2.Count];
//			list1.CopyTo (array, 0);
//			list2.CopyTo (array, list1.Count);				
//
//			var id = MathUtility.Random(array);
			var id = Client.CardOrderHandler.Instance.GetOpportunityCardId();
			if (GameModel.GetInstance.isPlayNet == false)
			{
				
				CardManager.Instance.OpenCard (id);
			} 
			else
			{
				NetWorkScript.getInstance().SendCard(GameModel.GetInstance.curRoomId,id,(int)SpecialCardType.bigChance);
			}


		}

		private void _ShowChanceCard()
		{
//			int[] array;
//			var list = CardManager.Instance.outerChanceList;
//			array = new int[list.Count];
//
//			list.CopyTo (array);
//
//			var id = MathUtility.Random(array);
			var id= Client.CardOrderHandler.Instance.GetChanceCardId();
			if (GameModel.GetInstance.isPlayNet == false)
			{
				
				CardManager.Instance.OpenCard (id);
			}
			else
			{
				var cardType = 0;
//
//				if (CardManager.Instance.outerChanceShares.Contains (id))
//				{
//					cardType = (int)SpecialCardType.sharesChance;
//				}
//				else if(CardManager.Instance.outerChanceFixed.Contains(id))
//				{
//					cardType = (int)SpecialCardType.fixedChance;
//				}

				cardType = (int)SpecialCardType.smallChance;
				NetWorkScript.getInstance().SendCard(GameModel.GetInstance.curRoomId,id,cardType);
			}


		}

		private void _SelfHandler()
		{
			var tmpRandom =UnityEngine.Random.Range(0,100) ;

			if (tmpRandom < 60)
			{
				_ShowChanceCard ();
			}
			else
			{
				_ShowOpprotunityCard ();
			}
			_controller.setVisible(false);
		}



		private Button btn_opportunity;
		private Button btn_chance;
	}
}

