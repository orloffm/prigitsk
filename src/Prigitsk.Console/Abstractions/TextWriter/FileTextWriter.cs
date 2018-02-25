using System.IO;
using System.IO.Abstractions;
using System.Text;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Abstractions.TextWriter
{
    public sealed class FileTextWriter : ITextWriter
    {
        private StreamWriter _sw;

        public FileTextWriter(IFileSystem fileSystem, string path)
        {
            Stream stream = fileSystem.File.OpenWrite(path);
            _sw = new StreamWriter(stream, Encoding.UTF8);
        }

        public void Dispose()
        {
            if (_sw != null)
            {
                _sw.Dispose();
                _sw = null;
            }
        }

        public void Append(string value)
        {
            _sw.Write(value);
        }

        public void AppendLine(string line = null)
        {
            _sw.WriteLine(line);
        }

        public void AppendLine(string format, params object[] arg)
        {
            AppendLine(string.Format(format, arg));
        }
    }
}