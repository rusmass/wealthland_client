using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    ///  单机版内圈健康管理卡牌
    /// </summary>
    public class InnerHealthAction : ActionBase
    {
        protected override void _OnStart()
        {
			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
			//int id;

			var sendCard = false;

			if (Client.GameModel.GetInstance.isPlayNet == false)
			{
				sendCard = true;
			}
			else
			{
				if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
				{
					
				}
				sendCard = true;
			}


			if (sendCard == true)
			{
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.InnerHealthType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.InnerHealthType);
			}           
        }
    }
}
