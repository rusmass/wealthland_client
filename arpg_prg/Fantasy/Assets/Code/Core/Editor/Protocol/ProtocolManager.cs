using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Core.AutoCode;
using Core;
using Net;

namespace Protocol
{
	public class ProtocolManager
	{
		public ProtocolManager ()
		{
			_GetAllTypes ();
		}

		private void _GetAllTypes ()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try 
				{
					foreach (var type in assembly.GetTypes())
					{
						if (typeof(SocketMessage).IsAssignableFrom (type) && type.BaseType != typeof(System.Object))
						{
							var files = type.GetFields();
							Console.WriteLine(1);


							for (int i = 0; i < files.Length; ++i)
							{
//								Console.WriteLine(files[i].Name + files[i].GetType());
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("assembly={0}, ex={1}", assembly.FullName, ex.ToString());
				}
			}
		}
	}
}