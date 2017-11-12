namespace GitWriter.Core.Nodes
{
    public interface INodeCleaner
    {
        void CleanUpGraph(
            IAssumedGraph graph,
            SimplificationOptions options);
    }
}