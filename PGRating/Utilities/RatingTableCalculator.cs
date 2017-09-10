using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using PGRating.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace PGRating.Utilities
{
    public class RatingTableCalculator
    {
        public async Task<IList<Participant>> GetParticipantsAsync()
        {
            var loader = new FileLoader();

            var crawler = new CompetitionsDataReader(new FileLoader());

            var executionRootPath = Path.GetFullPath(HostingEnvironment.ApplicationPhysicalPath);

            var projectRootPath = Directory.GetParent(Directory.GetParent(executionRootPath).FullName).FullName;

            var competitionsDataPath = $"{projectRootPath}\\SampleData\\AllCopmetitionsPageData.txt";

            var resultTable = await crawler.LoadUsedCompetitionsTableAsync(competitionsDataPath);

            var participants = new List<Participant>();

            return participants;
        }
    }
}