using Thinktecture.IO;
using Thinktecture.IO.Adapters;

namespace Prigitsk.Framework.IO
{
    public sealed class FileInfoFactory : IFileInfoFactory
    {
        public IFileInfo Create(string path)
        {
            return new FileInfoAdapter(path);
        }
    }
}