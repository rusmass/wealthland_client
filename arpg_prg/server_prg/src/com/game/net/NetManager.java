package com.game.net;

import com.game.core.ActionRegister;
import com.game.core.GameAction;

public class NetManager 
{
	protected NetManager()
	{
		
	}
	
	public void DoAction(int playerID, int actionID, Object userdata)
	{
		GameAction action = ActionRegister.Create(actionID);
		action.PutRespondMsg(userdata);
	}
	
	public static final NetManager Instance = new NetManager();
}
