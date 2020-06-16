using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial class UITargetInforWindow:UIWindow<UITargetInforWindow,UITargetInforWindowController>
	{
		public UITargetInforWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			btn_record = go.GetComponentEx<Button> (Layout.btn_record);
			lb_income = go.GetComponentEx<Text> (Layout.lb_income);
//			img_pig = go.GetComponentEx<Image> (Layout.img_pig);

			content_center = go.DeepFindEx(Layout.content_center);
			content_front = go.DeepFindEx (Layout.content_front);
			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);

		}

		protected override void _OnShow ()
		{

//			MoveIn ();

			EventTriggerListener.Get (btn_record.gameObject).onClick += _OnShowRecordBoard;

			var playerInfor = _controller.playerInfor;

			var tmpIncome = 0f;

			if (GameModel.GetInstance.isPlayNet == false)
			{
				tmpIncome=playerInfor.TargetIncome - playerInfor.CurrentIncome;
				if (tmpIncome <= 0)
				{
					tmpIncome = 0;
				}

			}
			else
			{
				tmpIncome = playerInfor.totalIncome;
			}


			lb_income.text=HandleStringTool.HandleMoneyTostring(tmpIncome);

//			img_pig.fillAmount = playerInfor.CurrentIncome / playerInfor.TargetIncome;

			content_front.SetActiveEx (false);

			if (_controller.GetTimeScoreList ().Count <= 0 && _controller.GetQualityScoreList ().Count <= 0)
			{
//				btn_record.SetActiveEx (false);
				btn_record.enabled=false;
				btn_record.image.color = btnbgColor;
			}
			else 
			{
                btn_record.enabled = true;
                //btn_record.enabled = false;

            }

		}

		protected override void _OnHide ()
		{
			EventTriggerListener.Get (btn_record.gameObject).onClick -= _OnShowRecordBoard;
		}

		protected override void _Dispose ()
		{
			_OnDisposeFront ();
		}

		private void _OnShowRecordBoard(GameObject go)
		{

			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("显示记录界面");
			content_center.SetActiveEx (false);
			var controller = UIControllerManager.Instance.GetController<UITotalInforWindowController> ();
			if (null != controller)
			{
				controller.HideBoard ();
			}
			content_front.SetActiveEx (true);
			if (isInitFront == false)
			{
				_OnInitFront (content_front.gameObject);
			}
			_OnShowFront ();
		}

		public void MoveOut()
		{
			if (_isOut == true)
			{
				return;
			}

			_isOut = true;
			_selfTransform.localPosition = _initPosition;
			var sequence = DOTween.Sequence();
			sequence.Append (_selfTransform.DOLocalMove(_outerPosition,1f));
			Console.WriteLine ("移动出去");


		}

		public void MoveIn()
		{
			_isOut = false;
			_selfTransform.localPosition = _outerPosition;
			var sequece = DOTween.Sequence ();
			sequece.Append (_selfTransform.DOLocalMove(_initPosition,1f));
			Console.WriteLine ("移动进来");
		}

		private Vector3 _outerPosition;
		private Vector3 _initPosition;

		private Transform _selfTransform;

		/// <summary>
		/// 判断是不是已经移动出去，
		/// </summary>
		private bool _isOut=false;


	

		private Transform content_center;
		private Transform content_front;

		private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);


		private Text lb_income;
//		private Image img_pig;
		private Button btn_record;
	}
}

