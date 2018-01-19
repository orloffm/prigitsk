namespace Prigitsk.Core.Nodes.Loading
{
    public interface INodeLoader
    {
        Node[] GetNodesCollection();

        void LoadFrom(string gitSubDirectory, ExtractionOptions extractOptions);
    }
}