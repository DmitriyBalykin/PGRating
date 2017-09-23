using PGRating.DAL.DataContext;
using PGRating.DAL.Repository;
using PGRating.Domain;
using PGRating.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGRating.Crawler.Crawler
{
    public class DataCrawler
    {
        public static void Start()
        {
            Task.Run(async () => {
                var competitions = await GetActualCompetitions();
                await SaveActualCompetitions(competitions).ConfigureAwait(false);
                });
            
        }

        private static async Task<List<Competition>> GetActualCompetitions()
        {
            var dataCalculator = new RatingTableCalculator();
            var competitionsDictionary = await dataCalculator.GetCompetitionsAsync();

            return competitionsDictionary.Values.ToList();
        }

        private static async Task SaveActualCompetitions(List<Competition> competitions)
        {
            try
            {
                var repository = new CivlDataRepository();

                await repository.SaveCompetitions(competitions);
            }
            catch (System.Exception ex)
            {

            }

        }
    }
}
