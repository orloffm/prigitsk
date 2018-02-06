using System.Collections.Generic;

namespace Prigitsk.Core.Nodes.Loading
{
    public interface INodeKeeper
    {
        void AddChildren(
            string parentHash,
            string hash);

        IEnumerable<INode> EnumerateNodes();
        void SetData(string hash, string caption, long time, int insertions, int deletions);
    }
}