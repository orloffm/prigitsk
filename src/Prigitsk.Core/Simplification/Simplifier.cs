using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Simplification
{
    public sealed class Simplifier : ISimplifier
    {
        private readonly ILogger _log;

        public Simplifier( ILogger log)
        {
            _log = log;
        }

        public void Simplify(ITree tree, ISimplificationOptions options)
        {
            // First, remove orphans, if applicable.
            if (options.RemoveOrphans)
            {
                RemoveOrphans(tree, options.RemoveOrphansEvenWithTags);
            }
        }

        /// <summary>
        /// Removes orphaned nodes (i.e. not contained in branches) that don't have tags.
        /// </summary>
        private void RemoveOrphans(ITree tree, bool removeOrphansEvenWithTags)
        {
            INode[] nodes = tree.Nodes.ToArray();

            foreach (INode node in nodes)
            {
                // It shouldn't be on any branch.
                IBranch containingBranch = tree.GetContainingBranch(node);
                if (containingBranch != null)
                {
                    continue;
                }

                if (!removeOrphansEvenWithTags)
                {
                    // It shouldn't have tags.
                    ITag someTag = tree.GetPointingTags(node).FirstOrDefault();
                    if (someTag != null)
                    {
                        continue;
                    }
                }

                tree.RemoveNode(node);
            }
        }
    }
}