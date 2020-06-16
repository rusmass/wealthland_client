using System;
using Core.FSM;

namespace Server.UnitFSM
{
    /// <summary>
    /// 升级，判断是从外圈进入到内圈，还是获取胜利
    /// </summary>
    public class UpGradeState : FSMState
    {
        public UpGradeState(Room content)
            : base(content, FSMStateType.UpGradState)
        {

        }

        public override void Enter(Event e, FSM.State lastState)
        {
          //  Console.WriteLine("Enter UpGradeState");

			if (_Content.players[_Content.CurrentPlayerIndex].Level == PlayerLevel.Inner)
			{
				VirtualServer.Instance.Send_NewSuccessState ();
			}
			else
			{
				VirtualServer.Instance.Send_NewUpGradeState();
			}
        }

        protected override void _OnExit(Event e, FSM.State nextState)
        {
            Console.WriteLine("Exit UpGradeState");
        }

        protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            if (_Content.UpGradeFinished)
            {
                _Content.UpGradeFinished = false;
                if (_Content.IsUpGrade)
                {
                    _Content.IsUpGrade = false;
                    if (_Content.players[_Content.CurrentPlayerIndex].Level == PlayerLevel.Inner)
                    {
						Console.WriteLine ("游戏胜利来了来了来了来了来了");
                        return new SuccessState(_Content);
                    }
                    else
                    {
                        _Content.players[_Content.CurrentPlayerIndex].Level = PlayerLevel.Inner;
						_Content.players [_Content.CurrentPlayerIndex].CurrentPos = 0;
                        return new StayState(_Content);
                    }
                }
            }
            return this;
        }
    }
}

