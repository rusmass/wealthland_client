using System;

namespace Metadata
{
    public class MetaType
    {
		public MetaType (Type rawType)
		{
			_rawType = rawType;

			if (typeof(Template).IsAssignableFrom(rawType))
			{
				IsTemplate = true;
			} 
			else if (typeof(Config).IsAssignableFrom(rawType))
			{
				IsConfig = true;
			}
		}

		public CodeAssembly GetCodeAssembly ()
		{
			if (_codeAssembly == CodeAssembly.None)
			{
				var fullname = _rawType.Assembly.FullName;
				if (fullname.StartsWith("Assembly-CSharp-firstpass"))
				{
					_codeAssembly = CodeAssembly.StandardAssembly;
				}
				else if (fullname.StartsWith("Assembly-CSharp-Editor"))
				{
					_codeAssembly = CodeAssembly.EditorAssembly;
				}
				else 
				{
					_codeAssembly = CodeAssembly.ClientAssembly;
				}
			}

			return _codeAssembly;
		}

		public string GetAutoCodeDirectory ()
		{
			var codeAssembly = GetCodeAssembly();

			if (codeAssembly == CodeAssembly.StandardAssembly)
			{
				return EditorMetaCommon.StandardAutoCodeDirectory;
			}
			else if (codeAssembly == CodeAssembly.ClientAssembly)
			{
				return EditorMetaCommon.ClientAutoCodeDirectory;
			}
			else if (codeAssembly == CodeAssembly.EditorAssembly)
			{
				return EditorMetaCommon.EditorAutoCodeDirectory;
			}
			
			return string.Empty;
		}

        public string GetAutoCodePath()
        {
            var path = GetAutoCodeDirectory() + _rawType.Name + ".AutoCode.cs";
            return path;
        }

		public string GetMetaTypeName ()
		{
			return EditorMetaCommon.GetMetaTypeName(_rawType);
		}

		public ExportFlags GetExportFlags ()
		{
			if (_exportFlags == ExportFlags.None)
			{
				_exportFlags = EditorMetaCommon.GetExportFlags(_rawType);
			}

			return _exportFlags;
		}

		public Type RawType		{ get { return _rawType; } }
		public bool IsTemplate	{ get; private set; }
		public bool IsConfig	{ get; private set; }
		public int	MetaIndex	{ get; set; }

		public string Name			{ get { return _rawType.Name; } }
		public string Namespace 	{ get { return _rawType.Namespace; } }
		public string FullName		{ get { return _rawType.FullName; } }
		public bool IsAbstract		{ get { return _rawType.IsAbstract; } }
		public bool IsNested		{ get { return _rawType.IsNested; } }
		public bool IsSerializable	{ get { return _rawType.IsSerializable; } }

		private Type _rawType;
		private CodeAssembly _codeAssembly;
		private ExportFlags _exportFlags;
    }
}