using System;
using UnityEngine;

namespace Core
{
	internal static class EditorCallBack
	{
		public static void AttachToUpdate(Action action)
		{
			if (!Application.isEditor || null == action) 
			{
				return;
			}

			if (null == _lpfnAttachToUpdate) 
			{
				var type = TypeTools.SerchType("Core.EditorTools");

				if (null != type)
				{
					TypeTools.CreateDelegate(type, "_AttachToUpdate", out _lpfnAttachToUpdate);
				}
			}

			if (null != _lpfnAttachToUpdate) 
			{
				_lpfnAttachToUpdate (action);
			}
			else 
			{
				Console.Error.WriteLine("[EditorCallBack._lpfnAttachToUpdate]");
			}
		}

		private static System.Action<Action> _lpfnAttachToUpdate;
	}
}
