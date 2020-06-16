package com.game.message;

import java.util.List;

import com.game.core.SocketMessage;

public class EnterRoomRequest extends SocketMessage
{
	public int roomID;
	public int playerID;
	
	public List<Integer> list;
}
