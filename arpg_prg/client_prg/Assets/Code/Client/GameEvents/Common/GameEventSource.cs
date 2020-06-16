using System;
using System.Collections.Generic;

namespace Client
{
	public class GameEventSource
	{
		class EventWrapper
		{
			public void FireEvent(GameEventArgs args)
			{
				if (null != OnEvent)
				{
					OnEvent(args);
				}
				else
				{
					Console.WriteLine("[FireEvent()] OnEvent is null");
				}
			}

			public event Action<GameEventArgs> OnEvent;
		}

		public GameEventSource()
		{
		}

		public void SubscribeEvent(int eventType, Action<GameEventArgs> handler)
		{
#if _DEBUG_ && _DEBUG_GameEvent
			Console.WriteLine("SubscribeEvent, event: {0}, handler: {1}", (GameEvents)eventType, handler.Method.ReflectedType.Name);
#endif
			_GetEventWrapper(eventType).OnEvent += handler;
		}

		public void UnsubscribeEvent(int eventType, Action<GameEventArgs> handler)
		{
#if _DEBUG_ && _DEBUG_GameEvent
			Console.WriteLine("UnsubscribeEvent, event: {0}, handler: {1}", (GameEvents)eventType, handler.Method.ReflectedType.Name);
#endif
			_GetEventWrapper(eventType).OnEvent -= handler;
		}

		public void FireEvent(GameEventArgs value)
		{
			var eventType = value.eventType;
			var wrapper = _events.GetEx((int)eventType);
			if (null != wrapper) 
			{
				wrapper.FireEvent (value);
			}
		}

		private EventWrapper _GetEventWrapper(int eventType)
		{
			return _events.GetEx(eventType) ?? _events.AddEx(eventType, new EventWrapper());
		}

		private readonly Dictionary<int, EventWrapper> _events = new Dictionary<int, EventWrapper>();
	}
}