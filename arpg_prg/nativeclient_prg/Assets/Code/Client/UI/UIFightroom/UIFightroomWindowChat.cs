using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client.UI
{

	public partial class UIFightroomWindow
	{
		private void _InitChat(GameObject go)
		{
			btn_send = go.GetComponentEx<Button> (Layout.btn_sendChat);
			lb_input = go.GetComponentEx<InputField> (Layout.lb_input);
			_scrollRect = go.GetComponentEx <ScrollRect> (Layout.scrollRect);
			lb_chat = go.GetComponentEx<Text> (Layout.lb_chat);
		}

		private void _ShowChat()
		{
			EventTriggerListener.Get (btn_send.gameObject).onClick += _OnClickSendHandler;
			lb_input.onEndEdit.AddListener (_OnEnded);
			_CreateAddPayGrid (lb_chat.gameObject);
		}

		private void _HideChat()
		{
			EventTriggerListener.Get (btn_send.gameObject).onClick -= _OnClickSendHandler;
			lb_input.onEndEdit.RemoveListener (_OnEnded);

			_controller.InitController ();
		}

        /// <summary>
        /// 输入文字后，判断字数是否超标
        /// </summary>
        /// <param name="value"></param>
		private void _OnEnded(string value)
		{
			if (value.Length > 30)
			{
				lb_input.text = value.Substring (0, 30);
			}
		}

		private void _DisposeChat ()
		{
			if (null != _chatGrid)
			{
				//				for (int i = 0; i < _chatGrid.Cells.Length; ++i)
				//				{
				//					var cell = _chatGrid.Cells[i];
				//					var display = cell.DisplayObject as UIFightChatItem;
				//					display.DisposeSelf ();
				//				}


				_chatGrid.Dispose ();
				_chatGrid.OnRefreshCell-=_OnRefreshCell;
			}
		}


        /// <summary>
        /// 刷新聊天数据
        /// </summary>
		public void UpateChatLog()
		{
			_CreateAddPayGrid (lb_chat.gameObject);
		}
        /// <summary>
        /// 发送消息数据
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickSendHandler(GameObject go)
		{
			if (lb_input.text != "")
			{
//				var chatvo = new NetChatVo ();
//				chatvo.chat = lb_input.text;
//				chatvo.playerName = GameModel.GetInstance.myHandInfor.nickName;
//				_controller.AddNewChatLog (chatvo);

//				Console.Warning.WriteLine ("当前面板的值是："+lb_input.ToString());

				NetWorkScript.getInstance ().RoomChatBroad (lb_input.text);

				lb_input.text = "";
			}
			else
			{
				MessageHint.Show ("请先输入文本");
			}

		}

		private void _CreateAddPayGrid(GameObject go)
		{
			var items = _controller.GetChatList();

			if (items.Count <= 0)
			{
				go.SetActive (false);
				return;
			}
			else
			{
				go.SetActive (true);
			}

			if (null == _chatGrid)
			{
				_chatGrid = new UIWrapGrid(go, items.Count);

				for (int i = 0; i < _chatGrid.Cells.Length; i++)
				{
					var cell = _chatGrid.Cells[i];
					cell.DisplayObject = new UIFightChatItem(cell.GetTransform().gameObject);
				}
				_chatGrid.OnRefreshCell += _OnRefreshCell;				
			}
			else
			{
				_chatGrid.GridSize = items.Count;
			}
			//			_chatGrid.Cells.GetUpperBound(items.Count-1);
			_chatGrid.Refresh();

			if (_scrollRect.verticalNormalizedPosition > 0.3f)
			{
				_scrollRect.verticalNormalizedPosition=0.05f;
			}
			else
			{
				_scrollRect.verticalNormalizedPosition=0;
			}
//			_scrollRect.verticalNormalizedPosition=0;

		
		}



		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetChatVoByIndex(index);
			var display = cell.DisplayObject as UIFightChatItem;
			display.Refresh(value);
		}

		private Button btn_send;
		private InputField lb_input;
		private ScrollRect _scrollRect;
		private Text lb_chat;

		private UIWrapGrid _chatGrid;
	}
}

