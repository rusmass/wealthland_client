using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取内圈投资卡牌随机数据
    /// </summary>
    public class InvestAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.innerInvestmentList;
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.investment;
				var id=Client.CardOrderHandler.Instance.GetInvestmentCardId();
				VirtualServer.Instance.Send_NewSelectState(id);
			}
        }
    }
}
