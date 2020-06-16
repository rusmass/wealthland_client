using System;
using Client.UI;
using System.Collections;
using System.Collections.Generic;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，外圈慈善事业卡牌数据
    /// </summary>
    public class CharityAction : ActionBase
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.CharityType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.CharityType);
			}           
        }
    }
}
