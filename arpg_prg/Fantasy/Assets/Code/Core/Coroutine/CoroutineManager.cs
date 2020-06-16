using System;
using System.Collections;

namespace Core
{
	public static class CoroutineManager
	{
		static CoroutineManager()
		{
			EditorCallBack.AttachToUpdate (Tick);
		}

		public static IEnumerator StartCoroutine(IEnumerator routine)
		{
			if (null != routine) 
			{
				try
				{
					if (routine.MoveNext())
					{
						_pool.Spawn(routine, true);
					}
				}
				catch(Exception ex)
				{
					Console.Error.WriteLine("[CoroutineManager.StartCoroutine() ex = {0}]", ex.ToStringEx());
				}
			}

			return routine;
		}

		public static void StopAllCoroutines()
		{
			_pool.Clear ();
		}

		public static void KillCoroutine(ref IEnumerator routine)
		{
			if (null != routine) 
			{
				var count = _pool.Count;
				for (int i = 0; i < count; ++i)
				{
					var item = _pool[i];
					if (item.routine == routine)
					{
						item.Kill();
						break;
					}
				}

				routine = null;
			}
		}

		internal static void Tick()
		{
			if (_pool.Count > 0) 
			{
				// 1. _pool.Count may increase at item.coroutine.MoveNext() because StartCoroutine() may be called;
				// 2. So we use snapshotCount to make sure we only call the snapshoted items;

				var snapshotCount = _pool.Count;
				var someDone = false;

				for (int i = 0; i < snapshotCount; ++i)
				{
					var item = _pool[i];
					if (!item.isDone && !item.isKilled)
					{
						try
						{
							var routine = item.routine;
							var checkDone = routine.Current as IIsYieldable;
							if (null == checkDone || checkDone.isYieldable)
							{
								item.isDone = !routine.MoveNext();
							}
						}
						catch(Exception ex)
						{
							item.isDone = true;
							Console.Error.WriteLine("[CoroutineManager.Tick() ex = {0}]", ex.ToStringEx());
						}
					}

					if (item.isDone || item.isKilled)
					{
						someDone = true;
					}
				}

				if (someDone)
				{
					_pool.Recycle();
				}
			}
		}

		private static readonly CoroutinePool _pool = new CoroutinePool();
	}
}
