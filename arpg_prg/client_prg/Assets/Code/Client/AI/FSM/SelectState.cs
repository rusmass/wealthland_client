using System;
using Client.Unit;
using Core.FSM;
using Client.UI;
using Metadata;

namespace Client.UnitFSM
{
    /// <summary>
    ///  选择卡牌的状态
    /// </summary>
    public class SelectState : FSMState
    {

		public static int Count=(int)SpecialCardType.UpGradType;

        public SelectState(Room content)
            : base(content, FSMStateType.SelectState)
        {
			
        }

        /// <summary>
        /// 进入状态，获取卡牌id，显示卡牌
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lastState"></param>
        public override void Enter(Event e, FSM.State lastState)
        {
            var info = e as SelectEvent;
            _cardID = info.cardID;

			//测试股票
//			var i = new Random ();
//			var cad= i.Next (0, 26);	20003	
//			if (PlayerManager.Instance.IsHostPlayerTurn () == true)
//			{
//				_cardID =20001 ;

//				Count++;
//
//				if (Count >= (int)SpecialCardType.InnerHealthType)
//				{
//					Count= (int)SpecialCardType.InnerHealthType;
//				}
//
//				_cardID =Count;
//
//			}
//			else
//			{
//				_cardID =50001;
//			}

//			Count++;
//
//			if (Count < 5)
//			{
//				_cardID =20001 ;
//			} 
//			else 
//			{
//				_cardID =50001;
//				if (Count > 8)
//				{
//					Count = 0;
//				}
//			}
//			var value = MathUtility.Random (50001, 50032);
//			_cardID = (int)SpecialCardType.GiveChildType;
			if (GameModel.GetInstance.isPlayNet == false)//不联网或者是机器人玩
			{
                //_cardID = (int)SpecialCardType.GiveChildType;
                CardManager.Instance.OpenCard(_cardID);
			}
			else
			{				
				CardManager.Instance.OpenCard ((int)GameModel.GetInstance.ShowCardType);
			}
           
        }

        /// <summary>
        ///  退出当前状态，调用处理方法。目前CloseCard用于操作完后，游戏信息的提示
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nextState"></param>
        protected override void _OnExit(Event e, FSM.State nextState)
        {
            CardManager.Instance.CloseCard(_cardID);
        }

        /// <summary>
        ///  接受事件，如果是stay，切换到站立状态；如果是walk，切换到行走状态;如果是upgrade,升级到内圈,如果是success,切换到胜利状态
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override FiniteStateMachine<Room>.State _DoEvent(Event e)
        {
            switch ((FSMEventType)e.ID)
            {
                case FSMEventType.StayEvent:
                    return new StayState(_Content);
                case FSMEventType.WalkEvent:
                    return new WalkState(_Content);
                case FSMEventType.UpGradeEvent:
                    return new UpGradeState(_Content);
                case FSMEventType.SuccessEvent:
                    return new SuccessState(_Content);
                default:
                    break;
            }

            return this;
        }

		protected override FiniteStateMachine<Room>.State _DoTick(float deltaTime)
        {
            return this;
        }

        private int _cardID;
    }
}

