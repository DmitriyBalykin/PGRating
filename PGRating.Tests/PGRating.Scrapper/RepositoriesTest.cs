using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using PGRating.DAL.Repository;
using PGRating.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGRating.Tests.PGRating.Scrapper
{
    [TestClass]
    public class RepositoriesTest
    {
        [TestMethod]
        public async Task NationsRepository_UpdateNations_RunsCorrectly()
        {
            var repo = new NationsRepository();

            await repo.ClearNationsAsync();

            var nations = await repo.GetNationsAsync();
            nations.Count.Should().Be(0);

            var dataReader = new NationsDataReader(new FileLoader());
            var nationsList = await dataReader.LoadNationsAsync(@"H:\workspace\PGRating\SampleData\AllNationsPageData.txt");
            await repo.SaveNationsAsync(nationsList);

            var nations1 = await repo.GetNationsAsync();

            nations1.Should().NotBeNull();
            nations1.Count.Should().Be(75);
        }

        [TestMethod]
        public async Task PilotsRepository_UpdatePilots_RunsCorrectly()
        {
            using (var repo = new PilotsRepository())
            {
                await repo.ClearPilotsAsync();

                var pilots = await repo.GetPilotsAsync();
                pilots.Count.Should().Be(0);

                var dataReader = new NationPilotsDataReader(new FileLoader());
                var pilotsList = await dataReader.LoadNationPilotsAsync(-1, @"H:\workspace\PGRating\SampleData\UkrainianPilotsTable.txt");

                await repo.SavePilotsAsync(pilotsList);

                var pilots1 = await repo.GetPilotsAsync();

                pilots1.Should().NotBeNull();
                pilots1.Count.Should().Be(95);
            }
        }
    }
}
