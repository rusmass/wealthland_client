using System;
using System.Collections.Generic;

namespace Core
{
	public static class TypeTools
	{
		private class AssemblyComparer :IComparer<System.Reflection.Assembly>
		{
			public int Compare (System.Reflection.Assembly lhs, System.Reflection.Assembly rhs)
			{
				// version = "0.0.0.0" means this is an our own assembly.
				var leftVersion = lhs.FullName.Split(_splitter)[1];
				var rightVersion = rhs.FullName.Split(_splitter)[1];
				
				var result = leftVersion.CompareTo(rightVersion);
				if (result == 0)
				{
					result = lhs.FullName.CompareTo(rhs.FullName);
				}
				
				return result;
			}
			
			private readonly char[] _splitter = new char[] { '=' };
		}

		/*
		 * Through the assembly to take, there must be a namespace.classname parameter.
		 * */
		public static Type SerchType (string typeFullName)
		{
			if (null != typeFullName) 
			{
				if (null == _currentAssemblies)
				{
					_currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
					Array.Sort(_currentAssemblies, new AssemblyComparer());
				}

				var count = _currentAssemblies.Length;
				for (int i = 0; i < count; ++i)
				{
					var assembly = _currentAssemblies[i];
					var type = assembly.GetType(typeFullName);

					if (null != type)
					{
						return type;
					}
				}
			}

			return null;
		}

		public static void CreateDelegate<T> (System.Reflection.MethodInfo methodInfo, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate (typeof(T), methodInfo) as T;
		}

		public static void CreateDelegate<T> (object obj, string func, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate (typeof(T), obj, func) as T;
		}

		public static void CreateDelegate<T> (Type classType, string func, out T lpfnMethod) where T: class
		{
			lpfnMethod = Delegate.CreateDelegate (typeof(T), classType, func) as T;
		}

		public static System.Reflection.Assembly GetEditorAssembly()
		{
			if (null != _editorAssembly) 
			{
				foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					var fullName = assembly.FullName;
					if (fullName.StartsWith("Assembly-CSharp-Editor"))
					{
						_editorAssembly = assembly;
						break;
					}
				}
			}

			return _editorAssembly;
		}

		private static System.Reflection.Assembly _editorAssembly;
		private static System.Reflection.Assembly[] _currentAssemblies;
	}
}