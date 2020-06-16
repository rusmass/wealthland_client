using System;

namespace Core.FSM
{
	public class Event
	{
		public Event(int id)
		{
			ID = id;
		}

		public int ID { get; private set; }
	}

	public class TickEvent : Event
	{
		public TickEvent(float deltaTime)
			: base(-1)
		{
			DeltaTime = deltaTime;
		}

		public float DeltaTime { get; set; }
	}

	public class ResetEvent : Event
	{
		public ResetEvent()
			: base(-1)
		{
		}
	}
}