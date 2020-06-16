using System;
using System.Collections.Generic;
namespace Client.UI
{
    /// <summary>
    /// 游戏邮箱界面
    /// </summary>
	public class UIGameMailWindowController:UIController<UIGameMailWindow,UIGameMailWindowController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigamemail.ab";
			}
		}

		public UIGameMailWindowController ()
		{

			for (var i = 0; i < 10; i++)
			{
				var tmpVo = new GameMailVo ();
				tmpVo.title = "sssssssss" + i.ToString ();
				tmpVo.content = "lllllllllllllllllll" + i.ToString ();
				inforList.Add (tmpVo);
			}
		}

		public List<GameMailVo> inforList = new List<GameMailVo> ();
	}
}

