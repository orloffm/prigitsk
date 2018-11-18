using System;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.Graph
{
    public class TreeWalker : ITreeWalker
    {
        public IEnumerable<INode> EnumerateAllParentsBreadthFirst(INode source, INode dontGoEarlierThan = null)
        {
            var returned = new HashSet<INode>();

            DateTimeOffset? minimum = dontGoEarlierThan == null
                ? (DateTimeOffset?) null
                : GetCommitterTime(dontGoEarlierThan);

            return EnumerateAllParentsBreadthFirstInternal(source, minimum, returned);
        }

        private IEnumerable<INode> EnumerateAllParentsBreadthFirstInternal(
            INode node
            , DateTimeOffset? minimum
            , ICollection<INode> returned
        )
        {
            INode[] level = {node};

            do
            {
                // All suitable nodes one level above.
                INode[] upperLevel = level.Select(parent => EnumeratePassingCurrentParents(parent, minimum, returned))
                    .SelectMany(n => n)
                    .Distinct()
                    .ToArray();

                // Return this level.
                foreach (INode parent in upperLevel)
                {
                    yield return parent;
                    returned.Add(parent);
                }

                level = upperLevel;
            } while (level.Length > 0);
        }

        private IEnumerable<INode> EnumeratePassingCurrentParents(
            INode source
            , DateTimeOffset? minimum
            , ICollection<INode> returned
        )
        {
            foreach (INode parent in source.Parents)
            {
                bool alreadyReturned = returned.Contains(parent);
                if (alreadyReturned)
                {
                    continue;
                }

                if (minimum != null)
                {
                    DateTimeOffset itsDate = GetCommitterTime(parent);
                    if (itsDate < minimum.Value)
                    {
                        continue;
                    }
                }

                yield return parent;
            }
        }

        private DateTimeOffset GetCommitterTime(INode n)
        {
            return n.Commit.Committer.When;
        }
    }
}