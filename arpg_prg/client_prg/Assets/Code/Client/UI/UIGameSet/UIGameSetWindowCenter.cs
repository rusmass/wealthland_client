using System;
using UnityEngine;
using UnityEngine.UI;
namespace Client.UI
{
	public partial class UIGameSetWindow
	{
		private void _OnInitCenter(GameObject go)
		{

			var imgMusic=go.GetComponentEx<Image> (Layout.img_music);
			img_music = new UIImageDisplay (imgMusic);

			var imgSound=go.GetComponentEx<Image> (Layout.img_sound);
			img_sound = new UIImageDisplay (imgSound);

			btn_back = go.GetComponentEx<Button> (Layout.btn_back);
			btn_cotinue = go.GetComponentEx < Button> (Layout.btn_continuebtn);
			btn_newplay = go.GetComponentEx<Button> (Layout.btn_newplaybtn);
			btn_about = go.GetComponentEx<Button> (Layout.btn_about);

			btn_sound = go.GetComponentEx<Button> (Layout.img_sound);
			btn_music = go.GetComponentEx<Button> (Layout.img_music);

			img_soundRound = go.GetComponentEx<Image> (Layout.btn_sound);
			img_musicRound = go.GetComponentEx<Image> (Layout.btn_music);

			btn_sureabout = go.GetComponentEx<Button> (Layout.btn_sure);
			img_aboutBoard = go.GetComponentEx<Image> (Layout.img_aboutBoard);

			lb_aboutContent = go.GetComponentEx<Text> (Layout.lb_aboutContent);

			transformContainer = go.transform.DeepFindEx (Layout.obj_container);

			_objInitPosition = transformContainer.localPosition;
			_replayPosition = btn_newplay.transform.localPosition;


		}

		private void _OnShowCenter()
		{
			img_aboutBoard.SetActiveEx(false);

			if (GameModel.GetInstance.isPlayNet == true)
			{
//				btn_back.SetActiveEx (false);
				btn_newplay.SetActiveEx (false);
				GameModel.GetInstance.isNetGameSetShow = true;

				btn_cotinue.transform.localPosition = _replayPosition;

				transformContainer.localPosition = new Vector3 (_objInitPosition.x, _objInitPosition.y - 40, _objInitPosition.z);
			}

			//初始话设置面板
			EventTriggerListener.Get (btn_back.gameObject).onClick += _OnBackHandle;
			EventTriggerListener.Get (btn_newplay.gameObject).onClick += _OnNewplayHandler;
			EventTriggerListener.Get (btn_cotinue.gameObject).onClick += _OnContinueHandler;
			EventTriggerListener.Get (btn_about.gameObject).onClick += _OnAboutHandler;
			EventTriggerListener.Get (btn_sound.gameObject).onClick += _OnHandlerSound;
			EventTriggerListener.Get (btn_music.gameObject).onClick += _OnHandlerMusic;
			EventTriggerListener.Get (btn_sureabout.gameObject).onClick += _OnCloseSureImg;

//			if (GameModel.GetInstance.isPlayNet==false)
//			{
//				Time.timeScale = 0.1f;
//			}

			_isOpenMusic = Audio.AudioManager.Instance.IsOpenMusic;
			_isOpenSound = Audio.AudioManager.Instance.IsOpenSound;

			_UpdateMusicState ();
			_UpdateSoundState ();

			lb_aboutContent.text = _aboutTitle;
		}

		private void _OnHideCenter()
		{

			if (GameModel.GetInstance.isPlayNet == true)
			{
				GameModel.GetInstance.isNetGameSetShow = false;
			}

			EventTriggerListener.Get (btn_back.gameObject).onClick -= _OnBackHandle;
			EventTriggerListener.Get (btn_newplay.gameObject).onClick -= _OnNewplayHandler;
			EventTriggerListener.Get (btn_cotinue.gameObject).onClick -= _OnContinueHandler;
			EventTriggerListener.Get (btn_about.gameObject).onClick -= _OnAboutHandler;
			EventTriggerListener.Get (btn_sureabout.gameObject).onClick -= _OnCloseSureImg;

			EventTriggerListener.Get (btn_sound.gameObject).onClick -= _OnHandlerSound;
			EventTriggerListener.Get (btn_music.gameObject).onClick -= _OnHandlerMusic;

//			if (GameModel.GetInstance.isPlayNet == false)
//			{
//				Time.timeScale = 1;
//			}

		}

		private void _OnDisposeCenter()
		{
			
		}

		/// <summary>
		/// 返回到游戏开始界面
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnBackHandle(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();

			if (GameModel.GetInstance.isPlayNet == true)
			{
				//var quitcontroller = UIControllerManager.Instance.GetController<UIQuitFightGameWindowController> ();
				NetWorkScript.getInstance ().AgreeQuitGame ();
				_controller.setVisible (false);
				return; 
			}
			else
			{
				MessageManager.getInstance ().netExitGameDeleteCards ();
			}

			LocalDataManager.Instance.IsNormalEnded = true;
			VirtualServer.Instance.Dispose ();
			Client.Unit.BattleController.Instance.Dispose();
//			Client.Room.Instance.Dispose ();
			MessageHint.Dispose ();

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

//			MBGame.Instance.OnApplicationRestart ();

			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			//controller.LoadSeletRoleUI();
			controller.LoadGameHallUI();

			_controller.setVisible (false);

			Audio.AudioManager.Instance.Stop();
		}

		/// <summary>
		/// 重新开始按钮
		/// </summary>
		/// <param name="go">Go.</param>
		public void _OnNewplayHandler(GameObject go)
		{
			LocalDataManager.Instance.IsNormalEnded = true;

			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("点击重新开始");

			//暂停状态机 ， 刷新人物数据信息 ， 刷新人物状态
			VirtualServer.Instance.ReStartGame();
			Client.Unit.BattleController.Instance.ReStartGame();
			PlayerManager.Instance.ReStartGame ();
			Client.Scenes.SceneManager.Instance.CurrentScene.RestartGame ();

			MessageHint.Dispose ();
			var effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
			effectController.ReInitConttoller ();
			effectController.setVisible (false);
			effectController = UIControllerManager.Instance.GetController<UISpecialeffectsWindowController> ();
			effectController.setVisible (true);

			var battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
			if (null != battlerController)
			{
				battlerController.setVisible (false);
				battlerController.RestartList ();
			}
			battlerController = UIControllerManager.Instance.GetController<UIBattleController> ();
			battlerController.setVisible (true);

			_controller.setVisible (false);
		}

		/// <summary>
		/// 继续游戏
		/// </summary>
		/// <param name="go">Go.</param>
		public void _OnContinueHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			if (null != _controller)
			{
				_controller.setVisible (false);
			}
		}

		/// <summary>
		/// 关于我们
		/// </summary>
		/// <param name="go">Go.</param>
		public void _OnAboutHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("关于游戏");
			img_aboutBoard.SetActiveEx (true);
		}

		public void _OnCloseSureImg(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			img_aboutBoard.SetActiveEx (false);
		}



		private void _OnHandlerMusic(GameObject go)
		{
			_isOpenMusic = !_isOpenMusic;
			_UpdateMusicState ();
		}

		private void _OnHandlerSound(GameObject go)
		{
			_isOpenSound = !_isOpenSound;
			_UpdateSoundState ();
		}

		private void _UpdateMusicState()
		{
			if (_isOpenMusic == true)
			{
				_ShowOpenMusic ();
			}
			else
			{
				_ShowCloseMusic ();
			}
		}

		private void _UpdateSoundState()
		{
			if (_isOpenSound == true)
			{
				_ShowOpenSound ();
			}
			else
			{
				_ShowCloseSound ();
			}
		}

		private void _ShowOpenMusic()
		{
			if (null != img_music)
			{
				img_music.Load (_selectOpen);
			}

			img_musicRound.transform.localPosition = _openPosition;
			Audio.AudioManager.Instance.OpenMusic ();
		}

		private void _ShowCloseMusic()
		{
			if (null != img_music)
			{
				img_music.Load (_selectClose);
			}
			img_musicRound.transform.localPosition = _closePosition;
			Audio.AudioManager.Instance.CloseMusic();
		}

		private void _ShowOpenSound()
		{
			if (null != img_sound)
			{
				img_sound.Load (_selectOpen);
			}

			img_soundRound.transform.localPosition = _openPosition;
			Audio.AudioManager.Instance.OpenSound();
		}

		private void _ShowCloseSound()
		{
			if (null != img_sound)
			{
				img_sound.Load (_selectClose);
			}

			img_soundRound.transform.localPosition = _closePosition;
			Audio.AudioManager.Instance.CloseSound ();
		}


		private Image img_aboutBoard;
		private Button btn_sureabout;

		private UIImageDisplay img_sound;
		private UIImageDisplay img_music;

		private Image img_soundRound;
		private Image img_musicRound;

		private bool _isOpenMusic=true;
		private bool _isOpenSound=true;

		private Vector3 _openPosition = new Vector3 (-52,1,0);
		private Vector3 _closePosition = new Vector3 (52, 1, 0);

		private string _selectOpen = "share/atlas/battle/setscene/set_open.ab";
		private string _selectClose = "share/atlas/battle/setscene/set_close.ab";

		private string _aboutTitle = "\u3000\u3000智达富（北京）科技有限公司主要是利用数字化信息技术、无线通讯技术及现代互联网科技系统，来建立财富教育和财富管理的服务体系，为个人和团体（家庭、企业等）提供全方位、多角度和精细化的财富导引与支持的科技公司。\n" +
			"\u3000\u3000人生没有简单的说明书，一切都需要靠自己去体会和领悟。如果将进入人生比喻为进入一个游乐场的话，那么，人们都很想拿到一份现成的游戏指南。然而，这几乎是一件不可能的事情。除非你打算埋没自己这份独一无二的礼物，否则，就无法偷懒，这份游乐场指南必须是自己来制作。“智达富”的存在，就是为了支持大家完成这份基本的功课。\n" +
			"\u3000\u3000智达富好比你的罗盘和旅行地图，我们需要去经历，既然要拓展，生命就必须经历遍所有的小格子。学习知识固然是必要的，但行动更重要，实践是检验思想是否正确的标准。科学需要做实验，而思想需要有体验才能验证，否则就只是一堆的假设。";

		private Text lb_aboutContent ;

		private Button btn_back;
		private Button btn_newplay;
		private Button btn_cotinue;
		private Button btn_about;

		private Button btn_music;
		private Button btn_sound;

		private Transform transformContainer;
		private Vector3 _objInitPosition;

		private Vector3 _initReplayPostion;
		private Vector3 _replayPosition;

	}
}

