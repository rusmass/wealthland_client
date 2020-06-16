using System;
using Metadata;
using Core.FSM;
using Server.Actions;


namespace Server.UnitFSM
{
    /// <summary>
    /// 进入选择选择卡牌状态，根据位置弹出不同的卡牌
    /// </summary>
    public class SelectState : FSMState
    {
        public SelectState(Room content)
            : base(content, FSMStateType.SelectState)
        {

        }

        public override void Enter(Event e, FSM.State lastState)
        {
            var player = _Content.players[_Content.CurrentPlayerIndex];
            var level = player.Level;
            var pointCount = _PointsCount(level);

            player.CurrentPos = (player.CurrentPos + player.RollPoints) % pointCount;

            var actionName = _GetActionName(level, player.CurrentPos);
            _action = ActionFactory.Instance.Create(actionName, player);
            _action.Start();

			Console.WriteLine ("currentPos : " + player.CurrentPos + " actionName :" + actionName);

            _timer = new Counter (VirtualServer.Instance.SelectStateTime);
			Console.WriteLine ("Enter SelectState");
        }

        protected override void _OnExit(Event e, FSM.State nextState)
        {
			Console.WriteLine ("Exit SelectState");
        }

        protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
			if (selected != int.MinValue)
			{
				Console.WriteLine ("玩家已经选择");
                //然后这里同步一次玩家数据
                if (_CanUpGrade())
                {
                    return new UpGradeState(_Content);
                }
                else
                {
				    return new StayState (_Content);
                }
			}

			if (_timer.Increase(deltaTime))
            {
				return new GiveUpState(_Content);
            }

            return this;
        }

        private int _PointsCount(PlayerLevel level)
        {
            var count = 0;

            switch (level)
            {
                case PlayerLevel.Outer:
                    {
                        var metadata = MetadataManager.Instance.GetTemplateTable<StageOuterPoint>();
                        if (null != metadata)
                        {
                            count = metadata.Count;
                        }
                    }
                    break;
                case PlayerLevel.Inner:
                    {
                        var metadata = MetadataManager.Instance.GetTemplateTable<StageInnerPoint>();
                        if (null != metadata)
                        {
                            count = metadata.Count;
                        }
                    }
                    break;

                default:
                    Console.Error.WriteLine("[SelectState _PointsCount] the player do not in inner or outer");
                    break;
            }

            return count;
        }

        private string _GetActionName(PlayerLevel level, int id)
        {
            var name = string.Empty;

            switch (level)
            {
                case PlayerLevel.Outer:
                    {
                        var template = MetadataManager.Instance.GetTemplate<StageOuterPoint>(id);
                        if (null != template)
                        {
                            name = template.action;
                        }
                    }
                    break;
                case PlayerLevel.Inner:
                    {
                        //if (Client.PlayerManager.Instance.IsHostPlayerTurn() == false)
                        //{
                        //    id = 5;
                        //}

                        var template = MetadataManager.Instance.GetTemplate<StageInnerPoint>(id);
                        if (null != template)
                        {
                            name = template.action;
                        }
                    }
                    break;

                default:
                    Console.Error.WriteLine("[SelectState _GetActionName] the player do not in inner or outer");
                    break;
            }

            return name;
        }

        private int _GetCheckDayNum(PlayerLevel level, int startPoint, int walkPoints)
        {
            int walkNum = 0;
            int checkDayNum = 0;
            if (level == PlayerLevel.Outer)
            {
                var table = MetadataManager.Instance.GetTemplateTable<StageOuterPoint>();
                var length = table.Count;

                while (walkNum++ < walkPoints)
                {
                    startPoint = (startPoint + 1) % length;
                    var template = MetadataManager.Instance.GetTemplate<StageOuterPoint>(startPoint);
                    if (template.IsCheckDay())
                    {
                        checkDayNum++;
                    }
                }
            }
            else
            {
                var table = MetadataManager.Instance.GetTemplateTable<StageInnerPoint>();
                var length = table.Count;

                while (walkNum++ < walkPoints)
                {
                    startPoint = (startPoint + 1) % length;
                    var template = MetadataManager.Instance.GetTemplate<StageInnerPoint>(startPoint);
                    if (template.IsCheckDay())
                    {
                        checkDayNum++;
                    }
                }
            }
            return checkDayNum;
        }

        private bool _CanUpGrade()
        {
            var index = _Content.CurrentPlayerIndex;
            var info = Client.PlayerManager.Instance.Players[index];
            var player = _Content.players[index];

            if (player.Level == PlayerLevel.Outer)
            {				
                return info.CanEnterInner();
            }
            else if (player.Level == PlayerLevel.Inner)
            {
                return info.CanInnerSuccess();
            }

            return false;
        }

		private Counter _timer;
		public int selected = int.MinValue;
        private ActionBase _action;
    }
}