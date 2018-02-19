using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public interface IBranchesData : IEntityData<IBranch>
    {
        IEnumerable<IBranch> GetFor(IRemote remote);
    }
}