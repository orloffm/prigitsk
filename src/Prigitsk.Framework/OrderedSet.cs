using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Framework
{
    public class OrderedSet<T> : IOrderedSet<T>
    {
        private readonly IDictionary<T, LinkedListNode<T>> _dictionary;
        private readonly LinkedList<T> _linkedList;

        public OrderedSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        public OrderedSet(IEqualityComparer<T> comparer)
        {
            _dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            _linkedList = new LinkedList<T>();
        }

        public int Count => _dictionary.Count;

        public T First => _linkedList.First.Value;

        public bool IsReadOnly => false;

        public T Last => _linkedList.Last.Value;

        public bool Add(T item)
        {
            return AddLast(item);
        }

        public bool AddFirst(T item)
        {
            return AddInternal(item, _linkedList.AddFirst);
        }

        public bool AddLast(T item)
        {
            return AddInternal(item, _linkedList.AddLast);
        }

        public void Clear()
        {
            _linkedList.Clear();
            _dictionary.Clear();
        }

        public bool Contains(T item)
        {
            return _dictionary.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _linkedList.CopyTo(array, arrayIndex);
        }

        public IEnumerable<T> EnumerateAfter(T item)
        {
            LinkedListNode<T> node = _dictionary[item];
            do
            {
                node = node.Next;
                if (node == null)
                {
                    yield break;
                }

                yield return node.Value;
            } while (true);
        }

        public IEnumerable<T> EnumerateBefore(T item)
        {
            LinkedListNode<T> node = _dictionary[item];
            do
            {
                node = node.Previous;
                if (node == null)
                {
                    yield break;
                }

                yield return node.Value;
            } while (true);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            // this is already the empty set; return
            if (Count == 0)
            {
                return;
            }

            // special case if other is this; a set minus itself is the empty set
            if (ReferenceEquals(other, this))
            {
                Clear();
                return;
            }

            // remove every element in other from this
            foreach (T element in other)
            {
                Remove(element);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            if (Count == 0)
            {
                return false;
            }

            foreach (T element in other)
            {
                if (Contains(element))
                {
                    return true;
                }
            }

            return false;
        }

        public T PickFirst(IEnumerable<T> items)
        {
            var itemsSet = new HashSet<T>(items);
            if (itemsSet.Count < 2)
            {
                return itemsSet.FirstOrDefault();
            }

            T currentItem = itemsSet.First();
            LinkedListNode<T> node = _dictionary[currentItem];
            do
            {
                node = node.Previous;
                if (node == null)
                {
                    return currentItem;
                }

                // Should we switch the current item?
                if (itemsSet.Contains(node.Value))
                {
                    currentItem = node.Value;
                }
            } while (true);
        }

        public bool Remove(T item)
        {
            bool found = _dictionary.TryGetValue(item, out LinkedListNode<T> node);
            if (!found)
            {
                return false;
            }

            _dictionary.Remove(item);
            _linkedList.Remove(node);
            return true;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                Add(item);
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        private bool AddInternal(T item, Func<T, LinkedListNode<T>> adder)
        {
            if (_dictionary.ContainsKey(item))
            {
                return false;
            }

            LinkedListNode<T> node = adder(item);
            _dictionary.Add(item, node);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}