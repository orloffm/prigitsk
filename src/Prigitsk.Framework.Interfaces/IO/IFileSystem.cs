using Thinktecture.IO;

namespace Prigitsk.Framework.IO
{
    public interface IFileSystem
    {
        IDirectory Directory { get; }

        IDirectoryInfoFactory DirectoryInfo { get; }

        IFile File { get; }

        IFileInfoFactory FileInfo { get; }

        IPath Path { get; }
    }
}