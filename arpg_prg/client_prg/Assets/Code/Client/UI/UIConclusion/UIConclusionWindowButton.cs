using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIConclusionWindow
	{
		private void _OnInitButton(GameObject go)
		{

			_btnSure = go.GetComponentEx<Button>(Layout.btn_sure);
			_btnLook = go.GetComponentEx<Button>(Layout.btn_Look);

			_btn_Knowledge = go.GetComponentEx<Button> (Layout.btn_konwledge);
			conclusion = go.DeepFindEx(Layout.txt_UIConclusion).gameObject;

			_btnDetailPinzhi = go.GetComponentEx<Button> (Layout.btn_detailpinzhi);
			_btnDetailChengzhang = go.GetComponentEx<Button> (Layout.btn_detailchengzhang);
			_btnDetailCaishang = go.GetComponentEx<Button> (Layout.btn_detailcaishang);
			_btnDetailFengxian = go.GetComponentEx<Button> (Layout.btn_detailfengxian);
		}
	
		private void _OnShowButton()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick += _OnBtnSureClick;
			EventTriggerListener.Get (_btn_Knowledge.gameObject).onClick += _OnShowAdviceHandler;
			EventTriggerListener.Get(_btnLook.gameObject).onClick += _OnBtnLookClick;

			EventTriggerListener.Get (_btnDetailPinzhi.gameObject).onDown += _ShowDetaiPinzhi;
			EventTriggerListener.Get (_btnDetailPinzhi.gameObject).onUp += _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailChengzhang.gameObject).onDown += _ShowDetailChengzhang;
			EventTriggerListener.Get (_btnDetailChengzhang.gameObject).onUp+= _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailCaishang.gameObject).onDown += _ShowDetailCaishang;
			EventTriggerListener.Get (_btnDetailCaishang.gameObject).onUp += _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailFengxian.gameObject).onDown += _ShowDetailFengXian;
			EventTriggerListener.Get (_btnDetailFengxian.gameObject).onUp += _HideDetailBoard;

			this.m_Within = _controller.mstate;
		}


		private void _OnHideButton()
		{
			EventTriggerListener.Get(_btnSure.gameObject).onClick -= _OnBtnSureClick;
			EventTriggerListener.Get (_btn_Knowledge.gameObject).onClick -= _OnShowAdviceHandler;
			EventTriggerListener.Get(_btnLook.gameObject).onClick -= _OnBtnLookClick;

			EventTriggerListener.Get (_btnDetailPinzhi.gameObject).onDown -= _ShowDetaiPinzhi;
			EventTriggerListener.Get (_btnDetailPinzhi.gameObject).onUp -= _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailChengzhang.gameObject).onDown -= _ShowDetailChengzhang;
			EventTriggerListener.Get (_btnDetailChengzhang.gameObject).onUp-= _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailCaishang.gameObject).onDown -= _ShowDetailCaishang;
			EventTriggerListener.Get (_btnDetailCaishang.gameObject).onUp -= _HideDetailBoard;
			EventTriggerListener.Get (_btnDetailFengxian.gameObject).onDown -= _ShowDetailFengXian;
			EventTriggerListener.Get (_btnDetailFengxian.gameObject).onUp -= _HideDetailBoard;
		}

		private void _OnBtnSureClick(GameObject go)
		{
			Console.WriteLine ("执行的方法~~~~~~~~~~~~~");

			if(m_Within == true)
			{
//				var gameOvercontroller = UIControllerManager.Instance.GetController<UIGameOverWindowController> ();
//				gameOvercontroller.setVisible (true);
				var gameOvercontroller = UIControllerManager.Instance.GetController<UIGameOverWindowController> ();
				gameOvercontroller.ShowOverScene ();
			}
			else
			{

				if (GameModel.GetInstance.isPlayNet == false)
				{
					Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
				}
				else
				{
					NetWorkScript.getInstance ().Send_GameEnterInnerFinished (GameModel.GetInstance.curRoomId);
				}

//				var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
//				controller.playerInfor = _controller.player;
//				controller.setVisible(true);
			}
			_controller.setVisible (false);
		}

		private void _OnBtnBaseAssetsClick(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			controller.playerInfor = _controller.player;
			controller.setVisible (true); 
//			var controller2 = UIControllerManager.Instance.GetController<UIBalanceAndIncomeWindowController> ();
//			controller2.playerInfor = player;
//			controller2.setVisible (true); 
		}

		private void _OnBtnBaseLiabilitiesClick(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			controller.playerInfor = _controller.player;
			controller.setVisible (true); 
//			var controller2 = UIControllerManager.Instance.GetController<UIDebtAndPaybackController> ();
//			controller2.playerInfor = player;
//			controller2.setVisible (true); 
		}

		private void _OnBtnBaseShouRuClick(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			controller.playerInfor = _controller.player;
			controller.setVisible (true); 
//			var controller2 = UIControllerManager.Instance.GetController<UIBalanceAndIncomeWindowController> ();
//			controller2.playerInfor = player;
//			controller2.setVisible (true); 
		}

		private void _OnBtnBaseZhiChuClick(GameObject go)
		{
			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			controller.playerInfor = _controller.player;
			controller.setVisible (true); 
//			var controller2 = UIControllerManager.Instance.GetController<UIDebtAndPaybackController> ();
//			controller2.playerInfor = player;
//			controller2.setVisible (true); 
		}


		private void _OnBtnLookClick(GameObject go)
		{
			var eventLogController = UIControllerManager.Instance.GetController<UIEventLogController>();
			eventLogController.createOpportunityCell();
			eventLogController.setVisible(true);

		}
			
		/// <summary>
		/// Ons the show advice handler. 添加咨询面板
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnShowAdviceHandler(GameObject go)
		{
			if (null != _controller)
			{
				if (_controller.player.isEnterInner == false)
				{
					MessageHint.Show ("内圈结算时可查看知识库");
				}
				else
				{
					var controller=UIControllerManager.Instance.GetController<UIConsultingWindowController>();
					controller.isShowBlackBg = true;
					controller.setVisible (true);
				}
			}
		}


        private int starIndex = 0;

		private void _ShowDetaiPinzhi(GameObject go)
		{
			if (null != _starTip)
			{
                this.starIndex++;
                if(starIndex>=6)
                {
                    starIndex = 1;
                }
                _starNumYunyong = starIndex;
                var tmpTip = GameTipManager.Instance.GetStarTipForPinzhi (_starNumYunyong);
				_starTip.ShowBoardTip (_yunyongArr,_yunyongnumArr,tmpTip,_pathPinzhi);
			}
		}

		private void _ShowDetailChengzhang(GameObject go)
		{
			if (null != _starTip)
			{
                this.starIndex++;
                if (starIndex >= 6)
                {
                    starIndex = 1;
                }
                _starNumChaoyue = starIndex;
                var tmpTip = GameTipManager.Instance.GetStarTipForChengzhang (_starNumChaoyue);
				_starTip.ShowBoardTip (_chaoyueArr,_chaoyueNumArr,tmpTip,_pathChengzhang);
			}
		}

		private void _ShowDetailCaishang(GameObject go)
		{
			if (null != _starTip)
			{
                this.starIndex++;
                if (starIndex >= 6)
                {
                    starIndex = 1;
                }
                _starNumChuangzao = starIndex;
                var tmpTip = GameTipManager.Instance.GetStarTipForCaishang (_starNumChuangzao);
				_starTip.ShowBoardTip (_chuangzaoArr,_chaungzaonumArr,tmpTip,_pathCaishang);
			}
		}


		private void _ShowDetailFengXian(GameObject go)
		{
			if (null != _starTip)
			{
                this.starIndex++;
                if (starIndex >= 6)
                {
                    starIndex = 1;
                }
                _starNumGuanli = starIndex;
                var tmpTip = GameTipManager.Instance.GetStarTipForFengxian (_starNumGuanli);
				_starTip.ShowBoardTip (_guanliArr,_guanlinumArr,tmpTip,_pathFengxian);
			}
		}

		private void _HideDetailBoard(GameObject go)
		{
			if (null != _starTip)
			{
				_starTip.HideBoardTip ();
			}
		}


		private Button _btnSure;
		private Button _btn_Knowledge;
		private Button _btnLook;

		private Button _btnDetailPinzhi;
		private Button _btnDetailChengzhang;
		private Button _btnDetailCaishang;
		private Button _btnDetailFengxian;

		private string _pathPinzhi = "share/atlas/battle/conclusion/pinzhizhishu.ab";
		private string _pathChengzhang = "share/atlas/battle/conclusion/game_billing_information_lingxingzhishu.ab";
		private string _pathCaishang="share/atlas/battle/conclusion/game_billing_information_caishangzhishu.ab";
		private string _pathFengxian = "share/atlas/battle/conclusion/game_billing_information_nijingzhishu.ab";

		private GameObject conclusion;

		public bool m_Within;
	}
}

