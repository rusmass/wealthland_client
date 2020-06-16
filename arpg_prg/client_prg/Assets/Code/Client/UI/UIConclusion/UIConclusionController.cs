using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Client.UI
{
	public class UIConclusionController : UIController<UIConclusionWindow,UIConclusionController>
	{
		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uiconclusionnew.ab";
			}
		}

		protected override void _OnLoad()
		{

		}

		protected override void _OnShow()
		{

		}

		protected override void _OnHide()
		{

		}

		protected override void _Dispose ()
		{

		}

		public void setUpBaseText(PlayerInfo playerInfor)
		{
			player = playerInfor;
		}

		public void setTiletText(bool _mstate)
		{
			mstate = _mstate;
		}


		public bool mstate = false;

		public PlayerInfo player;

		public override void Tick (float deltaTime)
		{
			var window = _window as UIConclusionWindow;
			if (null != window && getVisible ())
			{
				window.TickGame (deltaTime);
			}
		}
	}
}
