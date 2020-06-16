using System;

namespace Core.FSM
{
	public class FiniteStateMachine<T>
	{
		public class State
		{
			public State(T content)
			{
				if (content == null)
				{
					Console.Error.WriteLine("FSMState with content == null");
				}
				_Content = content;
			}

			public State Input(Event e)
			{
				return _DoEvent(e);
			}

			public State Tick(float deltaTime)
			{
				return _DoTick(deltaTime);
			}

			public virtual void Enter(Event e, State lastState)
			{
				_OnReset ();
				_OnEnter(e, lastState);
			}

			public void Exit(Event e, State nextState)
			{
				_OnExit(e, nextState);
			}

			protected virtual void _OnReset() {}
			protected virtual void _OnEnter(Event e, State lastState) {}
			protected virtual void _OnExit(Event e, State nextState) {}
			protected virtual State _DoEvent(Event e) { return this; }
			protected virtual State _DoTick(float deltaTime) { return this; }

			protected T _Content { get; set;}
		}

		public FiniteStateMachine(T content)
		{
			if (content == null)
			{
				Console.Error.WriteLine("FiniteStateMachine with content == null");
			}

			_Content = content;
		}

		public void Start()
		{
			_CreateEnterState ();
		}

		public void Stop()
		{
			_ChangeCurrentState (null, null);
		}

		public virtual bool Input(Event e)
		{
			if (_currentState == null)
			{
				Console.Error.WriteLine("FiniteStateMachine next state == null");
			}

			State nextState = _currentState.Input(e);
			if (nextState != _currentState)
			{
				return _ChangeCurrentState(nextState, e);
			}

			return false;
		}

		public virtual bool Tick(float deltaTime)
		{
			if (_currentState != null)
			{
				var caller = _currentState;
				State nextState = caller.Tick(deltaTime);
				if (caller != _currentState)
				{
					return false;
				}
				if (nextState != _currentState)
				{
					TickEvent e = new TickEvent(deltaTime);
					return _ChangeCurrentState(nextState, e);
				}
			}
			return false;
		}

		private bool _ChangeCurrentState(State nextState, Event e)
		{
			if (_currentState != null)
			{
				_OnPreStateChange();
				_currentState.Exit(e, nextState);
			}

			State lastState = _currentState;
			_currentState = nextState;

			if (nextState != null)
			{
				_currentState.Enter(e, lastState);
				_OnPostStateChange();
			}
			return true;
		}

		private void _CreateEnterState() 
		{
			if (null == _enterStateConstructor) 
			{
				Console.Error.WriteLine ("[FiniteStateMachine._enterStateConstructor is null!]");
			}

			var state = _enterStateConstructor ();
			ResetEvent e = new ResetEvent();
			_ChangeCurrentState(state, e);
		}

		protected T _Content { get; set;}
		protected Func<State> _enterStateConstructor;
		protected virtual void _OnPreStateChange() {}
		protected virtual void _OnPostStateChange() {}
		protected State _currentState { get; set;}
	}
}