﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Framework
{
    public class MultipleDictionary<TKey, TValue>
        : IMultipleDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, ISet<TValue>> _dic;

        public MultipleDictionary()
        {
            _dic = new Dictionary<TKey, ISet<TValue>>();
        }

        public int Count => _dic.Count;

        public bool IsReadOnly => false;

        public ISet<TValue> this[TKey key]
        {
            get => _dic[key];
            set => _dic[key] = value;
        }

        public ICollection<TKey> Keys => _dic.Keys;

        public ICollection<ISet<TValue>> Values => _dic.Values;

        public bool Add(TKey key, TValue value)
        {
            bool added = AddInternal(
                key,
                set => set.Add(value));

            return added;
        }

        public bool Add(TKey key, IEnumerable<TValue> values)
        {
            bool added = AddInternal(
                key,
                set =>
                {
                    bool anythingAdded = false;
                    foreach (TValue value in values)
                    {
                        anythingAdded |= set.Add(value);
                    }

                    return anythingAdded;
                }
            );

            return added;
        }

        public void Add(TKey key, ISet<TValue> values)
        {
            Add(key, values.AsEnumerable());
        }

        public void Clear()
        {
            _dic.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _dic.ContainsKey(key);
        }

        public bool Remove(TKey key, TValue value)
        {
            ISet<TValue> valueSet;
            if (!_dic.TryGetValue(key, out valueSet))
            {
                return false;
            }

            bool removed = valueSet.Remove(value);
            if (valueSet.Count == 0)
            {
                _dic.Remove(key);
            }

            return removed;
        }

        public bool Remove(TKey key)
        {
            return _dic.Remove(key);
        }

        public IEnumerable<TValue> TryEnumerateFor(TKey key)
        {
            ISet<TValue> valueSet;
            if (!_dic.TryGetValue(key, out valueSet))
            {
                yield break;
            }

            foreach (TValue value in valueSet)
            {
                yield return value;
            }
        }

        public bool TryGetValue(TKey key, out ISet<TValue> value)
        {
            return _dic.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<TKey, ISet<TValue>>>.Add(KeyValuePair<TKey, ISet<TValue>> item)
        {
            ((ICollection<KeyValuePair<TKey, ISet<TValue>>>) _dic).Add(item);
        }

        private bool AddInternal(TKey key, Func<ISet<TValue>, bool> addValue)
        {
            ISet<TValue> valueSet;
            if (!_dic.TryGetValue(key, out valueSet))
            {
                valueSet = new HashSet<TValue>();
                _dic.Add(key, valueSet);
            }

            return addValue(valueSet);
        }

        bool ICollection<KeyValuePair<TKey, ISet<TValue>>>.Contains(KeyValuePair<TKey, ISet<TValue>> item)
        {
            return ((ICollection<KeyValuePair<TKey, ISet<TValue>>>) _dic).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, ISet<TValue>>>.CopyTo(
            KeyValuePair<TKey, ISet<TValue>>[] array,
            int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, ISet<TValue>>>) _dic).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<TKey, ISet<TValue>>> IEnumerable<KeyValuePair<TKey, ISet<TValue>>>.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        bool ICollection<KeyValuePair<TKey, ISet<TValue>>>.Remove(KeyValuePair<TKey, ISet<TValue>> item)
        {
            return ((ICollection<KeyValuePair<TKey, ISet<TValue>>>) _dic).Remove(item);
        }
    }
}