using Thinktecture.IO;

namespace Prigitsk.Framework.IO
{
    public interface IFileInfoFactory
    {
        IFileInfo Create(string path);
    }
}