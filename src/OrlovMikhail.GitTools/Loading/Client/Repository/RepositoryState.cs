using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class RepositoryState : IRepositoryState
    {
        public CommitInfo[] CommitInfos { get; private set; }

        public RepositoryState(IEnumerable<CommitInfo> nodes)
        {
            CommitInfos = nodes.ToArray();
        }
    }
}