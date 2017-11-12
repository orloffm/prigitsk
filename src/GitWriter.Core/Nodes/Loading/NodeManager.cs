using System;
using System.Collections.Generic;

namespace GitWriter.Core.Nodes.Loading
{
    public class NodeManager : INodeManager
    {
        private readonly Dictionary<string, Node> _nodes;

        public NodeManager()
        {
            _nodes = new Dictionary<string, Node>();
        }

        public IEnumerable<Node> EnumerateNodes()
        {
            return _nodes.Values;
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
            n.Time = UnixTimeStampToDateTime(time);
            n.Insertions = insertions;
            n.Deletions = deletions;
        }

        public void AddChildren(string parentHash, string hash)
        {
            Node child = GetOrCreate(hash);
            Node parent = GetOrCreate(parentHash);
            child.Parents.Add(parent);
            parent.Children.Add(child);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
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