using System;
using Client;
using Client.UI;
using Metadata;
using System.Collections.Generic;
using UnityEngine;

public class NetCardHandler
{
	public NetCardHandler ()
	{
	}

	/// <summary>
	/// Updates the player red package. 发红包结束后其他人刷新信息
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="money">Money.</param>
	public static void UpdatePlayerRedPackage(PlayerInfo player , int money,bool isTarPlayer =false)
	{
		var battleControll = UIControllerManager.Instance.GetController<UIBattleController>();
		if (null != battleControll)
		{
			player.totalMoney += money;
			var index =Array.IndexOf ( PlayerManager.Instance.Players,player);
			//battleControll.SetCashFlow ((int)player.totalMoney, -1);
			if (index > 0)
			{
				battleControll.SetPersonInfor (player, index, false);
			}
		}
	}

}

