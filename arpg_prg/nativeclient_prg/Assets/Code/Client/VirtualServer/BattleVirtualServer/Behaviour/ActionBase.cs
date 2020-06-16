using System;

namespace Server.Actions
{
    /// <summary>
    /// 读取开拍数据的基类，内圈外圈读取卡牌时走相同接口
    /// </summary>
    public abstract class ActionBase
    {
        public void Start()
        {
            if (!IsStarted)
            {
                _OnStart();
                IsStarted = true;
            }
        }

        public void Cancel()
        {
            if (!IsInterrupted)
            {
                _OnInterrupt();
                IsInterrupted = true;
            }
        }

        public void Tick(float deltaTime)
        {
            _Tick(deltaTime);
        }

        protected void _Complete()
        {
            if (!IsCompleted)
            {
                _OnCompleted();
                IsCompleted = true;
            }
        }

        protected virtual void _Tick(float deltaTime) { }
        protected virtual void _OnStart() { }
        protected virtual void _OnInterrupt() { }
        protected virtual void _OnCompleted() { }

        public bool IsStarted { get; private set; }
        public bool IsInterrupted { get; private set; }
        public bool IsCompleted { get; private set; }

        public Player owner { get; set; }
    }
}
