using System;
using System.Collections.Generic;

namespace Prigitsk.Core.Git
{
    public interface IRepository : IDisposable
    {
        IEnumerable<ICommit> Commits { get; }
        IEnumerable<IBranch> Branches { get; }
        IEnumerable<ITag> TagsOnCommits { get; }
        IEnumerable<IRemote> Remotes { get; }
    }
}