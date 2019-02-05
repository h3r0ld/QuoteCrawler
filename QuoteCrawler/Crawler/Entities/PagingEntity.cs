using System;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;

namespace QuoteCrawler.Crawler.Entities
{
    [Entity(Expression = "(.//div[@class='lapoz']//div[@class='lapc']//a)[last()]", Type = SelectorType.XPath)]
    public class PagingEntity : AuthorEntity
    {
        [Column]
        [Field(Expression = ".", Option = FieldOptions.InnerText)]
        public int MaxPages { get; set; }
    }
}
