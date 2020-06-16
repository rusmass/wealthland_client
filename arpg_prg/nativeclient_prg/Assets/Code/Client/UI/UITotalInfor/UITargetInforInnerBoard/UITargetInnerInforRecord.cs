using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{
	public partial class UITargetInforInnerWindow
	{

		private void _OnInitFront(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_recordClose);
			lb_num = go.GetComponentEx<Text> (Layout.lb_recordNum);
			var tmpImg = go.GetComponentEx<Image> (Layout.img_icon);

			img_icon = new UIImageDisplay (tmpImg);
			img_recorditem = go.GetComponentEx<Image> (Layout.img_recorditem);
		}

		private void _OnShowFront()
		{
			EventTriggerListener.Get(btn_close.gameObject).onClick+=_OnCloseFrontContent;

			var tmpstr = "";
			var iconPath = "";
			if (_controller.recordType == TargetInnerRecordType.Flow)
			{
				tmpstr = HandleStringTool.HandleMoneyTostring (_controller.playerInfor.CurrentIncome);
				iconPath = "share/atlas/battle/totalinfor/targetinner/liudongxianjin.ab";
			}
			else if(_controller.recordType == TargetInnerRecordType.Time)
			{
				tmpstr = _controller.playerInfor.timeScore.ToString ();
				iconPath = "share/atlas/battle/totalinfor/targetinner/shijianjifen.ab";
			}
			else if(_controller.recordType == TargetInnerRecordType.Quality)
			{
				tmpstr = _controller.playerInfor.qualityScore.ToString ();
				iconPath = "share/atlas/battle/totalinfor/targetinner/pinzhijifen.ab";
			}

			lb_num.text = tmpstr;
			img_icon.Load (iconPath);
			if (null != img_recorditem)
			{
				_CreateTimeWrapGrid (img_recorditem.gameObject);
			}
		
		}

		private void _OnHideFront()
		{
			EventTriggerListener.Get(btn_close.gameObject).onClick-=_OnCloseFrontContent;
		}

		private void _OndisposeFront()
		{
			if (null != img_icon)
			{
				img_icon.Dispose ();
			}

			if (null != _recoreList)
			{
				_recoreList.Clear ();
				_recoreList = null;
			}
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

		private void _CreateTimeWrapGrid(GameObject go)
		{
			var items = _controller.GetTimeScoreList().Count;
			if (items > 0)
			{	
				var tmpLen =_recoreList.Count;

				if (items <=tmpLen)
				{
					for (int i = 0; i < tmpLen; i++)
					{
						var cell = _recoreList [i];
						if (items >i)
						{							
							cell.SetActive (true);
							cell.Refresh (_controller.GetTimeScoreByIndex (i));
						} 
						else
						{
							cell.SetActive(false);
						}
					}
				}
				else if(items >tmpLen)
				{
					for (int i = 0; i < items; i++)
					{
						UITimeRecordItem cell;

						//var newObj = false;

						if (tmpLen > i && tmpLen>0)
						{
							cell=_recoreList[i];
							cell.SetActive (true);
							cell.Refresh (_controller.GetTimeScoreByIndex(i));
						}
						else if(_recoreList.Count<=i)
						{
							GameObject tmpTransfor;
							if (i == 0)
							{
								tmpTransfor = go;
							}
							else
							{
								tmpTransfor=(GameObject)go.InstantiateEx ();
							}

							tmpTransfor.transform.parent = go.transform.parent;
							tmpTransfor.transform.localScale = Vector3.one;
							tmpTransfor.transform.localPosition = Vector3.one;
							var tmpRecord = new UITimeRecordItem (tmpTransfor);
							_recoreList.Add (tmpRecord);
							tmpRecord.Refresh (_controller.GetTimeScoreByIndex(i));
						}
					}
				}

			}
			else 
			{
				if (null != go) 
				{
					go.SetActive (false);
				}
			}
		}

		private void _OnRefreshTimeCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetTimeScoreByIndex(index);
			var display = cell.DisplayObject as UITimeRecordItem;
			display.Refresh(value);
		}


//		private UIWrapGrid _timeWrapGrid;

		private List<UITimeRecordItem> _recoreList=new List<UITimeRecordItem>();

		private Button btn_close;
		private Image img_recorditem;
		private UIImageDisplay img_icon;
		private Text lb_num;
		private bool isInitFront = false;

		
	}
}

