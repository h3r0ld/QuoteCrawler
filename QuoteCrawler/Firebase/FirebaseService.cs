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

        public async Task ReUpload(List<FirebaseQuote> quotes)
        {
            await Clear();
            await Upload(quotes);
        }

        public async Task Upload(List<FirebaseQuote> quotes)
        {
            var partials = quotes.Split(10);

            for (var i = 0; i < partials.Count(); i++)
            {
                Task.Run(async () => await Upload($"Uploading batch: {i}!", partials.ElementAt(i)));
            }
        }

        private async Task Upload(string message, IEnumerable<FirebaseQuote> quotes)
        {
            Console.WriteLine("Uploading quotes to firebase...");
            var cnt = 0;
            foreach(var quote in quotes)
            {
                Console.WriteLine($"{message} Author: {quote.Author}, {++cnt} of {quotes.Count()}");
                await _firebaseClient.Child(QUOTES_NODE).PostAsync(quote);
            }
        }

        public async Task Clear()
        {
            await _firebaseClient.Child(QUOTES_NODE).DeleteAsync();
        }
    }
}
