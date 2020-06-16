using System;

namespace Client.UI
{
	public class UIQuitFightGameWindowController:UIController<UIQuitFightGameWindow,UIQuitFightGameWindowController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uisetexitgame.ab";
			}
		}

		public UIQuitFightGameWindowController ()
		{
			
		}

		public override void Tick (float deltaTime)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIQuitFightGameWindow).TickCenter (deltaTime);
			}
		}

		public void SetPlayGameNnum(int value,int _totalNum)
		{
			agreeNum = value;
			totalNum = _totalNum;
			if (null != _window && getVisible ())
			{
				(_window as UIQuitFightGameWindow).ShowSelcetNum (agreeNum);
			}
		}

		public void SetHandlerNum(string playerId)
		{
			_handlerNum++;

			if (playerId == PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				_isHideBtn = true;
			}
			if (null != _window && getVisible ())
			{
				
				if (_isHideBtn == true)
				{
					(_window as UIQuitFightGameWindow)._HideButton ();
				}
			}
		}

		/// <summary>
		/// Handlers the exit room. 退出房间
		/// </summary>
		public void HandlerExitRoom()
		{
			_ExitGame();
		}

		private void _ExitGame()
		{
			setVisible (false);

			Client.Unit.BattleController.Instance.Dispose();
			VirtualServer.Instance.Dispose ();
			//			MessageHint.Dispose ();

			var effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
			effectController.ReInitConttoller ();
			effectController.setVisible (false);		

			var battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
			if (null != battlerController)
			{
				battlerController.setVisible (false);
				battlerController.RestartList ();
			}

			Client.Scenes.SceneManager.Instance.CurrentScene.Unload();
			PlayerManager.Instance.Dispose ();
			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			controller.LoadGameHallUI();

		}

		/// <summary>
		/// Sets the continue number.自动隐藏面板
		/// </summary>
		public void SetContinueNum()
		{
			setVisible (false);
		}

		public void InitData()
		{
			agreeNum=0;
			_handlerNum = 0;
			_isHideBtn = false;
		}


		public bool IsQuitGame()
		{
			var isquit = false;
			if (agreeNum >= totalNum)
			{
				isquit = true;
			}
			return isquit;
		}

		private int _handlerNum=0;

		public int agreeNum =0 ;

		public int totalNum=4;

		public bool _isHideBtn=false;

		public string faqirenInfor="";
	}
}

