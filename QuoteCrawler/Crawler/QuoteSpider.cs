using System;
using System.Collections.Generic;
using System.Linq;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Extension;
using QuoteCrawler.Crawler.Entities;

namespace QuoteCrawler.Crawler
{
    public class QuoteSpider : AuthorSpider
    {
        public int MaxPages { get; }

        public QuoteSpider(string author, string baseUrl, int maxPages) : base(author, baseUrl) 
        {
            MaxPages = maxPages;
        }

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);

            for(int i = 1; i <= MaxPages; i++)
            {
                AddRequest($"{BaseUrl}/{i}", new Dictionary<string, dynamic> { { AUTHOR_KEY, Author } });
            }

            AddEntityType<QuoteEntity>();
        }
    }
}
