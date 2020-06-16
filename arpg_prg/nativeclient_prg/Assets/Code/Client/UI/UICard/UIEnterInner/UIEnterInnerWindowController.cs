using System;

namespace Client.UI
{
    /// <summary>
    /// 进入内圈的UI
    /// </summary>
	public class UIEnterInnerWindowController:UIController<UIEnterInnerWindow,UIEnterInnerWindowController>
	{
		protected override string _windowResource {
			get 
			{
				return "prefabs/ui/scene/uienterinner.ab";				
			}
		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		protected override void _Dispose ()
		{

		}

		public void HandlerCardData()
		{
			var turnIndex = Client.Unit.BattleController.Instance.CurrentPlayerIndex;
			var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
          
            if (playerInfor.playerID == PlayerManager.Instance.HostPlayerInfo.playerID)
			{				
				

                if (GameModel.GetInstance.isPlayNet == false)
                {
                    playerInfor.EnterInner();
                    Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
                }
                else
                {
                    var isEndInner = true;

                    Console.Warning.WriteLine("要进入到内圈啦啦啦啦");

                    if(GameModel.GetInstance.isReconnecToGame==1)
                    {
                        isEndInner = false;
                        Console.Warning.WriteLine("当前的链接状态:"+GameModel.GetInstance.isReconnecToGame.ToString());
                    }

                    if(playerInfor.isEnterInner==true)
                    {
                        isEndInner = false;
                        Console.WriteLine("当前玩家是否是进入内圈了");
                    }

                    if(isEndInner==true)
                    {
                        Console.Warning.WriteLine("发送进入内圈");
                        NetWorkScript.getInstance().Send_GameEnterInnerFinished(GameModel.GetInstance.curRoomId);
                    }
                    else
                    {
                        Console.Warning.WriteLine("这家伙是重连上来的");
                        Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
                    }
                    //playerInfor.EnterInner();
                }
                controller.EnterInner();
                controller.SetNonLaberIncome((int)playerInfor.CurrentIncome, playerInfor.TargetIncome);
                controller.SetQualityScore((int)playerInfor.qualityScore, playerInfor.targetQualityScore);
                controller.SetTimeScore((int)playerInfor.timeScore, playerInfor.targetTimeScore);
                controller.SetCashFlow((int)playerInfor.totalMoney, playerInfor.TargetIncome);
                //Room.Instance.getCurrentPlay ().enterInnerShow ();
            }
            else
			{
                playerInfor.EnterInner();
                controller.SetPersonInfor (playerInfor,turnIndex);
                //Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
                if (GameModel.GetInstance.isPlayNet == false)
                {
                    Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
                }
            }

          
        }

		/// <summary>
		/// 进入内圈
		/// </summary>
		public void ConditionSuccess()
		{
			//Client.Unit.BattleController.Instance.Send_RoleSelected (1);
			Console.WriteLine("成功调用了哈哈哈哈");
			Client.Unit.BattleController.Instance.Send_UpGradeFinish(true);
		}

		public override void Tick(float deltaTime)
		{
            var window = _window as UIEnterInnerWindow;
            if (null != window && getVisible())
            {
                window.Tick(deltaTime);
            }
        }

		public PlayerInfo playerInfor;

	}
}

