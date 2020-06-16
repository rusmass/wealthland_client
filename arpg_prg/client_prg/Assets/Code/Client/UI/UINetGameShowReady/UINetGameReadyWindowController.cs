using System;
using System.Collections.Generic;

namespace Client.UI
{
	public class UINetGameReadyWindowController:UIController<UINetGameReadyWindow,UINetGameReadyWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uireadyscene.ab";
			}
		}

		public UINetGameReadyWindowController ()
		{
			
		}

		public void SetReadyPlayerId(string playerId)
		{
			for (var i = 0; i < _playerHeadList.Count; i++)
			{
				var tmpPlayer = _playerHeadList [i];
				if (null != tmpPlayer)
				{
					if (tmpPlayer.uuid == playerId)
					{
						tmpPlayer.isReady = true;
						break;
					}
				}

			}

			if (null != _window && getVisible () == true)
			{
				(_window as UINetGameReadyWindow).UpdatePlayerReadyInfor();
			}

		}

		/// <summary>
		/// Sets the player head list.设置人物头像的数组
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetPlayerHeadList(List<PlayerHeadInfor> value)
		{
			_playerHeadList = value;
		}

		/// <summary>
		/// Gets the player head list. 获取人物头像数组信息
		/// </summary>
		/// <returns>The player head list.</returns>
		public List<PlayerHeadInfor> GetPlayerHeadList()
		{
			return _playerHeadList;
		}

		public void HideTipHandler()
		{
			if (null != _window && getVisible ())
			{
				(_window as UINetGameReadyWindow)._HideTipHandler ();
			}
		}

		private List<PlayerHeadInfor> _playerHeadList;

	}
}

