using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace Client.UI
{
	public partial class UINetGameReadyWindow
	{
	
		private void _InitCenter(GameObject go)
		{
			for (var i = 0; i < 4; i++)
			{
				var tmpStr = Layout.head+(i+1).ToString();

				var tmpimg = go.GetComponentEx<Image> (tmpStr);
				var imgDisplay = new UIImageDisplay (tmpimg);
				_headList.Add (imgDisplay);

				var nameTxt = tmpimg.gameObject.GetComponentEx<Text> (Layout.lb_name);
				_nameList.Add (nameTxt);

				var readyTxt = tmpimg.gameObject.GetComponentEx<Image> (Layout.lb_ready);
				_lbReadyList.Add (readyTxt);

				var imgReady = tmpimg.gameObject.GetComponentEx<Image> (Layout.img_ready);
				_imgReadyList.Add (imgReady);

			}


		}

		private void _ShowCenter()
		{
			var headList = _controller.GetPlayerHeadList ();
			var allReady = true;
			for (var i = 0; i < headList.Count; i++)
			{
				var tmpHead = headList [i];

				var headDisplay = _headList [i];

				if (null !=tmpHead)
				{
					if (null != headDisplay)
					{
						headDisplay.Load (tmpHead.headImg);
					}
				}


				var lb_name = _nameList [i];
				lb_name.text = tmpHead.nickName;

				var lb_ready = _lbReadyList[i];
				var img_read = _imgReadyList [i];

				if (tmpHead.isReady == true)
				{
					lb_ready.SetActiveEx (false);
					img_read.SetActiveEx (true);
				}
				else
				{
					lb_ready.SetActiveEx (true);
					img_read.SetActiveEx (false);
					allReady = false;
				}				
			}

			if (allReady == true ||GameModel.GetInstance.isRoomAllReady==true)
			{
				_HideTipHandler ();
			}
		}

		public void UpdatePlayerReadyInfor()
		{
			var headList = _controller.GetPlayerHeadList ();

			var allReady = true;

			for (var i = 0; i < headList.Count; i++)
			{
				var tmpHead = headList [i];

				var lb_ready = _lbReadyList[i];
				var img_read = _imgReadyList [i];

				if (tmpHead.isReady == true)
				{
					lb_ready.SetActiveEx (false);
					img_read.SetActiveEx (true);
				}
				else
				{
					lb_ready.SetActiveEx (true);
					img_read.SetActiveEx (false);
					allReady = false;
				}
			}

//			if (allReady == true)
//			{
//				_HideTipHandler ();
//			}
		}

		public void _HideTipHandler()
		{
			var tmpImg = _imgReadyList [0];
			var tmpPostionx = tmpImg.rectTransform.localPosition.x;

			var sequence = DOTween.Sequence ();

			sequence.Append(tmpImg.transform.DOLocalMoveX(tmpPostionx,1.5f));
			sequence.AppendCallback (() =>{
				_controller.setVisible (false);
				
			});

		}

		private void _HideCenter()
		{
			
		}

		private void _DisposeCenter()
		{
			for (var i = 0; i < _headList.Count; i++)
			{
				var tmpImg = _headList [i];
				if (null != tmpImg)
				{
					tmpImg.Dispose ();
				}
			}
		}


		private List<UIImageDisplay> _headList=new List<UIImageDisplay>();
		private List<Text> _nameList=new List<Text>();
		private List<Image> _lbReadyList=new List<Image>();
		private List<Image> _imgReadyList=new List<Image>();

	}
}

