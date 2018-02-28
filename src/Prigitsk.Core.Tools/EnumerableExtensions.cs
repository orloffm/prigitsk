using System.Collections.Generic;
using System.Linq;

namespace Prigitsk.Core.Tools
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T exception)
        {
            return items.Except(new[] {exception});
        }

        public static bool IsSingle<T>(this IEnumerable<T> items)
        {
            using (IEnumerator<T> e = items.GetEnumerator())
            {
                // Not one.
                if (!e.MoveNext())
                {
                    return false;
                }

                // More then one.
                if (e.MoveNext())
                {
                    return false;
                }

                return true;
            }
        }

        public static IEnumerable<T> WrapAsEnumerable<T>(this IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                yield return item;
            }
        }
    }
}