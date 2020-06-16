using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;

namespace Client.UI
{
	public partial class UIBattleWindow
	{
		private void _InitTop(GameObject go)
		{
            GameModel.GetInstance.MathWidthOrHeightByCondition(go,0);

			_lbQualityScore = go.GetComponentEx<Text> (Layout.quality);
			_lbTimeScore = go.GetComponentEx<Text> (Layout.time );			 
			_lbCashFlow = go.GetComponentEx<Text> (Layout.cashflow );
			_lbNonLaberIncome = go.GetComponentEx<Text> (Layout.nonIncome);

//			_btnQuality = go.GetComponentEx<Button> (Layout.quality);
//			_btnTime = go.GetComponentEx<Button> (Layout.time);
//			_btnCashFlow = go.GetComponentEx<Button> (Layout.cashflow);
//			_btnSettings = go.GetComponentEx<Button> (Layout.btn_settings);
			_incomeProgress = go.GetComponentEx<Image> (Layout.bar_income+Layout.img_progressbar);
			_timeProgress = go.GetComponentEx<Image> (Layout.bar_time+Layout.img_progressbar);
			_qualityProgress = go.GetComponentEx<Image> (Layout.bar_quality+Layout.img_progressbar);

//			_timeBg = go.GetComponentEx<Image> (Layout.bar_time);
//			_qualityBg = go.GetComponentEx<Image> (Layout.bar_quality);
			_incomeProgress.fillAmount = 0;
			_timeProgress.fillAmount = 0;
			_qualityProgress.fillAmount = 0;
//			_btnPigIncome=go.GetComponentEx<Button>(Layout.bar_income);
//			_btnPigQuality = go.GetComponentEx<Button> (Layout.bar_quality);
//			_btnPigTime = go.GetComponentEx<Button> (Layout.bar_time);
//			_btnQualityProgress = go.GetComponentEx<Button> (Layout.bar_quality);
//			_btnTimeProgress = go.GetComponentEx<Button> (Layout.bar_time);

		}

		private void _OnTopShow()
		{
//			EventTriggerListener.Get(_btnQuality.gameObject).onClick += _QualityBtnClick;
//			EventTriggerListener.Get(_btnTime.gameObject).onClick += _TimeBtnClick;
//			EventTriggerListener.Get(_btnNonLaberIncome.gameObject).onClick += _NonLaberIncomeBtnClick;
//			EventTriggerListener.Get(_btnCashFlow.gameObject).onClick += _CashFlowBtnClick;
//			EventTriggerListener.Get(_btnSettings.gameObject).onClick += _SettingsBtnClick;
//			EventTriggerListener.Get(_btnPigIncome.gameObject).onClick+=_SetPigIncomeTip;
//			EventTriggerListener.Get(_btnPigQuality.gameObject).onClick+=_SetPigQualityTip;
//			EventTriggerListener.Get(_btnPigTime.gameObject).onClick+=_SetPigTimeTip;

			if (GameModel.GetInstance.isPlayNet == true)
			{
				GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
			}
			else
			{
				GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameSingleGameState;
			}

			_lbQualityScore.text = "0";
			_lbTimeScore.text = "0";
			_lbCashFlow.text = "0";
			_lbNonLaberIncome.text = "0";

			var playerInfor = PlayerManager.Instance.HostPlayerInfo;
			if (null!=playerInfor) 
			{
				SetCashFlow ((int)playerInfor.totalMoney);
				_controller.SetNonLaberIncome((int)playerInfor.totalIncome,(int)playerInfor.MonthPayment);
			}

			_timeProgress.SetActiveEx (false);
			_qualityProgress.SetActiveEx (false);

			var _effectController = UIControllerManager.Instance.GetController<UIGameEffectController> ();
			_effectController.setVisible (true);
//			_timeBg.SetActiveEx (false);
//			_qualityBg.SetActiveEx (false);
		}

	
        /// <summary>
        /// 未引用
        /// </summary>
        /// <param name="go"></param>
		private void _SetPigQualityTip(GameObject go)
		{
			if (null != _tipTime)
			{
				_tipTime.Reset ();
			}
			else
			{
				_tipTime = new Counter (_tipShowTime);
			}
		}
        /// <summary>
        /// 未引用
        /// </summary>
        /// <param name="go"></param>
		private void _SetPigTimeTip(GameObject go)
		{			
			if (null != _tipTime)
			{
				_tipTime.Reset ();

			}
			else
			{
				_tipTime = new Counter (_tipShowTime);
			}
		}

        /// <summary>
        /// 设置非劳务收入的进度条
        /// </summary>
        /// <param name="value"></param>
		private void _SetIncomeBarPercent(float value)
		{
			if (value < 0)
			{
				value = 0;
			}

			if (value > 1)
			{
				value = 1;
			}
			
			_incomeProgress.fillAmount = value;
		}

        /// <summary>
        /// 设置时间积分的进度条
        /// </summary>
        /// <param name="value"></param>
		private void _SetTimeBarPercent(float value)
		{
			if (value < 0)
			{
				value = 0;
			}

			if (value > 1)
			{
				value = 1;
			}
			if (_timeProgress.IsActive()==false)
			{				
				_timeProgress.SetActiveEx (true);
			}
			_timeProgress.fillAmount = value;

		}

        /// <summary>
        /// 设置品质积分的进度条
        /// </summary>
        /// <param name="value"></param>
		private void _SetQualityBarPercent(float value)
		{

			if (value < 0)
			{
				value = 0;
			}

			if (value > 1)
			{
				value = 1;
			}
			
			if (_qualityProgress.IsActive()==false)
			{
				_qualityProgress.SetActiveEx (true);
			}
			_qualityProgress.fillAmount = value;
		}	

		private void _OnTopHide()
		{
//			EventTriggerListener.Get(_btnQuality.gameObject).onClick -= _QualityBtnClick;
//			EventTriggerListener.Get(_btnTime.gameObject).onClick -= _TimeBtnClick;
//			EventTriggerListener.Get(_btnNonLaberIncome.gameObject).onClick -= _NonLaberIncomeBtnClick;
//			EventTriggerListener.Get(_btnCashFlow.gameObject).onClick -= _CashFlowBtnClick;
//			EventTriggerListener.Get(_btnSettings.gameObject).onClick -= _SettingsBtnClick;
//			EventTriggerListener.Get(_btnPigIncome.gameObject).onClick-=_SetPigIncomeTip;
//			EventTriggerListener.Get(_btnPigQuality.gameObject).onClick-=_SetPigTimeTip;
//			EventTriggerListener.Get(_btnPigTime.gameObject).onClick-=_SetPigTimeTip;

			playerCurretMoney = -1;
		}

        /// <summary>
        /// 设置品质积分
        /// </summary>
        /// <param name="score"></param>
        /// <param name="tarScore"></param>
		public void SetQualityScore(int score,float tarScore)
        {
			if (PlayerManager.Instance.HostPlayerInfo.isEnterInner == true)
			{
				_lbQualityScore.SetTextEx(string.Format("{0}/{1}",score.ToString(),tarScore.ToString()));
			}
			else
			{
				_lbQualityScore.SetTextEx (score.ToString());
			}
			

			if(tarScore>0)
			{				
				var tmpPercent=score/tarScore;
				_SetQualityBarPercent(tmpPercent);
			}

        }

        /// <summary>
        /// 设置时间积分
        /// </summary>
        /// <param name="score"></param>
        /// <param name="tarScore"></param>
		public void SetTimeScore(float score,float tarScore)
        {
			if (PlayerManager.Instance.HostPlayerInfo.isEnterInner == true)
			{
				_lbTimeScore.SetTextEx(string.Format("{0}/{1}",score.ToString(),tarScore.ToString()));
			}
			else
			{
				_lbTimeScore.SetTextEx (score.ToString());
			}


			if (tarScore > 0)
			{				
				var tmpPercent =score/tarScore;
				_SetTimeBarPercent (tmpPercent);
			}

        }

        /// <summary>
        /// 设置非劳务收入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tarScore"></param>
		public void SetNonLaberIncome(float value,int tarScore)
        {
			
			_lbNonLaberIncome.SetTextEx (string.Format("{0}/{1}",value.ToString(),tarScore.ToString()));

			if(tarScore>0)
			{
				
				var tmpPercent=value/tarScore;
				_SetIncomeBarPercent(tmpPercent); 
			}            
        }

        /// <summary>
        /// 设置现金数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tarscore"></param>
        /// <param name="mUpdate"></param>
		public void SetCashFlow(float value,int tarscore=-1,bool mUpdate = true)
        {
			if (playerCurretMoney <= 0)
			{
				playerCurretMoney = value;
			}
			else
			{
				if (playerCurretMoney < value)
				{
//					var _initWorldPosition = Room.Instance.GetCurrentPlayerByIndex(0).getPlayPos ().position;
//					var tmpPosition =Camera.main.WorldToScreenPoint(new Vector3(_initWorldPosition.x,_initWorldPosition.y+2,_initWorldPosition.z));

					UIControllerManager.Instance.GetController<UIGameEffectController> ().AddMoneyEffect (0,new Vector3(0,0,0));		

					if(mUpdate == true)
					{
						var controller = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
						controller.mStart = true;
						if (null != Room.Instance.getCurrentPlay ())
						{
							controller.SetHeadText(Room.Instance.getCurrentPlay().getPlayPos(),value - playerCurretMoney);
						}

					}
				}
				else if(playerCurretMoney > value)
				{
					if(mUpdate == true)
					{
						var controller = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
						controller.mStart = true;

						if (null != Room.Instance.getCurrentPlay ())
						{
							controller.SetHeadText(Room.Instance.getCurrentPlay().getPlayPos(),value - playerCurretMoney);
						}
					}
				}

				playerCurretMoney = value;
			}		
			
			var tmpStr = HandleStringTool.HandleMoneyTostring(value);  
			_lbCashFlow.SetTextEx(tmpStr);
        }


        private void _QualityBtnClick(GameObject go)
		{
			Console.WriteLine ("!!!!!");
		}

		private void _TimeBtnClick(GameObject go)
		{
			Console.WriteLine ("!!!!!");
		}

		private void _NonLaberIncomeBtnClick(GameObject go)
		{
			Console.WriteLine ("!!!!!");
		}

		private void _CashFlowBtnClick(GameObject go)
		{
			Console.WriteLine ("!!!!!");
		}

		private void _SettingsBtnClick(GameObject go)
		{
			Console.WriteLine ("!!!!!");
		}

        /// <summary>
        /// 进入内圈
        /// </summary>
		public void EnterInner()
		{
			this._topEnterInner ();
//			this._CenterEnterEnner ();

		}

		private void _topEnterInner ()
		{
//			_btnNonLaberIncome.SetActiveEx (false);

//			_timeBg.SetActiveEx (true);
//			_qualityBg.SetActiveEx (true);

			_qualityProgress.SetActiveEx(true);
			_timeProgress.SetActiveEx (true);

		}

		private Text _lbQualityScore;
		private Text _lbTimeScore;
		private Text _lbNonLaberIncome;
		private Text _lbCashFlow;

		private float playerCurretMoney=-1;

//		private Button _btnQuality;
//		private Button _btnTime;
//		private Button _btnNonLaberIncome;
//		private Button _btnCashFlow;
//		private Button _btnSettings;

		private Image _incomeProgress;
		private Image _timeProgress;
		private Image _qualityProgress;

//		private Button _btnPigIncome;
//		private Button _btnPigQuality;
//		private Button _btnPigTime;

		///ytf20161013 控制时间进度条的显示隐藏。现修改一直存在。因为背景图同时问文字的地板
//		private Image _timeBg;
//		private Image _qualityBg;

//		private Button _btnTimeProgress;
//		private Button _btnQualityProgress;

		private Counter _tipTime;
		private float _tipShowTime=2f;


	}
}

