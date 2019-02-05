using System.Collections.Generic;
using System.Linq;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extraction.Model;
using QuoteCrawler.Crawler.Entities;
using QuoteCrawler.Firebase.Entities;

namespace QuoteCrawler.Extension
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this ICollection<T> array, int size)
        {
            for (var i = 0; i < (float)array.Count / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static List<T> ToEntityList<T>(this CollectionEntityPipeline collectionEntityPipeline) where T : BaseEntity
        {
            return collectionEntityPipeline.GetCollection(typeof(T).FullName).ToEntityList<T>();
        }

        public static List<T> ToEntityList<T>(this IList<IBaseEntity> list) where T : BaseEntity
        {
            return list.Select(x => x as T).ToList();
        }

        public static List<FirebaseQuote> ToFirebaseQuotes(this CollectionEntityPipeline collectionEntityPipeline, int maxQuoteLength)
        {
            return collectionEntityPipeline.GetCollection(typeof(QuoteEntity).FullName).ToFirebaseQuotes(maxQuoteLength);
        }

        public static List<FirebaseQuote> ToFirebaseQuotes(this IList<IBaseEntity> list, int maxQuoteLength) {
            return list.Select(x => x as QuoteEntity)
                       .Where(x => x.Quote.Length < maxQuoteLength)
                       .Select(x => {
                           return new FirebaseQuote
                           {
                               Author = x.Author,
                               Quote = x.Quote
                           };
                       })
                       .ToList();
        }
    }
}
