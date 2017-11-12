﻿using System;
using System.Collections.Generic;
using System.Linq;
using GitWriter.Core.Graph.Strategy;
using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph.Making
{
    public class BranchAssumer : IBranchAssumer
    {
        private readonly IBranchingStrategy _bs;
        private readonly Func<Pointer, bool> _pickStrategy;

        public BranchAssumer(IBranchingStrategy bs, Func<Pointer, bool> pickStrategy)
        {
            _bs = bs;
            _pickStrategy = pickStrategy;
        }

        public IAssumedGraph AssumeTheBranchGraph(IEnumerable<Node> nodesEnumerable)
        {
            var unprocessedNodes = new HashSet<Node>(nodesEnumerable);

            // All origin branches and tags that we have.
            Pointer[] allPointers = GetAllOriginBranchesAndTags(unprocessedNodes)
                .Where(_pickStrategy)
                .ToArray();
            OriginBranch[] allBranches = allPointers.OfType<OriginBranch>().ToArray();
            Tag[] allTags = allPointers.OfType<Tag>().ToArray();

            // Ask branching strategy to sort them.
            OriginBranch[] sortedBranches = _bs.SortByPriority(allBranches).ToArray();

            // Now we build the graph.
            IAssumedGraph ret = new AssumedGraph();
            foreach (OriginBranch b in sortedBranches)
            {
                var nodesInBranch = new List<Node>(unprocessedNodes.Count / 2);
                IEnumerable<Node> upTheTree = b.Source.EnumerateFirstParentsUpTheTreeBranchAgnostic(true);
                foreach (Node node in upTheTree)
                {
                    if (!unprocessedNodes.Contains(node))
                    {
                        break;
                    }
                    nodesInBranch.Add(node);
                    unprocessedNodes.Remove(node);
                }
                nodesInBranch.Reverse();
                ret.SetBranchNodes(b, nodesInBranch);
            }
            ret.SetTags(allTags);
            ret.SetLeftOverNodes(unprocessedNodes);

            return ret;
        }

        private IEnumerable<Pointer> GetAllOriginBranchesAndTags(IEnumerable<Node> nodes)
        {
            foreach (Node n in nodes)
            {
                foreach (GitRef c in n.GitRefs)
                {
                    if (c.IsOriginBranch)
                    {
                        OriginBranch b = OriginBranch.FromCaption(c, n);
                        yield return b;
                    }
                    else if (c.IsTag)
                    {
                        Tag t = Tag.FromCaption(c, n);
                        yield return t;
                    }
                }
            }
        }
    }
}