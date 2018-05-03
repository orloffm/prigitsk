using Thinktecture.IO;
using Thinktecture.IO.Adapters;

namespace Prigitsk.Framework.IO
{
    public sealed class FileSystem : IFileSystem
    {
        public FileSystem(
            IFileInfoFactory fileInfoFactory,
            IDirectoryInfoFactory directoryInfoFactory)
        {
            FileInfo = fileInfoFactory;
            DirectoryInfo = directoryInfoFactory;
            File = new FileAdapter();
            Directory = new DirectoryAdapter();
            Path = new PathAdapter();
        }

        public IDirectory Directory { get; }

        public IDirectoryInfoFactory DirectoryInfo { get; }

        public IFile File { get; }

        public IFileInfoFactory FileInfo { get; }

        public IPath Path { get; }
    }
}