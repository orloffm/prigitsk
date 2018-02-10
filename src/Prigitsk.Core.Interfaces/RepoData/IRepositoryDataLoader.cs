using System.Collections.Generic;

namespace Prigitsk.Core.Nodes.Loading
{
    /// <summary>
    /// Loads offline data from a Git repository.
    /// </summary>
    public interface  IRepositoryDataLoader
    {
        IRepositoryData LoadFrom(string gitRepository);
    }
}