using System.Collections.Generic;

namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Immutable representation of a Git commit.
    /// </summary>
    public interface ICommit : ITreeish
    {
        ISignature Author { get; }

        ISignature Committer { get; }

        IHash Hash { get; }

        string Message { get; }

        IEnumerable<IHash> Parents { get; }

        bool Equals(ICommit other);
    }
}