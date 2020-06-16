using System;
using System.Collections.Generic;

namespace Client.UI
{
	public class UITargetInforWindowController:UIController<UITargetInforWindow,UITargetInforWindowController>
	{
		public UITargetInforWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/targetinforouter.ab";
			}
		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		protected override void _Dispose ()
		{

		}


		public List<InforRecordVo> GetTimeScoreList()
		{
			if (null != playerInfor)
			{
				return playerInfor.timeScoreList;
			}
			return null;
		}

		public InforRecordVo GetTimeScoreByIndex(int index)
		{
			var values = playerInfor.timeScoreList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}



		public List<InforRecordVo> GetQualityScoreList()
		{
			if (null != playerInfor)
			{
				return playerInfor.qualityScoreList;
			}
			return null;
		}

		public InforRecordVo GetQualityScoreByIndex(int index)
		{
			var values = playerInfor.qualityScoreList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}



		public void MoveOut()
		{
			var window = _window as UITargetInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			var window = _window as UITargetInforWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}



		public PlayerInfo playerInfor;
	}
}

