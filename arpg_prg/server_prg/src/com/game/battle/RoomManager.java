package com.game.battle;

import java.util.HashMap;

import io.netty.channel.ChannelHandlerContext;

public class RoomManager 
{
	protected RoomManager()
	{
		
	}
	
	public boolean EnterOrCreateRoom(int roomID, Player player, ChannelHandlerContext ctx)
	{
		Room room;
		if (_rooms.containsKey(roomID))
		{
			room = _rooms.get(roomID);
		}
		else
		{
			room = new Room();
			room.Init();
			_rooms.put(roomID, room);
		}
		
		return room.Add(player, ctx);
	}
	
	public void LeaveRoom(int roomID, Player player)
	{
		if (_rooms.containsKey(roomID))
		{
			Room room = _rooms.get(roomID);
			room.Remove(player);
		}	 
	}
	
	public static RoomManager Instance = new RoomManager();
	private HashMap<Integer, Room> _rooms = new HashMap<Integer, Room>();
}
