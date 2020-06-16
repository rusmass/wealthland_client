using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGameHallWindow
	{
		private void _InitBottom(GameObject go)
		{
			btn_paihang = go.GetComponentEx<Button> (Layout.btn_paihang);
			btn_haoyou = go.GetComponentEx<Button> (Layout.btn_haoyou);
			btn_help = go.GetComponentEx<Button> (Layout.btn_help);
			btn_set = go.GetComponentEx<Button> (Layout.btn_set);

			btn_chat = go.GetComponentEx<Button> (Layout.btn_chat);
			lb_chat = go.GetComponentEx<Text> (Layout.lb_chat);

			initChatPosition = lb_chat.rectTransform.localPosition;


		}

		private void _ShowBottom()
		{
			btn_paihang.SetActiveEx (false);

//			btn_haoyou.SetActiveEx (true);
//			btn_help.SetActiveEx (true);

			EventTriggerListener.Get (btn_paihang.gameObject).onClick += _OnClickPaiHang;
			EventTriggerListener.Get (btn_haoyou.gameObject).onClick += _OnClickHaoyou;
			EventTriggerListener.Get (btn_help.gameObject).onClick += _OnClickHelp;
			EventTriggerListener.Get (btn_set.gameObject).onClick += _OnClickSet;
			EventTriggerListener.Get (btn_chat.gameObject).onClick += _OnClickChatHandler;



		}

		private void _HideBottom()
		{
			EventTriggerListener.Get (btn_paihang.gameObject).onClick -= _OnClickPaiHang;
			EventTriggerListener.Get (btn_haoyou.gameObject).onClick -= _OnClickHaoyou;
			EventTriggerListener.Get (btn_help.gameObject).onClick -= _OnClickHelp;
			EventTriggerListener.Get (btn_set.gameObject).onClick -= _OnClickSet;
			EventTriggerListener.Get (btn_chat.gameObject).onClick -= _OnClickChatHandler;
		}	

		/// <summary>
		/// Ons the click pai hang. 显示排行榜
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnClickPaiHang(GameObject go)
		{
			return;
			//NetWorkScript.getInstance ().GameRank_Active ();
		}

		private void _OnClickHaoyou(GameObject go)
		{
//			MBGame.Instance.ShareWeiChat (ShareContentInfor.Instance.normalTitleContent);
		}


		private void _OnClickHelp(GameObject go)
		{
            //MBGame.Instance.ShareWeiChatMonment (ShareContentInfor.Instance.normalTitleContent);

            //var controller = UIControllerManager.Instance.GetController<UIFeelingBaordController>();
            //controller.setVisible(true);
            //return;

            var tipController = UIControllerManager.Instance.GetController<UIConsultingWindowController>();
            tipController.setVisible(true);
        }

		private void _OnClickSet(GameObject go)
		{
			var hallset = UIControllerManager.Instance.GetController<UIGameHallSetController> ();
			hallset.setVisible (true);
		}

		/// <summary>
		/// Ons the click chat handler.游戏大厅聊天功能
		/// </summary>
		/// <param name="go">Go.</param>
		private void _OnClickChatHandler (GameObject go)
		{
			var gameChat = UIControllerManager.Instance.GetController<UIGameHallChatController> ();
//			gameChat.InitController ();
			gameChat.setVisible (true);
		}

		public void ShowChat(NetChatVo value)
		{
			lb_chat.text = value.playerName + ":" + value.chat;
			lb_chat.rectTransform.localPosition = initChatPosition;
			chatWidth = lb_chat.preferredWidth;
			isUpdateChat = true;
		}

		/// <summary>
		/// Bottoms the tick.刷新对话话术
		/// </summary>
		/// <param name="delayTime">Delay time.</param>
		public void BottomTick(float delayTime)
		{
			if (isUpdateChat == true)
			{
				var tmpPosition = lb_chat.rectTransform.localPosition;
				lb_chat.rectTransform.localPosition = new Vector3 (tmpPosition.x - _moveSpd * delayTime, tmpPosition.y, tmpPosition.z);

				if (lb_chat.rectTransform.localPosition.x < -chatWidth - 10)
				{
					isUpdateChat = false;
					lb_chat.text = "点击这里开始聊天";
					lb_chat.rectTransform.localPosition = initChatPosition;
				}

			}
		}

		private bool isUpdateChat=false;
		private Vector3 initChatPosition;
		private float _moveSpd=20;
		private float chatWidth=0;

		private Button btn_paihang ;

		private Button btn_haoyou;

		private Button btn_help;

		private Button btn_set;

		private Button btn_chat;

		private Text lb_chat;
	
	}
}

