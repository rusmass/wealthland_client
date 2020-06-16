package com.game.actions;

import com.game.action.ids.ActionIDs;
import com.game.core.LogicAction;
import com.game.message.LoginRequest;
import com.game.message.LoginRespond;
import com.game.net.SessionManager;

public class LoginAction extends LogicAction<LoginRequest, LoginRespond> {

	protected LoginAction() 
	{
		super(ActionIDs.LoginActionID, LoginRequest.class, LoginRespond.class);
	}

	@Override
	public void OnRequest(LoginRequest request, Object userdata) 
	{
		SessionManager.Instance.addSession(request.sessionID, this.ctx);
	}

	@Override
	public void OnRespond(LoginRespond response, Object userdata) 
	{
		
	}

}
