using System;

namespace Server.Actions
{
    public class CheckDayAction : ActionBase
    {
        /// <summary>
        /// 单机版，外圈结账日卡牌数据
        /// </summary>
        protected override void _OnStart()
        {
			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
			var sendcard = false;
			if (Client.GameModel.GetInstance.isPlayNet == false)
			{
				sendcard = true;
			}
			else
			{
				if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
				{
				}
				sendcard = true;

			}

			if (sendcard == true)
			{
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.CheckDayType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.CheckDayType);
			}        
        }
    }
}
