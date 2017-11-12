using System.Collections.Generic;

namespace GitWriter.Core.Nodes.Loading
{
    public interface INodeManager
    {
        void AddChildren(
            string parentHash,
            string hash);

        IEnumerable<Node> EnumerateNodes();
        void SetData(string hash, string caption, long time, int insertions, int deletions);
    }
}