using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class RepositoryData : IRepositoryData
    {
        public Node[] Nodes { get; private set; }

        public RepositoryData(IEnumerable<Node> nodes)
        {
            Nodes = nodes.ToArray();
        }
    }
}