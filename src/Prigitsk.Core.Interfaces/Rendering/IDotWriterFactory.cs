using Thinktecture.IO;

namespace Prigitsk.Core.Rendering
{
    public interface IDotWriterFactory
    {
        /// <summary>
        ///     Creates a DOT format writer.
        /// </summary>
        IDotWriter CreateWriter(ITextWriter textWriter);
    }
}