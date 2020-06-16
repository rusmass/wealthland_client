using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取内圈有钱有闲随机卡牌
    /// </summary>
    public class RelaxAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.innerRelaxList;
//            var id = MathUtility.Random(list.ToArray());
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.richRelax;
				var id = Client.CardOrderHandler.Instance.GetRelaxCardId();
				VirtualServer.Instance.Send_NewSelectState(id);			
			}
        }
    }
}
