using HtmlAgilityPack;

namespace PGRating.Crawler.Loader
{
    public interface ILoader
    {
        void Load(string url);

        HtmlNode DocumentNode { get; }
    }
}
