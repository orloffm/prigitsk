using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace Prigitsk.Console.Abstractions.TextWriter
{
    public sealed class FileTextWriter : IFileTextWriter
    {
        private StreamWriter _sw;

        public FileTextWriter(IFileSystem fileSystem, string path, Encoding encoding = null)
        {
            Stream stream = fileSystem.File.OpenWrite(path);
            _sw = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            _sw.AutoFlush = true;
        }

        public void Append(string value)
        {
            _sw.Write(value);
        }

        public void AppendLine()
        {
            _sw.WriteLine();
        }

        public void AppendLine(string line)
        {
            _sw.WriteLine(line);
        }

        public void AppendLine(string format, params object[] arg)
        {
            AppendLine(string.Format(format, arg));
        }

        public void Dispose()
        {
            if (_sw != null)
            {
                _sw.Flush();
                _sw.Dispose();
                _sw = null;
            }
        }
    }
}