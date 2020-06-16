using System;

namespace Metadata
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
    public class ExportAttribute: Attribute
    {
		public ExportAttribute (ExportFlags flags)
		{
			_flags = flags;
		}

		public ExportFlags GetExportFlags ()
		{
			return _flags;
		}

		private ExportFlags	_flags;
    }
}
