using System;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 好友对战的房间界面
    /// </summary>
	public class UIFightroomController:UIController<UIFightroomWindow,UIFightroomController>
	{
		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uifightroom.ab";
			}
		}

		public UIFightroomController ()
		{
		}

		/// <summary>
		/// Sets the player infors. 刷新房间人物信息
		/// </summary>
		/// <param name="tmpList">Tmp list.</param>
		public void SetPlayerInfors(List<PlayerHeadInfor> tmpList)
		{
			if (null != _window && _window.Visible == true)
			{
				(_window as UIFightroomWindow).SetPlayerHaed (tmpList);
			}

		}

		public List<PlayerHeadInfor> headInforList;

		/// <summary>
		/// Sets the sure button disabled. 设置确认按钮不可点击
		/// </summary>
		public void SetSureBtnDisabled()
		{
			if (null != _window && getVisible ())
			{
				(_window as UIFightroomWindow).SetSureBtnDisabled ();
			}

		}
        /// <summary>
        /// 获取聊天的列表
        /// </summary>
        /// <returns></returns>
		public List<NetChatVo> GetChatList()
		{
			return _chatList;
		}
        /// <summary>
        /// 添加新的聊天信息
        /// </summary>
        /// <param name="value"></param>
		public void AddNewChatLog(NetChatVo value)
		{
			if (_window != null && getVisible ())
			{

				if (null != _chatList)
				{
					_chatList.Add (value);

					if (_chatList.Count > 30)
					{
						_chatList.RemoveAt (0);
						_chatList.TrimExcess ();
					}
				}
				(_window as UIFightroomWindow).UpateChatLog ();
			}			
		}
        /// <summary>
        /// 根据索引值获取聊天数据vo
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public NetChatVo GetChatVoByIndex(int index)
		{
			var values = _chatList;

			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}

        /// <summary>
        /// 清空聊天记录
        /// </summary>
		public void InitController()
		{
			_chatList.Clear ();
		}

		private List<NetChatVo> _chatList=new List<NetChatVo>();

	}
}

