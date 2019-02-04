using System;
using System.Collections.Generic;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;
using QuoteCrawler.Crawler.Entities;

namespace QuoteCrawler.Crawler
{
    public class PagingSpider : AuthorSpider
    {
        public PagingSpider(string author, string baseUrl) : base(author, baseUrl) {}

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);
            AddRequest(BaseUrl, new Dictionary<string, dynamic> { { AUTHOR_KEY, Author } });
            AddEntityType<PagingEntity>();
        }
    }
}
