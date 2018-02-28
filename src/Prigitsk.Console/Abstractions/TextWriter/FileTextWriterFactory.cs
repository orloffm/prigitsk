using System;
using System.IO.Abstractions;
using System.Text;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Abstractions.TextWriter
{
    public sealed class FileTextWriterFactory : IFileTextWriterFactory
    {
        private readonly Func<string, Encoding, IFileTextWriter> _writerMaker;

        public FileTextWriterFactory( Func<string, Encoding, IFileTextWriter> writerMaker)
        {
            _writerMaker = writerMaker;
        }

        public ITextWriter OpenForWriting(string path, Encoding encoding = null)
        {
            return _writerMaker(path, encoding);
        }
    }
}