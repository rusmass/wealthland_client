using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UIGameHallWindow
	{

		private void _InitCenter(GameObject go)
		{
			btn_danji = go.GetComponentEx<Button> (Layout.btn_danji);
			btn_lianji = go.GetComponentEx<Button> (Layout.btn_net);
			btn_room = go.GetComponentEx<Button> (Layout.btn_room);

			btn_enteroom = go.GetComponentEx<Button> (Layout.btn_enteroom);
		}

		private void _ShowCenter()
		{
            //Audio.AudioManager.Instance.StartMusic ();
            //Audio.AudioManager.Instance.StartMusic();
           // Console.Error.WriteLine("播放开始音乐了呢");
            //Audio.AudioManager.Instance.StartMusic();
            //Audio.AudioManager.Instance.GameBgMusic();
            Audio.AudioManager.Instance.Play("share/audio/youxi2.ab", true);

            EventTriggerListener.Get (btn_danji.gameObject).onClick += _OnClickDanji;
			EventTriggerListener.Get (btn_lianji.gameObject).onClick += _OnClickNet;
			EventTriggerListener.Get (btn_room.gameObject).onClick += _OnClickRoom;
			EventTriggerListener.Get (btn_enteroom.gameObject).onClick += _OnClickEnterRoomHandler;

            if (GameGuidManager.GetInstance.DoneGameHall == false)
            {
                GameGuidManager.GetInstance.ShowGameHallGuid();
            }
            //			var tmpChatVo = new NetChatVo ();
            //			tmpChatVo.chat = "哇哈哈哈哈哈哈哈哈哈哈哈哈流量啦啦啦啦啦啦啦";
            //			tmpChatVo.playerName = GameModel.GetInstance.myHandInfor.nickName;
            //			_controller.SetChatWord (tmpChatVo);
        }

		private void _HideCenter()
		{
			Audio.AudioManager.Instance.Stop();
			EventTriggerListener.Get (btn_danji.gameObject).onClick -= _OnClickDanji;
			EventTriggerListener.Get (btn_lianji.gameObject).onClick -= _OnClickNet;
			EventTriggerListener.Get (btn_room.gameObject).onClick -= _OnClickRoom;
			EventTriggerListener.Get (btn_enteroom.gameObject).onClick -= _OnClickEnterRoomHandler;
		}

		private void _OnClickDanji(GameObject go)
		{
			Console.WriteLine ("进入单机游戏");
			var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			controller.setVisible (true);
			controller.LoadSeletRoleUI();

			GameModel.GetInstance.isPlayNet = false;

			_controller.setVisible (false);
		}

		private void _OnClickNet(GameObject go)
		{
			Console.WriteLine ("进入联网游戏");
			GameModel.GetInstance.isPlayNet = true;
            GameModel.GetInstance.playNetMode = 1;

            GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNormalState;

            var waitcontorller = UIControllerManager.Instance.GetController<UITipWaitingWindowController>();
            waitcontorller.setVisible(true);
            waitcontorller.SetCallBack(this._OnCancleMatch);
            NetWorkScript.getInstance().RequestMatchGame(GameModel.GetInstance.myHandInfor.uuid);

            //RequestMatchGame
            //var waitController = UIControllerManager.Instance.GetController<UIGameSimpleTipController>();
            //waitController.SetSureTip("功能开发中，敬请期待");
            //waitController.setVisible(true);
        }

        private void _OnCancleMatch()
        {
            NetWorkScript.getInstance().CancleMatchGame(GameModel.GetInstance.myHandInfor.uuid);
        }

        private void  _OnClickRoom(GameObject go)
		{
			Console.WriteLine ("开房间游戏");
			GameModel.GetInstance.isPlayNet = true;
            GameModel.GetInstance.playNetMode = 1;
            NetWorkScript.getInstance ().RequestRoomGame (GameModel.GetInstance.myHandInfor.uuid);

		}

		public void ShowFightRoomWindow()
		{
			var roomController = UIControllerManager.Instance.GetController<UIFightroomController> ();
			roomController.InitController ();
			roomController.setVisible (true);
		}

		public void SetRoomPlayerInfor()
		{
			
		}

		private void _OnClickEnterRoomHandler(GameObject go)
		{
			_OnShowEnteroomImg ();
		}

		private Button btn_danji;

		private Button btn_lianji;

		private Button btn_room;

		private Button btn_enteroom;
	}
}

