package com.game.core;

import io.netty.channel.ChannelHandlerContext;

public abstract class GameAction
{
	protected GameAction (int actionId)
	{
		this.actionId = actionId;
	}
	
	public abstract void GetRequestMsg(ChannelHandlerContext ctx, int messageID, byte[] array);
	public abstract void PutRespondMsg(Object userdata);

	protected int actionId;
	protected ChannelHandlerContext ctx;
}
