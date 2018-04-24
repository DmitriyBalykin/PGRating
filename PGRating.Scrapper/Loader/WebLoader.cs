using HtmlAgilityPack;

namespace PGRating.Crawler.Loader
{
    public class WebLoader : HtmlWeb, ILoader
    {
        public HtmlNode DocumentNode => this.documentNode;

        private HtmlNode documentNode;

        void ILoader.Load(string url)
        {
            this.documentNode = base.Load(url).DocumentNode;
        }
    }
}
