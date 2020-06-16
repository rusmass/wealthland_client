using System;
using UnityEditor;

namespace Core
{
	public struct LayoutHorizontalScope: IDisposable
	{
		public LayoutHorizontalScope(int placeholder)
		{
			EditorGUILayout.BeginHorizontal();
		}

		public void Dispose()
		{
			try
			{
				EditorGUILayout.EndHorizontal();	
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.ToString ());
			}
		}
	}
}