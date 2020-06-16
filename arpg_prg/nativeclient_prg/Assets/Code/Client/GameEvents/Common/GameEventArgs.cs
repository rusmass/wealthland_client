using System;

namespace Client
{
	public class GameEventArgs : EventArgs
	{
		public GameEventArgs(GameEvents eventType)
			: base()
		{
			_eventType = eventType;
		}

		public GameEvents eventType { get { return _eventType; } }

		public override string ToString()
		{
			return string.Format("Type:{0}, eventType:{1}", GetType().Name, eventType);
		}

		private readonly GameEvents _eventType;
	}
}