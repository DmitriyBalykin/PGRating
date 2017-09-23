using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using System.Threading.Tasks;

namespace PGRating.Tests.PGRating.Crawler
{
    [TestClass]
    public class CrawlerTest
    {
        [TestMethod]
        public async Task Crawler_SimpleData_Load_Test()
        {
            var crawler = new ActualCompetitionsDataReader(new FileLoader());

            var result = await crawler.LoadUsedCompetitionsPageAsync(@"H:\workspace\PGRating\SampleData\AllCopmetitionsPageData.txt");

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task Crawler_CompetitionsTable_Load_Test()
        {
            var crawler = new ActualCompetitionsDataReader(new FileLoader());

            var resultTable = await crawler.LoadUsedCompetitionsTableAsync(@"H:\workspace\PGRating\SampleData\AllCopmetitionsPageData.txt");

            Assert.IsTrue(resultTable != null);
            Assert.IsTrue(resultTable.Rows.Count > 0);
        }

        [TestMethod]
        public async Task Crawler_NationPilotsTable_Load_Test()
        {
            var crawler = new NationPilotsDataReader(new FileLoader());

            var resultTable = await crawler.LoadNationPilotsTableAsync(@"H:\workspace\PGRating\SampleData\UkrainianPilotsTable.txt");

            Assert.IsTrue(resultTable != null);
            Assert.IsTrue(resultTable.Rows.Count > 0);
        }
    }
}
