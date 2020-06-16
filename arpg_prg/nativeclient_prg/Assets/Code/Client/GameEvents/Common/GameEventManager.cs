using System;
using System.Collections.Generic;

namespace Client
{
	public sealed class GameEventManager
	{
		public enum Usage
		{
			Immediate,
			AfterTick,
		}
        
		static GameEventManager()
		{
		}

		private GameEventManager()
		{

		}

		public static GameEventManager Instance = new GameEventManager();

		public void FireEvent(GameEvents eventType, Usage usage = Usage.Immediate)
		{
			FireEvent(new GameEventArgs(eventType), usage);
		}

		public void SubscribeEvent(GameEvents eventType, Action<GameEventArgs> handler)
		{
			_source.SubscribeEvent((int)eventType, handler);
		}

		public void UnsubscribeEvent(GameEvents eventType, Action<GameEventArgs> handler)
		{
			_source.UnsubscribeEvent((int)eventType, handler);
		}

		public void FireEvent(GameEventArgs value, Usage usage = Usage.Immediate)
		{
			switch (usage)
			{
			case Usage.Immediate:
				_FireEventImmediate(value);
				break;
			case Usage.AfterTick:
				_tickEvents.Add(value);
				break;
			}
		}

		private void _FireEventImmediate(GameEventArgs value)
		{
			if (null == value) 
			{
				Console.Error.WriteLine ("GameEventManager._FireEventImmediate[] value is null!");
			}
			_source.FireEvent(value);
		}

		public void Tick(float deltaTime)
		{
			if (_tickEvents.Count > 0)
			{
				for (int i = 0; i < _tickEvents.Count; ++i)
				{
					_FireEventImmediate(_tickEvents[i]);
				}
				_tickEvents.Clear();
			}
		}


		private GameEventSource _source = new GameEventSource();
		private List<GameEventArgs> _tickEvents = new List<GameEventArgs>();
	}
}
