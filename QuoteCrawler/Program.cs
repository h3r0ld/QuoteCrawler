
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotnetSpider.Extraction.Model;
using QuoteCrawler.Crawler;
using QuoteCrawler.Crawler.Entities;
using QuoteCrawler.Firebase;
using QuoteCrawler.Firebase.Entities;

namespace QuoteCrawler
{
    class Program
    {
        private static FirebaseService _firebaseService;

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Required arguments are missing: [First Argument: firebase host url] [Second Argument: Maximum quote length ] [Third Argument -> Author names separated with commas");
            }

            var firebaseHost = args[0];
            Console.WriteLine($"Using firebase host: {firebaseHost}");

            _firebaseService = new FirebaseService(firebaseHost);

            int.TryParse(args[1], out int maxQuoteLength);

            Console.WriteLine($"Max allowed quote length: {maxQuoteLength}");

            string[] authors = args[2].Split(',');

            Console.WriteLine($"Processing authors: {string.Join(',', authors)}");

            var stopWatch = Stopwatch.StartNew();

            await _firebaseService.Clear();

            foreach(var author in authors)
            {
                await ProcessQuotes(author, maxQuoteLength);
            }

            stopWatch.Stop();
            Console.WriteLine($"Finished in {stopWatch.ElapsedMilliseconds} ms");
        }

        public static async Task ProcessQuotes(string author, int maxQuoteLength)
        {
            var baseUrl = $"https://citatum.hu/szerzo/{author.Replace(' ', '_')}";

            PagingSpider pagingSpider = new PagingSpider(author, baseUrl)
            {
            };
            pagingSpider.Run();

            var pagingEntities = pagingSpider.CollectionEntityPipeline.GetCollection(typeof(PagingEntity).FullName);

            var paging = pagingEntities.First() as PagingEntity;
            Console.WriteLine($"Author: {paging.Author} Max pages: {paging.MaxPages}");

            QuoteSpider quoteSpider = new QuoteSpider(author, baseUrl, paging.MaxPages)
            {
                
                ThreadNum = 2
            };
            quoteSpider.Run();

            var quoteEntities = quoteSpider.CollectionEntityPipeline.GetCollection(typeof(QuoteEntity).FullName);;

            var firebaseQuotes = new List<FirebaseQuote>();

            foreach (IBaseEntity item in quoteEntities)
            {
                var quoteEntity = item as QuoteEntity;

                if (quoteEntity.Quote.Length < maxQuoteLength)
                {
                    firebaseQuotes.Add(new FirebaseQuote
                    {
                        Author = quoteEntity.Author,
                        Quote = quoteEntity.Quote
                    });
                }
            }

            await _firebaseService.Upload(firebaseQuotes);
        }
    }
}
