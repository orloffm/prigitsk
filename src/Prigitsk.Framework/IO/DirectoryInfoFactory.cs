using Thinktecture.IO;
using Thinktecture.IO.Adapters;

namespace Prigitsk.Framework.IO
{
    public class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        public IDirectoryInfo Create(string path)
        {
            return new DirectoryInfoAdapter(path);
        }
    }
}