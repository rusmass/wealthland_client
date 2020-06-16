using UnityEditor;

namespace Core
{
	public static class PublishTools
	{
		public static void DefineGlobalMacro(BuildTargetGroup target, string symbol)
		{
			var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup (target);
			if (!defineSymbols.Contains (symbol)) 
			{
				defineSymbols += ";" + symbol;

				PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defineSymbols);
			}
		}

		public static void RemoveGlobalMacro(BuildTargetGroup target, string symbol)
		{
			var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup (target);
			if (defineSymbols.Contains (symbol)) 
			{
				defineSymbols.Replace(symbol, string.Empty);
				defineSymbols.Replace(";;", ";");

				PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defineSymbols);
			}
		}
	}
}

