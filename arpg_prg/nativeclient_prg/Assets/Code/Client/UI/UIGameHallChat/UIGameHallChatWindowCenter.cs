using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public partial class UIGameHallChatWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_back = go.GetComponentEx<Button> (Layout.btn_back);
			btn_send = go.GetComponentEx<Button> (Layout.btn_send);
			lb_input = go.GetComponentEx<InputField> (Layout.lb_input);
			_scrollRect = go.GetComponentEx <ScrollRect> (Layout.scrollrect);
			img_chatItem = go.GetComponentEx<Image> (Layout.chatitem);

		}

		private void _OnShowCenter()
		{
			EventTriggerListener.Get(btn_back.gameObject).onClick+=_OnClickBackHandler;
			EventTriggerListener.Get(btn_send.gameObject).onClick+=_OnClickSendHandler;

			lb_input.onEndEdit.AddListener (_InputEnded);

			_CreateAddPayGrid (img_chatItem.gameObject);
		}

		private void _OnHideCenter()
		{
			EventTriggerListener.Get(btn_back.gameObject).onClick-=_OnClickBackHandler;
			EventTriggerListener.Get(btn_send.gameObject).onClick-=_OnClickSendHandler;
			lb_input.onEndEdit.RemoveListener (_InputEnded);
		}

        /// <summary>
        /// 检测输入文字长度
        /// </summary>
        /// <param name="value"></param>
		private void _InputEnded(string value)
		{
			if (value.Length > 30)
			{
				lb_input.text = value.Substring (0, 30);
			}
		}

		public void UpateChatLog()
		{
			_CreateAddPayGrid (img_chatItem.gameObject);
		}

		private void _OnClickBackHandler(GameObject go)
		{
			_controller.setVisible (false);
		}
        /// <summary>
        /// 发送聊天
        /// </summary>
        /// <param name="go"></param>
		private void _OnClickSendHandler(GameObject go)
		{
			if (lb_input.text != "")
			{
//				var chatvo = new NetChatVo ();
//				chatvo.chat = lb_input.text;
//				chatvo.playerName = GameModel.GetInstance.myHandInfor.nickName;
//				chatvo.playerId = GameModel.GetInstance.myHandInfor.uuid;
//				_controller.AddNewChatLog (chatvo);
				NetWorkScript.getInstance ().GameHollChatBroad (lb_input.text);
				lb_input.text = "";
			}
			else
			{
				MessageHint.Show ("请先输入文本");
			}

		}

		private void _CreateAddPayGrid(GameObject go,bool resetGrid=false)
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

				for (int i = 0; i < _chatGrid.Cells.Length; ++i)
				{
					var cell = _chatGrid.Cells[i];
					cell.DisplayObject = new UIGameHallChatItem(cell.GetTransform().gameObject);
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


		
		}

		private void _OnDisposeCenter()
		{
			if (null != _chatGrid)
			{
				for (int i = 0; i < _chatGrid.Cells.Length; i++)
				{
					var cell = _chatGrid.Cells[i];
					var display = cell.DisplayObject as UIGameHallChatItem;
					display.DisposeSelf ();
				}


				_chatGrid.Dispose ();
				_chatGrid.OnRefreshCell-=_OnRefreshCell;
			}
            Resources.UnloadUnusedAssets();
        }

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
//			Console.Warning.WriteLine ("当前的索引值:"+cell.Index.ToString());
			var index = cell.Index;
			var value = _controller.GetChatVoByIndex(index);
			var display = cell.DisplayObject as UIGameHallChatItem;
			display.Refresh(value);
		}



		private Button btn_back;
		private Button btn_send;
		private InputField lb_input;
		private ScrollRect _scrollRect;

		private UIWrapGrid _chatGrid;
		private Image img_chatItem;
	}
}

