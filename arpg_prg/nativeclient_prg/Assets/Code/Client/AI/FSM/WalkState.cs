using System;
using Client.Unit;

namespace Client.UnitFSM
{
    /// <summary>
    ///  玩家行走的状态
    /// </summary>
    public class WalkState : FSMState
	{
		public WalkState (Room content)
			:base(content, FSMStateType.WalkState)
		{
			
		}

		public override void Enter (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State lastState)
		{
			_lpfnWalk = (e as WalkEvent).lpfnWalk;

            var controller = UIControllerManager.Instance.GetController<Client.UI.UIBattleController>();
            if (null != controller)
            {
                controller.HideRollAnimation();
            }
        }

		protected override void _OnExit (Core.FSM.Event e, Core.FSM.FiniteStateMachine<Room>.State nextState)
		{
            if (!_finish)
            {

            }
		}

        /// <summary>
        ///  接受事件，切换状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		protected override Core.FSM.FiniteStateMachine<Room>.State _DoEvent (Core.FSM.Event e)
		{
			switch ((FSMEventType)e.ID) 
			{
			case FSMEventType.SelectEvent:
				return new SelectState (_Content);

			default :
				break;
			}

			return this;
		}


		protected override Core.FSM.FiniteStateMachine<Room>.State _DoTick (float deltaTime)
		{
			if (!_finish) 
			{
				_finish = _lpfnWalk (deltaTime);
                if (_finish)
                {
                    BattleController.Instance.Send_WalkFinish();
                }
			}
			return this;
		}

		private bool _finish;
		private Func<float, bool> _lpfnWalk;
	}
}

