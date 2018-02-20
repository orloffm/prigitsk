using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.RepoData
{
    public abstract class EntityData<T> : IEntityData<T>
    {
        protected EntityData(IEnumerable<T> data)
        {
            Data = data.ToArray();
        }

        public int Count => Data.Length;

        protected T[] Data { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>) Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}