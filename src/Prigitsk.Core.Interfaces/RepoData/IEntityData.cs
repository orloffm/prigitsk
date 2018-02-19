using System.Collections.Generic;

namespace Prigitsk.Core.RepoData
{
    public interface IEntityData<T> : IEnumerable<T>
    {
        int Count { get; }
    }
}