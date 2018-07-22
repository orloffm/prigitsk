using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Graph
{
    public interface ITagInfo
    {
        IBranch ContainingBranch { get; }

        INode Node { get; }

        ITag Tag { get; }
    }
}