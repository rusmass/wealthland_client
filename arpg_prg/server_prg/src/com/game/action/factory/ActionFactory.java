package com.game.action.factory;

import com.game.action.ids.ActionIDs;
import com.game.actions.*;
import com.game.core.ActionRegister;

public class ActionFactory 
{
	protected ActionFactory()
	{
	
	}
	
	public void RegisterAllActions()
	{
		ActionRegister.RegisterAction(ActionIDs.EnterRoomActionID, EnterRoomAction.class);
		ActionRegister.RegisterAction(ActionIDs.LoginActionID, LoginAction.class);
	}
	
	public static final ActionFactory Instance = new ActionFactory();
}
