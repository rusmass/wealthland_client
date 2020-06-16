using System;

namespace Client.GameFSM
{
    /// <summary>
    ///  状态机event 类型
    /// </summary>
    public enum FSMEventType
	{
        /// <summary>
        /// 默认状态
        /// </summary>
        None,
        /// <summary>
        /// 登录
        /// </summary>
		LoginEvent,
        /// <summary>
        /// 单机版选择角色
        /// </summary>
		SelectRoleEvent,
        /// <summary>
        /// 重新开始游戏
        /// </summary>
		ReStartGameEvent,
        /// <summary>
        /// 游戏大厅
        /// </summary>
		GameHallEvent,
        /// <summary>
        /// 加载
        /// </summary>
		LoadingStateEvent,
        /// <summary>
        /// 网络版选择角色
        /// </summary>
		NetSelectRoleEvent,
    }

	public class FSMEvent : Core.FSM.Event
	{
		public FSMEvent(FSMEventType type)
			: base((int)type)
		{
		}
	}

    /// <summary>
    /// 登录
    /// </summary>
    public class LoginEvent : FSMEvent
    {
        public LoginEvent()
            : base(FSMEventType.LoginEvent)
        {
        }
    }

    /// <summary>
    /// 单机版选择角色
    /// </summary>
	public class SelectRoleEvent : FSMEvent
    {
        public SelectRoleEvent()
            : base(FSMEventType.SelectRoleEvent)
        {

        }
    }

    /// <summary>
    /// Re start game event.游戏重新开始
    /// </summary>
    public class ReStartGameEvent : FSMEvent
    {
        public ReStartGameEvent()
            : base(FSMEventType.ReStartGameEvent)
        {

        }
    }

    /// <summary>
    ///  游戏大厅
    /// </summary>
	public class GameHallEvent : FSMEvent
    {
        public GameHallEvent() : base(FSMEventType.GameHallEvent)
        {

        }
    }

    /// <summary>
    ///  网络版选择角色
    /// </summary>
	public class NetSelectRoleEvent : FSMEvent
    {
        public NetSelectRoleEvent() : base(FSMEventType.NetSelectRoleEvent)
        {

        }
    }

    /// <summary>
    ///  加载
    /// </summary>
	public class LoadingEvent : FSMEvent
    {
        public LoadingEvent() : base(FSMEventType.LoadingStateEvent)
        {

        }
    }
}

