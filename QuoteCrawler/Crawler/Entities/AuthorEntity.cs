using System;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model.Attribute;

namespace QuoteCrawler.Crawler.Entities
{
    public abstract class AuthorEntity : BaseEntity
    {
        [Column]
        [Field(Expression = "AUTHOR", Type = SelectorType.Enviroment)]
        public string Author { get; set; }
    }
}
