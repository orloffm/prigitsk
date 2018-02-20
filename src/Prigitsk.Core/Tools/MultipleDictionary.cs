using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.Tools
{
    public class MultipleDictionary<TKey, TValue> : IMultipleDictionary<TKey, TValue>
    {
        public bool Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, ISet<TValue>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, ISet<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, ISet<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, ISet<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, ISet<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }

        public bool IsReadOnly { get; }

        public void Add(TKey key, ISet<TValue> value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out ISet<TValue> value)
        {
            throw new NotImplementedException();
        }

        public ISet<TValue> this[TKey key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ICollection<TKey> Keys { get; }

        public ICollection<ISet<TValue>> Values { get; }
    }
}