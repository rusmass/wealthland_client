using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
    /// <summary>
    /// 玩家头像展示板信息
    /// </summary>
	public class UIBattlePlayerInforItem
	{
		public UIBattlePlayerInforItem (GameObject go)
		{
			go.SetActiveEx (true);

			btn_head = go.GetComponentEx<Button> (go.name);
			lb_playerName = go.GetComponentEx<Text> (Layout.lb_name);
			lb_playerAge = go.GetComponentEx<Text> (Layout.lb_age);

			_roleHead = go.GetComponentEx<RawImage> (Layout.raw_HeadImg);
			raw_headDisplay = new UIRawImageDisplay (_roleHead);

			lb_totalMoney = go.GetComponentEx<Text> (Layout.lb_totalMoney);
			lb_quality = go.GetComponentEx<Text> (Layout.lb_quality);
			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			lb_career=go.GetComponentEx<Text>(Layout.lb_career);
			lb_nonIncome = go.GetComponentEx<Text> (Layout.lb_nonIncome);

			_incomeProgress = go.GetComponentEx<Image> (Layout.img_progressMoney+Layout.img_progressbar);
			_qualityProgress = go.GetComponentEx<Image> (Layout.img_progressQuality+Layout.img_progressbar);
			_timeProgress = go.GetComponentEx<Image> (Layout.img_progressTime+Layout.img_progressbar);

//			_qualityProgressBg = go.GetComponentEx<Image> (Layout.img_progressQuality);
//			_timeProgressBg = go.GetComponentEx<Image> (Layout.img_progressTime);

//			_timeProgressBg.SetActiveEx (false);
//			_qualityProgressBg.SetActiveEx (false);

			_incomeProgress.fillAmount = 0;
			_qualityProgress.fillAmount = 0;
			_timeProgress.fillAmount = 0;
//			var tmpLetter = go.GetComponentEx<Image> (Layout.img_letter);
//			_letterImg = new UIImageDisplay (tmpLetter);
//			img_round = go.GetComponentEx<Image> (Layout.img_npcRound);
			img_offline = go.GetComponentEx<Image> (Layout.img_offline);
			_selfGameObj = go;
		}
        /// <summary>
        /// 获取人物头像	
        /// </summary>
        /// <returns></returns>
        public RawImage GetPlayerHead()
		{
			return _roleHead;
		}

		/// <summary>
		/// Gets the image offline. 获取掉线图标
		/// </summary>
		/// <returns>The image offline.</returns>
		public Image GetImgOffline()
		{
			return img_offline;
		}

        /// <summary>
        /// 获取当前玩家的id
        /// </summary>
        /// <returns></returns>
		public string GetPlayerId()
		{
			if (null != _playerInfor)
			{
				return _playerInfor.playerID;
			}
			return "";
		}

        /// <summary>
        /// 初始化时添加点击事件
        /// </summary>
		public void OnShowItem()
		{
			EventTriggerListener.Get(btn_head.gameObject).onClick+=_OnShowOrHideBoard;
		}

        /// <summary>
        /// 隐藏时移除事件
        /// </summary>
		public void OnHideItem()
		{
			EventTriggerListener.Get(btn_head.gameObject).onClick-=_OnShowOrHideBoard;
		}

        /// <summary>
        /// 设置组件可见
        /// </summary>
        /// <param name="value"></param>
		public void setActivEx(bool value)
		{
			_selfGameObj.SetActiveEx (value);
		}

        /// <summary>
        /// 获取当前组件坐标，未引用
        /// </summary>
        /// <returns></returns>
		public Vector3 GetLocalPosition()
		{			
			return _selfGameObj.transform.localPosition;
		}

		private void _OnShowOrHideBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			var controll = UIControllerManager.Instance.GetController<UIBattleController> ();

			if (controll.CanClick () == false)
			{
				return;
			}

//			if (_playerInfor.isEnterInner == false)
//			{
				var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController>();
				controller.playerInfor = _playerInfor;
				controller.setVisible (true);

//			}
		}

        /// <summary>
        /// 刷新玩家信息
        /// </summary>
        /// <param name="player"></param>
		public void SetPlayerData(PlayerInfo player)
		{
			if (_isFirstInit == true)
			{
				var tmpName = player.playerName;
				if (tmpName.Length > 6)
				{
					tmpName = player.playerName.Substring (0, 6);
				}
				lb_playerName.text = tmpName;

				if (null != raw_headDisplay)
				{
					raw_headDisplay.Load (player.headName);
				}
				lb_career.text = player.career;
				_isFirstInit = false;
			}

			lb_playerAge.text = player.totalAge.ToString ();//+"岁";

			totalMoney =(int) player.totalMoney;
			qualityScore =(int) player.qualityScore;
			timeScore =(int) player.timeScore;
			nonIncome = (int)player.CurrentIncome;

			if (currentMoeny <= 0)
			{
				currentMoeny = player.totalMoney;
			}
			else
			{
				if (currentMoeny < player.totalMoney)
				{

//					var _initWorldPosition = Room.Instance.GetCurrentPlayerByIndex(playerIndex).getPlayPos ().position;
//					var tmpPosition =Camera.main.WorldToScreenPoint(new Vector3(_initWorldPosition.x,_initWorldPosition.y+2,_initWorldPosition.z));
					UIControllerManager.Instance.GetController<UIGameEffectController> ().AddMoneyEffect (playerIndex,new Vector3(0,0,0));
				}
				currentMoeny = player.totalMoney;
			}

			_UpdataTxtInfor (player);
			_playerInfor = player;
		}

        /// <summary>
        /// 更新时间积分，品质积分，金币数等
        /// </summary>
        /// <param name="player"></param>
		private void _UpdataTxtInfor(PlayerInfo player)
		{
			var tmpStr = totalMoney.ToString ();
			if (totalMoney >= 100000)
			{
				var tmpMoney =(int) (totalMoney / 10000f);
				tmpStr = string.Format ("{0}万",tmpMoney.ToString());
			}

			lb_totalMoney.SetTextEx (tmpStr);

			lb_nonIncome.SetTextEx (string.Format("{0}/{1}",nonIncome.ToString(),player.TargetIncome.ToString()));
			if (player.isEnterInner == false) 
			{
				lb_time.SetTextEx (timeScore.ToString());
				lb_quality.SetTextEx (qualityScore.ToString());
			
				var tmpIncomePer = player.CurrentIncome / player.TargetIncome;
				_SetIncomeBarPercent (tmpIncomePer);
			}
			else
			{

				lb_time.SetTextEx (string.Format("{0}/{1}", timeScore.ToString(),player.targetTimeScore.ToString()));
				lb_quality.SetTextEx (string.Format("{0}/{1}", qualityScore.ToString(),player.targetQualityScore.ToString()));

				var tmpIncomePer = player.CurrentIncome / player.TargetIncome;
				_SetIncomeBarPercent (tmpIncomePer);

				var tmpQualityPer = player.qualityScore / player.targetQualityScore;
				_SetQualityBarPercent (tmpQualityPer);

				var tmpTimePer = player.timeScore / player.targetTimeScore;
				_SetTimeBarPercent (tmpTimePer);
			}
		}

        /// <summary>
        /// 设置费劳务收入的进度条
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
			if (_timeProgress.isActiveAndEnabled)
			{				
				_timeProgress.fillAmount = value;
			}

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

			if (_qualityProgress.isActiveAndEnabled)
			{
				_qualityProgress.fillAmount = value;
			}
		}

        /// <summary>
        /// 设置年龄，暂时未应用
        /// </summary>
        /// <param name="value"></param>
		public void SetPlayerAge(string value)
		{
			if (null != lb_playerAge)
			{
				lb_playerAge.text = value;
			}		
		}

        /// <summary>
        /// 更新字母按钮，已经屏蔽
        /// </summary>
        /// <param name="imgPath"></param>
		public void UpdateLetterImg(string imgPath)
		{
//			if (null != _letterImg)
//			{
//				_letterImg.Load (imgPath);
//			}
		}

        /// <summary>
        /// 更新评价字符的进度条
        /// </summary>
        /// <param name="value"></param>
		public void UpdateLetterPercent(float value)
		{
//			if (value < 0)
//			{
//				value = 0;
//			}
//			else if(value>1)
//			{
//				value = 1;
//			}
//
//			img_round.fillAmount = value;
		}

        /// <summary>
        /// 释放资源
        /// </summary>
		public void OnDispose()
		{
			if (null != raw_headDisplay)
			{
				raw_headDisplay.Dispose ();
			}

			if (null != _letterImg)
			{
				_letterImg.Dispose ();
			}

			_isFirstInit = false;
				
		}

//		private float _movingTime=0.3f;
//		private bool _isShowBoard=false;
		private bool _isFirstInit=true;
//		private bool _canShowMoving=true;

		private int totalMoney=0;
		private int qualityScore = 0;
		private int timeScore=0;
		private int nonIncome = 0;

		private UIRawImageDisplay raw_headDisplay;
		private Text lb_playerName;
		private Text lb_playerAge;
		private Text lb_totalMoney;
		private Text lb_quality;
		private Text lb_time;
		private Text lb_career;
		private Text lb_nonIncome;
		private Button btn_head;

		private Image _incomeProgress;
		private Image _timeProgress;
		private Image _qualityProgress;

		public int playerIndex = 0;
		private float currentMoeny=-1;


//		private Image _timeProgressBg;
//		private Image _qualityProgressBg;

		private PlayerInfo _playerInfor;

		private UIImageDisplay _letterImg;
	

		private RawImage _roleHead;

		private Image img_offline;

		private bool isLoadImg;
		private GameObject _selfGameObj;

//		private Image img_round;

		private class Layout
		{
			public static string raw_HeadImg="heroHead";
			public static string lb_name="nametxt";
			public static string lb_age="agetxt";
			public static string lb_totalMoney="playerIncome";
			public static string lb_quality="playerQualityscre";
			public static string lb_time="playerTimeScore";
			public static string lb_nonIncome = "playernonIncome";

			public static string lb_career = "careertxt";

			public static string img_letter = "npclitter";

			/// <summary>
			/// The image npc round. npc评分进度条
			/// </summary>
			public static string img_npcRound = "npcround";

			public static string img_offline="img_offline";


			public static string img_progressMoney="moneybgbar";
			public static string img_progressTime="timebgbar";
			public static string img_progressQuality="qualitybgbar";
			public static string img_progressbar="/progressbar";
		}
	}
}

