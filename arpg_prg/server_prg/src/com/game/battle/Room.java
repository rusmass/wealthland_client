package com.game.battle;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import io.netty.channel.ChannelHandlerContext;

public class Room 
{
	public void Init()
	{
		_players = new ArrayList<Player>();
		_allChannels = new HashMap<Integer, ChannelHandlerContext>();
	}
	
	public boolean isFull()
	{
		return _players.size() >= _maxMembers;
	}
	
	public synchronized boolean Add(Player p, ChannelHandlerContext ctx)
	{
		if (isFull())
		{
			return false;
		}
		
		_players.add(p);
		_allChannels.put(p.playerID, ctx);
		
		return true;
	}
	
	public synchronized void Remove(Player p)
	{
		if (_players.contains(p))
		{
			_players.remove(p);
		}
		
		if (_allChannels.containsKey(p.playerID))
		{
			_allChannels.remove(p.playerID);
		}
	}
	
	public void Clear()
	{
		_allChannels.clear();
		_players.clear();
		
		_allChannels = null;
		_players = null;
	}
	
	private List<Player> _players;
	private static final int _maxMembers = 6;
	private HashMap<Integer, ChannelHandlerContext> _allChannels;
}
