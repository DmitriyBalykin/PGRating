using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using System.Threading.Tasks;

namespace PGRating.Tests.PGRating.Crawler
{
    [TestClass]
    public class DataReadersTest
    {
        [TestMethod]
        public async Task DataReaders_SimpleData_Load_Test()
        {
            var reader = new ActualCompetitionsDataReader(new FileLoader());

            var result = await reader.LoadUsedCompetitionsPageAsync(@"H:\workspace\PGRating\SampleData\AllCompetitionsPageData.txt");

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task DataReaders_CompetitionsTable_Load_Test()
        {
            var reader = new ActualCompetitionsDataReader(new FileLoader());

            var resultTable = await reader.LoadUsedCompetitionsTableAsync(@"H:\workspace\PGRating\SampleData\AllCompetitionsPageData.txt");

            Assert.IsTrue(resultTable != null);
            Assert.IsTrue(resultTable.Rows.Count > 0);
        }

        [TestMethod]
        public async Task DataReaders_NationPilotsTable_Load_Test()
        {
            var reader = new NationPilotsDataReader(new FileLoader());

            var resultTable = await reader.LoadNationPilotsTableAsync(url: @"H:\workspace\PGRating\SampleData\UkrainianPilotsTable.txt");

            Assert.IsTrue(resultTable != null);
            Assert.IsTrue(resultTable.Rows.Count > 0);
        }

        [TestMethod]
        public async Task DataReaders_NationsTable_Load_Test()
        {
            var reader = new NationsDataReader(new FileLoader());

            var results = await reader.LoadNationsAsync(@"H:\workspace\PGRating\SampleData\AllNationsPageData.txt");

            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Count == 75);
        }
    }
}
