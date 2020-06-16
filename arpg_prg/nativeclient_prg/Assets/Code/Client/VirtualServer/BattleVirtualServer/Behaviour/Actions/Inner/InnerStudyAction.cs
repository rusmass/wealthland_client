using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版内圈健康学习卡牌
    /// </summary>
    public class InnerStudyAction : ActionBase
    {
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.InnerStudyType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.InnerStudyType);
			}           
        }
    }
}
