namespace Prigitsk.Core.Nodes.Loading
{
    /// <summary>
    /// Creates an object that collects data about repository and provides an immutable result.
    /// </summary>
    public interface IRepositoryDataBuilderFactory
    {
        IRepositoryDataBuilder CreateBuilder();
    }
}