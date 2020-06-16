using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using Core.Web;

namespace Client.UI
{
	public class UIConclusionWindowCell : IDisposable
	{
		public UIConclusionWindowCell (GameObject go)
		{
			_imgPic = go.GetComponentEx<Image> (Layout.img_pic);
			_txtNnum = go.GetComponentEx<Text>(Layout.txt_num);
			_txtTitle = go.GetComponentEx<Text>(Layout.txt_title);

			_btnPic = go.GetComponentEx<Button>(Layout.img_pic);

			EventTriggerListener.Get(_btnPic.gameObject).onClick =_OnBtnPicClick;
		}


	
		private void _OnBtnPicClick(GameObject go)
		{
			OpenCard();
		}


		public void RefreshOuterOpportunity(Opportunity value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			opportunity = value;
		}

		public void RefreshChance(Chance value)
		{
			if(value.cash_belongsTo == 2)
			{
				_txtTitle.text = value.cash_title;
				_txtNnum.text = "1";
				if(null != _imgPic)
				{
					WebManager.Instance.LoadWebItem (value.cash_cardPath, item => {
						using (item)
						{
							_imgPic.sprite = item.sprite;
						}
					});	
				}
			}
			else
			{
				_txtTitle.text = value.title;
				_txtNnum.text = "1";
				if(null != _imgPic)
				{
					WebManager.Instance.LoadWebItem (value.cardPath, item => {
						using (item)
						{
							_imgPic.sprite = item.sprite;
						}
					});	
				}
			}

			chance = value;
		}

		public void RefreshRisk(Risk value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			risk = value;
		}

		public void RefreshFate(Fate value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			fate = value;
		}

		public void RefreshInvestment(Investment value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			investment = value;
		}

		public void RefreshQualityLife(QualityLife value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			qualityLife = value;
		}

		public void RefreshRichLeisure(Relax value)
		{
			_txtTitle.text = value.title;
			_txtNnum.text = "1";
			if(null != _imgPic)
			{
				WebManager.Instance.LoadWebItem (value.cardPath, item => {
					using (item)
					{
						_imgPic.sprite = item.sprite;
					}
				});	
			}

			relax = value;
		}


		void OpenCard()
		{
			if(m_bool_Big == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowBigWindowController> ();
				showBigController.setOpportunity(opportunity);
				showBigController.setVisible (true);
			}
			else if(m_bool_Small == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowSmallWindowController> ();
				showBigController.setChance(chance);
				showBigController.setVisible (true); 
			}
			else if(m_bool_Fate == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowFateWindowController> ();
				showBigController.setFate(fate);
				showBigController.setVisible (true);
			}
			else if(m_bool_Investment == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowInvestmentWindowController> ();
				showBigController.setInvestment(investment);
				showBigController.setVisible (true); 
			}
			else if(m_bool_Quality == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowQualityWindowController> ();
				showBigController.setQualityLife(qualityLife);
				showBigController.setVisible (true); 
			}
			else if(m_bool_RichLeisure == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowRelaxWindowController> ();
				showBigController.setRelax(relax);
				showBigController.setVisible (true); 
			}
			else if(m_bool_Risk == true)
			{
				var showBigController = UIControllerManager.Instance.GetController<UIShowRiskWindowController> ();
				showBigController.setRisk(risk);
				showBigController.setVisible (true); 
			}
		}

		public void setBoolInit()
		{
			m_bool_Big = false;
			m_bool_Small = false;
			m_bool_Risk = false;
			m_bool_Fate = false;
			m_bool_Quality = false;
			m_bool_Investment = false;
			m_bool_RichLeisure = false;
		}


		public void Dispose()
		{
			if (null != _imgPic)
			{
				_imgPic.DestroyEx ();
				_imgPic = null;
			}			
		}

		private Image _imgPic;
		private Text  _txtNnum;
		private Text  _txtTitle;
		private Button _btnPic;

		public bool m_bool_Big;
		public bool m_bool_Small;
		public bool m_bool_Risk;
		public bool m_bool_Fate;
		public bool m_bool_Quality;
		public bool m_bool_Investment;
		public bool m_bool_RichLeisure; 

		private Opportunity opportunity;
		private Risk risk;
		private Chance chance;
		private Fate fate;
		private Investment investment;
		private QualityLife qualityLife;
		private Relax relax;


		class Layout 
		{
			public const string img_pic = "img_pic";
			public const string txt_num = "num";
			public const string txt_title = "lb_title";
		}
	}
}
