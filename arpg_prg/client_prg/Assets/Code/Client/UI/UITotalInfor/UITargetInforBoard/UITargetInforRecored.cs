using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UITargetInforWindow
	{
		
		private void _OnInitFront(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			img_timeitem = go.GetComponentEx<Image> (Layout.img_timeitem);
			img_qualityitem = go.GetComponentEx<Image> (Layout.img_qualityitem);

			lb_time = go.GetComponentEx<Text> (Layout.lb_time);
			lb_quality = go.GetComponentEx<Text> (Layout.lb_quality);

			img_qualityrecord = go.GetComponentEx<Image> (Layout.img_qualityrecord);
			img_timerecord = go.GetComponentEx<Image> (Layout.img_timerecord);
		}

		private void _OnShowFront()
		{
			EventTriggerListener.Get(btn_close.gameObject).onClick+=_OnCloseFrontContent;

			if (_controller.playerInfor.timeScoreList.Count <= 0)
			{
				img_timerecord.SetActiveEx (false);
			}
			else
			{
				img_timerecord.SetActiveEx (true);
				_CreateTimeWrapGrid (img_timeitem.gameObject);
			}

			if (_controller.playerInfor.qualityScoreList.Count <= 0)
			{
				img_qualityrecord.SetActiveEx (false);
			}
			else
			{
				img_qualityrecord.SetActiveEx (true);
				_CreateQualityWrapGrid (img_qualityitem.gameObject);
			}

			var tmpTimeScore = "";
			var tmpQualityScore = "";

			if (GameModel.GetInstance.isPlayNet == false)
			{
				tmpTimeScore= _controller.playerInfor.timeScore.ToString ();
				tmpQualityScore= _controller.playerInfor.qualityScore.ToString ();
			}
			else
			{
				tmpTimeScore = _controller.playerInfor.netTargetTimeScore.ToString ();
				tmpQualityScore = _controller.playerInfor.netTargetQualityScore.ToString ();
			}

			lb_time.text = tmpTimeScore;
			lb_quality.text = tmpQualityScore;

		}

		private void _OnHideFront()
		{
			EventTriggerListener.Get(btn_close.gameObject).onClick-=_OnCloseFrontContent;
		}


		private void _OnCloseFrontContent(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_OnHideFront ();
			content_front.SetActiveEx (false);
			content_center.SetActiveEx (true);

			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			if (null != controller)
			{
				controller.ShowBoard ();
			}
		}


		private void _CreateQualityWrapGrid(GameObject go)
		{
			var items = _controller.GetQualityScoreList();

			if (null == _qualityWrapGrid)
			{
				_qualityWrapGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _qualityWrapGrid.Cells.Length; ++i)
				{
					var cell = _qualityWrapGrid.Cells[i];
					cell.DisplayObject = new UIQualityRecordItem(cell.GetTransform().gameObject);
				}
				_qualityWrapGrid.OnRefreshCell += _OnRefreshQualityCell;
			}
			else
			{
				_qualityWrapGrid.GridSize = items.Count;
			}
			_qualityWrapGrid.Refresh();

		}

		private void _OnRefreshQualityCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetQualityScoreByIndex(index);
			var display = cell.DisplayObject as UIQualityRecordItem;
			display.Refresh(value);
		}

		private void _CreateTimeWrapGrid(GameObject go)
		{
			var items = _controller.GetTimeScoreList();

			if (null == _timeWrapGrid)
			{
				_timeWrapGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _timeWrapGrid.Cells.Length; ++i)
				{
					var cell = _timeWrapGrid.Cells[i];
					cell.DisplayObject = new UITimeRecordItem(cell.GetTransform().gameObject);
				}
				_timeWrapGrid.OnRefreshCell += _OnRefreshTimeCell;
			}
			else
			{
				_timeWrapGrid.GridSize = items.Count;
			}
			_timeWrapGrid.Refresh();

		}

		private void _OnRefreshTimeCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetTimeScoreByIndex(index);
			var display = cell.DisplayObject as UITimeRecordItem;
			display.Refresh(value);
		}

		private void _OnDisposeFront()
		{
			if (null != _timeWrapGrid)
			{
				_timeWrapGrid.Dispose ();
				_timeWrapGrid.OnRefreshCell-=_OnRefreshTimeCell;
			}

			if (null != _qualityWrapGrid)
			{
				_qualityWrapGrid.Dispose ();
				_qualityWrapGrid.OnRefreshCell-=_OnRefreshQualityCell;
			}

		}

		private UIWrapGrid _timeWrapGrid;
		private UIWrapGrid _qualityWrapGrid;


		private Button btn_close;
		private Image img_timeitem;
		private Image img_qualityitem;
		private Text lb_time;
		private Text lb_quality;

		private Image img_timerecord;
		private Image img_qualityrecord;

		private bool isInitFront = false;


	}
}

