
using System;
using System.Linq;
using DotnetSpider.Extraction.Model;
using QuoteCrawler.Crawler;
using QuoteCrawler.Crawler.Entities;

namespace QuoteCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            PagingSpider pagingSpider = new PagingSpider("Paulo Coelho", "https://citatum.hu/szerzo/Paulo_Coelho");
            pagingSpider.Run();

            var pagingEntities = pagingSpider.CollectionEntityPipeline.GetCollection(typeof(PagingEntity).FullName);

            var paging = pagingEntities.First() as PagingEntity;
            Console.WriteLine($"Author: {paging.Author} Max pages: {paging.MaxPages}");

            QuoteSpider quoteSpider = new QuoteSpider("Paulo Coelho", "https://citatum.hu/szerzo/Paulo_Coelho", paging.MaxPages);
            quoteSpider.Run();

            var quoteEntities = quoteSpider.CollectionEntityPipeline.GetCollection(typeof(QuoteEntity).FullName);

            foreach(IBaseEntity item in quoteEntities)
            {
                var quoteEntity = item as QuoteEntity;
                Console.WriteLine($"Quote: {quoteEntity.Quote}");
            }
        }
    }
}
