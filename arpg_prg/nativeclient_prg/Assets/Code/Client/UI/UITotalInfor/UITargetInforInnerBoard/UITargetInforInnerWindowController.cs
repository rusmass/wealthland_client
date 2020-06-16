using System;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 玩家内圈目标的窗口
    /// </summary>
	public class UITargetInforInnerWindowController:UIController<UITargetInforInnerWindow,UITargetInforInnerWindowController>
	{
		public UITargetInforInnerWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/targetinforinner.ab";
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
			return _tmpList;
		}

		public InforRecordVo GetTimeScoreByIndex(int index)
		{
			var values = _tmpList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}

		private List<InforRecordVo> _tmpList;
		private TargetInnerRecordType _recordType;
		public TargetInnerRecordType recordType
		{
			get
			{
				return _recordType;
			}
		}

		public void UpdateRecordType(TargetInnerRecordType value)
		{
			_recordType = value;

			if (value == TargetInnerRecordType.Flow)
			{
				_tmpList = playerInfor.flowScoreList;
			}
			else if(value == TargetInnerRecordType.Time)
			{
				_tmpList = playerInfor.timeScoreList;
			}
			else if(value == TargetInnerRecordType.Quality)
			{
				_tmpList = playerInfor.qualityScoreList;
			}
		}


		public void MoveOut()
		{
			var window = _window as UITargetInforInnerWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			var window = _window as UITargetInforInnerWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}

		public PlayerInfo playerInfor;
	}
}

