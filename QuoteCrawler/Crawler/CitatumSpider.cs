using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;

namespace QuoteCrawler.Crawler
{
    public abstract class CitatumSpider: EntitySpider
    {
        protected static readonly string AUTHOR_KEY = "AUTHOR";

        public string BaseUrl { get => "https://citatum.hu/szerzo/"; }

        public CollectionEntityPipeline CollectionEntityPipeline { get; }

        protected CitatumSpider()
        {
            CollectionEntityPipeline = new CollectionEntityPipeline();
        }

        protected override void OnInit(params string[] arguments)
        {
            base.OnInit(arguments);
            AddPipeline(CollectionEntityPipeline);
        }
    }
}
