using UnityEngine;

namespace Core
{
	public class WaitForSeconds : IIsYieldable
	{
		public WaitForSeconds(float delayTime)
		{
			_endTime = Time.realtimeSinceStartup + delayTime;
		}

		bool IIsYieldable.isYieldable 
		{ 
			get
			{
				return Time.realtimeSinceStartup >= _endTime;
			}
		}

		private float _endTime;
	}
}
