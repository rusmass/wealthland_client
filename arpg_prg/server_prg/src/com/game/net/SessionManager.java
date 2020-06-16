package com.game.net;

import java.util.HashMap;
import io.netty.channel.ChannelHandlerContext;

public class SessionManager 
{
	protected SessionManager()
	{
		
	}
	
	public void addSession(int sessionID, ChannelHandlerContext ctx)
	{
		if (!_sessionsMap.containsKey(sessionID))
		{
			_sessionsMap.put(sessionID, ctx);
		}
		else
		{
			ChannelHandlerContext chc = _sessionsMap.get(sessionID);
			if (null == chc)
			{
				_sessionsMap.replace(sessionID, ctx);
			}
		}
	}
	
	public ChannelHandlerContext getChannelHandler(int sessionID)
	{
		if (_sessionsMap.containsKey(sessionID))
		{
			return _sessionsMap.get(sessionID);
		}
		
		return null;
	}
	
	public void deleteSession(int sessionID)
	{
		if (_sessionsMap.containsKey(sessionID))
		{
			ChannelHandlerContext ctx = _sessionsMap.get(sessionID);
			if (null != ctx)
			{
				ctx.disconnect();
				ctx = null;
			}
			
			_sessionsMap.remove(sessionID);
		}
	}
	
	public void deleteSession(ChannelHandlerContext ctx)
	{
		if (_sessionsMap.containsValue(ctx))
		{
			for(Object key:_sessionsMap.keySet()) 
			{
				if(_sessionsMap.get(key).equals(ctx)) 
				{
					_sessionsMap.remove(key);
					break;
				}
			}
		}
	}
	
	public static final SessionManager Instance = new SessionManager();
	private HashMap<Integer, ChannelHandlerContext> _sessionsMap = new HashMap<Integer, ChannelHandlerContext>();
}
