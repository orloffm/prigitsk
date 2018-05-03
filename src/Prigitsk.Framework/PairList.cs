using System;
using System.Collections.Generic;

namespace Prigitsk.Framework
{
    public class PairList<T, TU>
    {
        private readonly Dictionary<T, HashSet<TU>> _dic;

        public PairList()
        {
            _dic = new Dictionary<T, HashSet<TU>>();
        }

        public void Add(
            T key,
            TU value)
        {
            HashSet<TU> sub;
            if (!_dic.TryGetValue(key, out sub))
            {
                sub = new HashSet<TU>();
                _dic[key] = sub;
            }

            sub.Add(value);
        }

        public bool Contains(
            T key,
            TU value)
        {
            HashSet<TU> sub;
            if (!_dic.TryGetValue(key, out sub))
            {
                return false;
            }

            return sub.Contains(value);
        }

        public IEnumerable<Tuple<T, TU>> EnumerateItems()
        {
            foreach (KeyValuePair<T, HashSet<TU>> kvp in _dic)
            {
                T key = kvp.Key;
                foreach (TU value in kvp.Value)
                {
                    yield return Tuple.Create(key, value);
                }
            }
        }
    }
}