using Thinktecture.IO;

namespace Prigitsk.Framework.IO
{
    public interface IDirectoryInfoFactory
    {
        IDirectoryInfo Create(string path);
    }
}