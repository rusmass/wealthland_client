using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UILoadingNetGameWindow
	{
		private void _InitCenter(GameObject go)
		{
            GameModel.GetInstance.MathWidthOrHeightByCondition(go,0);


			progressBar = go.GetComponentEx<Slider>(Layout.progressBar);
			_roleList.Clear ();
			for (var i = 0; i < 4; i++)
			{
				var tmpimg = go.GetComponentEx<Image> (Layout.img_role+(i+1).ToString());
				var tmpDisplay = new UIImageDisplay (tmpimg);
				_roleList.Add (tmpDisplay);

				var tmpTxt = go.GetComponentEx<Text> (Layout.lb_name + (i + 1).ToString ());
				_nameList.Add (tmpTxt);
			}
		}

		private void _ShowCenter()
		{

			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
			var playerList = PlayerManager.Instance.Players;
			for (var i = 0; i < playerList.Length; i++)
			{
				var playerinfo = playerList [i];
				_roleList [i].Load (playerinfo.playerImgPath);
				_nameList [i].text = playerinfo.playerName;
			}
		}

		private void _HideCenter()
		{
			GameModel.GetInstance.IsPlayingGame = GamePlayingState.GameNetGameState;
		}

		private void _DisposeCenter()
		{
			for (var i = 0; i < _roleList.Count; i++)
			{
				var tmpRole = _roleList [i];
				if (null != tmpRole)
				{
					tmpRole.Dispose ();
				}
			}
		}

		public void setProgressBarValue(float value)
		{
			progressBar.value = value;
		}

		private Slider progressBar;

		private List<UIImageDisplay> _roleList=new List<UIImageDisplay>();

		private List<Text> _nameList = new List<Text>();

	}
}

