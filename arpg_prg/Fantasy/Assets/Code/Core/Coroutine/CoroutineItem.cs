using System;
using System.Collections;

namespace Core
{
	public class CoroutineItem : IIsYieldable
	{
		internal CoroutineItem ()
		{

		}

		public void Kill ()
		{
			isKilled = true;
		}

		bool IIsYieldable.isYieldable
		{ 
			get 
			{
				return isDone || isKilled;
			}
		}

		internal IEnumerator 	routine 		{ get; set;}
		internal bool	 		isRecyclable 	{ get; set;}

		public bool 			isDone 			{ get; internal set;}
		public bool 			isKilled 		{ get; internal set;}
	}
}