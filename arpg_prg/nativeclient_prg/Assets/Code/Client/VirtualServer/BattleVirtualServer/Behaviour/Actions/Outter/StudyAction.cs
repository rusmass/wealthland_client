﻿using System;
using Client.UI;
using System.Collections;
using System.Collections.Generic;

namespace Server.Actions
{
    /// <summary>
    /// 单机版，进修学习卡牌数据
    /// </summary>
    public class StudyAction : ActionBase
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
				Client.GameModel.GetInstance.sendCardType = (int)SpecialCardType.StudyType;
				VirtualServer.Instance.Send_NewSelectState((int)SpecialCardType.StudyType);
			}
        }
    }
}
