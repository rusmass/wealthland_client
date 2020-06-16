using System;
using System.IO;
using System.Text;

namespace Core.AutoCode
{
    public class CodeWriter : IDisposable
    {
        public CodeWriter(string path)
        {
            _writer = new StreamWriter(path);
            _writer.NewLine = os.linesep;

            _InitTabs();
            var lineComment = _GetLineCommend(path);
            _WriteFileHead(lineComment);
        }

        ~CodeWriter()
        {
            _DoDispose(false);
        }

        public void Dispose()
        {
            _DoDispose(true);
        }

        protected virtual void _DoDispose(bool isDisposing)
        {
            os.dispose(ref _writer);
        }

        public void IncreaseIndent()
        {
            _indent = _indent >= _kMaxIndentCount ? _kMaxIndentCount : (_indent + 1);
        }

        public void DecreaseIndent()
        {
            _indent = _indent <= 0 ? 0 : (_indent - 1);
        }

        public void Write(string message)
        {
            _writer.Write(_indentTexts[_indent]);
            _writer.Write(message ?? string.Empty);
        }

        public void Write(string format, params object[] args)
        {
            if (null != format)
            {
                _writer.Write(_indentTexts[_indent]);
                _writer.Write(string.Format(null, format, args));
            }
        }

        public void WriteLine()
        {
            _writer.WriteLine();
        }

        public void WriteLine(string message)
        {
            Write(message);
            _writer.WriteLine();
        }

        public void WriteLine(string format, params object[] args)
        {
            if (null != format)
            {
                Write(format, args);
                _writer.WriteLine();
            }
        }

        private void _InitTabs()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _kMaxIndentCount; ++i)
            {
                _indentTexts[i] = sb.ToString();
                sb.Append("    ");
            }
        }

        private string _GetLineCommend(string path)
        {
            if (path.EndsWith(".cs"))
            {
                return "//";
            }
            else if (path.EndsWith(".lua"))
            {
                return "-- ";
            }

            return string.Empty;
        }

        private void _WriteFileHead(string lineComment)
        {
            _writer.WriteLine();

            _writer.Write(lineComment);
            _writer.WriteLine("Warning: all code of this file are generated automatically, so do not modify it manually ~");

            _writer.WriteLine();
        }

        public StreamWriter BaseWriter { get { return _writer; } }
        public int Indent { get { return _indent; } }

        private const int _kMaxIndentCount = 20;
        private string[] _indentTexts = new string[_kMaxIndentCount];

        private StreamWriter _writer;
        private int _indent;
    }
}