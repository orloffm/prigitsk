using System.Collections.Generic;

namespace Prigitsk.Core.Tools
{
    public interface IMultipleDictionary<TKey, TValue> : IDictionary<TKey, ISet<TValue>>
    {
        bool Add(TKey key, TValue value);

        bool Remove(TKey key, TValue value);
    }
}