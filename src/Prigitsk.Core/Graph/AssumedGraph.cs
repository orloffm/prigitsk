using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph
{
    public class AssumedGraph : IAssumedGraph
    {
        private readonly Dictionary<Node, OriginBranch> _branchesByNodes;
        private readonly Dictionary<OriginBranch, List<Node>> _nodesByBranchConsecutive;
        private HashSet<Node> _leftOvers;
        private Tag[] _tags;

        public AssumedGraph()
        {
            _nodesByBranchConsecutive = new Dictionary<OriginBranch, List<Node>>();
            _branchesByNodes = new Dictionary<Node, OriginBranch>();
            _tags = new Tag[0];
        }

        public void RemoveNodeFromLeftOvers(Node n)
        {
            _leftOvers.Remove(n);
        }

        public IEnumerable<Node> EnumerateAllLeftOvers()
        {
            return _leftOvers;
        }

        public IEnumerable<Node> EnumerateLeftOversWithoutBranchesAndTags()
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

        public void SetBranchNodes(OriginBranch branch, IEnumerable<Node> consecutiveNodes)
        {
            var list = new List<Node>(consecutiveNodes);
            _nodesByBranchConsecutive.Add(branch, list);
            foreach (Node item in list)
            {
                _branchesByNodes.Add(item, branch);
            }
        }

        public void SetTags(IEnumerable<Tag> allTags)
        {
            _tags = allTags.ToArray();
        }

        public OriginBranch GetBranch(Node node)
        {
            OriginBranch branch;
            _branchesByNodes.TryGetValue(node, out branch);
            return branch;
        }

        public int GetIndexOnBranch(Node child)
        {
            OriginBranch ba = GetBranch(child);
            if (ba == null)
            {
                return -1;
            }
            List<Node> nodesList = _nodesByBranchConsecutive[ba];
            int index = nodesList.IndexOf(child);
            return index;
        }

        public IEnumerable<Node> EnumerateNodesUpTheBranch(Node nodeInQuestion)
        {
            return EnumerateNodesAround(nodeInQuestion, true);
        }

        public IEnumerable<Node> EnumerateNodesDownTheBranch(Node nodeInQuestion)
        {
            return EnumerateNodesAround(nodeInQuestion, false);
        }

        public IEnumerable<Node> EnumerateAllContainedNodes()
        {
            foreach (Node n in _branchesByNodes.Keys)
            {
                yield return n;
            }
            foreach (Node n in EnumerateAllLeftOvers())
            {
                yield return n;
            }
        }

        public void RemoveNodeFromBranch(OriginBranch branch, Node node)
        {
            _nodesByBranchConsecutive[branch].Remove(node);
            _branchesByNodes.Remove(node);
        }

        public bool AnyPointersAreSourcedFrom(Node currentNode)
        {
            return _tags.Any(t => t.Source == currentNode)
                   || _nodesByBranchConsecutive.Keys.Any(b => b.Source == currentNode);
        }

        public void SetLeftOverNodes(IEnumerable<Node> nodes)
        {
            _leftOvers = new HashSet<Node>(nodes ?? new Node[0]);
        }

        public OriginBranch[] GetCurrentBranches()
        {
            return _nodesByBranchConsecutive.Where(z => z.Value.Count > 0)
                .Select(kvp => kvp.Key).ToArray();
        }

        public OriginBranch[] GetOrphanedBranches()
        {
            return _nodesByBranchConsecutive.Where(z => z.Value.Count == 0)
                .Select(kvp => kvp.Key).ToArray();
        }

        public Tag[] GetAllTags()
        {
            return _tags;
        }

        public Node[] GetNodesConsecutive(OriginBranch branch)
        {
            return _nodesByBranchConsecutive[branch].ToArray();
        }

        public DateTime GetFirstNodeDate(OriginBranch branch)
        {
            return GetNodesConsecutive(branch).First().Time;
        }

        private IEnumerable<Node> EnumerateNodesAround(Node nodeInQuestion, bool up)
        {
            // If there it's not on a branch, we don't return anything.
            OriginBranch branch = GetBranch(nodeInQuestion);
            if (branch == null)
            {
                yield break;
            }
            List<Node> nodesList = _nodesByBranchConsecutive[branch];
            int index = nodesList.IndexOf(nodeInQuestion);
            int step = up ? -1 : +1;
            for (int i = index + step; i >= 0 && i < nodesList.Count; i += step)
            {
                yield return nodesList[i];
            }
        }
    }
}