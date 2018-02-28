using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public sealed class RemotesData : EntityData<IRemote>, IRemotesData
    {
        public RemotesData(IEnumerable<IRemote> data) : base(data)
        {
        }

        public IRemote GetRemoteByName(string name)
        {
            return Data.FirstOrDefault(r => string.Equals(r.RemoteName, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}