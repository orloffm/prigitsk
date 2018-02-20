using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Git
{
    public interface IRepository : IDisposable
    {
        IEnumerable<IBranch> Branches { get; }

        IEnumerable<ICommit> Commits { get; }

        IEnumerable<IRemote> Remotes { get; }

        IEnumerable<ITag> TagsOnCommits { get; }
    }
}