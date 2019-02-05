using System.Collections.Generic;
using QuoteCrawler.Crawler.Entities;

namespace QuoteCrawler.Crawler
{
    public class PagingSpider : CitatumSpider
    {
        public List<string> Authors { get; }

        public PagingSpider(List<string> authors) { 
            Authors = authors;
        }

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);
            Authors.ForEach(author =>
            {
                var authorWithUnderscore = author.Replace(' ', '_');
                AddRequest($"{BaseUrl}{authorWithUnderscore}", new Dictionary<string, dynamic> { { AUTHOR_KEY, author } });
            });

            AddEntityType<PagingEntity>();
        }
    }
}
