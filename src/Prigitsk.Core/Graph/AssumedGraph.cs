using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph
{
    [Obsolete]
    public class AssumedGraph : IAssumedGraph
    {
        private readonly Dictionary<INode, OriginBranch> _branchesByNodes;
        private readonly Dictionary<OriginBranch, List<INode>> _nodesByBranchConsecutive;
        private HashSet<INode> _leftOvers;
        private Tag[] _tags;

        public AssumedGraph()
        {
            _nodesByBranchConsecutive = new Dictionary<OriginBranch, List<INode>>();
            _branchesByNodes = new Dictionary<INode, OriginBranch>();
            _tags = new Tag[0];
        }

        public bool AnyPointersAreSourcedFrom(INode currentNode)
        {
            return _tags.Any(t => t.Source == currentNode)
                   || _nodesByBranchConsecutive.Keys.Any(b => b.Source == currentNode);
        }

        public IEnumerable<INode> EnumerateAllContainedNodes()
        {
            foreach (INode n in _branchesByNodes.Keys)
            {
                yield return n;
            }

            foreach (INode n in EnumerateAllLeftOvers())
            {
                yield return n;
            }
        }

        public IEnumerable<INode> EnumerateAllLeftOvers()
        {
            return _leftOvers;
        }

        public IEnumerable<INode> EnumerateLeftOversWithoutBranchesAndTags()
        {
            return EnumerateAllLeftOvers()
                .Where(
                    n =>
                    {
                        // It didn't have any git refs at all.
                        if (n.HasTagsOrNonLocalBranches == false)
                        {
                            return true;
                        }

                        // OK, it had something, but do we got them in the graph?
                        // Let's check if some tags or branches were sourced from it.
                        if (_tags.Any(t => t.Source == n))
                        {
                            return false;
                        }

                        if (_nodesByBranchConsecutive.Keys.Any(b => b.Source == n))
                        {
                            return false;
                        }

                        return true;
                    });
        }

        private IEnumerable<INode> EnumerateNodesAround(INode nodeInQuestion, bool up)
        {
            // If there it's not on a branch, we don't return anything.
            OriginBranch branch = GetBranch(nodeInQuestion);
            if (branch == null)
            {
                yield break;
            }

            List<INode> nodesList = _nodesByBranchConsecutive[branch];
            int index = nodesList.IndexOf(nodeInQuestion);
            int step = up ? -1 : +1;
            for (int i = index + step; i >= 0 && i < nodesList.Count; i += step)
            {
                yield return nodesList[i];
            }
        }

        public IEnumerable<INode> EnumerateNodesDownTheBranch(INode nodeInQuestion)
        {
            return EnumerateNodesAround(nodeInQuestion, false);
        }

        public IEnumerable<INode> EnumerateNodesUpTheBranch(INode nodeInQuestion)
        {
            return EnumerateNodesAround(nodeInQuestion, true);
        }

        public Tag[] GetAllTags()
        {
            return _tags;
        }

        public OriginBranch GetBranch(INode node)
        {
            OriginBranch branch;
            _branchesByNodes.TryGetValue(node, out branch);
            return branch;
        }

        public OriginBranch[] GetCurrentBranches()
        {
            return _nodesByBranchConsecutive.Where(z => z.Value.Count > 0)
                .Select(kvp => kvp.Key).ToArray();
        }

        public DateTime GetFirstNodeDate(OriginBranch branch)
        {
            return GetNodesConsecutive(branch).First().Time;
        }

        public int GetIndexOnBranch(INode child)
        {
            OriginBranch ba = GetBranch(child);
            if (ba == null)
            {
                return -1;
            }

            List<INode> nodesList = _nodesByBranchConsecutive[ba];
            int index = nodesList.IndexOf(child);
            return index;
        }

        public INode[] GetNodesConsecutive(OriginBranch branch)
        {
            return _nodesByBranchConsecutive[branch].ToArray();
        }

        public OriginBranch[] GetOrphanedBranches()
        {
            return _nodesByBranchConsecutive.Where(z => z.Value.Count == 0)
                .Select(kvp => kvp.Key).ToArray();
        }

        public void RemoveNodeFromBranch(OriginBranch branch, INode node)
        {
            _nodesByBranchConsecutive[branch].Remove(node);
            _branchesByNodes.Remove(node);
        }

        public void RemoveNodeFromLeftOvers(INode n)
        {
            _leftOvers.Remove(n);
        }

        public void SetBranchNodes(OriginBranch branch, IEnumerable<INode> consecutiveNodes)
        {
            var list = new List<INode>(consecutiveNodes);
            _nodesByBranchConsecutive.Add(branch, list);
            foreach (INode item in list)
            {
                _branchesByNodes.Add(item, branch);
            }
        }

        public void SetLeftOverNodes(IEnumerable<INode> nodes)
        {
            _leftOvers = new HashSet<INode>(nodes ?? new INode[0]);
        }

        public void SetTags(IEnumerable<Tag> allTags)
        {
            _tags = allTags.ToArray();
        }
    }
}