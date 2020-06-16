using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取内圈命运随机卡牌
    /// </summary>
    public class FateAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.outerFateList;
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.outFate;
				var id = Client.CardOrderHandler.Instance.GetOuterFateCardId();
				VirtualServer.Instance.Send_NewSelectState(id);
			}

        }
    }
}
