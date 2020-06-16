using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Metadata;

namespace Client.UI
{
	public partial class UIBattleWindow
	{
		private void _InitBottom(GameObject go)
		{
			_btnBorrowMoney = go.GetComponentEx<Button> (Layout.btn_borrow);
			_btnAdvice = go.GetComponentEx<Button> (Layout.btn_advice);
			_btnPayBack = go.GetComponentEx<Button> (Layout.btn_packback);
			_btnGameSet = go.GetComponentEx<Button> (Layout.btn_settings);

			_lbPlayerName = go.GetComponentEx<Text> (Layout.lb_heroName);
			_lbPlayerAge = go.GetComponentEx<Text> (Layout.lb_heroAge);
			_lbCareer = go.GetComponentEx<Text> (Layout.lb_heroCareer);
		
			var headImg = go.GetComponentEx<RawImage> (Layout.img_heroHead);
			_rawHeadDisplay = new UIRawImageDisplay (headImg);

			_shadeEffectImg [0] = headImg;
			_offlineImgs [0] = go.GetComponentEx<Image> (Layout.img_imgOffline);

			for (var i = 0; i < _playerInfors.Length; i++)
			{
				var tmpImg = go.GetComponentEx<Image> (Layout.img_plaerHeadBg+(i+1));
//				tmpImg.SetActiveEx (true);
				var playerInfor = new UIBattlePlayerInforItem (tmpImg.gameObject);
				playerInfor.setActivEx (true);
				playerInfor.playerIndex = i + 1;
				_playerInfors[i]=playerInfor;
				_shadeEffectImg [i + 1] = playerInfor.GetPlayerHead();
				_offlineImgs [i + 1] = playerInfor.GetImgOffline ();

			}

			btn_heroInfor = go.GetComponentEx<Button> (Layout.img_heroHeadBg);

			_showHideBtn = true;

			_tipRecordBoard = new UIBattleRecordBaord (go.GetComponentEx<Image>(Layout.img_tipRecordBoard).gameObject);


//			var letterImg = go.GetComponentEx<Image> (Layout.img_letter);
//			playerLetterImg = new UIImageDisplay (letterImg);

			img_roundimg = go.GetComponentEx<Image> (Layout.img_round);

			_imgLight1 = go.GetComponentEx<Image> (Layout.img_light1);
			_imgLight2 = go.GetComponentEx<Image> (Layout.img_light2);
			_imgLight3 = go.GetComponentEx<Image> (Layout.img_light3);
			_imgLight4 = go.GetComponentEx<Image> (Layout.img_light4);

			setLightImage(_imgLight1);

			img_chat1 = go.GetComponentEx<Image> (Layout.img_chat1);
			img_chat2 = go.GetComponentEx<Image> (Layout.img_chat2);
			img_chat3 = go.GetComponentEx<Image> (Layout.img_chat3);
			img_chat4 = go.GetComponentEx<Image> (Layout.img_chat4);
			img_chat1.SetActiveEx (false);
			img_chat2.SetActiveEx (false);
			img_chat3.SetActiveEx (false);
			img_chat4.SetActiveEx (false);

		}

		public void UpdateTipRecord()
		{
			if (null != _tipRecordBoard)
			{
				_tipRecordBoard.UpdateTipRecord();
			}
		}

		/// <summary>
		/// Updates the chat item infor.刷新聊天
		/// </summary>
		public void UpdateChatItemInfor(NetChatVo value)
		{
			var tmpId = value.playerId;
			Image tmpimg=null;
			if (tmpId == PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				tmpimg = img_chat1;
			}
			else if(tmpId==_playerInfors[0].GetPlayerId())
			{
				tmpimg = img_chat2;
			}
			else if(tmpId==_playerInfors[1].GetPlayerId())
			{
//				setLightImage(_imgLight3);
				tmpimg=img_chat3;
			}
			else if(tmpId ==_playerInfors [2].GetPlayerId())
			{
//				setLightImage(_imgLight4);
				tmpimg=img_chat4;
			}

			if (null != tmpimg)
			{
				tmpimg.SetActiveEx (true);
				tmpimg.GetComponentInChildren<Text> ().text=value.chat;
				var quance = DOTween.Sequence ();
				quance.Append (tmpimg.transform.DOScaleZ(1,2.5f));
				quance.AppendCallback (()=>{
					tmpimg.SetActiveEx(false);
				});
			}

//			Console.Warning.WriteLine ("开始更新人物聊天面板了");

			if (null != _tipRecordBoard)
			{
//				Console.Warning.WriteLine ("有人物聊天面板");
				_tipRecordBoard.UpdateChatItemRecord ();
			}			
		}



		private void _OnBottomShow()
		{
		    _isPlayerStand=true;
			_loadHeroHead=false;

            var hostPlayer = PlayerManager.Instance.HostPlayerInfo;
			SetPlayerInfor(hostPlayer,0);
            _currentTurnCount = 1;

			if (GameModel.GetInstance.isPlayNet == true)
			{
				_btnAdvice.SetActiveEx (false);
			}

			EventTriggerListener.Get (_btnBorrowMoney.gameObject).onClick += _ShowBorrowWindow;

			EventTriggerListener.Get (_btnAdvice.gameObject).onClick+=_OnAdviceHandler;
			EventTriggerListener.Get (btn_heroInfor.gameObject).onClick += _ShowHeroTotalInfor;
			EventTriggerListener.Get (_btnPayBack.gameObject).onClick += _PayBackHandler;
			EventTriggerListener.Get (_btnGameSet.gameObject).onClick += _GameSetHandler;

			var players = PlayerManager.Instance.Players;
			var playerNum = players.Length;


			for (var i = 0; i < _playerInfors.Length; i++)
			{
				var playerInfor = _playerInfors[i];
				if (null != playerInfor)
				{
					if (i < playerNum-1)
					{
						playerInfor.OnShowItem ();
						var tmpPlayers = players [i + 1];


						if(null != tmpPlayers)
						{
							playerInfor.SetPlayerData (tmpPlayers);
						}
						else
						{
							playerInfor.setActivEx(false);
						}
							
					}
					else
					{
						playerInfor.setActivEx(false);
					}				

				}
			}

			if (null != _tipRecordBoard)
			{
				_tipRecordBoard.OnShowRecordBoard ();
				_tipRecordBoard.SetBattleController (_controller);

			}
					
//			this.GetType ().GetField (str).GetValue (this).ToString ();
        }


		public void SetPlayerInfor(PlayerInfo player,int index,bool changePalyer=true)
		{
			if (index == 0)
			{
				player.Age = _currentTurnCount;
				_lbPlayerAge.text = player.totalAge.ToString ();
				if (_loadHeroHead == false)
				{
					var tmpName = player.playerName;
					if (tmpName.Length > 6)
					{
						tmpName = player.playerName.Substring (0, 6);
					}

					_lbPlayerName.text = tmpName;
					_lbCareer.text = player.career;
					_rawHeadDisplay.Load (player.headName);
					_loadHeroHead = true;
				}
				_isPlayerStand = true;

			}
			else
			{
				player.Age = _currentTurnCount;
				var playerInforData=_playerInfors[index - 1];
				playerInforData.SetPlayerData(player);

//				if (changePalyer == true)
//				{
//					_SetBgBase (playerInforData.GetLocalPosition(),false);
//				}
			}

			if (changePalyer == true)
			{
				_currentPlayer = player;
//				_currentPlayerIndex = index;



				if (null != _curShadeImg)
				{
					_KillDotweenImg(_curShadeImg.rectTransform);
				}
				_curShadeImg = _shadeEffectImg[index];
				_DotweenRowImg(_curShadeImg.rectTransform);

				ShowLightImage (player.playerID);
			}
		}

		private void _DotweenRowImg(RectTransform rt)
		{
			var seque = DOTween.Sequence ();
			seque.Append (rt.DOScale(new Vector3(0.8f,0.8f,0.8f),1f));
			seque.Append (rt.DOScale(Vector3.one,1f));
			_curSequence =  seque.SetLoops(int.MaxValue);

		}

		private void _KillDotweenImg(RectTransform rt)
		{
			if(null != _curSequence)
			{
				_curSequence.Kill(false);
			}
			rt.localScale = Vector3.one;
		}

		private void _OnBottomHide()
		{			
			EventTriggerListener.Get (_btnBorrowMoney.gameObject).onClick -= _ShowBorrowWindow;
			EventTriggerListener.Get (_btnAdvice.gameObject).onClick-=_OnAdviceHandler;
			EventTriggerListener.Get (btn_heroInfor.gameObject).onClick -= _ShowHeroTotalInfor;
			EventTriggerListener.Get (_btnPayBack.gameObject).onClick -= _PayBackHandler;
			EventTriggerListener.Get (_btnGameSet.gameObject).onClick -= _GameSetHandler;

			if(null != _curSequence)
			{
				_curSequence.Kill(false);
			}

			for (var i = 0; i < _playerInfors.Length; i++)
			{
				var playerInfor = _playerInfors[i];
				if (null != playerInfor)
				{
					playerInfor.OnHideItem();
				}
			}

			if (null != _tipRecordBoard)
			{				
				_tipRecordBoard.OnHideRecordBoard ();
			}
		}

		public bool IsCanClicked()
		{
			var canClick=true;

			if(PlayerManager.Instance.IsHostPlayerTurn()==false)
			{
				canClick = false;
				MessageHint.Show (SubTitleManager.Instance.subtitle.noChoiceToCheck);
				return canClick;
			}

			if (_isPlayerStand == false)
			{
				canClick = false;
				return canClick;
			}

			return canClick;
		}

		private void _ShowHeroTotalInfor(GameObject go)
		{
//			this.GetType ().GetField ("tip3").GetValue (this).ToString ();

			if (IsCanClicked () == false)
			{
				return;
			}

			var playerInfor = PlayerManager.Instance.HostPlayerInfo;

//			if (playerInfor.isEnterInner == false)
//			{
				var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
				controller.playerInfor =playerInfor;
				controller.setVisible (true);
			Audio.AudioManager.Instance.BtnMusic ();
//			}
		}

		private void _ShowBorrowWindow(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (IsCanClicked () == false)
			{
				return;
			}

			//
			if (GameModel.GetInstance.isPlayNet == false)
			{
				var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
				controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;		
				controller.isInitPayback=false;
				controller.setVisible (true);
				controller.SetBlackBg ();	
			}
			else
			{
				NetWorkScript.getInstance ().GetBorrowInfor ();
			}


		}

		private void _PayBackHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (IsCanClicked () == false)
			{
				return;
			}

			if (GameModel.GetInstance.isPlayNet == false)
			{
				var controller = UIControllerManager.Instance.GetController<UIBorrowWindowController> ();
				controller.playerInfor = PlayerManager.Instance.HostPlayerInfo;		
				controller.isInitPayback=true;
				controller.setVisible (true);
				controller.SetBlackBg ();		
			}
			else
			{
				NetWorkScript.getInstance ().GetBorrowInfor ();
			}


		}

		private void _GameSetHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

//			MessageHint.Show ("功能尚未开放",null);
//			return;

			//20170608如果联网游戏重连进入游戏了，但是未完全加入游戏，则不能点击设置按钮
//			if (GameModel.GetInstance.isPlayNet == true)
//			{
//				if (GameModel.GetInstance.isReconnecToGame == 1)
//				{
//					return;
//				}
//			}

			var controller = UIControllerManager.Instance.GetController<UIGameSetWindowController> ();
			controller.setVisible (true);
		}

		public void HidePaybackBtn()
		{
			if (null != _btnPayBack)
			{
				_btnPayBack.SetActiveEx (false);
			}
		}

		public void ShowPaybackBtn()
		{
			if(null !=_btnPayBack)
			{
				_btnPayBack.SetActiveEx (true);
			}
		}


		public bool IsPackBtnActive()
		{
			if (null != _btnPayBack)
			{
				return _btnPayBack.IsActive ();
			}
			
			return false;
		}
	

		private void _OnAdviceHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (IsCanClicked () == false)
			{
				return;
			}
//			MessageHint.Show ("功能尚未开放，敬请期待");

			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIConsultingWindowController>();
			control.setVisible(true);
		}

        private void _OnBottomTick(float deltaTime)
        {
            var turn = _battleController.CurrentTurnCount;
            if (turn != _currentTurnCount)
            {
				if (null != _currentPlayer)
				{
					_currentTurnCount = turn;
				}
            }

			if (null != _tipRecordBoard)
			{
				_tipRecordBoard.RecordTick (deltaTime);
			}

        }

		private void _OnBottomDispose ()
		{
			if (null != _rawHeadDisplay)
			{
				_rawHeadDisplay.Dispose ();
			}

//			if (null != playerLetterImg)
//			{
//				playerLetterImg.Dispose ();
//			}

			for (var i = 0; i < _playerInfors.Length; i++)
			{
				var playerInfor = _playerInfors[i];
				if (null != playerInfor)
				{
					playerInfor.OnDispose();
					playerInfor = null;

				}
			}

			if (null != _tipRecordBoard)
			{				
				_tipRecordBoard.OnDisposeBoard ();
			}
		}



		/// <summary>
		/// Updates the letter level.更新人物评分排名
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="letterPath">Letter path.</param>
		public void UpdateLetterLevel(string id , string letterPath)
		{
//			var tmpPath = string.Format ("share/atlas/battle/wordlitter/{0}.ab",letterPath);
//			if (id == PlayerManager.Instance.HostPlayerInfo.playerID)
//			{
//				if (null != playerLetterImg)
//				{
//					playerLetterImg.Load (tmpPath);
//				}
//			}
//			else if(id==_playerInfors[0].GetPlayerId())
//			{
//				_playerInfors [0].UpdateLetterImg (tmpPath);
//			}
//			else if(id==_playerInfors[1].GetPlayerId())
//			{
//				_playerInfors [1].UpdateLetterImg (tmpPath);
//			}
//			else if(id ==_playerInfors [2].GetPlayerId())
//			{
//				_playerInfors [2].UpdateLetterImg (tmpPath);
//			}
		}

		/// <summary>
		/// Updates the letter percent. 显示字母的百分比
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="value">Value.</param>
		public void UpdateLetterPercent(string id , float value)
		{
//			if(value <0)
//			{
//				value = 0;
//			}
//			else if(value>1)
//			{
//				value = 1;
//			}
//
//			if (id == PlayerManager.Instance.HostPlayerInfo.playerID)
//			{
//				if (null != img_roundimg)
//				{
//					img_roundimg.fillAmount = value;
//				}
//			}
//			else if(id==_playerInfors[0].GetPlayerId())
//			{
//				_playerInfors [0].UpdateLetterPercent(value);
//			}
//			else if(id==_playerInfors[1].GetPlayerId())
//			{
//				_playerInfors [1].UpdateLetterPercent (value);
//			}
//			else if(id ==_playerInfors [2].GetPlayerId())
//			{
//				_playerInfors [2].UpdateLetterPercent (value);
//			}
		}

		/// <summary>
		/// 展示流光图
		/// </summary>
		/// <param name="id">Identifier.</param>
		private void ShowLightImage(string id)
		{

			if (id == PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				setLightImage(_imgLight1);
			}
			else if(id==_playerInfors[0].GetPlayerId())
			{
				setLightImage(_imgLight2);
			}
			else if(id==_playerInfors[1].GetPlayerId())
			{
				setLightImage(_imgLight3);
			}
			else if(id ==_playerInfors [2].GetPlayerId())
			{
				setLightImage(_imgLight4);
			}
		}


		/// <summary>
		/// Sets the head gray. 设置掉线后玩家头像
		/// </summary>
		/// <param name="index">Index.</param>
		public void SetHeadGray(int index)
		{
			if (null != _shadeEffectImg)
			{
				var tmpRaw = _shadeEffectImg [index];
				var tmpOffline =_offlineImgs[index];

				if (null != tmpOffline)
				{
					tmpOffline.SetActiveEx (true);
				}

				if (null != tmpRaw)
				{
					tmpRaw.color = grayColor;
				}
			}
		}

		/// <summary>
		/// Sets the head bright.玩家上线后头像颜色
		/// </summary>
		/// <param name="index">Index.</param>
		public void SetHeadBright(int index)
		{
			if (null != _shadeEffectImg)
			{
				var tmpRaw = _shadeEffectImg [index];
				if (null != tmpRaw)
				{
					tmpRaw.color = initColor;
				}
			}

			if (null != _offlineImgs)
			{
				var tmpOffline =_offlineImgs[index];

				if (null != tmpOffline)
				{
					tmpOffline.SetActiveEx (false);
				}
			}
		}

		private Color grayColor = new Color (60f/255,60f/255,60f/255,1f);
		private Color initColor = new Color (255f/255,255f/255,255f/255,1f);



		/// <summary>
		/// Sets the light image. 设置光圈的动画
		/// </summary>
		/// <param name="img">Image.</param>
		private void setLightImage(Image img)
		{
			return;
			_imgLight1.SetActiveEx(false);
			_imgLight2.SetActiveEx(false);
			_imgLight3.SetActiveEx(false);
			_imgLight4.SetActiveEx(false);
				
//			img.SetActiveEx(true);
		}

//		private UIImageDisplay playerLetterImg;
		private Image img_roundimg;





		// 人物头像信息
		private Text _lbPlayerName;
		private Text _lbPlayerAge;
		private Text _lbCareer;
		private UIRawImageDisplay _rawHeadDisplay;
		private UIBattlePlayerInforItem[] _playerInfors=new UIBattlePlayerInforItem[3];

		private RawImage[] _shadeEffectImg = new RawImage[4];
		private Image[]    _offlineImgs = new Image[4];
		private RawImage _curShadeImg;

        
		private int _currentTurnCount;
		private Sequence _curSequence;

		/// <summary>
		///玩家是否是休息的状态 
		/// </summary>
		private bool _isPlayerStand=true;
		private bool _loadHeroHead=false;

		private PlayerInfo _currentPlayer;
//		private int _currentPlayerIndex=0;

		private bool _showHideBtn=false;

		// 结账单按钮
//		private Button _btnCheckOut;
		// 资产按钮
		private Button _btnBorrowMoney;
		private Button _btnAdvice;
		private Button _btnPayBack;
		private Button _btnGameSet;

		private Button btn_heroInfor;

		private UIBattleRecordBaord _tipRecordBoard;

		private Image _imgLight1;
		private Image _imgLight2;
		private Image _imgLight3;
		private Image _imgLight4;

		private Image img_chat1;
		private Image img_chat2;
		private Image img_chat3;
		private Image img_chat4;
		//private Image img_chat



	}
}

