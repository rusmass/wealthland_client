package com.game.entry;

import com.game.action.factory.ActionFactory;
import com.game.core.GameServer;

public class GameEntry {

	public static void main(String[] args) 
	{
		InitActionFactory();
		
		GameServer.start();
	}
	
	private static void InitActionFactory()
	{
		ActionFactory.Instance.RegisterAllActions();
	}
}
