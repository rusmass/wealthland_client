using System;

namespace Net
{
	public abstract class LogicAction<T, V> : GameAction where T : SocketMessage, new() where V : SocketMessage, new()
	{
		protected LogicAction(int actionId) 
			:base(actionId)
		{
			
		}

		public virtual void OnRequest(T request, object userdata)
		{
			
		}

		public virtual void OnRespond(V response, object userdadta)
		{
			
		}

		public sealed override byte[] GetRequestMsg (object userdata)
		{
			T protoRequest = new T ();

			OnRequest (protoRequest, userdata);

			return NetUtil.Encode (ActionId, protoRequest);
		}

		public sealed override void PutRespondMsg (byte[] msg, object userdata)
		{
			V protoRespond = NetUtil.Deserialize<V> (msg);

			OnRespond (protoRespond, userdata);
		}
	}
}

