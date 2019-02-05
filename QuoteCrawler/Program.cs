using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QuoteCrawler.Crawler;
using QuoteCrawler.Crawler.Entities;
using QuoteCrawler.Extension;
using QuoteCrawler.Firebase;

namespace QuoteCrawler
{
    class Program
    {
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
            var stopWatch = Stopwatch.StartNew();

            var firebaseHost = args[0];
            Console.WriteLine($"Using firebase host: {firebaseHost}");

            int.TryParse(args[1], out int maxQuoteLength);

            Console.WriteLine($"Max allowed quote length: {maxQuoteLength}");

            string[] authors = args[2].Split(',');

            Console.WriteLine($"Processing authors: {string.Join(',', authors)}");

            await ProcessQuotes(authors.ToList(), maxQuoteLength, firebaseHost);

            stopWatch.Stop();
            Console.WriteLine($"Finished in {stopWatch.ElapsedMilliseconds} ms");
        }

        public static async Task ProcessQuotes(List<string> authors, int maxQuoteLength, string firebaseHost)
        {
            // Get max page numbers for every author
            PagingSpider pagingSpider = new PagingSpider(authors);
            pagingSpider.Run();

            // Convert the result to PagingEntity objects
            var pagingEntities = pagingSpider.CollectionEntityPipeline.ToEntityList<PagingEntity>();

            // Get all quotes from all pages for the author
            QuoteSpider quoteSpider = new QuoteSpider(pagingEntities.ToList())
            {
                ThreadNum = 4
            };
            quoteSpider.Run();

            // Convert the result to FirebaseQuote objects
            var firebaseQuotes = quoteSpider.CollectionEntityPipeline.ToFirebaseQuotes(maxQuoteLength);

            // Delete all existing quotes from firebase, then upload all quotes
            var firebaseService = new FirebaseService(firebaseHost);
            await firebaseService.ReUpload(firebaseQuotes, batchSize: 50);
        }
    }
}
