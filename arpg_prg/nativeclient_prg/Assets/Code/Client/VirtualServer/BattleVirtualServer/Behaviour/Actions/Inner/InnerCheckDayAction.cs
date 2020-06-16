using System;

namespace Server.Actions
{
    /// <summary>
    /// 单机版内圈结账日卡牌数据
    /// </summary>
    public class InnerCheckDayAction : ActionBase
    {
        protected override void _OnStart()
        {
			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
			//int id;
			if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.InnerCheckDayType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.InnerCheckDayType);

			}
			else
			{
				if (Client.GameModel.GetInstance.isPlayNet == false)
				{
								
				}
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.InnerCheckDayType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.InnerCheckDayType);	
			}
           
        }
    }
}
