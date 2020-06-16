using System;
using Core.FSM;

namespace Client.UnitFSM
{
    /// <summary>
    /// event事件的类型
    /// </summary>
    public enum FSMEventType
    {
        None,
        StayEvent,
        RollEvent,
        WalkEvent,
        SelectEvent,
        UpGradeEvent,
        SuccessEvent,
    }

    public class FSMEvent : Event
    {
        public FSMEvent(FSMEventType type)
            : base((int)type)
        {

        }
    }

    /// <summary>
    /// 站立
    /// </summary>
    public class StayEvent : FSMEvent
    {
        public StayEvent()
            : base(FSMEventType.StayEvent)
        {

        }
    }
    /// <summary>
    ///  掷筛子
    /// </summary>
    public class RollEvent : FSMEvent
    {
        public RollEvent(int points)
            : base(FSMEventType.RollEvent)
        {
            this.points = points;
        }

        public int points;
    }


    /// <summary>
    ///  行走
    /// </summary>
    public class WalkEvent : FSMEvent
    {
        public WalkEvent(Func<float, bool> lpfnWalk)
            : base(FSMEventType.WalkEvent)
        {
            this.lpfnWalk = lpfnWalk;
        }

        public Func<float, bool> lpfnWalk;
    }
    /// <summary>
    /// 选择卡牌
    /// </summary>
    public class SelectEvent : FSMEvent
    {
        public SelectEvent(int pos, int id)
            : base(FSMEventType.SelectEvent)
        {
            this.pos = pos;
            this.cardID = id;
        }

        public int pos; //当前角色所在地图块的index
        public int cardID; // 所选择卡片的ID
    }
    /// <summary>
    /// 升级进入内圈
    /// </summary>
    public class UpGradeEvent : FSMEvent
    {
        public UpGradeEvent(Action func)
            : base(FSMEventType.UpGradeEvent)
        {
            lpfnUpGradeToInner = func;
        }

        public Action lpfnUpGradeToInner;
    }
    /// <summary>
    ///  胜利
    /// </summary>
    public class SuccessEvent : FSMEvent
    {
        public SuccessEvent()
            : base(FSMEventType.SuccessEvent)
        {

        }
    } 
}

