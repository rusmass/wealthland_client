using System;
using Client.UI;
using System.Collections;
using System.Collections.Generic;

namespace Server.Actions
{
    /// <summary>
    ///  单机版，机会卡牌数据。机器人直接读取。玩家自己会弹板，小于10000小机会，大于10000弹选择面板
    /// </summary>
    public class ChanceAction : ActionBase
    {
        protected override void _OnStart()
        {
            var player = Client.PlayerManager.Instance.GetPlayerInfo(owner.PlayerID);

			if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				int id;

				if (player.CanGetOpportunity == true)
				{
					id = (int)SpecialCardType.SelectChance;
					Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.bigChance;
				}
				else
				{
					id = Client.CardOrderHandler.Instance.GetChanceCardId();
					if (id > 30000 && id < 40000)
					{
						Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.sharesChance;
					}
					else
					{
						Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.fixedChance;
					}
				}

				VirtualServer.Instance.Send_NewSelectState(id);

			}
			else
			{
				int id = 0;

				if (Client.GameModel.GetInstance.isPlayNet == false)
				{
					if (player.CanGetOpportunity == true)
					{

						var tmprandow = UnityEngine.Random.Range (0, 100);

						if (tmprandow < 60)
						{
							id = Client.CardOrderHandler.Instance.GetChanceCardId();
						}
						else
						{
							id = Client.CardOrderHandler.Instance.GetOpportunityCardId();
						}

					}
					else
					{
						id = Client.CardOrderHandler.Instance.GetChanceCardId();
					}				
				}
				VirtualServer.Instance.Send_NewSelectState(id);
			}
        }
    }
}
