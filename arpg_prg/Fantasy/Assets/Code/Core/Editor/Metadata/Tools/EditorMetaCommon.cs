using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Metadata
{
    public static class EditorMetaCommon
    {
		public static void Init (MetaManager metaManager)
		{
			if (null != metaManager)
			{
				_baseTypes.Clear();

				foreach (var type in metaManager.MetaTypes)
				{
					var baseType = type.RawType.BaseType;
					if (null != baseType)
					{
						_baseTypes.Add(baseType);
					}
				}
			}
		}

		public static bool IsFinalType (Type type)
		{
			if (null != type)
			{
				var isFinal = !_baseTypes.Contains(type);
				return isFinal;
			}

			return false;
		}

		public static string GetMetaTypeName (Type type)
		{
			var fullname = type.FullName;
			
			var withoutNamespace = fullname;
			if (type.Namespace != null)
			{
				withoutNamespace = fullname.Substring(type.Namespace.Length + 1);
			}
			
			return withoutNamespace.Replace('+', '_').Replace('.', '_');
		}
		
		public static bool IsMetadata (Type type)
		{
			return typeof(IMetadata).IsAssignableFrom(type) && !type.IsInterface;
		}

        public static bool IsAutoCodeIgnore (MemberInfo member)
        {
            var ignore = member.GetCustomAttributes(typeof(AutoCodeIgnoreAttribute), false).Length > 0;
            return ignore;
        }
        
        public static Type GetRootMetadata (Type type)
        {
            while(true)
            {
                var baseType = type.BaseType;

				if(!IsMetadata(baseType))
                {
                    break;
                }

                type = baseType;
            }

            return type;
        }
        
        public static string GetNestedClassName (Type type)
        {
            var fullname = type.FullName;
            var withoutNamespace = fullname;
            if (type.Namespace != null)
            {
                withoutNamespace = fullname.Substring(type.Namespace.Length + 1);
            }

            return !type.IsNested ? withoutNamespace : withoutNamespace.Replace('+', '.');
        }

		public static ExportFlags GetExportFlags (Type type)
		{
			var attributes = type.GetCustomAttributes(typeof(ExportAttribute), false);
			
			foreach (ExportAttribute attribute in attributes)
			{
				var flags = attribute.GetExportFlags();
				return flags;
			}

			return ExportFlags.ExportRaw;
		}

		public static CodeAssembly GetCodeAssembly (Type type)
		{
			var codeAssembly = CodeAssembly.None;
			var fullname = type.Assembly.FullName;

			if (fullname.StartsWith("Assembly-CSharp-firstpass"))
			{
				codeAssembly = CodeAssembly.StandardAssembly;
			}
			else if (fullname.StartsWith("Assembly-CSharp-Editor"))
			{
				codeAssembly = CodeAssembly.EditorAssembly;
			}
			else 
			{
				codeAssembly = CodeAssembly.ClientAssembly;
			}

			return codeAssembly;
		}

		private static string _standardAutoCodeDirectory;
		public static string StandardAutoCodeDirectory
		{
			get
			{
				if (null == _standardAutoCodeDirectory)
				{
					_standardAutoCodeDirectory = Application.dataPath + "/Standard Assets/Code/Metadata/AutoCode/";
				}
				
				return _standardAutoCodeDirectory;
			}
		}

		private static string _clientAutoCodeDirectory;
        public static string ClientAutoCodeDirectory
        {
            get
            {
				if (null == _clientAutoCodeDirectory)
				{
					_clientAutoCodeDirectory = Application.dataPath + "/Code/Metadata/AutoCode/";
				}

				return _clientAutoCodeDirectory;
            }
        }

		private static string _editorAutoCodeDirectory;
		public static string EditorAutoCodeDirectory
		{
			get
			{
				if (null == _editorAutoCodeDirectory)
				{
					_editorAutoCodeDirectory = Application.dataPath + "/Code/Metadata/Editor/AutoCode/";
				}
				
				return _editorAutoCodeDirectory;
			}
		}

        public const string MenuRoot = "*Metadata/";

		private static HashSet<Type> _baseTypes = new HashSet<Type>();
    }
}