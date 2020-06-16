using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    ///  单机版踩到内圈自由选择，机器人自动选择一个卡牌，真人弹板
    /// </summary>
    public class FreeChoiceAction : ActionBase
    {
        protected override void _OnStart()
        {
            //Invest、Quality、Relax
            //temprory quality
//            var list = CardManager.Instance.innerQualtyList;
//            var id = MathUtility.Random(list.ToArray());

			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
			int id;

			if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				id=(int)SpecialCardType.SelectInnerFree;
				Client.GameModel.GetInstance.sendCardType =(int) SpecialCardType.SelectInnerFree;
				VirtualServer.Instance.Send_NewSelectState(id);
			}
			else
			{
				var tmpRandom = UnityEngine.Random.Range (0,120);
				if (tmpRandom > 80)
				{
					//出示投资卡牌
					id = Client.CardOrderHandler.Instance.GetInvestmentCardId();
				}
				else if(tmpRandom>40)
				{
					//出示有钱有闲卡牌
					id = Client.CardOrderHandler.Instance.GetRelaxCardId();
				}
				else
				{
					//出示品质生活卡拍
					id = Client.CardOrderHandler.Instance.GetQualityCardId();
				}
				VirtualServer.Instance.Send_NewSelectState(id);
			}


        }
    }
}
