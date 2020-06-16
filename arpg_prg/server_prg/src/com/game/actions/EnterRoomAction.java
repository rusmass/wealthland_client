package com.game.actions;

import com.game.action.ids.ActionIDs;
import com.game.core.LogicAction;
import com.game.message.EnterRoomRequest;
import com.game.message.EnterRoomRespond;

public class EnterRoomAction extends LogicAction<EnterRoomRequest, EnterRoomRespond>
{
	public EnterRoomAction() 
	{
		super(ActionIDs.EnterRoomActionID, EnterRoomRequest.class, EnterRoomRespond.class);
	}

	@Override
	public void OnRequest(EnterRoomRequest request, Object userdata) 
	{
		System.out.println(request.sessionID);
		System.out.println(request.roomID);
		System.out.println(request.playerID);
		System.out.println(request.list.get(0));
	}

	@Override
	public void OnRespond(EnterRoomRespond response, Object userdadta) 
	{
		response.roomID = 111111111;
	}
	
}
