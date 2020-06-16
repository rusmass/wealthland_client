package com.game.core;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.ByteToMessageDecoder;

import java.util.List;

import com.game.net.SessionManager;

/**
 * 解码器
 */
public class Decoder extends ByteToMessageDecoder 
{
	private byte ZreoByteCount = 0;
	private ByteBuf bytes;
	private long secondTime = 0;
	private int reveCount = 0;

	ByteBuf bytesAction(ByteBuf inputBuf) 
	{
		ByteBuf bufferLen = Unpooled.buffer();
		if (bytes != null) 
		{
			bufferLen.writeBytes(bytes);
			bytes = null;
		}
		
		bufferLen.writeBytes(inputBuf);
		return bufferLen;
	}

	/**
	 * 留存无法读取的byte等待下一次接受的数据包
	 *
	 * @param intputBuf 数据包
	 * @param startI 起始位置
	 * @param lenI 可用数据的长度
	 */
	void bytesAction(ByteBuf intputBuf, int startI, int lenI) 
	{
		if (lenI > 0) 
		{
			bytes = Unpooled.buffer();
			bytes.writeBytes(intputBuf, startI, lenI);
		}
	}
	
	@Override
	public void channelUnregistered(ChannelHandlerContext ctx) throws Exception 
	{
		super.channelUnregistered(ctx);
		SessionManager.Instance.deleteSession(ctx);
	}

	@Override
	protected void decode(ChannelHandlerContext chc, ByteBuf inputBuf, List<Object> outputMessage) 
	{
		if (isTooBusy())
		{
			chc.disconnect();
			return;
		}

		if (inputBuf.readableBytes() > 0) 
		{
			ZreoByteCount = 0;
			ByteBuf buffercontent = bytesAction(inputBuf);
			for (;;) 
			{
				if (buffercontent.readableBytes() >= 8) 
				{
					buffercontent.markReaderIndex();
					int len = buffercontent.readInt();
					
					if (buffercontent.readableBytes() < len)
					{
						buffercontent.resetReaderIndex();
					}
					else
					{
						int actionID = buffercontent.readInt();
						byte[] bytes = new byte[len - 4];
						buffercontent.readBytes(bytes);
						
						GameAction action = ActionRegister.Create(actionID);
						action.GetRequestMsg(chc, actionID, bytes);

						if (buffercontent.readableBytes() > 0) 
						{
							bytesAction(buffercontent, buffercontent.readerIndex(), buffercontent.readableBytes());
							continue;
						}
						else
						{
							break;
						}
					}
				}
				
				bytesAction(buffercontent, buffercontent.readerIndex(), buffercontent.readableBytes());
				break;
			}
		} 
		else 
		{
			ZreoByteCount++;
			if (ZreoByteCount >= 3) 
			{
				System.out.println("decode 空包处理 连续三次空包");
				chc.close();
			}
		}
	}
	
	private boolean isTooBusy()
	{
		if (System.currentTimeMillis() - secondTime < 1000L) 
		{
			reveCount++;
		}
		else 
		{
			secondTime = System.currentTimeMillis();
			reveCount = 0;
		}

		if (reveCount > 50) 
		{
			System.out.println("发送消息过于频繁");
			return true;
		}
		
		return false;
	}
}