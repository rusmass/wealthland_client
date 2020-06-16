using System;
using Core.FSM;

namespace Server.UnitFSM
{
    /// <summary>
    /// 状态机事件类型
    /// </summary>
    public enum FSMEventType
	{
		None,
		StayEvent,
		RollEvent,
		WalkEvent,
		SelectEvent,
	}

	public class FSMEvent : Event
	{
		public FSMEvent(FSMEventType type)
			:base((int)type)
		{
			
		}
	}

    /// <summary>
    /// 站立
    /// </summary>
	public class StayEvent : FSMEvent
	{
		public StayEvent()
			:base(FSMEventType.StayEvent)
		{
			
		}
	}
    /// <summary>
    /// 掷筛子
    /// </summary>
    public class RollEvent : FSMEvent
	{
		public RollEvent()
			:base(FSMEventType.RollEvent)
		{
			
		}
	}
    /// <summary>
    /// 行走
    /// </summary>
    public class WalkEvent : FSMEvent
	{
		public WalkEvent()
			:base(FSMEventType.WalkEvent)
		{
			
		}
	}
    /// <summary>
    /// 选择卡牌
    /// </summary>
    public class SelectEvent : FSMEvent
	{
		public SelectEvent()
			:base(FSMEventType.SelectEvent)
		{
			
		}
	}
}

