using System.Collections.Generic;
using QuoteCrawler.Crawler.Entities;

namespace QuoteCrawler.Crawler
{
    public class QuoteSpider : CitatumSpider
    {
        public List<PagingEntity> AuthorsWithMaxPages { get; }

        public QuoteSpider(List<PagingEntity> pagingEntities)
        {
            AuthorsWithMaxPages = pagingEntities;
        }

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);

            AuthorsWithMaxPages.ForEach(authorWithMaxPage =>
            {
                for (int i = 1; i <= authorWithMaxPage.MaxPages; i++)
                {
                    var authorWithUnderscore = authorWithMaxPage.Author.Replace(' ', '_');
                    AddRequest($"{BaseUrl}{authorWithUnderscore}/{i}", 
                        new Dictionary<string, dynamic> { { AUTHOR_KEY, authorWithMaxPage.Author } });
                }
            });

            AddEntityType<QuoteEntity>();
        }
    }
}
