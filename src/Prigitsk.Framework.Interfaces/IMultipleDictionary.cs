using System.Collections.Generic;

namespace Prigitsk.Framework
{
    /// <summary>
    ///     A dictionary that keeps a set of values per key.
    /// </summary>
    public interface IMultipleDictionary<TKey, TValue> : IDictionary<TKey, ISet<TValue>>
    {
        bool Add(TKey key, TValue value);

        bool Add(TKey key, IEnumerable<TValue> values);

        bool Remove(TKey key, TValue value);

        /// <summary>
        ///     Enumerates all values for the key. If none are present, returns an empty enumerable.
        /// </summary>
        IEnumerable<TValue> TryEnumerateFor(TKey node);
    }
}