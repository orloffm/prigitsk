using Thinktecture.IO;
using Thinktecture.Text;

namespace Prigitsk.Framework.IO
{
    public interface ITextWriterFactory
    {
        IStreamWriter CreateStreamWriter(IStream stream);

        IStreamWriter CreateStreamWriter(IStream stream, IEncoding encoding);

        IStringWriter CreateStringWriter();
    }
}