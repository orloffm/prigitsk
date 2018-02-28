using System.Collections.Generic;

namespace Prigitsk.Core.RepoData
{
    public interface IEntityData<out T> : IEnumerable<T>
    {
        int Count { get; }
    }
}