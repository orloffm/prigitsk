using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Entities
{
    /// <summary>
    /// Immutable representation of a Git commit.
    /// </summary>
    public interface ICommit
    {
        IHash Hash { get; }
        IEnumerable<IHash> Parents { get; }
        DateTimeOffset? CommittedWhen { get; }
        bool Equals(ICommit other);
    }
}