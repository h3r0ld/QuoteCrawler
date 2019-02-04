using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoteCrawler.Extension
{
    public static class ICollectionExtension
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this ICollection<T> array, int size)
        {
            for (var i = 0; i < (float)array.Count / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
}
