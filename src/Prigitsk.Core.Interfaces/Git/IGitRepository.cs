using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Git
{
    public interface IGitRepository : IDisposable
    {
        IEnumerable<IGitBranch> Branches { get; }

        IEnumerable<IGitCommit> Commits { get; }

        IEnumerable<IGitRemote> Remotes { get; }

        IEnumerable<IGitTag> TagsOnCommits { get; }
    }
}