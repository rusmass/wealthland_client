using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取内圈命运卡牌随机数据
    /// </summary>
    public class InnerFateAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.innerFateList;
//            var id = MathUtility.Random(list.ToArray());

			bool sendcard = false;

			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
	
			if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				sendcard = true;
			}
			else
			{
				if (Client.GameModel.GetInstance.isPlayNet == false)
				{
					
				}
				sendcard = true;
			}

			if (sendcard == true)
			{
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.inFate;
				var id = Client.CardOrderHandler.Instance.GetInnerFateCardId();
				VirtualServer.Instance.Send_NewSelectState(id);

			}


        }
    }
}
