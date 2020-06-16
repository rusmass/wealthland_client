using System;
using UnityEngine;
using UnityEngine.UI;
using Audio;

namespace Client.UI
{
	public partial class UIInnerSelectFreeWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			btn_investment = go.GetComponentEx<Button> (Layout.btn_investment);
			btn_relax = go.GetComponentEx<Button> (Layout.btn_relax);
			btn_quality = go.GetComponentEx<Button> (Layout.btn_quality);
		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get (btn_investment.gameObject).onClick += _OnClickInvestmentHandler;
			EventTriggerListener.Get (btn_relax.gameObject).onClick += _OnClickRelaxHandler;
			EventTriggerListener.Get (btn_quality.gameObject).onClick += _OnClickQualityHandler;
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_investment.gameObject).onClick -= _OnClickInvestmentHandler;
			EventTriggerListener.Get (btn_relax.gameObject).onClick -= _OnClickRelaxHandler;
			EventTriggerListener.Get (btn_quality.gameObject).onClick -= _OnClickQualityHandler;

		}

		private void _OnDisposeCenter()
		{

		}


		private void _OnClickInvestmentHandler(GameObject go)
		{
			_handleSuccess = true;
			AudioManager.Instance.BtnMusic ();
			_ShowInvestmentCard ();
			_controller.setVisible(false);
		}

		private void _OnClickRelaxHandler(GameObject go)
		{
			_handleSuccess = true;
			AudioManager.Instance.BtnMusic ();
			_ShowRelaxCard ();
			_controller.setVisible(false);
		}

		private void _OnClickQualityHandler(GameObject go)
		{
			_handleSuccess = true;
			AudioManager.Instance.BtnMusic ();
			_ShowQualityCard ();
			_controller.setVisible(false);
		}

		private void _ShowInvestmentCard()
		{
			var id = Client.CardOrderHandler.Instance.GetInvestmentCardId();
			if (GameModel.GetInstance.isPlayNet == false)
			{
				CardManager.Instance.OpenCard (id);
			}
			else
			{
				NetWorkScript.getInstance().SendCard(GameModel.GetInstance.curRoomId,id,(int)SpecialCardType.investment);
			}

		}

		private void _ShowRelaxCard()
		{
			var id = Client.CardOrderHandler.Instance.GetRelaxCardId ();
			if(GameModel.GetInstance.isPlayNet==false)
			{
				CardManager.Instance.OpenCard (id);
			}
			else
			{
				NetWorkScript.getInstance().SendCard(GameModel.GetInstance.curRoomId,id,(int)SpecialCardType.richRelax);
			}
		}

		private void _ShowQualityCard()
		{
			var id = Client.CardOrderHandler.Instance.GetQualityCardId ();

			if(GameModel.GetInstance.isPlayNet==false)
			{
				CardManager.Instance.OpenCard (id);
			}
			else
			{
				NetWorkScript.getInstance().SendCard(GameModel.GetInstance.curRoomId,id,(int)SpecialCardType.qualityLife);
			}

		}

		private void _SelfHandler()
		{
			var tmpRandom =UnityEngine.Random.Range(0,120) ;

			if (tmpRandom >80)
			{
				_ShowRelaxCard ();
			}
			else if(tmpRandom>40)
			{
				_ShowInvestmentCard ();
			}
			else
			{
				_ShowQualityCard ();
			}
			_controller.setVisible(false);
		}



		private Button btn_investment;
		private Button btn_relax;
		private Button btn_quality;
		
	}

}

