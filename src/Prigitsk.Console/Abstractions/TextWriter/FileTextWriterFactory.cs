using System.IO.Abstractions;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Abstractions.TextWriter
{
    public sealed class FileTextWriterFactory : IFileTextWriterFactory
    {
        private readonly IFileSystem _fileSystem;

        public FileTextWriterFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ITextWriter OpenForWriting(string path)
        {
            return new FileTextWriter(_fileSystem, path);
        }
    }
}