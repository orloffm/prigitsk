using System;
using System.Collections.Generic;

namespace GitWriter.Core.Tools
{
    public class PairList<T, U>
    {
        private readonly Dictionary<T, HashSet<U>> _dic;

        public PairList()
        {
            _dic = new Dictionary<T, HashSet<U>>();
        }

        public void Add(
            T key,
            U value)
        {
            HashSet<U> sub;
            if (!_dic.TryGetValue(key, out sub))
            {
                sub = new HashSet<U>();
                _dic[key] = sub;
            }
            sub.Add(value);
        }

        public bool Contains(
            T key,
            U value)
        {
            HashSet<U> sub;
            if (!_dic.TryGetValue(key, out sub))
            {
                return false;
            }
            return sub.Contains(value);
        }

        public IEnumerable<Tuple<T, U>> EnumerateItems()
        {
            foreach (KeyValuePair<T, HashSet<U>> kvp in _dic)
            {
                T key = kvp.Key;
                foreach (U value in kvp.Value)
                {
                    yield return Tuple.Create(key, value);
                }
            }
        }
    }
}