﻿using System;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 游戏排行榜
    /// </summary>
	public class UIGameRankWindowController:UIController<UIGameRankWindow,UIGameRankWindowController>
	{

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uirankwindow.ab";
			}
		}

        /// <summary>
        /// 未引用
        /// </summary>
		public UIGameRankWindowController ()
		{
			for (var i = 1; i < 10; i++)
			{
				var tmpvo = new GameRankVo ();
				tmpvo.rankTip = "2"+i;
				tmpvo.playerName = "wahaha" + i.ToString ();
				tmpvo.headPath = GameModel.GetInstance.myHandInfor.headImg;
				tmpvo.rankIndex = i;
				activeRankList.Add (tmpvo);
			}
		}

		public bool isShowBlackBg=false;

        /// <summary>
        /// 活跃排行榜
        /// </summary>
		public List<GameRankVo> activeRankList=new List<GameRankVo>();

		public override void Tick (float deltaTime)
		{
			var window = _window as UIConsultingWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}
	}
}

