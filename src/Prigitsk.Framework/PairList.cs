using System;
using System.Collections;
using System.Collections.Generic;

namespace Prigitsk.Framework
{
    public sealed class PairList<TA, TB> : IPairList<TA, TB>
    {
        private readonly List<Tuple<TA, TB>> _items;

        public PairList()
        {
            _items = new List<Tuple<TA, TB>>();
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public Tuple<TA, TB> this[int index]
        {
            get =>  _items[index];
            set => _items[index] = value;
        }

        public void Add(Tuple<TA, TB> item)
        {
           _items.Add(item);
        }

        public void Add(TA a, TB b)
        {
            Add(Tuple.Create(a, b));
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Tuple<TA, TB> item)
        {
            return _items.Contains(item);
        }

        public bool Contains(TA a, TB b)
        {
            return Contains(Tuple.Create(a, b));
        }

        public void CopyTo(Tuple<TA, TB>[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Tuple<TA, TB>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(Tuple<TA, TB> item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Tuple<TA, TB> item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(Tuple<TA, TB> item)
        {
            return _items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}