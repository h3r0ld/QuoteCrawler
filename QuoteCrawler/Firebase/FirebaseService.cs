using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using QuoteCrawler.Extension;
using QuoteCrawler.Firebase.Entities;

namespace QuoteCrawler.Firebase
{
    public class FirebaseService
    {
        private static readonly string QUOTES_NODE = "quotes";

        private readonly FirebaseClient _firebaseClient;

        public FirebaseService(string baseUrl)
        {
            _firebaseClient = new FirebaseClient(baseUrl);
        }

        public async Task ReUpload(List<FirebaseQuote> quotes, int batchSize)
        {
            await Clear();
            await Upload(quotes, batchSize);
        }

        public async Task Upload(List<FirebaseQuote> quotes, int batchSize)
        {
            // Split the quotes to batches, 
            // so we can post them on multiple threads simultaneously
            var batches = quotes.Split(batchSize);

            var tasks = new List<Task>();

            for (var i = 0; i < batches.Count(); i++)
            {
                tasks.Add(Upload(batches.ElementAt(i)));
            }

            await Task.WhenAll(tasks);
        }

        public async Task Clear()
        {
            await _firebaseClient.Child(QUOTES_NODE).DeleteAsync();
        }

        private async Task Upload(IEnumerable<FirebaseQuote> quotes)
        {
            Console.WriteLine("Uploading quotes to firebase...");
            foreach (var quote in quotes)
            {
                await _firebaseClient.Child(QUOTES_NODE).PostAsync(quote);
            }
        }
    }
}
