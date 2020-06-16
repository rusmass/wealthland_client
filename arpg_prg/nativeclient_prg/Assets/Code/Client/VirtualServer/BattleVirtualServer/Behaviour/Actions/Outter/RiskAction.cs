using System;
using Client.UI;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，读取风险卡牌数据
    /// </summary>
    public class RiskAction : ActionBase
    {
        protected override void _OnStart()
        {
//            var list = CardManager.Instance.outerRiskList;
//            var id = MathUtility.Random(list.ToArray());

			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
			var sendcard = false;
			if (Client.GameModel.GetInstance.isPlayNet == false)
			{
				sendcard = true;
			}
			else
			{

				sendcard = true;

			}

			if (sendcard == true)
			{
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.risk;
				var id = Client.CardOrderHandler.Instance.GetRiskChanceCardId();

                if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
                {
                    var myPlayer = Client.PlayerManager.Instance.HostPlayerInfo;
                    if(myPlayer.Settlement._unemploymentNum<2)
                    {
                        var rdm = MathUtility.Random(0,100);
                        if(rdm>90)
                        {
                            id = 10045;
                        }
                    }
                }
                //id = 10045;
                VirtualServer.Instance.Send_NewSelectState(id);
			}

        }
    }
}
