package com.game.core;

import java.util.HashMap;

@SuppressWarnings({ "rawtypes" })
public class ActionRegister 
{
	protected ActionRegister()
	{
		
	}
	
	public static GameAction Create(int actionId)
	{
		if (!_actions.containsKey(actionId))
		{
			System.out.println("[ActionFactory.Create] Error no has this action. actionId = " + actionId);
			return null;
		}
		
		try 
		{
			return (GameAction) _actions.get(actionId).newInstance();
		} 
		catch (InstantiationException | IllegalAccessException e) 
		{
			e.printStackTrace();
		}
		
		return null;
	}
	
	public static void RegisterAction(int actionId, Class type)
	{
		if (_actions.containsKey(actionId))
		{
			System.out.println("[ActionFactory.RegisterAction] Error actionId already exists. actionId = " + actionId);
			return;
		}
		
		_actions.put(actionId, type);
	}
	
	private static HashMap<Integer, Class> _actions = new HashMap<Integer, Class>();
}
