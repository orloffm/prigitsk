using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Nodes.Loading
{
    public class NodeKeeper : INodeKeeper
    {
        private readonly Dictionary<string, Node> _nodes;
        private readonly ITimeHelper _timeHelper;
        private readonly ITreeManipulator _manipulator;

        public NodeKeeper(ITimeHelper timeHelper, ITreeManipulator manipulator)
        {
            _nodes = new Dictionary<string, Node>();
            _timeHelper = timeHelper;
            _manipulator = manipulator;
        }

        public IEnumerable<INode> EnumerateNodes()
        {
            return _nodes.Values.Cast<INode>();
        }

        public void SetData(
            string hash,
            string caption,
            long time,
            int insertions,
            int deletions)
        {
            Node n = GetOrCreate(hash);
            if (!string.IsNullOrWhiteSpace(caption))
            {
                n.SetCaptions(caption);
            }

            n.Time = _timeHelper.UnixTimeStampToDateTime(time);
            n.Insertions = insertions;
            n.Deletions = deletions;
        }

        public void AddChildren(string parentHash, string hash)
        {
            INode child = GetOrCreate(hash);
            INode parent = GetOrCreate(parentHash);
            _manipulator.AddParent(child,parent);
            parent.Children.Add(child);
        }

        private Node GetOrCreate(string hash)
        {
            Node n;
            if (!_nodes.TryGetValue(hash, out n))
            {
                n = new Node(hash);
                _nodes.Add(hash, n);
            }

            return n;
        }
    }
}