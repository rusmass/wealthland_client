using System;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 游戏大厅界面
    /// </summary>
	public partial class UIGameHallWindowController:UIController<UIGameHallWindow,UIGameHallWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigamehall.ab";
			}
		}

		public UIGameHallWindowController ()
		{
		}

		/// <summary>
		/// Shows the fight room. 显示匹配房间的界面
		/// </summary>
		public void ShowFightRoom()
		{
			if (null != _window && getVisible() == true)
			{
				(_window as UIGameHallWindow).ShowFightRoomWindow ();
			}
			
		}

        /// <summary>
        /// 隐藏输入房间号
        /// </summary>
		public void HideEnterRoomImg()
		{
			if (null != _window && getVisible () == true)
			{
				(_window as UIGameHallWindow).OnHideEnterroomImg ();
			}
		}

		/// <summary>
		/// Raises the re init number text event.重置房间号
		/// </summary>
		public void OnReInitNumTxt()
		{
			if (null != _window && getVisible () == true)
			{
				(_window as UIGameHallWindow).OnReInitNumTxt();
			}
		}

		/// <summary>
		/// Updates the player head infor. 刷新人物头像
		/// </summary>
		public void UpdatePlayerHeadInfor()
		{
			if (null != _window && getVisible()==true)
			{
				Console.WriteLine ("llllllllllllllllllllllllllllllllllllll");
				(_window as UIGameHallWindow).UpdatePlayerHeadInfor ();
				Console.WriteLine ("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
			}
		}

		/// <summary>
		/// Sets the chat word.显示话术
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetChatWord(NetChatVo value)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIGameHallWindow).ShowChat (value);
			}
		}

		/// <summary>
		/// Tick the specified deltaTime.实时更新聊天信息
		/// </summary>
		/// <param name="deltaTime">Delta time.</param>
		public override void Tick (float deltaTime)
		{
			if (null != _window && getVisible ())
			{
				(_window as UIGameHallWindow).BottomTick (deltaTime);
			}
		}

	}
}

