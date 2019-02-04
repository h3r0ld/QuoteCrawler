using System;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;

namespace QuoteCrawler.Crawler
{
    public abstract class AuthorSpider: EntitySpider
    {
        protected static readonly string AUTHOR_KEY = "AUTHOR";

        public string Author { get; }
        public string BaseUrl { get; }

        public CollectionEntityPipeline CollectionEntityPipeline { get; }

        protected AuthorSpider(string author, string baseUrl)
        {
            Author = author;
            BaseUrl = baseUrl;
            CollectionEntityPipeline = new CollectionEntityPipeline();
        }

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);
            AddPipeline(CollectionEntityPipeline);
        }
    }
}
