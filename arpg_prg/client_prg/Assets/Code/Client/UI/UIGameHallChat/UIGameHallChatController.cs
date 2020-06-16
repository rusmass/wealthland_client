using System;
using System.Collections.Generic;

namespace Client.UI
{
	public class UIGameHallChatController:UIController<UIGameHallChatWindow,UIGameHallChatController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uigamehallchat.ab";
			}
		}

		public UIGameHallChatController ()
		{
		}

		public List<NetChatVo> GetChatList()
		{
			return _chatList;
		}

		public void AddNewChatLog(NetChatVo value)
		{
			if (null != _chatList)
			{
				_chatList.Add (value);
				if (_chatList.Count >30)
				{
//					_chatList.RemoveRange (0, 20);
					_chatList.RemoveAt(0);
					_chatList.TrimExcess ();
				}

				if (_window != null && getVisible ())
				{
					(_window as UIGameHallChatWindow).UpateChatLog ();
				}
			}			
		}

		public NetChatVo GetChatVoByIndex(int index)
		{
			var values = _chatList;

			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public void InitController()
		{
			_chatList.Clear ();
		}

		private List<NetChatVo> _chatList=new List<NetChatVo>();
	}
}

