﻿using System.Collections.Generic;
using System.Linq;

namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class Node
    {
        public Node(string hash, IEnumerable<Node> parentNodes, IEnumerable<string> branches, IEnumerable<string> tags, string description)
        {
            Hash = hash;
            Parents = parentNodes.ToArray();
            Branches = branches.ToArray();
            Tags = tags.ToArray();
            Description = description;
        }

        public string Description { get; private set; }

        public string[] Tags { get; private set; }

        public string[] Branches { get; private set; }

        public string Hash { get; private set; }

        public Node[] Parents { get; private set; }
    }
}