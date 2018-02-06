﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.Nodes
{
    public class TreeWalker : ITreeWalker
    {
        public IEnumerable<INode> EnumerateFirstParentsUpTheTreeBranchAgnostic(INode source, bool includeSelf = false)
        {
            if (includeSelf)
            {
                yield return source;
            }

            INode n = source.Parents.FirstOrDefault();
            while (n != null)
            {
                yield return n;
                n = n.Parents.FirstOrDefault();
            }
        }

        public IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, DateTime? minimum)
        {
            var returned = new HashSet<INode>();

            return EnumerateAllParentsBreadthFirstInternal(source, minimum, returned);
        }

        private IEnumerable<INode> EnumerateAllParentsBreadthFirstInternal(
            INode source,
            DateTime? minimum,
            ICollection<INode> returned)
        {
            var parentsToGo = new List<INode>(source.Parents.Count);

            foreach (INode parent in source.Parents)
            {
                bool alreadyReturned = returned.Contains(parent);
                if (!alreadyReturned)
                {
                    yield return parent;
                    returned.Add(parent);
                    parentsToGo.Add(parent);
                }
            }

            foreach (INode parent in parentsToGo)
            {
                IEnumerable<INode> grandParents = EnumerateAllParentsBreadthFirstInternal(parent, minimum, returned);
                foreach (INode grandParent in grandParents)
                {
                    yield return grandParent;
                }
            }
        }
    }
}