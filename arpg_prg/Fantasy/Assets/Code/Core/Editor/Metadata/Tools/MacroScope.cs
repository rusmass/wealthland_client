using System;
using System.IO;

namespace Core.AutoCode
{
    public struct MacroScope : IDisposable
    {
        private MacroScope(StreamWriter writer, string prefix)
        {
            _writer = writer;

            if (!string.IsNullOrEmpty(prefix))
            {
                _writer.Write(prefix);
            }
        }

        public static MacroScope CreateEditorScope(StreamWriter writer)
        {
            return new MacroScope(writer, "#if UNITY_EDITOR\n");
        }

        public void Dispose()
        {
            _writer.Write("#endif\n");
        }

        private StreamWriter _writer;
    }
}