using Prigitsk.Core.Tools;

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