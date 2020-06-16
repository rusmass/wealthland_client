using System;

namespace Server.Actions
{
    /// <summary>
    /// 外圈生孩子卡牌数据
    /// </summary>
    public class GiveChildAction : ActionBase
    {
        protected override void _OnStart()
        {

//			var player = Client.PlayerManager.Instance.GetPlayerInfo (owner.PlayerID);
//			var sendcard = false;
//			if (Client.GameModel.GetInstance.isPlayNet == false)
//			{
//				sendcard = true;
//			}
//			else
//			{
//				if (player.playerID == Client.PlayerManager.Instance.HostPlayerInfo.playerID)
//				{
//				}
//
//				sendcard = true;
//
//			}
//
//			if (sendcard == true)
//			{
//				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.GiveChildType;
//				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.GiveChildType);
//
//			}

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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.GiveChildType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.GiveChildType);
			}           
        }
    }
}
