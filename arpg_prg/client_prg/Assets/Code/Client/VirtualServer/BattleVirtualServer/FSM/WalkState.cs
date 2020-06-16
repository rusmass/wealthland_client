using System;

namespace Server.UnitFSM
{
    /// <summary>
    ///  行走，计算要到达的目的地
    /// </summary>
	public class WalkState : FSMState
	{
		public WalkState (Room content)
			:base(content, FSMStateType.WalkState)
		{
			
		}

		public override void Enter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
		{
			Console.WriteLine ("Enter WalkState");
			VirtualServer.Instance.Send_NewWalkState ();
			float walkTime = _Content.players [_Content.CurrentPlayerIndex].RollPoints * _walkTimeUnity;
			_timer = new Counter (walkTime);
		}

		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
		{
			Console.WriteLine ("Exit WalkState");
		}

		protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick (float deltaTime)
		{
			if ((null !=_timer && _timer.Increase (deltaTime)) || _Content.WalkFinished) 
			{
                _Content.WalkFinished = false;
				_timer = null;
				return new SelectState (_Content);
			}
			return this;
		}

		private Counter _timer;
		private const float _walkTimeUnity = float.MaxValue;
	}
}

