using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGRating.Crawler.Scrapper;
using PGRating.DAL.Repository;
using System.Threading.Tasks;

namespace PGRating.Tests.PGRating.Scrapper
{
    [TestClass]
    public class RegularTasksTest
    {
        [TestMethod]
        public async Task RegularTasks_UpdateNations_RunsCorrectly()
        {
            var repo = new NationsRepository();

            await repo.ClearNationsAsync();

            var nations = await repo.GetNationsAsync();
            nations.Count.Should().Equals(0);

            await RegularTasks.UpdateNationsTask();
            
            var nations1 = await repo.GetNationsAsync();

            nations1.Should().NotBeNull();
            nations1.Count.Should().Equals(75);
        }
    }
}
