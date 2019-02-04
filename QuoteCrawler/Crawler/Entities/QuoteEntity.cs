using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;

namespace QuoteCrawler.Crawler.Entities
{
    [Schema("quotecrawler", "citatum")]
    [Entity(Expression = ".//div[@id='idz']//div[@id]//p", Type = SelectorType.XPath)]
    public class QuoteEntity : AuthorEntity
    {
        [Column]
        [Field(Expression = ".", Option = FieldOptions.InnerText)]
        public string Quote { get; set; }
    }
}
