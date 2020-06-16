using System;
using UnityEngine;
using System.Collections.Generic;
using Metadata;

namespace Client.UI
{
	public class UIBattleController : UIController<UIBattleWindow, UIBattleController>
	{
		protected override string _windowResource { get { return "prefabs/ui/scene/uibattle.ab";} }

		public static bool isOpenRecordWindow=true;
		public void Re_RequestRoll(int points)
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible()) 
			{
				window.Re_RequestRoll (points);
			}
		}
		public void Re_RequestRollArrs(int[] arr)
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible()) 
			{
				window.Re_RequestRollS(arr);
			}
		}
		// 显示开始按钮 掷点的
        public void ShowCraps(bool flag)
        {
            var window = _window as UIBattleWindow;
            if (null != window && getVisible())
            {
                window.ShowCraps(flag);
            }
        }
		public void SetQualityScore(int score,int tarScore=-1)
        {
            var window = _window as UIBattleWindow;
            if (null != window)
            {
				window.SetQualityScore(score,tarScore);
            }
        }
		public void SetTimeScore(int score,int tarScore = -1)
        {
            var window = _window as UIBattleWindow;
            if (null != window)
            {
				window.SetTimeScore(score,tarScore);
            }
        }

		public void SetNonLaberIncome(int value,int tarScore=-1)
        {
            var window = _window as UIBattleWindow;
            if (null != window)
            {
				window.SetNonLaberIncome(value,tarScore);

//				var playerInfor = PlayerManager.Instance.HostPlayerInfo;
//				if(null != playerInfor)
//				{
//					var tmpstr = string.Format ("{0}/{1}",playerInfor.totalIncome.ToString(),playerInfor.MonthPayment.ToString());
//					window.SetIncomeTip (tmpstr);
//				}
            }
        }
		public void SetCashFlow(int value,int tarScore=-1,bool mUpdate = true)
        {
            var window = _window as UIBattleWindow;
			if (null != window && this.getVisible())
            {
				window.SetCashFlow(value,tarScore,mUpdate);
            }
        }

		public void SetPersonInfor(PlayerInfo player,int index,bool changePlayer=true)
		{
			var window = _window as UIBattleWindow;
			if(null != window)
			{
				window.SetPlayerInfor(player,index,changePlayer);
			}
		}

		public bool CanClick()
		{
			var window = _window as UIBattleWindow;
			var canCllick = true;
			if(null !=window &&this.getVisible()==true)
			{
				canCllick = window.IsCanClicked ();
			}

			return canCllick;
		}

		public void SetHeadGray(int index)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIBattleWindow).SetHeadGray (index);
			}
		}
		public void SetHeadBright(int index)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIBattleWindow).SetHeadBright (index);
			}
		}
        protected override void _Dispose ()
		{			
		}

		protected override void _OnLoad ()
		{			
		}

		protected override void _OnShow ()
		{
            GameEventManager.Instance.SubscribeEvent(GameEvents.RoleTurnChanged, _RoleTurnChanged);
		}

		protected override void _OnHide ()
		{
            GameEventManager.Instance.UnsubscribeEvent(GameEvents.RoleTurnChanged, _RoleTurnChanged);
		}

		public void EnterInner()
		{
			var window = _window as UIBattleWindow;
			if(null != window && getVisible())
			{
				window.EnterInner ();
			}
		}

        public override void Tick(float deltaTime)
        {
            var window = _window as UIBattleWindow;
            if (null != window && getVisible())
            {
                window.Tick(deltaTime);
				//1202
				Audio.AudioManager.Instance.Tick (deltaTime);
            }

			//2016-09-22 zll fix
			//2016-9-28  zll  注释掉
//			if(m_bool_play == true)
//			{
//				index+= 0.5f;
//
//				if(index >= 5)
//				{
//					PlayMusicAndBrighten();
//					m_bool_play = false;
//					index = 0;
//				}
//			}
        }

		/// <summary>
		/// Inits the laizi hide button.收到初始化完成后，隐藏开始按钮
		/// </summary>
		public void InitLaiziHideBtn()
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible ())
			{
				window.HideStartBtn ();
			}
		}

		/// <summary>
		/// Inits the laizi start liazi.全部初始化之后，开始游戏
		/// </summary>
		public void InitLaiziStartLiazi()
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible ())
			{
				window.StartGameAction();
			}
		}

		public void OnShowGameTip(string value)
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible ())			
			{
				window.OnShowGameTip (value);
			}
		}
			
        private void _RoleTurnChanged(GameEventArgs args)
        {
            var index = (args as GameEventRoleTurnChanged).RoleIndex;
			Console.WriteLine("role turn changed. {0}", index);
			SetPersonInfor (PlayerManager.Instance.Players[index],index);
            // ytf0927 npc掷色子
			if (index != 0)
			{
				var selfRoll = false;
				if (GameModel.GetInstance.isPlayNet == false)
				{
					selfRoll = true;
				}

				if (selfRoll == true)
				{
					var window = _window as UIBattleWindow;
					window.Send_RequestRoll ();
				}
			}

			ShowStayRoundTip ();
        }

		public void NetSendRequestRoll()
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible())
			{
				window.Send_RequestRoll ();
			}
		}

		public void SetTipRecord(string value)
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible())
			{
				_recordList.Add (value);
				window.UpdateTipRecord ();
			}
		}

		public void ConditionStartGame()
		{
			var window = _window as UIBattleWindow;
			if (null != window && getVisible () == true)
			{
				window.ConditionStartGame ();
			}
		}

		/// <summary>
		/// Hides the crap button.隐藏掷骰子按钮
		/// </summary>
		public void HideCrapBtn()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIBattleWindow).HideCrapsBtn ();
			}
		}

		/// <summary>
		///  更新玩家和npc的
		/// </summary>
//		public void UpdateLetterLevel(string id , string letterPath)
//		{
//			var window = _window as UIBattleWindow;
//
//			if (null != window && this.getVisible () == true)
//			{
//				window.UpdateLetterLevel (id,letterPath);
//			}
//		}

		/// <summary>
		/// Updates the lettle percent.更新玩家和npc的评分百分比
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="value">Value.</param>
//		public void UpdateLettlePercent(string id,float value)
//		{
//			var window = _window as UIBattleWindow;
//
//			if (null != window && this.getVisible () == true)
//			{
//				window.UpdateLetterPercent(id,value);
//			}
//		}
	
		public void RestartList()
		{
			if (null != _recordList)
			{
				_recordList.Clear ();
				_recordList.TrimExcess ();
			}

			if (null != _chatVoList)
			{
				_chatVoList.Clear ();
				_chatVoList.TrimExcess ();
			}
		}

		public List<string> GetDataList()
		{
			if (_recordList.Count == 0)
			{
				_recordList.Add ("欢迎来到财富之旅");
			}
			return _recordList;
		}

		public string GetValueByIndex(int index)
		{
			var values = _recordList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public List<NetChatVo> GetChatList()
		{
			return _chatVoList;
		}

		public NetChatVo GetChatValueByIndex(int index)
		{
			var values = _chatVoList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		/// <summary>
		/// Updates the chat data. 更新聊天信息
		/// </summary>
		/// <param name="value">Value.</param>
		public void UpdateChatData(NetChatVo value)
		{
			if (null != value)
			{				
				if (null != _window && getVisible ())
				{
					_chatVoList.Add (value);
					if (_chatVoList.Count > 30)
					{
						_chatVoList.RemoveAt (0);
						_chatVoList.TrimExcess ();
					}
					(_window as UIBattleWindow).UpdateChatItemInfor (value);
				}
			}
		}


		/// <summary>
		/// Shows the payback button. 显示还款按钮
		/// </summary>
		public void ShowPaybackBtn()
		{
			var window = _window as UIBattleWindow;
			if (null != window && this.getVisible ())
			{
				window.ShowPaybackBtn ();
			}
		}

		/// <summary>
		/// Hides the payback button. 隐藏还款按钮
		/// </summary>
		public void HidePaybackBtn()
		{
			var window = _window as UIBattleWindow;

			if (null != window && this.getVisible ())
			{
				window.HidePaybackBtn ();
			}
		}


		public bool IsPackbackActive()
		{
			var window = _window as UIBattleWindow;

			if (null != window && this.getVisible ())
			{
				return  window.IsPackBtnActive ();
			}

			return false;
		}


		//2016-09-22 zll 修改播放音乐
		private void PlayMusicAndBrighten()
		{
			var window = _window as UIBattleWindow;
			window._OnHideCountdown();
		}

		/// <summary>
		/// 展示流光图
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void ShowLightImage(int id)
		{
			var window = _window as UIBattleWindow;

			if (null != window && this.getVisible ())
			{
//				window.ShowLightImage(id);
			}

		}      

        public void ShowControllerBoard(string playerName,string cardTitle)
        {
            if(null!=_window && getVisible())
            {
                (_window as UIBattleWindow).ShowControlerBoard(playerName,cardTitle);
            }
        }


		public void setBoolTrue()
		{
			m_bool_play = true;
		}

		/// <summary>
		/// Shows the stay round tip. 显示轮到哪个玩家来进行游戏
		/// </summary>
		private void ShowStayRoundTip()
		{
			if (null != _window && this.getVisible ())
			{
				(_window as UIBattleWindow).ShowStayRoundTip ();
			}
		}

			
		private List<string> _recordList=new List<string>();

		private List<NetChatVo> _chatVoList = new List<NetChatVo> ();

		private bool m_bool_play = false;
    }
}

