using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Metadata;


namespace Client.UI
{
	public partial class UIEventLogWindow 
	{
		private void _OnInitEventLog(GameObject go)
		{

			_btnBig = go.GetComponentEx<Button>(Layout.btn_Big);
			_btnSmall = go.GetComponentEx<Button>(Layout.btn_Small);
			_btnRisk = go.GetComponentEx<Button>(Layout.btn_Risk);
			_btnFate = go.GetComponentEx<Button>(Layout.btn_fate);
			_btnInvestment = go.GetComponentEx<Button>(Layout.btn_Investment);
			_btnRichLeisure = go.GetComponentEx<Button>(Layout.btn_RichLeisure);
			_btnQuality = go.GetComponentEx<Button>(Layout.btn_Quality);
			_btnLook = go.GetComponentEx<Button>(Layout.btn_Look);
			_btnClose = go.GetComponentEx<Button>(Layout.btn_close);

			_objItem = go.GetComponentEx<Image>(Layout.obj_Item);
			_objContent = go.GetComponentEx<GridLayoutGroup>(Layout.obj_content);

			_imgQuality = go.GetComponentEx<Image>(Layout.img_Quality);
			_imgInvestment = go.GetComponentEx<Image>(Layout.img_Investment);
			_imgfate  = go.GetComponentEx<Image>(Layout.img_fate);
			_imgRisk = go.GetComponentEx<Image>(Layout.img_Risk);
			_imgSmall = go.GetComponentEx<Image>(Layout.img_Small);
			_imgBig = go.GetComponentEx<Image>(Layout.img_Big);
			_imgRichLeisure = go.GetComponentEx<Image>(Layout.img_RichLeisure);

		}


		private void _OnBtnShow()
		{
			if(_controller.kindStr == "buildFateCell")
			{
				this.buildFateCell();
			}
			else if(_controller.kindStr == "buildRiskCell")
			{
				this.buildRiskCell();
			}
			else if(_controller.kindStr == "buildOpportunityCell")
			{
				this.buildOpportunityCell();
			}
			else if(_controller.kindStr == "buildInvestmentCell")
			{
				this.buildInvestmentCell();
			}
			else if(_controller.kindStr == "buildQualityLifeCell")
			{
				this.buildQualityLifeCell();
			}
			else if(_controller.kindStr == "buildRelaxkCell")
			{
				this.buildRelaxkCell();
			}
			else if(_controller.kindStr == "buildChanceCell")
			{
				this.buildChanceCell();
			}

			EventTriggerListener.Get(_btnBig.gameObject).onClick += _OnBtnBigClick;
			EventTriggerListener.Get(_btnSmall.gameObject).onClick += _OnBtnSmallClick;
			EventTriggerListener.Get(_btnFate.gameObject).onClick += _OnBtnFateClick;
			EventTriggerListener.Get(_btnInvestment.gameObject).onClick += _OnBtnInvestmentClick;
			EventTriggerListener.Get(_btnLook.gameObject).onClick += _OnBtnLookClick;
			EventTriggerListener.Get(_btnQuality.gameObject).onClick += _OnBtnQualityClick;
			EventTriggerListener.Get(_btnRichLeisure.gameObject).onClick += _OnBtnRichLeisureClick;
			EventTriggerListener.Get(_btnRisk.gameObject).onClick += _OnBtnRiskClick;
			EventTriggerListener.Get(_btnClose.gameObject).onClick += _OnBtnCloseClick;

			setBtnState();
		}

		private void _OnHideButton()
		{
			EventTriggerListener.Get(_btnBig.gameObject).onClick -= _OnBtnBigClick;
			EventTriggerListener.Get(_btnSmall.gameObject).onClick -= _OnBtnSmallClick;
			EventTriggerListener.Get(_btnFate.gameObject).onClick -= _OnBtnFateClick;
			EventTriggerListener.Get(_btnInvestment.gameObject).onClick -= _OnBtnInvestmentClick;
			EventTriggerListener.Get(_btnLook.gameObject).onClick -= _OnBtnLookClick;
			EventTriggerListener.Get(_btnQuality.gameObject).onClick -= _OnBtnQualityClick;
			EventTriggerListener.Get(_btnRichLeisure.gameObject).onClick -= _OnBtnRichLeisureClick;
			EventTriggerListener.Get(_btnRisk.gameObject).onClick -= _OnBtnRiskClick;
			EventTriggerListener.Get(_btnClose.gameObject).onClick -= _OnBtnCloseClick;
		}

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnCloseClick(GameObject go)
		{
			_controller.setVisible(false);
		}

        /// <summary>
        /// 点击大机会
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnBigClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildOpportunityCell();
		}
        /// <summary>
        /// 点击小机会
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnSmallClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildChanceCell();
		}
        /// <summary>
        /// 点击风险卡牌
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnRiskClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildRiskCell();
		}
        /// <summary>
        /// 点击查看命运卡牌信息
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnFateClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildFateCell();
		}
        /// <summary>
        /// 点击查看投资信息
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnInvestmentClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildInvestmentCell();
		}
        /// <summary>
        /// 点击查看有钱有闲信息
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnRichLeisureClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildRelaxkCell();
		}
        /// <summary>
        /// 点击查看品质积分
        /// </summary>
        /// <param name="go"></param>
		private void _OnBtnQualityClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			buildQualityLifeCell();
		}

		private void _OnBtnLookClick(GameObject go)
		{
			_objItem.SetActiveEx(true);
			setUpLookCell();
		}
	

		public void buildRiskCell()
		{
			setBoolInit();
			m_bool_Risk = true;
			SetImageState(_imgRisk);
			_createCurrentRiskCell(_objItem.gameObject);
		}

		public void buildOpportunityCell()
		{
			setBoolInit();
			m_bool_Big = true;
			SetImageState(_imgBig);
			_createCurrentOpportunityCell(_objItem.gameObject);
		}

		public void buildFateCell()
		{
			setBoolInit();
			m_bool_Fate = true;
			SetImageState(_imgfate);
			_createCurrentFateCell(_objItem.gameObject);
		}

		public void buildInvestmentCell()
		{
			setBoolInit();
			m_bool_Investment = true;
			SetImageState(_imgInvestment);
			_createCurrentInvestmentCell(_objItem.gameObject);
		}

		public void buildQualityLifeCell()
		{
			setBoolInit();
			m_bool_Quality = true;
			SetImageState(_imgQuality);
			_createCurrentQualityLifeCell(_objItem.gameObject);
		}

		public void buildRelaxkCell()
		{
			setBoolInit();
			m_bool_RichLeisure = true;
			SetImageState(_imgRichLeisure);
			_createCurrentRelaxCell(_objItem.gameObject);
		}

		public void buildChanceCell()
		{
			setBoolInit();
			m_bool_Small = true;
			SetImageState(_imgSmall);
			_createCurrentChangeCell(_objItem.gameObject);
		}

		void _createCurrentChangeCell(GameObject go)
		{

			cellDestory();

			chanceAllList = CardManager.Instance.chanceAllList;
			foreach(Chance r in chanceAllList)
			{
				foreach(int i in CardManager.Instance.chanceIDList)
				{
					if(i == r.id)
					{
						currentChanceList.Add(r);
					}
				}
			}

			if(currentChanceList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentChanceList.Count ; i++)
				{
					
						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshChance(currentChanceList[i]);
							cell.setBoolInit();
							cell.m_bool_Small = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshChance (currentChanceList[i]);
							cell.setBoolInit();
							cell.m_bool_Small = true;
							cellObj.Add(tmpObj);
						}
					}
				}
		}


		void _createCurrentRiskCell(GameObject go)
		{

			cellDestory();

			outerRiskAllList = CardManager.Instance.outerRiskAllList;
			foreach(Risk r in outerRiskAllList)
			{
				foreach(int i in CardManager.Instance.riskIDList)
				{
					if(i == r.id)
					{
						currentRiskList.Add(r);
					}
				}
			}

			if(currentRiskList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentRiskList.Count ; i++)
				{
						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshRisk(currentRiskList[i]);
							cell.setBoolInit();
							cell.m_bool_Risk = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshRisk (currentRiskList[i]);
							cell.setBoolInit();
							cell.m_bool_Risk = true;
							cellObj.Add(tmpObj);
						}
				 }
			}
		}

		void _createCurrentFateCell(GameObject go)
		{

			cellDestory();

			FateAllList = CardManager.Instance.FateAllList;
			foreach(Fate r in FateAllList)
			{
				foreach(int i in CardManager.Instance.fateIDList)
				{
					if(i == r.id)
					{
						currentFateList.Add(r);
					}
				}
			}

			if(currentFateList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentFateList.Count ; i++)
				{

						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshFate(currentFateList[i]);
							cell.setBoolInit();
							cell.m_bool_Fate = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshFate (currentFateList[i]);
							cell.setBoolInit();
							cell.m_bool_Fate = true;
							cellObj.Add(tmpObj);
						}
					}
				}
		}

		void _createCurrentRelaxCell(GameObject go)
		{

			cellDestory();

			innerRelaxkAllList = CardManager.Instance.innerRelaxkAllList;
			foreach(Relax r in innerRelaxkAllList)
			{
				foreach(int i in CardManager.Instance.relaxkIDList)
				{
					if(i == r.id)
					{
						currentRelaxkList.Add(r);
					}
				}
			}
			if(currentRelaxkList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentRelaxkList.Count ; i++)
				{
					
						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshRichLeisure(currentRelaxkList[i]);
							cell.setBoolInit();
							cell.m_bool_RichLeisure = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshRichLeisure (currentRelaxkList[i]);
							cell.setBoolInit();
							cell.m_bool_RichLeisure = true;
							cellObj.Add(tmpObj);
						}
					}
				}
		}


		void _createCurrentQualityLifeCell(GameObject go)
		{

			cellDestory();

			innerQualityLifeAllList = CardManager.Instance.innerQualityLifeAllList;
			foreach(QualityLife r in innerQualityLifeAllList)
			{
				foreach(int i in CardManager.Instance.qualityLifeIDList)
				{
					if(i == r.id)
					{
						currentQualityLifeList.Add(r);
					}
				}
			}

			if(currentQualityLifeList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentQualityLifeList.Count ; i++)
				{
					
						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshQualityLife(currentQualityLifeList[i]);
							cell.setBoolInit();
							cell.m_bool_Quality = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshQualityLife (currentQualityLifeList[i]);
							cell.setBoolInit();
							cell.m_bool_Quality = true;
							cellObj.Add(tmpObj);
						}
					}
				}
		}


		void _createCurrentInvestmentCell(GameObject go)
		{

			cellDestory();

			innerInvestmentAllList = CardManager.Instance.innerInvestmentAllList;
			foreach(Investment r in innerInvestmentAllList)
			{
				foreach(int i in CardManager.Instance.investmentIDList)
				{
					if(i == r.id)
					{
						currentInvestmentList.Add(r);
					}
				}
			}

			if(currentInvestmentList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentInvestmentList.Count ; i++)
				{

						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshInvestment(currentInvestmentList[i]);
							cell.setBoolInit();
							cell.m_bool_Investment = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshInvestment (currentInvestmentList[i]);
							cell.setBoolInit();
							cell.m_bool_Investment = true;
							cellObj.Add(tmpObj);
						}
					}
				}
		}


		void _createCurrentOpportunityCell(GameObject go)
		{

			cellDestory();

			outerOpportunityAllList = CardManager.Instance.outerOpportunityAllList;
			foreach(Opportunity r in outerOpportunityAllList)
			{
				foreach(int i in CardManager.Instance.opportunityIDList)
				{
					if(i == r.id)
					{
						currentOpportunityList.Add(r);
					}
				}
			}

			if(currentOpportunityList.Count == 0)
			{
				_objItem.SetActiveEx(false);
			}
			else
			{
				for (int i = 0 ; i < currentOpportunityList.Count ; i++)
				{
						if(i == 0)
						{
							var cell = new UIConclusionWindowCell(go);
							cell.RefreshOuterOpportunity(currentOpportunityList[i]);
							cell.setBoolInit();
							cell.m_bool_Big = true;
						}
						else
						{
							var tmpObj = go.CloneEx ();
							tmpObj.transform.SetParent (go.transform.parent);
							tmpObj.transform.localPosition = go.transform.localPosition;
							tmpObj.transform.localScale = Vector3.one;
							var cell = new UIConclusionWindowCell (tmpObj);
							cell.RefreshOuterOpportunity (currentOpportunityList[i]);
							cell.setBoolInit();
							cell.m_bool_Big = true;
							cellObj.Add(tmpObj);
						}
					}
			}
		}

		private void _createChanceCell(GameObject go)
		{
			cellDestory();

			chanceAllList = CardManager.Instance.chanceAllList;

			listNum = chanceAllList.Count / divisor;
			Debug.LogError(chanceAllList.Count);
			if(chanceAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = chanceAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = chanceAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshChance(chanceAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Small = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshChance (chanceAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Small = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void _createOuterOpportunityCell(GameObject go)
		{
			cellDestory();

			outerOpportunityAllList = CardManager.Instance.outerOpportunityAllList;

			listNum = outerOpportunityAllList.Count / divisor;

			if(outerOpportunityAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = outerOpportunityAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = outerOpportunityAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshOuterOpportunity(outerOpportunityAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Big = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshOuterOpportunity (outerOpportunityAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Big = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void _createOuterRiskCell(GameObject go)
		{

			outerRiskAllList = CardManager.Instance.outerRiskAllList;
			listNum = outerRiskAllList.Count / divisor;

			if(outerRiskAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = outerRiskAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = outerRiskAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshRisk(outerRiskAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Risk = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRisk (outerRiskAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Risk = true;
					cellObj.Add(tmpObj);
				}
			}
		}
			
		private void _createFateCell(GameObject go)
		{
			cellDestory();

			FateAllList = CardManager.Instance.FateAllList;
			listNum = FateAllList.Count / divisor;
			if(outerRiskAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = FateAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = FateAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshFate(FateAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Fate = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshFate (FateAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Fate = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void _createInvestmentCell(GameObject go)
		{
			cellDestory();

			innerInvestmentAllList = CardManager.Instance.innerInvestmentAllList;
			listNum = innerInvestmentAllList.Count / divisor;

			if(outerRiskAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = innerInvestmentAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = innerInvestmentAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshInvestment(innerInvestmentAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Investment = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshInvestment (innerInvestmentAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Investment = true;
					cellObj.Add(tmpObj);
				}
			}
		}


		private void _createQualityCell(GameObject go)
		{
			cellDestory();

			innerQualityLifeAllList = CardManager.Instance.innerQualityLifeAllList;
			listNum = innerQualityLifeAllList.Count / divisor;

			if(outerRiskAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = innerQualityLifeAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = innerQualityLifeAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshQualityLife(innerQualityLifeAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Quality = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshQualityLife (innerQualityLifeAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Quality = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void _createRichLeisureCell(GameObject go)
		{
			cellDestory();

			innerRelaxkAllList = CardManager.Instance.innerRelaxkAllList;
			listNum = innerQualityLifeAllList.Count / divisor;

			if(outerRiskAllList.Count % divisor != 0)
			{
				updateNum = listNum + 1;
				remainder = innerRelaxkAllList.Count % divisor;
			}
			else
			{
				updateNum = listNum;
				remainder = innerRelaxkAllList.Count % divisor;
			}

			for (int i = 0 ; i < 10 ; i++)
			{
				if(i == 0)
				{
					var cell = new UIConclusionWindowCell(go);
					cell.RefreshRichLeisure(innerRelaxkAllList[i]);
					cell.setBoolInit();
					cell.m_bool_RichLeisure = true;
				}
				else
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRichLeisure (innerRelaxkAllList[i]);
					cell.setBoolInit();
					cell.m_bool_RichLeisure = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void UpdateRichLeisureCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRichLeisure (innerRelaxkAllList[i]);
					cell.setBoolInit();
					cell.m_bool_RichLeisure = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRichLeisure (innerRelaxkAllList[i]);
					cell.setBoolInit();
					cell.m_bool_RichLeisure = true;
					cellObj.Add(tmpObj);
				}
			}
		}


		private void UpdateQualityCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshQualityLife (innerQualityLifeAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Quality = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshQualityLife (innerQualityLifeAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Quality = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void UpdateChanceCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshChance (chanceAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Small = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshChance (chanceAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Small = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void UpdateInvestmentCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshInvestment (innerInvestmentAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Investment = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshInvestment (innerInvestmentAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Investment = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void UpdateFateCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshFate (FateAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Fate = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshFate (FateAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Fate = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		private void UpdateOuterOpportunityCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshOuterOpportunity (outerOpportunityAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Big = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshOuterOpportunity (outerOpportunityAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Big = true;
					cellObj.Add(tmpObj);
				}
			}
		}
			
		private void UpdateOuterRiskCell(GameObject go,int index)
		{
			if(updateNum - currentUpdateNum != 1)
			{
				for(int i = 10 * index + 1; i < 10 * (index + 1);i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRisk (outerRiskAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Risk = true;
					cellObj.Add(tmpObj);
				}
			}
			else
			{
				for(int i = 10 * index + 1; i < (10 * index) + remainder;i++)
				{
					var tmpObj = go.CloneEx ();
					tmpObj.transform.SetParent (go.transform.parent);
					tmpObj.transform.localPosition = go.transform.localPosition;
					tmpObj.transform.localScale = Vector3.one;
					var cell = new UIConclusionWindowCell (tmpObj);
					cell.RefreshRisk (outerRiskAllList[i]);
					cell.setBoolInit();
					cell.m_bool_Risk = true;
					cellObj.Add(tmpObj);
				}
			}
		}

		void setUpLookCell()
		{
			if(m_bool_Big == true)
			{
				_createOuterOpportunityCell(_objItem.gameObject);
			}
			else if(m_bool_Small == true)
			{
				_createChanceCell(_objItem.gameObject);
			}
			else if(m_bool_Fate == true)
			{
				_createFateCell(_objItem.gameObject);
			}
			else if(m_bool_Investment == true)
			{
				_createInvestmentCell(_objItem.gameObject);
			}
			else if(m_bool_Quality == true)
			{
				_createQualityCell(_objItem.gameObject);
			}
			else if(m_bool_RichLeisure == true)
			{
				_createRichLeisureCell(_objItem.gameObject);
			}
			else if(m_bool_Risk == true)
			{
				_createOuterRiskCell(_objItem.gameObject);
			}
		}


		void setUpdateCell()
		{
			if(m_bool_Big == true)
			{
				UpdateOuterOpportunityCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_Small == true)
			{
				UpdateChanceCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_Fate == true)
			{
				UpdateFateCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_Investment == true)
			{
				UpdateInvestmentCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_Quality == true)
			{
				UpdateQualityCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_RichLeisure == true)
			{
				UpdateRichLeisureCell(_objItem.gameObject, currentUpdateNum);
			}
			else if(m_bool_Risk == true)
			{
				UpdateOuterRiskCell(_objItem.gameObject, currentUpdateNum);
			}
		}

		/// <summary>
		/// 清除cell
		/// </summary>
		void cellDestory()
		{
			for(int i = 0; i < cellObj.Count;i++)
			{
				if(null != cellObj[i])
				{
					cellObj[i].DestroyEx();
				}
			}

			initNum();

			currentRiskList.Clear();
			currentOpportunityList.Clear();
			currentFateList.Clear();
			currentInvestmentList.Clear();
			currentQualityLifeList.Clear();
			currentRelaxkList.Clear();
			currentChanceList.Clear();
		}

		/// <summary>
		/// 检查是否到底端
		/// </summary>
		public void checkBottom()
		{

			if(currentUpdateNum != updateNum)
			{
				if(_objContent.gameObject.GetComponent<RectTransform>().rect.height - _objContent.transform.localPosition.y <= 325
					&& _objContent.transform.localPosition.y >= 0 )
				{
					setUpdateCell();

					currentUpdateNum += 1;
				}
			}
				
		}

		/// <summary>
		/// 初始化
		/// </summary>
		void initNum()
		{
			listNum = 0;
			currentUpdateNum = 0;
			updateNum = 0;
			_objContent.transform.localPosition = Vector3.zero;
		}

		void setBoolInit()
		{
			m_bool_Big = false;
			m_bool_Small = false;
			m_bool_Risk = false;
			m_bool_Fate = false;
			m_bool_Quality = false;
			m_bool_Investment = false;
			m_bool_RichLeisure = false;
		}

		void SetImageState(Image img)
		{
			_imgQuality.SetActiveEx(false);
			_imgInvestment.SetActiveEx(false);
			_imgfate.SetActiveEx(false);
			_imgRisk.SetActiveEx(false);;
			_imgSmall.SetActiveEx(false);;
			_imgBig.SetActiveEx(false);;
			_imgRichLeisure.SetActiveEx(false);;

			img.SetActiveEx(true);
		}

		void setBtnState()
		{
			if(_controller.m_bool_inner == false)
			{
				_btnInvestment.SetActiveEx(false);
				_btnRichLeisure.SetActiveEx(false);
				_btnQuality.SetActiveEx(false);
			}
			else
			{
				_btnInvestment.SetActiveEx(true);
				_btnRichLeisure.SetActiveEx(true);
				_btnQuality.SetActiveEx(true);
			}
		}

		private Button _btnBig;
		private Button _btnSmall;
		private Button _btnRisk;
		private Button _btnFate;
		private Button _btnInvestment;
		private Button _btnRichLeisure;
		private Button _btnQuality;
		private Button _btnLook;
		private Button _btnClose;

		private Image _imgQuality;
		private Image _imgInvestment;
		private Image _imgfate;
		private Image _imgRisk;
		private Image _imgSmall;
		private Image _imgBig;
		private Image _imgRichLeisure;

		private Image _objItem;
		private UIWrapGrid _objCell;
		private GridLayoutGroup _objContent;

		private List<GameObject> cellObj = new List<GameObject>();

		private List<Risk> outerRiskAllList = new List<Risk>();
		private List<Opportunity> outerOpportunityAllList = new List<Opportunity>();
		private List<Chance> chanceAllList = new List<Chance>();
		private List<Fate> FateAllList = new List<Fate>();

		private List<Investment> innerInvestmentAllList = new List<Investment>();
		private List<QualityLife> innerQualityLifeAllList = new List<QualityLife>();
		private List<Relax> innerRelaxkAllList = new List<Relax>(); 



		private List<Risk> currentRiskList = new List<Risk>();
		private List<Opportunity> currentOpportunityList = new List<Opportunity>();
		private List<Chance> currentChanceList = new List<Chance>();

		private List<Fate> currentFateList = new List<Fate>();
		private List<Investment> currentInvestmentList = new List<Investment>();
		private List<QualityLife> currentQualityLifeList = new List<QualityLife>();
		private List<Relax> currentRelaxkList = new List<Relax>();




		private int updateNum = 0;
		private int listNum = 0;
		private int remainder = 0;
		private int currentUpdateNum = 0;
		private int divisor = 10;

		private bool m_bool_Big;
		private bool m_bool_Small;
		private bool m_bool_Risk;
		private bool m_bool_Fate;
		private bool m_bool_Quality;
		private bool m_bool_Investment;
		private bool m_bool_RichLeisure;
	}
}
