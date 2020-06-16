using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
	public partial  class UITargetInforInnerWindow
	{
		private void _OnInitCenter (GameObject go)
		{
			btn_recordFlow = go.GetComponentEx<Button> (Layout.btn_recordFlow);
			lb_currentFlow = go.GetComponentEx<Text> (Layout.lb_currentflow);

			btn_recordTime = go.GetComponentEx<Button> (Layout.btn_recordtime);
			lb_currentTime = go.GetComponentEx<Text> (Layout.lb_currentTime);

			btn_recordQuality = go.GetComponentEx<Button> (Layout.btn_recordquality);
			lb_currentQuality = go.GetComponentEx<Text> (Layout.lb_currentScore);

			content_center = go.DeepFindEx(Layout.content_center);
			content_front = go.DeepFindEx (Layout.content_front);

			_selfTransform = go.transform.Find("content");
			_initPosition = _selfTransform.localPosition;		
			_outerPosition = new Vector3 (-860,_initPosition.y,_initPosition.z);

		}

		private void _OnShowCenter ()
		{
//			MoveIn ();

			EventTriggerListener.Get (btn_recordFlow.gameObject).onClick += _OnShowRecordFlowBoard;
			EventTriggerListener.Get (btn_recordTime.gameObject).onClick += _OnShowRecordTimeBoard;
			EventTriggerListener.Get (btn_recordQuality.gameObject).onClick += _OnShowRecordQualityBoard;

			var playerInfor = _controller.playerInfor;

			var tmpFlow =playerInfor.CurrentIncome;

			if (GameModel.GetInstance.isPlayNet == true)
			{
				tmpFlow = playerInfor.netTargetCashFlowScore;
			}

			if (tmpFlow <= 0)
			{
				tmpFlow = 0;
			}


			lb_currentFlow.text =string.Format("{0}/{1}",tmpFlow.ToString(),playerInfor.TargetIncome.ToString());
			if (playerInfor.CurrentIncome <= 0)
			{
				btn_recordFlow.SetActiveEx (false);
			}


			var tmpNeedTime =playerInfor.timeScore;

			if (GameModel.GetInstance.isPlayNet == true)
			{
				tmpNeedTime = playerInfor.netTargetTimeScore;
			}

			if (tmpNeedTime <= 0)
			{
				tmpNeedTime = 0;
			}

			lb_currentTime.text=string.Format("{0}/{1}",tmpNeedTime.ToString(),playerInfor.targetTimeScore.ToString());
			if (playerInfor.timeScore <= 0)
			{
				btn_recordTime.SetActiveEx (false);
			}

			var tmpNeedQuality =  playerInfor.qualityScore;

			if(GameModel.GetInstance.isPlayNet==true)
			{
				tmpNeedQuality = playerInfor.netTargetQualityScore;
			}


			if (tmpNeedQuality <= 0)
			{
				tmpNeedQuality = 0;
			}

			lb_currentQuality.text = string.Format ("{0}/{1}", tmpNeedQuality.ToString (), playerInfor.targetQualityScore.ToString ());
			if(playerInfor.qualityScore<=0)
			{
				btn_recordQuality.SetActiveEx (false);
			}

			content_front.SetActiveEx (false);
		}

		private void _OnHideCenter ()
		{
			EventTriggerListener.Get (btn_recordFlow.gameObject).onClick -= _OnShowRecordFlowBoard;
			EventTriggerListener.Get (btn_recordTime.gameObject).onClick -= _OnShowRecordTimeBoard;
			EventTriggerListener.Get (btn_recordQuality.gameObject).onClick -= _OnShowRecordQualityBoard;
		}

		private void _DisposeCenter ()
		{

		}

		private void _OnShowRecordFlowBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			Console.WriteLine ("显示记录界面");
			_controller.UpdateRecordType (TargetInnerRecordType.Flow);
			_ShowFrontBoard ();
		}

		private void _OnShowRecordTimeBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.UpdateRecordType (TargetInnerRecordType.Time);
			_ShowFrontBoard ();
		}

		private void _OnShowRecordQualityBoard(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic ();
			_controller.UpdateRecordType (TargetInnerRecordType.Quality);
			_ShowFrontBoard ();
		}


		private void _ShowFrontBoard()
		{
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

		private Button btn_recordFlow;
		private Text lb_currentFlow;

		private Button btn_recordTime;
		private Text lb_currentTime;

		private Button btn_recordQuality;
		private Text lb_currentQuality;



	}
}

