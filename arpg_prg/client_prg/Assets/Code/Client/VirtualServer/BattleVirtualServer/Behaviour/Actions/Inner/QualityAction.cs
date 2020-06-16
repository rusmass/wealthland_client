using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取内圈品质生活随机卡牌
    /// </summary>
    public class QualityAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.innerQualtyList;
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.qualityLife;
				var id=Client.CardOrderHandler.Instance.GetQualityCardId();
				VirtualServer.Instance.Send_NewSelectState(id);
			}
        }
    }
}
