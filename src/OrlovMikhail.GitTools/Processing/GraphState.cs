using System;
using System.Collections.Generic;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Processing
{
    public class GraphState : IGraphState
    {
        public void AddNode(string hash, CommitInfo[] ciParents)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> EnumerateUpFrom(string hash)
        {
            throw new NotImplementedException();
        }

        public void AssignBranch(string hash, string branch)
        {
            throw new NotImplementedException();
        }

        public void AddTag(string hash, string tagName)
        {
            throw new NotImplementedException();
        }

        public void AddBranch(string hash, string branch)
        {
            throw new NotImplementedException();
        }
    }
}