using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Immutable representation of a Git commit.
    /// </summary>
    public interface ICommit : ITreeish
    {
        DateTimeOffset? CommittedWhen { get; }

        IHash Hash { get; }

        IEnumerable<IHash> Parents { get; }

        bool Equals(ICommit other);
    }
}