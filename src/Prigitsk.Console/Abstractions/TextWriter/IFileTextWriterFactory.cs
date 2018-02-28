using System.Text;
using Prigitsk.Core.Tools;

namespace Prigitsk.Console.Abstractions.TextWriter
{
    /// <summary>
    ///     Creates a text writer that writes to files.
    /// </summary>
    public interface IFileTextWriterFactory
    {
        ITextWriter OpenForWriting(string path, Encoding encoding = null);
    }
}