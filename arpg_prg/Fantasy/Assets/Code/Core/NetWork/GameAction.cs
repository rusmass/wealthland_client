using System;

namespace Net
{
	public abstract class GameAction
	{
		protected GameAction (int actionId)
		{
			_actionId = actionId;
			ActionFactory.Instance.RegisterAction (actionId, this);
		}
			
		public abstract byte[] GetRequestMsg(object userdata = null);
		public abstract void PutRespondMsg(byte[] msg, object userdata = null);

		private int _actionId;
		public int ActionId { get { return _actionId; } }
	}
}

