namespace Prigitsk.Core.Nodes.Loading
{
    public interface INodeKeeperFactory
    {
        INodeKeeper CreateKeeper();
    }
}