using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，健康管理卡牌数据
    /// </summary>
    public class HealthAction : ActionBase
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.HealthType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.HealthType);
			}
        }
    }
}
