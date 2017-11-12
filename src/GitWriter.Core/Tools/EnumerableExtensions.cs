using System.Collections.Generic;
using System.Linq;

namespace GitWriter.Core.Tools
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T exception)
        {
            return items.Except(new[] {exception});
        }
    }
}