using System;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
	public class ActionFactory
	{
		protected ActionFactory ()
		{
		}

		internal GameAction Create<T>()
		{
			bool isExists = false;
			var name = typeof(T).Name;
			_CheckActionExists (name, ref isExists);
			var id = _actionNames.FindIndex (value => value == name);

			return _actions [id];
		}

		internal GameAction Create(int actionId)
		{
			bool isExists = false;
			_CheckActionExists (actionId, ref isExists);
			var id = _actionIds.FindIndex (value => value == actionId);

			return _actions[id];
		}

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		private void _CheckActionExists(int actionId, ref bool isExists)
		{
			isExists = _actionIds.Contains (actionId);

			if (!isExists) 
			{
				Console.Error.WriteLine ("[ActionFactory.Create] Error no has action. actionName = " + actionId);
			}
		}

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		private void _CheckActionExists(string actionName, ref bool isExists)
		{
			isExists = _actionNames.Contains (actionName);

			if (!isExists) 
			{
				Console.Error.WriteLine ("[ActionFactory.Create] Error no has action. actionId = " + actionName);
			}
		}

		internal void RegisterAction(int actionId, GameAction action)
		{
			if (_actionIds.Contains (actionId)) 
			{
				Console.Error.WriteLine ("[ActionFactory.RegisterAction] Error actionId({0} already exists.)", actionId);
				return;
			}

			_actionIds.Add (actionId);
			_actionNames.Add (action.GetType ().Name);
			_actions.Add (action);
		}

		internal void Clear()
		{
			_actionIds.Clear ();
			_actionNames.Clear ();
			_actions.Clear ();
		}

		public static ActionFactory Instance = new ActionFactory();

		private List<int> _actionIds = new List<int>();
		private List<string> _actionNames = new List<string>();
		private List<GameAction> _actions = new List<GameAction>();
	}
}

