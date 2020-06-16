
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGameHallSetWindow
	{
		private void _OnInitCenter(GameObject go)
		{

			var imgMusic=go.GetComponentEx<Image> (Layout.img_music);
			img_music = new UIImageDisplay (imgMusic);

			var imgSound=go.GetComponentEx<Image> (Layout.img_sound);
			img_sound = new UIImageDisplay (imgSound);

			btn_back = go.GetComponentEx<Button> (Layout.btn_back);
			btn_logout = go.GetComponentEx < Button> (Layout.btn_logout);

			btn_sound = go.GetComponentEx<Button> (Layout.img_sound);
			btn_music = go.GetComponentEx<Button> (Layout.img_music);

			img_soundRound = go.GetComponentEx<Image> (Layout.btn_sound);
			img_musicRound = go.GetComponentEx<Image> (Layout.btn_music);
		}

		private void _OnShowCenter()
		{
//			btn_logout.SetActiveEx(false);
			//初始话设置面板
			EventTriggerListener.Get (btn_back.gameObject).onClick += _OnBackHandler;
			EventTriggerListener.Get (btn_logout.gameObject).onClick += _OnLogoutHandler;
			EventTriggerListener.Get (btn_sound.gameObject).onClick += _OnHandlerSound;
			EventTriggerListener.Get (btn_music.gameObject).onClick += _OnHandlerMusic;

			_isOpenMusic = Audio.AudioManager.Instance.IsOpenMusic;
			_isOpenSound = Audio.AudioManager.Instance.IsOpenSound;

			_UpdateMusicState ();
			_UpdateSoundState ();
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get (btn_back.gameObject).onClick -= _OnBackHandler;
			EventTriggerListener.Get (btn_logout.gameObject).onClick -= _OnLogoutHandler;
			EventTriggerListener.Get (btn_sound.gameObject).onClick -= _OnHandlerSound;
			EventTriggerListener.Get (btn_music.gameObject).onClick -= _OnHandlerMusic;

			EventTriggerListener.Get (btn_sound.gameObject).onClick -= _OnHandlerSound;
			EventTriggerListener.Get (btn_music.gameObject).onClick -= _OnHandlerMusic;


		}

		private void _OnDisposeCenter()
		{

		}


		/// <summary>
		/// 继续游戏
		/// </summary>
		/// <param name="go">Go.</param>
		public void _OnBackHandler(GameObject go)
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
		public void _OnLogoutHandler(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("登出游戏，，，，");

			NetWorkScript.getInstance ().CloseNet();
			PlayerPrefs.SetString (GameModel.GetInstance.UserId, "");
			PlayerPrefs.SetString(GameModel.GetInstance.WeChatLastLoginTime,"-1");
			var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController> ();
			gameHall.setVisible (false);

			_controller.setVisible (false);

			Game.Instance.SwitchLoginWindow ();

//			var loginController = UIControllerManager.Instance.GetController<UILoginController> ();
//			loginController.setVisible (true);
		}

		private void _OnHandlerMusic(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_isOpenMusic = !_isOpenMusic;
			_UpdateMusicState ();
		}

		private void _OnHandlerSound(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
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

		private Text lb_aboutContent ;

		private Button btn_back;
		private Button btn_logout;

		private Button btn_music;
		private Button btn_sound;

	}
}

