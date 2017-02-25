using System.Collections.Generic;

namespace OrlovMikhail.GitTools.Processing
{
    public class Node
    {
        public Node(string hash)
        {
            Hash = hash;
            Parents = new List<Node>();
            Children = new List<Node>();
            Tags = new List<Tag>();
            Branches = new List<Branch>();
        }

        public string Hash { get; private set; }

        public List<Node> Parents { get; private set; }

        public List<Node> Children { get; private set; }

        public List<Tag> Tags { get; private set; }

        public List<Branch> Branches { get; private set; }

        public Branch AssignedBranch { get; set; }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}