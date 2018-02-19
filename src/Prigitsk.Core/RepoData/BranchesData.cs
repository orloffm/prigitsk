using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public sealed class BranchesData : EntityData<IBranch>, IBranchesData
    {
        public BranchesData(IEnumerable<IBranch> data) : base(data)
        {

        }

      public IEnumerable<IBranch> GetFor(IRemote remote)
        {
            return Data.Where(b => string.Equals(b.RemoteName, remote.RemoteName, StringComparison.OrdinalIgnoreCase));
        }
    }
}