using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public interface ICommitsData : IEntityData<ICommit>
    {
        /// <summary>
        ///     Returns the commit, its first parent, and so on up to the initial commit of the repository.
        /// </summary>
        IEnumerable<ICommit> EnumerateUpTheHistoryFrom(ICommit tip);

        /// <summary>
        ///     Returns the commit by hash.
        /// </summary>
        ICommit GetByHash(IHash hash);
    }
}