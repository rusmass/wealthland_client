package com.game.core;

import com.dyuproject.protostuff.LinkedBuffer;
import com.dyuproject.protostuff.ProtobufIOUtil;
import com.dyuproject.protostuff.Schema;
import com.dyuproject.protostuff.runtime.RuntimeSchema;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.ChannelHandlerContext;

@SuppressWarnings("unchecked")
public abstract class LogicAction<T extends SocketMessage, V extends SocketMessage> extends GameAction
{
	private Class<T> _tType;
	private Class<V> _vType;
	protected LogicAction(int actionId, Class<T> tType, Class<V> vType) 
	{
		super(actionId);
		
		this._tType = tType;
		this._vType = vType;
	}
	
	public abstract void OnRequest(T request, Object userdata);

	public abstract void OnRespond(V response, Object userdata);

	@Override
	public void GetRequestMsg(ChannelHandlerContext ctx, int messageID, byte[] array) 
	{
		System.out.println(array.length);
		SocketMessage bean;
		try 
		{
			this.ctx = ctx;
			bean = _tType.newInstance();
			Schema<SocketMessage> schema = (Schema<SocketMessage>) RuntimeSchema.getSchema(_tType);
			ProtobufIOUtil.mergeFrom(array, bean, schema);
			
			OnRequest((T) bean, null);
		}
		catch (InstantiationException | IllegalAccessException e) 
		{
			e.printStackTrace();
		}
	}
	
	@Override
	public void PutRespondMsg(Object userdata) 
	{
		LinkedBuffer buffer = LinkedBuffer.allocate(1024);
		
		SocketMessage response;
		try
		{
			response = _vType.newInstance();
			Schema<SocketMessage> schema = (Schema<SocketMessage>) RuntimeSchema.getSchema(_vType);
			
			OnRespond((V)response, userdata);
			
			byte[] msgData = ProtobufIOUtil.toByteArray(response, schema, buffer);
			
			byte[] heads = Utility.intToBytes(msgData.length + 4);
			byte[] idBytes = Utility.intToBytes(actionId);
			byte[] bytes = Utility.concat(heads, idBytes);
			
			ByteBuf sendData = Unpooled.copiedBuffer(bytes, msgData);
			
			ctx.write(sendData);
			ctx.flush();
		}
		catch (InstantiationException | IllegalAccessException e) 
		{
			e.printStackTrace();
		}
	}
}
