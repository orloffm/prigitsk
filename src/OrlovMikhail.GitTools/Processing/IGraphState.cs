using System.Collections.Generic;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Processing
{
    public interface IGraphState
    {
        void AddNode(string hash, CommitInfo[] ciParents);

        IEnumerable<Node> EnumerateUpFrom(string hash);

        void AssignBranch(string hash, string branch);

        void AddTag(string hash, string tagName);

        void AddBranch(string hash, string branch);
    }
}