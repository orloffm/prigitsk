using System.IO;
using Thinktecture.IO;
using Thinktecture.IO.Adapters;
using Thinktecture.Text;

namespace Prigitsk.Framework.IO
{
    public class TextWriterFactory : ITextWriterFactory
    {
        public IStreamWriter CreateStreamWriter(IStream stream)
        {
            return new StreamWriterAdapter(stream);
        }

        public IStreamWriter CreateStreamWriter(IStream stream, IEncoding encoding)
        {
            return new StreamWriterAdapter(stream, encoding);
        }

        public IStringWriter CreateStringWriter()
        {
            return new StringWriterAdapter();
        }
    }
}